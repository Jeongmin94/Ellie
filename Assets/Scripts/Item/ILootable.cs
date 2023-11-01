using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Item
{
    public interface ILootable
    {
        public void Visit(PlayerLooting player);
    }
}
