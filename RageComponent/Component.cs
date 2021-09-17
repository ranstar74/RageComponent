using GTA;

namespace RageComponent
{
    /// <summary>
    /// Component that could be attached to any entity.
    /// </summary>
    public class Component<T>
    {
        /// <summary>
        /// <see cref="GTA.Entity"/> this Component belongs to.
        /// </summary>
        public virtual Entity Entity { get; set; }

        /// <summary>
        /// Class you can access from component.
        /// </summary>
        public T Base { get; set; }

        /// <summary>
        /// If True - OnTick function will be called, otherwise False.
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
        /// Called when entity is destroyed or on script abort.
        /// </summary>
        public virtual void Destroy()
        {

        }
    }
}
