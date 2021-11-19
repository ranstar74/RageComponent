using System;

namespace RageComponent.Core
{
    /// <summary>
    /// Defines a class that contains a <see cref="ComponentCollection"/>.
    /// </summary>
    /// <remarks>
    /// Implementation of <see cref="SetComponentHandle(int)"/> and <see cref="InvalidateHandle"/>
    /// was left on developer because sometimes it requires additional actions, for i.e. 
    /// writing handle in entity decorator.
    /// </remarks>
    public interface IComponentObject : IDisposable
    {
        /// <summary>
        /// Gets a list of object <see cref="Component"/>'s.
        /// </summary>
        /// <returns>List of object <see cref="Component"/>.</returns>
        ComponentCollection GetComponents();

        /// <summary>
        /// Handle of the <see cref="IComponentObject"/>.
        /// </summary>
        int ComponentHandle { get; }

        /// <summary>
        /// Sets handle of the <see cref="IComponentObject"/>.
        /// </summary>
        void SetComponentHandle(int componentHandle);

        /// <summary>
        /// Invalidates object handle. Being called on <see cref="ComponentObjectPool"/> dispose.
        /// </summary>
        /// <remarks>
        /// This method must set <see cref="ComponentHandle"/> to -1.
        /// </remarks>
        void InvalidateHandle();
    }
}
