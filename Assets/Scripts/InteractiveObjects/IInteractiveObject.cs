using Channels.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    public interface IInteractiveObject
    {
        public void Interact(GameObject obj);
        public InteractiveType GetInteractiveType();
    }
}
