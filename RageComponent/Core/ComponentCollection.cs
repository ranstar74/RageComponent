using System;
using System.Collections.Generic;
using System.Linq;

namespace RageComponent.Core
{
    /// <summary>
    /// Defines a collection of <see cref="Component"/>'s.
    /// </summary>
    public abstract class ComponentCollection
    {
        /// <summary>
        /// Class the <see cref="ComponentCollection"/> is attached to.
        /// Also could be called 'Parent Class'.
        /// </summary>
        /// <remarks>
        /// Made as workaround of Unity's GameObject.
        /// </remarks>  
        public object GameObject => gameObject;

        private readonly List<Component> components = new List<Component>();
        private readonly object gameObject;

        /// <summary>
        /// Creates a new instance of <see cref="ComponentCollection"/>.
        /// </summary>
        /// <param name="gameObject">
        /// Parent class of the <see cref="ComponentCollection"/>.
        /// In most cases just pass 'this'.
        /// </param>
        public ComponentCollection(object gameObject)
        {
            this.gameObject = gameObject;
        }

        /// <summary>
        /// Tries to get the specified component.
        /// </summary>
        /// <typeparam name="T">Type of a component of given type.</typeparam>
        /// <param name="component">When this method returns, 
        /// if method returned true, contains a instance of found component. 
        /// If method returned true, contains null.</param>
        /// <returns>True if component was found, otherwise False.</returns>
        public bool TryGetComponent<T>(out T component) where T : Component
        {
            var tComponents = components.OfType<T>();

            component = tComponents.FirstOrDefault();
            return tComponents.Count() > 0;
        }

        /// <summary>
        /// Gets the first component that is found. 
        /// <para>
        /// If you expect there to be more than one component of the same type, use <see cref="GetComponents{T}"/> instead.
        /// </para>
        /// </summary>
        /// <typeparam name="T">Type of a component of given type.</typeparam>
        /// <returns>The component of given type if the game object has one attached, null if it doesn't.</returns>
        public T GetComponent<T>() where T : Component
        {
            var tComponents = components.OfType<T>();

            if (tComponents.Count() == 0)
                return null;
            return tComponents.FirstOrDefault();
        }

        /// <summary>
        /// Gets all components of given type.
        /// </summary>
        /// <typeparam name="T">Type of the required component.</typeparam>
        /// <returns>A <see cref="List{T}"/> of components of given type.</returns>
        public List<T> GetComponents<T>() where T : Component
        {
            return components
                .OfType<T>()
                .ToList();
        }

        /// <summary>
        /// Creates and registers a component of given type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="Component"/> needs to be created.</typeparam>
        /// <returns>A new instance of the <see cref="Component"/></returns>
        public T Create<T>() where T : Component
        {
            Component component = (T)Activator.CreateInstance(typeof(T), this);

            components.Add(component);

            return (T)component;
        }

        /// <summary>
        /// Calls <see cref="Component.Start"/> for every component.
        /// </summary>
        /// <remarks>
        /// Must be called manually after creating after components in constructor.
        /// </remarks>
        public void OnStart()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Start();
            }
        }

        /// <summary>
        /// Calls <see cref="Component.Update"/> for every component.
        /// </summary>
        public void OnUpdate()
        {
            for(int i = 0; i < components.Count; i++)
            {
                var component = components[i];

                if(component.IsEnabled)
                    component.Update();
            }
        }

        /// <summary>
        /// Calls <see cref="Component.Dispose"/> for every component.
        /// </summary>
        public void OnDispose()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Dispose();
            }
        }
    }
}
