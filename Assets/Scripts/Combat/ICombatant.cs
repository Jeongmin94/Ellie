using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Combat
{
    public interface ICombatant
    {
        public void Attack(IBaseEventPayload payload);
        public void ReceiveDamage(IBaseEventPayload payload);
    }
}
