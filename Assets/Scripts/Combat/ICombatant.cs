namespace Combat
{
    public interface ICombatant
    {
        public void Attack(IBaseEventPayload payload);
        public void ReceiveDamage(IBaseEventPayload payload);
    }
}