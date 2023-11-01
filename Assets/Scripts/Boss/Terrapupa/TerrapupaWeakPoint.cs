using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Boss.Terrapupa
{
    public class TerrapupaWeakPoint : MonoBehaviour
    {
        public Action collisionAction;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Stone"))
            {
                collisionAction?.Invoke();
            }
        }
    }
}