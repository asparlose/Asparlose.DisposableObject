using System;
using System.Threading;

namespace Asparlose
{
    public abstract class DisposableObject : IDisposable
    {
        protected event EventHandler Disposed;
        protected bool IsDisposed { get; }

        int disposed;

        protected virtual void ReleaseUnmanagedResources() { }
        protected virtual void ReleaseManagedResources() { }

        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref disposed, 1) == 0)
            {
                if (disposing)
                    ReleaseManagedResources();

                ReleaseUnmanagedResources();

                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }

        ~DisposableObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
