using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;

namespace TheKiwiCoder
{

    [System.Serializable]
    public class SubTree : ActionNode
    {
        [Tooltip("Behaviour tree asset to run as a subtree")] public BehaviourTree treeAsset;
        [HideInInspector] public BehaviourTree treeInstance;

        public NodeProperty<string> controllerNames;

        public override void OnInit()
        {
            if (treeAsset)
            {
                treeInstance = treeAsset.Clone();

                foreach (var key in blackboard.keys)
                {
                    var keyInstance = treeInstance.blackboard.Find(key.name);
                    if (keyInstance != null)
                    {
                        //treeInstance.blackboard.ReplaceKey(key.name, key);
                        keyInstance.Subscribe(key);
                        key.Subscribe(keyInstance);
                        keyInstance.CopyFrom(key);
                    }
                }

                treeInstance.Bind(context);

                if(context.btController != null)
                {
                    context.btController.RegisterBlackboardData(controllerNames.Value, treeInstance);
                }
            }
        }

        protected override void OnStart()
        {
            if (treeInstance)
            {
                treeInstance.treeState = Node.State.Running;
            }
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (treeInstance)
            {
                return treeInstance.Update();
            }
            return State.Failure;
        }
    }
}
