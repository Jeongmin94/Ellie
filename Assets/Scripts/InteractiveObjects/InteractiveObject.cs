using Channels.UI;
using Outline;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    public abstract class InteractiveObject: MonoBehaviour
    {
        public abstract void Interact(GameObject obj);
        public abstract InteractiveType GetInteractiveType();
        public abstract OutlineType GetOutlineType();
        public abstract Renderer GetRenderer();
    }
}
