using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheKiwiCoder
{
    [AddComponentMenu("BehaviourTree/BehaviourTreeInstance")]
    public class BehaviourTreeInstance : SerializedMonoBehaviour
    {
        // The main behaviour tree asset
        [Tooltip("BehaviourTree asset to instantiate during Awake")]
        public BehaviourTree behaviourTree;

        [Tooltip("Run behaviour tree validation at startup (Can be disabled for release)")]
        public bool validate = true;
        
        // These values override the keys in the blackboard
        public List<BlackboardKeyValuePair> blackboardOverrides = new();
        
        // Storage container object to hold game object subsystems
        private Context context;
        private BehaviourTree runtimeTree;

        public BehaviourTree RuntimeTree
        {
            get
            {
                if (runtimeTree != null)
                {
                    return runtimeTree;
                }

                return behaviourTree;
            }
        }

        // Update is called once per frame
        private void Update()
        {
	        runtimeTree.Update();
        }
        
        // Start is called before the first frame update
        private void OnEnable()
        {
            var isValid = ValidateTree();
            if (isValid)
            {
                context = CreateBehaviourTreeContext();
                runtimeTree = behaviourTree.Clone();
                if (context.btController != null)
                {
                    context.btController.InitRootTree(runtimeTree);
                }

                runtimeTree.Bind(context);

                ApplyKeyOverrides();
            }
            else
            {
                runtimeTree = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (!runtimeTree)
            {
                return;
            }

            BehaviourTree.Traverse(runtimeTree.rootNode, n =>
            {
                if (n.drawGizmos)
                {
                    n.OnDrawGizmos();
                }
            });
        }

        private void ApplyKeyOverrides()
        {
            foreach (var pair in blackboardOverrides)
            {
                // Find the key from the new behaviour tree instance
                var targetKey = runtimeTree.blackboard.Find(pair.key.name);
                var sourceKey = pair.value;
                if (targetKey != null && sourceKey != null)
                {
                    targetKey.CopyFrom(sourceKey);
                }
            }
        }

        private Context CreateBehaviourTreeContext()
        {
            return Context.CreateFromGameObject(gameObject);
        }

        private bool ValidateTree()
        {
            if (!behaviourTree)
            {
                Debug.LogWarning($"No BehaviourTree assigned to {name}, assign a behaviour tree in the inspector");
                return false;
            }

            var isValid = true;
            if (validate)
            {
                string cyclePath;
                isValid = !IsRecursive(behaviourTree, out cyclePath);

                if (!isValid)
                {
                    Debug.LogError($"Failed to create recursive behaviour tree. Found cycle at: {cyclePath}");
                }
            }

            return isValid;
        }

        private bool IsRecursive(BehaviourTree tree, out string cycle)
        {
            // Check if any of the subtree nodes and their decendents form a circular reference, which will cause a stack overflow.
            var treeStack = new List<string>();
            var referencedTrees = new HashSet<BehaviourTree>();

            var cycleFound = false;
            var cyclePath = "";

            Action<Node> traverse = null;
            traverse = node =>
            {
                if (!cycleFound)
                {
                    if (node is SubTree subtree && subtree.treeAsset != null)
                    {
                        treeStack.Add(subtree.treeAsset.name);
                        if (referencedTrees.Contains(subtree.treeAsset))
                        {
                            var index = 0;
                            foreach (var tree in treeStack)
                            {
                                index++;
                                if (index == treeStack.Count)
                                {
                                    cyclePath += $"{tree}";
                                }
                                else
                                {
                                    cyclePath += $"{tree} -> ";
                                }
                            }

                            cycleFound = true;
                        }
                        else
                        {
                            referencedTrees.Add(subtree.treeAsset);
                            BehaviourTree.Traverse(subtree.treeAsset.rootNode, traverse);
                            referencedTrees.Remove(subtree.treeAsset);
                        }

                        treeStack.RemoveAt(treeStack.Count - 1);
                    }
                }
            };
            treeStack.Add(tree.name);

            referencedTrees.Add(tree);
            BehaviourTree.Traverse(tree.rootNode, traverse);
            referencedTrees.Remove(tree);

            treeStack.RemoveAt(treeStack.Count - 1);
            cycle = cyclePath;
            return cycleFound;
        }

        public BlackboardKey<T> FindBlackboardKey<T>(string keyName)
        {
            if (runtimeTree)
            {
                return runtimeTree.blackboard.Find<T>(keyName);
            }

            return null;
        }

        public void SetBlackboardValue<T>(string keyName, T value)
        {
            if (runtimeTree)
            {
                runtimeTree.blackboard.SetValue(keyName, value);
            }
        }

        public T GetBlackboardValue<T>(string keyName)
        {
            if (runtimeTree)
            {
                return runtimeTree.blackboard.GetValue<T>(keyName);
            }

            return default;
        }
    }
}
