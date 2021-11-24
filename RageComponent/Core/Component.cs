using GTA;
using RageComponent.Core;
using System;

namespace RageComponent
{
    /// <summary>
    /// Defines a component that does some specific functionality.
    /// </summary>
    public abstract class Component : IDisposable
    {
        /// <summary>
        /// Component collection the <see cref="Component"/> created in.
        /// </summary>
        public ComponentCollection Components => components;

        /// <summary>
        /// Shortcut for to <see cref="Game.Player.Character"/>.
        /// </summary>
        public Ped GPlayer => Game.Player.Character;

        /// <summary>
        /// If Set To False, <see cref="Update"/> will be skipped.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Defines how often update is called.
        /// </summary>
        /// <remarks>
        /// -1 is every tick, 250 is every 250ms
        /// </remarks>
        public int UpdateTime { get; set; } = -1;

        internal int NextUpdateTime = 0;
        private readonly ComponentCollection components;

        /// <summary>
        /// Creates a new instance of <see cref="Component"/>.
        /// </summary>
        /// <param name="components">A collection of components the <see cref="Component"/> created in.</param>
        public Component(ComponentCollection components)
        {
            this.components = components;
        }

        /// <summary>
        /// Gets component parent.
        /// </summary>
        /// <returns>A parent of the <see cref="IComponentObject"/></returns>
        public T GetParent<T>() where T : class
        {
            return (T)components.GameObject;
        }

        /// <summary>
        /// Called when component begins to exist.
        /// </summary>
        public virtual void Start()
        {

        }

        /// <summary>
        /// Called every frame.
        /// </summary>
        public virtual void Update()
        {
            
        }

        /// <summary>
        /// Called on script abort.
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
