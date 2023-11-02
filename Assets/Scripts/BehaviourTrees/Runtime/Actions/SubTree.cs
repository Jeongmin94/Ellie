using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;

namespace TheKiwiCoder {

    [System.Serializable]
    public class SubTree : ActionNode {
        
        [Tooltip("Behaviour tree asset to run as a subtree")] public BehaviourTree treeAsset;
        [HideInInspector] public BehaviourTree treeInstance;

        //public List<NodeProperty> blackboardKeys;
        //public NodeProperty<bool> isCopyRootTreeBlackboard;

        public List<NodeProperty<string>> controllerNames;

        public override void OnInit() {
            if (treeAsset) {
                treeInstance = treeAsset.Clone();

                //if(isCopyRootTreeBlackboard.Value)
                //{
                //    foreach (var key in blackboard.keys)
                //    {
                //        var keyInstance = treeInstance.blackboard.Find(key.name);
                //        if (keyInstance != null)
                //        {
                //            treeInstance.blackboard.ReplaceKey(key.name, key);
                //        }
                //    }
                //}
                //else
                //{
                //    foreach (var key in blackboardKeys)
                //    {
                //        var keyInstance = treeInstance.blackboard.Find(key.reference.name);
                //        if (keyInstance != null)
                //        {
                //            treeInstance.blackboard.ReplaceKey(key.reference.name, key.reference);
                //        }
                //    }
                //}

                treeInstance.Bind(context);

                foreach (var key in controllerNames)
                {
                    context.btController.RegisterBlackboardData(key.Value, treeInstance);
                }
            }
        }

        protected override void OnStart() {
            if (treeInstance) {
                treeInstance.treeState = Node.State.Running;
            }
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (treeInstance) {
                return treeInstance.Update();
            }
            return State.Failure;
        }
    }
}
