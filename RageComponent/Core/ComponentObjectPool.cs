using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RageComponent.Core
{
    /// <summary>
    /// This class manages a collection of <see cref="IComponentObject"/>.
    /// </summary>
    public class ComponentObjectPool : IList<IComponentObject>
    {
        private static int handleCounter = 1;
        private readonly Dictionary<long, IComponentObject> componentObjects = new Dictionary<long, IComponentObject>();

        /// <summary>
        /// Gets number of elements in the <see cref="ComponentObjectPool"/>.
        /// </summary>
        public int Count => componentObjects.Count;

        /// <summary>
        /// Gets whether the <see cref="ComponentObjectPool"/> is read only or not.
        /// </summary>
        /// <remarks>
        /// Hardcoded to false.
        /// </remarks>
        public bool IsReadOnly => false;

        /// <summary>
        /// Creates a new instance of <see cref="ComponentObjectPool"/>.
        /// </summary>
        public ComponentObjectPool()
        {
            Main.OnTick += Update;
            Main.OnAbort += Dispose;
        }

        /// <summary>
        /// Gets element at specific index.
        /// </summary>
        /// <remarks>
        /// Set does nothing.
        /// </remarks>
        /// <param name="index">Index of the element.</param>
        /// <returns>Element at the specified index.</returns>
        public IComponentObject this[int index] { get => componentObjects.ElementAt(index).Value; set { } }

        /// <summary>
        /// Adds a <see cref="IComponentObject"/> to the collection.
        /// </summary>
        /// <param name="componentObject"><see cref="IComponentObject"/> to add.</param>
        public void Add(IComponentObject componentObject)
        {
            componentObjects.Add(handleCounter, componentObject);

            componentObject.SetComponentHandle(handleCounter);

            // Enable components after setting handle
            componentObject.GetComponents().IsEnabled = true;

            handleCounter++;
        }

        /// <summary>
        /// Gets a component by given handle.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <param name="handle">Handle to look for.</param>
        /// <returns>Component if found, otherwise null.</returns>
        public T GetByHandle<T>(int handle) where T : IComponentObject
        {
            return (T) componentObjects[handle];
        }

        /// <summary>
        /// Removes a <see cref="IComponentObject"/> from the collection by its handle and invalidates its handle.
        /// </summary>
        /// <param name="componentObjectHandle">Handle of <see cref="IComponentObject"/> to remove.</param>
        public void Remove(int componentObjectHandle)
        {
            IComponentObject componentObject = componentObjects[componentObjectHandle];
            componentObject.InvalidateHandle();

            componentObjects.Remove(componentObjectHandle);
        }

        /// <summary>
        /// Removes a <see cref="IComponentObject"/> from the collection and invalidates its handle.
        /// </summary>
        /// <param name="componentObject"><see cref="IComponentObject"/> to remove.</param>
        public void Remove(IComponentObject componentObject)
        {
            if (componentObject == null)
                return;

            componentObject.InvalidateHandle();

            Remove(componentObject.ComponentHandle);
        }

        /// <summary>
        /// Calls <see cref="ComponentCollection.OnStart"/> for every component of the <see cref="IComponentObject"/>.
        /// </summary>
        public void Update()
        {
            foreach (IComponentObject componentObject in componentObjects.Values)
            {
                componentObject.GetComponents().OnUpdate();
            }
        }

        /// <summary>
        /// Calls Dispose for every <see cref="IComponentObject"/>.
        /// <para>
        /// Invalidates handle.
        /// </para>
        /// </summary>
        public void Dispose()
        {
            foreach (IComponentObject componentObject in componentObjects.Values)
            {
                componentObject.InvalidateHandle();
                componentObject.Dispose();
            }
            componentObjects.Clear();
        }

        /// <summary>
        /// In constract from <see cref="Dispose"/>, disposes only components.
        /// </summary>
        public void DisposeComponents()
        {
            foreach (IComponentObject componentObject in componentObjects.Values)
            {
                componentObject.InvalidateHandle();
                componentObject.GetComponents().OnDispose();
            }
            componentObjects.Clear();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerator<IComponentObject> GetEnumerator()
        {
            return componentObjects.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return componentObjects.Values.GetEnumerator();
        }

        /// <summary>
        /// Determines index of item in the <see cref="ComponentObjectPool"/>.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>Index of item.</returns>
        public int IndexOf(IComponentObject item)
        {
            return componentObjects.Values
                .ToList()
                .IndexOf(item);
        }

        /// <summary>
        /// Removes item at index.
        /// </summary>
        /// <param name="index">Index of item needs to be removed.</param>
        public void RemoveAt(int index)
        {
            IComponentObject item = componentObjects[index];
            item.InvalidateHandle();
            componentObjects.Remove(index);
        }

        /// <summary>
        /// Determines whether the <see cref="ComponentObjectPool"/> contains item.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True if <see cref="ComponentObjectPool"/> contains item, otherwise False.</returns>
        public bool Contains(IComponentObject item)
        {
            return componentObjects.Values
                .ToList()
                .Contains(item);
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        public void Insert(int index, IComponentObject item)
        {
            throw new System.NotSupportedException();
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        public void Clear()
        {
            foreach (IComponentObject componentObject in componentObjects.Values)
            {
                componentObject.InvalidateHandle();
            }
            componentObjects.Clear();
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        public void CopyTo(IComponentObject[] array, int arrayIndex)
        {
            throw new System.NotSupportedException();
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        bool ICollection<IComponentObject>.Remove(IComponentObject item)
        {
            throw new System.NotSupportedException();
        }
    }
}
