using System;

namespace RageComponent.Core
{
    /// <summary>
    /// Defines a class that contains a <see cref="ComponentCollection"/>.
    /// </summary>
    public interface IComponentObject : IDisposable
    {
        /// <summary>
        /// Gets a list of object <see cref="Component"/>'s.
        /// </summary>
        /// <returns>List of object <see cref="Component"/>.</returns>
        ComponentCollection GetComponents();

        /// <summary>
        /// Unique identificator of component object.
        /// Could be handle of entity that component object holds,
        /// so later you can get the <see cref="IComponentObject"/> by that handle.
        /// </summary>
        int Handle { get; }
    }
}
