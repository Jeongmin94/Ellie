using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public class PoolTask
    {
        private readonly Dictionary<Poolable, WithdrawScheduleInfo> scheduleInfos = new Dictionary<Poolable, WithdrawScheduleInfo>();

        private readonly ConcurrentQueue<WithdrawScheduleInfo> scheduleQueue = new ConcurrentQueue<WithdrawScheduleInfo>();

        public Action<WithdrawScheduleInfo> schedulePushAction;

        public WithdrawScheduleInfo GetInfo(Poolable poolable)
        {
            if (scheduleInfos.TryGetValue(poolable, out var info))
                return info;

            return null;
        }

        public void SetInfo(Poolable poolable, WithdrawScheduleInfo info)
        {
            scheduleInfos.TryAdd(poolable, info);
        }

        public void SetInfo(WithdrawScheduleInfo info)
        {
            SetInfo(info.poolable, info);
        }

        public void EnqueueInfo(WithdrawScheduleInfo info)
        {
            scheduleQueue.Enqueue(info);
        }

        public void Handle()
        {
            if (scheduleQueue.IsEmpty)
                return;

            while (scheduleQueue.TryDequeue(out var scheduleInfo))
            {
                if (!scheduleInfo.IsScheduled)
                    continue;
                
                schedulePushAction?.Invoke(scheduleInfo);
            }
        }
    }
}