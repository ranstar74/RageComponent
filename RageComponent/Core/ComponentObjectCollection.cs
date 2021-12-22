using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RageComponent.Core
{
    /// <summary>
    /// Manages a collection of <see cref="T"/>.
    /// <para>
    /// Calls Update method for every component object.
    /// </para>
    /// </summary>
    public class ComponentObjectCollection<T> : IEnumerable<T> where T : IComponentObject
    {
        /// <summary>
        /// List of all component objects.
        /// </summary>
        protected readonly List<T> ComponentObjects = new List<T>();

        /// <summary>
        /// Creates a new instance of <see cref="ComponentObjectCollection{T}"/>.
        /// </summary>
        public ComponentObjectCollection()
        {
            Main.OnTick += Update;
        }

        /// <summary>
        /// Gets object at specified index.
        /// </summary>
        /// <param name="index">Index of the element.</param>
        /// <returns>An object at specified index.</returns>
        public T this[int index] => ComponentObjects[index];

        /// <summary>
        /// Adds an object to the collection.
        /// </summary>
        /// <param name="componentObject">Object to add.</param>
        public T Add(T componentObject)
        {
            if (ComponentObjects.Any(x => x.Handle == componentObject.Handle))
                throw new ArgumentException("An object with the same handle is already in the collection.");

            ComponentObjects.Add(componentObject);
            return componentObject;
        }

        /// <summary>
        /// Gets a value indicating whether collection contains an object with given handle or not.
        /// </summary>
        /// <param name="handle">Handle to check.</param>
        /// <returns>True if collection contains given object with given handle, otherwise False.</returns>
        public bool Contains(int handle)
        {
            return ComponentObjects.Any(x => x.Handle == handle);
        }

        /// <summary>
        /// Removes a component object from the collection by handle.
        /// </summary>
        /// <param name="handle">Handle of component object to remove.</param>
        public void Remove(int handle)
        {
            ComponentObjects.RemoveAll(x => x.Handle == handle);
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            ComponentObjects.Clear();
        }

        /// <summary>
        /// Disposes all the objects and clears the collection.
        /// </summary>
        public void DisposeAllAndClear()
        {
            foreach(T componentObject in this)
            {
                componentObject.Dispose();
            }
            Clear();
        }

        /// <summary>
        /// Gets a component by given handle.
        /// </summary>
        /// <param name="handle">Handle to look for.</param>
        /// <returns>Component if found, otherwise null.</returns>
        public T GetByHandle(int handle)
        {
            IEnumerable<T> components = ComponentObjects.Where(x => x.Handle == handle);

            if (components.Count() == 0)
                throw new ArgumentException($"Component Object with given handle: {handle} was not found.");

            return components.FirstOrDefault();
        }

        /// <summary>
        /// Calls <see cref="ComponentCollection.OnUpdate"/> for every component of the <see cref="T"/>.
        /// </summary>
        public void Update()
        {
            foreach (T componentObject in ComponentObjects)
            {
                componentObject.GetComponents().OnUpdate();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ComponentObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return null;
        }
    }

    /// <summary>
    /// Manages a collection of <see cref="IComponentObject"/>
    /// <para>
    /// Calls Update method for every component object.
    /// </para>
    /// </summary>
    public class ComponentObjectCollection : ComponentObjectCollection<IComponentObject>
    {
        /// <summary>
        /// Adds an object to the collection.
        /// </summary>
        /// <param name="componentObject">Object to add.</param>
        public T2 Add<T2>(T2 componentObject) where T2 : IComponentObject
        {
            if (ComponentObjects.Any(x => x.Handle == componentObject.Handle))
                throw new ArgumentException("An object with the same handle is already in the collection.");

            ComponentObjects.Add(componentObject);
            return componentObject;
        }

        /// <summary>
        /// Gets a component by given handle.
        /// </summary>
        /// <param name="handle">Handle to look for.</param>
        /// <returns>Component if found, otherwise null.</returns>
        public T2 GetByHandle<T2>(int handle) where T2 : IComponentObject
        {
            IEnumerable<IComponentObject> components = ComponentObjects.Where(x => x.Handle == handle);

            if (components.Count() == 0)
                throw new ArgumentException($"Component Object with given handle: {handle} was not found.");

            return (T2) Convert.ChangeType(components.FirstOrDefault(), typeof(T2));
        }
    }
}
