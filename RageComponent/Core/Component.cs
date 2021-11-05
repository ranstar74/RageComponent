using System;

namespace RageComponent
{
    /// <summary>
    /// Component that could be attached to any entity.
    /// </summary>
    public class Component<T> : IDisposable where T : class
    {
        /// <summary>
        /// Parent of the <see cref="Component{Parent}"/>.
        /// </summary>
        public T Parent { get; set; }

        /// <summary>
        /// If Set To False, <see cref="OnTick"/> will be skipped.
        /// </summary>
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// Called when component begins to exist.
        /// </summary>
        public virtual void Start()
        {

        }

        /// <summary>
        /// Called every frame.
        /// </summary>
        public virtual void OnTick()
        {

        }

        /// <summary>
        /// Called on script abort.
        /// </summary>
        public virtual void Destroy()
        {

        }

        /// <summary>
        /// Invokes <see cref="Destroy"/>.
        /// </summary>
        public void Dispose()
        {
            Destroy();
        }
    }
}
