using FusionLibrary.Extensions;
using GTA;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RageComponent
{
    /// <summary>
    /// Handles components.
    /// </summary>
    public class ComponentsHandler<T>
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
            Utils.ProcessAllClassFieldsByBaseType<Component<T>>(obj, component =>
            {
                var componentValue = (dynamic) Activator.CreateInstance(component.FieldType);

                // Check if entity attribute is defined and if not look for property value in class and assign it
                var entityAttribute = component.GetCustomAttribute(typeof(EntityAttribute)) as EntityAttribute;
                if (entityAttribute != null)
                    componentValue.Entity = (Entity)Utils.GetClassPropertyValueByName(obj, entityAttribute.EntityProperty);

                componentValue.Base = (T)obj;
                component.SetValue(obj, componentValue);

                AllComponents.Add(componentValue);
            });

            OnInit();
        }

        /// <summary>
        /// Constructs new instance of <see cref="ComponentsHandler{T}"/>.
        /// </summary>
        private ComponentsHandler()
        {

        }

        /// <summary>
        /// Registers new components handler.
        /// </summary>
        /// <returns>New instance of <see cref="Component"/></returns>
        public static ComponentsHandler<T> RegisterComponentHandler()
        {
            var componentsHandler = new ComponentsHandler<T>();
            Main.OnTick += componentsHandler.OnTick;

            return componentsHandler;
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
