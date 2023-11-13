using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPCs
{
    public class BlackSmithNPC : BaseNPC
    {
        private int curDialogIdx;

        public override void Interact(GameObject obj)
        {
            base.Interact(obj);
            curDialogIdx = 0;

        }
    }
}
