using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder
{
    [CreateAssetMenu]
    public class BehaviourTree : ScriptableObject
    {
        [SerializeReference] public RootNode rootNode;

        [SerializeReference] public List<Node> nodes = new();

        public Node.State treeState = Node.State.Running;

        public Blackboard blackboard = new();

        public BehaviourTree()
        {
            rootNode = new RootNode();
            nodes.Add(rootNode);
        }

        private void OnEnable()
        {
            // Validate the behaviour tree on load, removing all null children
            nodes.RemoveAll(node => node == null);
            Traverse(rootNode, node =>
            {
                if (node is CompositeNode composite)
                {
                    composite.children.RemoveAll(child => child == null);
                }
            });
        }

        public Node.State Update()
        {
            if (treeState == Node.State.Running)
            {
                treeState = rootNode.Update();
            }

            return treeState;
        }

        public static List<Node> GetChildren(Node parent)
        {
            var children = new List<Node>();

            if (parent is DecoratorNode decorator && decorator.child != null)
            {
                children.Add(decorator.child);
            }

            if (parent is RootNode rootNode && rootNode.child != null)
            {
                children.Add(rootNode.child);
            }

            if (parent is CompositeNode composite)
            {
                return composite.children;
            }

            return children;
        }

        public static void Traverse(Node node, Action<Node> visiter)
        {
            if (node != null)
            {
                visiter.Invoke(node);
                var children = GetChildren(node);
                children.ForEach(n => Traverse(n, visiter));
            }
        }

        public BehaviourTree Clone()
        {
            var tree = Instantiate(this);
            return tree;
        }

        public void Bind(Context context)
        {
            Traverse(rootNode, node =>
            {
                node.context = context;
                node.blackboard = blackboard;
                node.OnInit();
            });
        }

        #region EditorProperties

        public Vector3 viewPosition = new(600, 300);
        public Vector3 viewScale = Vector3.one;

        #endregion
    }
}
