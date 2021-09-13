using GTA;

namespace RageComponent
{
    /// <summary>
    /// Component that could be attached to any entity.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// <see cref="GTA.Entity"/> this <see cref="Component"/> belongs to.
        /// </summary>
        public abstract Entity Entity { get; set; }

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
        /// Called when entity is destroyed.
        /// </summary>
        public virtual void Destroy()
        {

        }
    }
}
