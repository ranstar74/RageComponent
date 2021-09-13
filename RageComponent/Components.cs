using FusionLibrary.Extensions;
using System.Collections.Generic;

namespace RageComponent
{
    /// <summary>
    /// Component functions.
    /// </summary>
    public class Components
    {
        /// <summary>
        /// All created components.
        /// </summary>
        internal readonly static List<Component> AllComponents;

        /// <summary>
        /// Registers new component.
        /// </summary>
        /// <param name="component">Component to register.</param>
        public static void RegisterComponent(Component component)
        {
            AllComponents.Add(component);
        }

        /// <summary>
        /// Call it on first tick.
        /// </summary>
        internal static void OnInit()
        {
            for(int i = 0; i < AllComponents.Count; i++)
            {
                AllComponents[i].Start();
            }
        }

        /// <summary>
        /// Call it every frame.
        /// </summary>
        internal static void OnTick()
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
