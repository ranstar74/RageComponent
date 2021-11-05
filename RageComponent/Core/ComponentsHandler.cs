using System;
using System.Collections.Generic;

namespace RageComponent
{
    /// <summary>
    /// Handles multiple components.
    /// </summary>
    public class ComponentsHandler<T> : IDisposable where T : class
    {
        /// <summary>
        /// Temporary buffer of components, <see cref="Lock"/> uses it to create array from it.
        /// </summary>
        private readonly List<Component<T>> componentsBuffer = new List<Component<T>>();

        /// <summary>
        /// All <see cref="Component{T}"/>'s of the <see cref="ComponentsHandler{T}"/>.
        /// </summary>
        private Component<T>[] components;

        /// <summary>
        /// Whether the <see cref="ComponentsHandler{T}"/> is locked or not.
        /// <para>
        /// Locked <see cref="ComponentsHandler{T}"/> can't append new <see cref="Component{T}"/>. 
        /// </para>
        /// </summary>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// Whether handler is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ComponentsHandler{T}"/>.
        /// </summary>
        public ComponentsHandler()
        {
            // Subscribe on SHVDN Events
            Main.OnTick += OnTick;
            Main.OnAbort += OnAbort;
        }

        /// <summary>
        /// Locks and initializes the <see cref="ComponentsHandler{T}"/>.
        /// <para>
        /// After locking new component can't be added.
        /// </para>
        /// <para>
        /// Must be called after adding all components.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Invokes <see cref="Component{T}.Start"/> for every <see cref="Component{T}"/>.
        /// </remarks>
        public void Lock()
        {
            if (IsLocked)
                return;

            /*
                * Why is this made? Based on performance tests,
                * for iteration of array is 2x faster than for iteration of list,
                * And since there's no need to add new components on runtime, 
                *   we can convert list to array which will increase performance.
            */
            components = componentsBuffer.ToArray();
            componentsBuffer.Clear();

            OnInit();

            IsLocked = true;
        }

        /// <summary>
        /// Creates new <see cref="Comparer{T}"/>.
        /// </summary>
        /// <remarks>
        /// Throws <see cref="SystemException"/> if called after Locking.
        /// </remarks>
        /// <returns>A new instance of <see cref="Comparer{T}"/></returns>
        public void Create<Component>(ref Component component) where Component : Component<T>
        {
            if (IsLocked)
                throw new SystemException("Locked component can't create new component's.");

            component = (Component) Activator.CreateInstance(typeof(Component)); //new Component<T>();
            componentsBuffer.Add(component);
        }

        /// <summary>
        /// Calls <see cref="OnAbort"/> for every <see cref="Component{T}"/>.
        /// </summary>
        private void OnAbort()
        {
            for (int i = 0; i < components.Length; i++)
            {
                components[i].Destroy();
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Calls <see cref="OnInit"/> for every <see cref="Component{T}"/>.
        /// </summary>
        private void OnInit()
        {
            for (int i = 0; i < components.Length; i++)
            {
                components[i].Start();
            }
        }

        /// <summary>
        /// Calls <see cref="OnTick"/> for every <see cref="Component{T}"/>.
        /// </summary>
        private void OnTick()
        {
            if (!IsLocked)
                return;

            if (IsDisposed)
                return;

            // Process all components
            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if(component.IsEnabled)
                    component.OnTick();
            }
        }

        /// <summary>
        /// Disposes all <see cref="Component{T}"/> of the <see cref="ComponentsHandler{T}"/>.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            for(int i = 0; i < components.Length; i++)
            {
                components[i].Dispose();
            }

            // Unsubscribe from SHVDN Events
            Main.OnTick -= OnTick;
            Main.OnAbort -= OnAbort;
        }
    }
}
