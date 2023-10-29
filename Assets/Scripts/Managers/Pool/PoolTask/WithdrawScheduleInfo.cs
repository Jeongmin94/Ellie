namespace Assets.Scripts.Managers
{
    public class WithdrawScheduleInfo
    {
        public readonly Poolable poolable;
        public readonly Pool pool;
        public bool IsScheduled => isScheduled;

        private bool isScheduled;
        private int version;

        public WithdrawScheduleInfo(Poolable poolable, Pool pool)
        {
            this.poolable = poolable;
            this.pool = pool;
            isScheduled = false;
            version = 0;
        }

        public static WithdrawScheduleInfo Of(Poolable poolable, Pool pool)
        {
            return new WithdrawScheduleInfo(poolable, pool);
        }

        public int Reserve()
        {
            isScheduled = true;
            return ++version;
        }

        public void Cancel()
        {
            if (!isScheduled) return;
            isScheduled = false;
            version++;
        }

        public bool Validate(int prevVersion)
        {
            return isScheduled && (version == prevVersion);
        }
    }
}