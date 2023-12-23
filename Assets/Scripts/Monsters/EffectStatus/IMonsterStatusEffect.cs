namespace Assets.Scripts.Monsters.EffectStatus
{
    public interface IMonsterStatusEffect
    {
        public void ApplyStatusEffect(MonsterEffectStatusController controller, MonsterStatusEffectInfo info);
    }
}