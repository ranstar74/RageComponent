using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RageComponent.Core
{
    /// <summary>
    /// Manages a collection of <see cref="IComponentObject"/>.
    /// <para>
    /// Calls Update method for every component object.
    /// </para>
    /// </summary>
    public class ComponentObjectCollection : IEnumerable<IComponentObject>
    {
        private readonly List<IComponentObject> _componentObjects = new List<IComponentObject>();

        /// <summary>
        /// Creates a new instance of <see cref="ComponentObjectCollection"/>.
        /// </summary>
        public ComponentObjectCollection()
        {
            Main.OnTick += Update;
        }

        /// <summary>
        /// Adds a <see cref="IComponentObject"/> to the collection.
        /// </summary>
        /// <param name="componentObject"><see cref="IComponentObject"/> to add.</param>
        public T Add<T>(T componentObject) where T: IComponentObject
        {
            if (_componentObjects.Any(x => x.Handle == componentObject.Handle))
                throw new ArgumentException("An object with the same handle is already in the collection.");

            _componentObjects.Add(componentObject);
            return componentObject;
        }

        /// <summary>
        /// Gets a value indicating whether collection contains an object with given handle or not.
        /// </summary>
        /// <param name="handle">Handle to check.</param>
        /// <returns>True if collection contains given object with given handle, otherwise False.</returns>
        public bool Contains(int handle)
        {
            return _componentObjects.Any(x => x.Handle == handle);
        }

        /// <summary>
        /// Removes a component object from the collection by handle.
        /// </summary>
        /// <param name="handle">Handle of component object to remove.</param>
        public void Remove(int handle)
        {
            _componentObjects.RemoveAll(x => x.Handle == handle);
        }

        /// <summary>
        /// Gets a component by given handle.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <param name="handle">Handle to look for.</param>
        /// <returns>Component if found, otherwise null.</returns>
        public T GetByHandle<T>(int handle) where T : IComponentObject
        {
            IEnumerable<IComponentObject> components = _componentObjects.Where(x => x.Handle == handle);

            if (components.Count() == 0)
                throw new ArgumentException($"Component Object with given handle: {handle} was not found.");

            return (T)components.FirstOrDefault();
        }

        /// <summary>
        /// Calls <see cref="ComponentCollection.OnUpdate"/> for every component of the <see cref="IComponentObject"/>.
        /// </summary>
        public void Update()
        {
            foreach (IComponentObject componentObject in _componentObjects)
            {
                componentObject.GetComponents().OnUpdate();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<IComponentObject> GetEnumerator()
        {
            return _componentObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return null;
        }
    }
}
