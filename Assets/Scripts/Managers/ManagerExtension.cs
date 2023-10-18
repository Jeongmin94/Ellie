using System;

namespace Assets.Scripts.Managers
{
    public static class ManagerExtension
    {
        public static void Subscribe(this Action publisher, Action subscriber)
        {
            publisher -= subscriber;
            publisher += subscriber;
        }

        public static void Subscribe<T>(this Action<T> publisher, Action<T> subscriber)
        {
            publisher -= subscriber;
            publisher += subscriber;
        }
    }
}