using Channels.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// !TODO: 상호작용 오브젝트, 돌멩이에 아웃라인 추가해주기
namespace Assets.Scripts.InteractiveObjects
{
    public interface IInteractiveObject
    {
        public void Interact(GameObject obj);
        public InteractiveType GetInteractiveType();
    }
}
