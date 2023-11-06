using Assets.Scripts.InteractiveObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public class BaseNPC : MonoBehaviour, IInteractiveObject
    {
        List<string>[] dialogList;
        public void Interact(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        protected virtual void Init()
        {
            
        }
        // !TODO

    }
}