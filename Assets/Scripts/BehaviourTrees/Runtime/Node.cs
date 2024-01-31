using System;
using UnityEngine;

namespace TheKiwiCoder
{
    [Serializable]
    public abstract class Node
    {
        public enum State
        {
            Running,
            Failure,
            Success
        }

        [HideInInspector] public State state = State.Running;
        [HideInInspector] public bool started;
        [HideInInspector] public string guid = Guid.NewGuid().ToString();
        [HideInInspector] public Vector2 position;
        [HideInInspector] public Blackboard blackboard;
        [TextArea] public string description;

        [Tooltip("When enabled, the nodes OnDrawGizmos will be invoked")]
        public bool drawGizmos;

        [HideInInspector] public Context context;

        public virtual void OnInit()
        {
            // Nothing to do here
        }

        public State Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            if (state != State.Running)
            {
                OnStop();
                started = false;
            }

            return state;
        }

        public void Abort()
        {
            BehaviourTree.Traverse(this, node =>
            {
                node.started = false;
                node.state = State.Running;
                node.OnStop();
            });
        }

        public virtual void OnDrawGizmos()
        {
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();

        protected virtual void Log(string message)
        {
            Debug.Log($"[{GetType()}]{message}");
        }
    }
}
