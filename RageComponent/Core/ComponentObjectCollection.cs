using System.Collections.Generic;

namespace RageComponent.Core
{
    /// <summary>
    /// This class manages a collection of <see cref="IComponentObject"/>.
    /// </summary>
    public class ComponentObjectCollection
    {
        private readonly List<IComponentObject> componentObjects = new List<IComponentObject>();

        /// <summary>
        /// Creates a new instance of <see cref="ComponentObjectCollection"/>.
        /// </summary>
        public ComponentObjectCollection()
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
            componentObjects.Add(componentObject);
        }

        /// <summary>
        /// Removes a <see cref="IComponentObject"/> from the collection.
        /// </summary>
        /// <param name="componentObject"><see cref="IComponentObject"/> to remove.</param>
        public void Remove(IComponentObject componentObject)
        {
            componentObjects.Add(componentObject);
        }

        /// <summary>
        /// Calls <see cref="ComponentCollection.OnStart"/> for every component of the <see cref="IComponentObject"/>.
        /// </summary>
        public void Update()
        {
            foreach (IComponentObject componentObject in componentObjects)
            {
                componentObject.GetComponents().OnUpdate();
            }
        }

        /// <summary>
        /// Calls <see cref="ComponentCollection.OnDispose"/> for every component of the <see cref="IComponentObject"/>.
        /// </summary>
        public void Dispose()
        {
            foreach (IComponentObject componentObject in componentObjects)
            {
                componentObject.GetComponents().OnDispose();
            }
        }
    }
}
