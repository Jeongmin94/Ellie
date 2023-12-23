using Assets.Scripts.Player;

namespace Assets.Scripts.Item
{
    public interface ILootable
    {
        public void Visit(PlayerLooting player);
    }
}