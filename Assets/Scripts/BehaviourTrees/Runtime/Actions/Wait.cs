using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    [System.Serializable]
    public class Wait : ActionNode {
        public NodeProperty<float> timeValue;

        [Tooltip("Amount of time to wait before returning success")] public float duration = 1;
        float startTime;

        protected override void OnStart() {
            if(timeValue.Value > 0.0f)
            {
                duration = timeValue.Value;
            }
            startTime = Time.time;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            
            float timeRemaining = Time.time - startTime;
            if (timeRemaining > duration) {
                return State.Success;
            }
            return State.Running;
        }
    }
}
