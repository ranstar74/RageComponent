using System.Collections;
using System.Collections.Generic;

namespace RageComponent.Core
{
    /// <summary>
    /// This class manages a collection of <see cref="IComponentObject"/>.
    /// </summary>
    public class ComponentObjectPool : IEnumerable<IComponentObject>
    {
        private static int handleCounter = int.MinValue;
        private readonly Dictionary<long, IComponentObject> componentObjects = new Dictionary<long, IComponentObject>();

        /// <summary>
        /// Creates a new instance of <see cref="ComponentObjectPool"/>.
        /// </summary>
        public ComponentObjectPool()
        {
            Main.OnTick += Update;
            Main.OnAbort += Dispose;
        }

        /// <summary>
        /// Adds a <see cref="IComponentObject"/> to the collection.
        /// </summary>
        /// <param name="componentObject"><see cref="IComponentObject"/> to add.</param>
        public void Add(IComponentObject componentObject)
        {
            componentObjects.Add(handleCounter, componentObject);

            componentObject.SetComponentHandle(handleCounter++);
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
        /// Removes a <see cref="IComponentObject"/> from the collection by its handle.
        /// </summary>
        /// <param name="componentObjectHandle">Handle of <see cref="IComponentObject"/> to remove.</param>
        public void Remove(int componentObjectHandle)
        {
            componentObjects.Remove(componentObjectHandle);
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
        /// Calls <see cref="ComponentCollection.OnDispose"/> for every component of the <see cref="IComponentObject"/>.
        /// </summary>
        public void Dispose()
        {
            foreach (IComponentObject componentObject in componentObjects.Values)
            {
                componentObject.GetComponents().OnDispose();
            }
        }

        public IEnumerator<IComponentObject> GetEnumerator()
        {
            return componentObjects.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return componentObjects.Values.GetEnumerator();
        }
    }
}
