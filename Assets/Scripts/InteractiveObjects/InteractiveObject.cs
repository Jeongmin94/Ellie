using Channels.UI;
using Outline;
using UnityEngine;

// !TODO: 상호작용 오브젝트, 돌멩이에 아웃라인 추가해주기
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
