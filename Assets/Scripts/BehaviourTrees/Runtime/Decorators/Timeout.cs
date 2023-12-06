using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    [System.Serializable]
    public class Timeout : DecoratorNode {
        public NodeProperty<float> timeValue;

        [Tooltip("Returns failure after this amount of time if the subtree is still running.")] public float duration = 1.0f;
        float startTime;

        protected override void OnStart() {
            if (timeValue.Value > 0.0f)
            {
                duration = timeValue.Value;
            }
            startTime = Time.time;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (child == null) {
                return State.Failure;
            }

            if (Time.time - startTime > duration) {
                return State.Failure;
            }

            return child.Update();
        }
    }
}