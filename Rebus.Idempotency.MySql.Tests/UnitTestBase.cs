﻿using System;
using System.Collections.Concurrent;
using Rebus.Time;

namespace Rebus.Idempotency.MySql.Tests
{
    public abstract class UnitTestBase : IDisposable
    {
        private readonly ConcurrentStack<IDisposable> _disposables = new ConcurrentStack<IDisposable>();

        public UnitTestBase()
        {
            RebusTimeMachine.Reset();
            _disposables.Clear();
        }

        protected TDisposable Using<TDisposable>(TDisposable disposable) where TDisposable : IDisposable
        {
            _disposables.Push((IDisposable)disposable);
            return disposable;
        }

        protected void CleanUpDisposables()
        {
            IDisposable result;
            while (_disposables.TryPop(out result))
            {
                Console.WriteLine(string.Format("Disposing {0}", result));
                result.Dispose();
            }
        }

        public void Dispose()
        {
            CleanUpDisposables();
        }
    }
}
