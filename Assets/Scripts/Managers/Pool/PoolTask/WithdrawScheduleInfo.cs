namespace Assets.Scripts.Managers
{
    public class WithdrawScheduleInfo
    {
        public readonly Pool pool;
        public readonly Poolable poolable;

        private int version;

        public WithdrawScheduleInfo(Poolable poolable, Pool pool)
        {
            this.poolable = poolable;
            this.pool = pool;
            IsScheduled = false;
            version = 0;
        }

        public bool IsScheduled { get; private set; }

        public static WithdrawScheduleInfo Of(Poolable poolable, Pool pool)
        {
            return new WithdrawScheduleInfo(poolable, pool);
        }

        public int Reserve()
        {
            IsScheduled = true;
            return ++version;
        }

        public void Cancel()
        {
            if (!IsScheduled)
            {
                return;
            }

            IsScheduled = false;
            version++;
        }

        public bool Validate(int prevVersion)
        {
            return IsScheduled && version == prevVersion;
        }
    }
}