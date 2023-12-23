using Assets.Scripts.ActionData;

namespace Assets.Scripts.UI.Monster
{
    public class MonsterDataContainer
    {
        public readonly Data<int> CurrentHp = new();
        public int PrevHp { get; set; }
        public int MaxHp { get; set; }

        public string Name { get; set; }
    }
}