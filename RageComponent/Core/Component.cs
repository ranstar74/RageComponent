using GTA;
using RageComponent.Core;
using System;

namespace RageComponent
{
    /// <summary>Defines a component that does some specific functionality.</summary>
    public abstract class Component
    {
        /// <summary>Component collection the <see cref="Component"/> created in.</summary>
        public ComponentCollection Components => components;

        /// <summary>Shortcut for to game player.</summary>
        public Ped GPlayer => Game.Player.Character;

        /// <summary>If Set To False, <see cref="Update"/> will be skipped.</summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>Defines a time interval of calling <see cref="Update"/> in ms.</summary>
        /// <remarks>Set -1 to call update every tick.</remarks>
        public int UpdateTime { get; set; } = -1;

        internal int NextUpdateTime = 0;
        internal int YieldTicks = 0;
        private readonly ComponentCollection components;

        /// <summary>
        /// Creates a new instance of <see cref="Component"/>.
        /// </summary>
        /// <param name="components">A collection of components the <see cref="Component"/> created in.</param>
        public Component(ComponentCollection components)
        {
            this.components = components;
        }

        /// <summary>Skips execution of <see cref="Update"/> for given amount of frames.</summary>
        /// <param name="ticks">A natural number of frames to skip.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Yield(int ticks = 1)
        {
            if(ticks <= 0)
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(ticks), 
                    message: "Amount of ticks to yield must be a natural number.");

            YieldTicks = ticks;
        }

        /// <summary>Gets component parent.</summary>
        /// <returns>A parent of the <see cref="IComponentObject"/></returns>
        protected T GetParent<T>() where T : class
        {
            return (T)components.GameObject;
        }

        /// <summary>Called when component begins to exist.</summary>
        public virtual void Start()
        {

        }

        /// <summary>Called before <see cref="Update"/>.</summary>
        public virtual void EarlyUpdate()
        {

        }

        /// <summary>Called every frame.</summary>
        public virtual void Update()
        {
            
        }

        /// <summary>Called after <see cref="Update"/>.</summary>
        public virtual void LateUpdate()
        {

        }

        /// <summary>Called when <see cref="IComponentObject"/> being disposed.
        /// <para>Dispose component related objects here, for static use <see cref="Reload"/>.</para>
        /// </summary>
        public virtual void Dispose()
        {

        }

        /// <summary>Called on script reload.
        /// <para>Useful for disposing static objects.</para></summary>
        public virtual void Reload()
        {

        }
    }
}
