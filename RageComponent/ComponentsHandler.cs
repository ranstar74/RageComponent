using FusionLibrary;
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
        private readonly List<Component<T>> AllComponents = new List<Component<T>>();

        /// <summary>
        /// All props of components that belongs to this handler.
        /// </summary>
        private readonly List<AnimateProp> AllHandlerComponentProps = new List<AnimateProp>();

        private bool _initialized = false;

        /// <summary>
        /// Whether handler is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

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
                {
                    var entity = (Entity)Utils.GetClassFieldValueByName(obj, entityAttribute.EntityProperty);

                    if(entity == null)
                        throw new NullReferenceException(
                            $"Entity {entityAttribute.EntityProperty} of {nameof(component)} was not found.");

                    componentValue.Entity = entity;
                }

                componentValue.Base = (T)obj;
                component.SetValue(obj, componentValue);

                AllComponents.Add(componentValue);
            });

            OnInit();

            // Add all props to prop component
            for (int i = 0; i < AllComponents.Count; i++)
            {
                var component = AllComponents[i];

                // AnimateProp
                AllHandlerComponentProps.AddRange(Utils.GetAllFieldValues<AnimateProp>(component));

                // AnimatePropsHandler
                var animatePropHandlers = Utils.GetAllFieldValues<AnimatePropsHandler>(component);
                for (int k = 0; k < animatePropHandlers.Count; k++)
                {
                    var handler = animatePropHandlers[k];
                    AllHandlerComponentProps.AddRange(handler.Props);
                }

                // List<AnimateProp>
                var animatePropList = Utils.GetAllFieldValues<List<AnimateProp>>(component);
                for (int k = 0; k < animatePropList.Count; k++)
                {
                    var propList = animatePropList[i];
                    AllHandlerComponentProps.AddRange(propList);
                }
            }

            for (int i = 0; i < AllHandlerComponentProps.Count; i++)
            {
                var prop = AllHandlerComponentProps[i];

                // Sometimes prop could be spawned in constructor so
                // we don't want to spawn duplicate
                if (!prop.IsSpawned)
                    prop.SpawnProp();
            }

            for (int i = 0; i < AllComponents.Count; i++)
            {
                var component = AllComponents[i];
                component.IsEnabled = true;
            }
            _initialized = true;
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
        /// <returns>New instance of <see cref="ComponentsHandler{T}"/></returns>
        public static ComponentsHandler<T> RegisterComponentHandler()
        {
            var componentsHandler = new ComponentsHandler<T>();
            Main.OnTick += componentsHandler.OnTick;
            Main.OnAbort += componentsHandler.OnAbort;

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

            
            for (int i = 0; i < AllHandlerComponentProps.Count; i++)
            {
                var prop = AllHandlerComponentProps[i];

                prop.Dispose();
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Call it on first tick.
        /// </summary>
        public void OnInit()
        {
            for (int i = 0; i < AllComponents.Count; i++)
            {
                var component = AllComponents[i];
                component.Start();
            }
        }

        /// <summary>
        /// Call it every frame.
        /// </summary>
        public void OnTick()
        {
            if (IsDisposed)
                return;

            if (!_initialized)
                return;

            // Process all components
            for (int i = 0; i < AllComponents.Count; i++)
            {
                var component = AllComponents[i];

                if(component.IsEnabled)
                    component.OnTick();
            }
        }
    }
}
