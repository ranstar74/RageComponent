using FusionLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RageComponent
{
    /// <summary>
    /// Component functions.
    /// </summary>
    public class Components<T>
    {
        /// <summary>
        /// All created components.
        /// </summary>
        private readonly static List<Component<T>> AllComponents = new List<Component<T>>();

        /// <summary>
        /// Registers all components of given object.
        /// </summary>
        public void RegisterComponents(Object obj)
        {
            Utils.ProcessAllClassFieldsByType<Component<T>>(obj, component =>
            {
                var componentValue = (Component<T>)Activator.CreateInstance(typeof(Component<T>));
                component.SetValue(obj, componentValue);

                AllComponents.Add(componentValue);
            });

            OnInit();
        }

        /// <summary>
        /// Call it on script abort.
        /// </summary>
        public void OnAbort()
        {
            for (int i = 0; i < AllComponents.Count; i++)
            {
                AllComponents[i].Destroy();
            }
        }

        /// <summary>
        /// Call it on first tick.
        /// </summary>
        public void OnInit()
        {
            for(int i = 0; i < AllComponents.Count; i++)
            {
                AllComponents[i].Start();
            }
        }

        /// <summary>
        /// Call it every frame.
        /// </summary>
        public void OnTick()
        {
            // Process all components
            for(int i = 0; i < AllComponents.Count; i++)
            {
                var component = AllComponents[i];

                // Destroy component if entity no longer exists
                if (component.Entity.NotNullAndExists())
                {
                    component.Destroy();
                    return;
                }

                component.OnTick();
            }
        }
    }
}
