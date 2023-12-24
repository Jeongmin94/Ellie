using Player;

namespace Item
{
    public interface ILootable
    {
        public void Visit(PlayerLooting player);
    }
}