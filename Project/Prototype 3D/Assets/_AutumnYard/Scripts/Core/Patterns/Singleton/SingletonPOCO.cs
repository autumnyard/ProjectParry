﻿
namespace AutumnYard.Core
{
    public abstract class SingletonPOCO<T> where T : new()
    {
        protected SingletonPOCO() { }

        private static T _instance;

        // We now have a lock object that will be used to synchronize threads
        // during first access to the Singleton.
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                // This conditional is needed to prevent threads stumbling over the
                // lock once the instance is ready.
                if (_instance == null)
                {
                    // Now, imagine that the program has just been launched. Since
                    // there's no Singleton instance yet, multiple threads can
                    // simultaneously pass the previous conditional and reach this
                    // point almost at the same time. The first of them will acquire
                    // lock and will proceed further, while the rest will wait here.
                    lock (_lock)
                    {
                        // The first thread to acquire the lock, reaches this
                        // conditional, goes inside and creates the Singleton
                        // instance. Once it leaves the lock block, a thread that
                        // might have been waiting for the lock release may then
                        // enter this section. But since the Singleton field is
                        // already initialized, the thread won't create a new
                        // object.
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}