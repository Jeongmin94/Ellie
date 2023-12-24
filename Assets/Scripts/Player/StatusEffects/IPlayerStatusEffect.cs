namespace Player.StatusEffects
{
    public interface IPlayerStatusEffect
    {
        public void InitStatusEffect();
        public void ApplyStatusEffect(PlayerStatusEffectController controller, StatusEffectInfo info);
    }
}