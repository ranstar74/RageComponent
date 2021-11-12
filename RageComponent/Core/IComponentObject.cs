namespace RageComponent.Core
{
    /// <summary>
    /// Defines a class that contains a <see cref="ComponentCollection"/>.
    /// </summary>
    public interface IComponentObject
    {
        /// <summary>
        /// Gets a list of object <see cref="Component"/>'s.
        /// </summary>
        /// <returns>List of object <see cref="Component"/>.</returns>
        ComponentCollection GetComponents();

        /// <summary>
        /// Initializes <see cref="IComponentObject"/> components.
        /// </summary>
        void InitializeComponents();

        /// <summary>
        /// Handle of the <see cref="IComponentObject"/>.
        /// </summary>
        int ComponentHandle { get; }

        /// <summary>
        /// Sets handle of the <see cref="IComponentObject"/>.
        /// </summary>
        void SetComponentHandle(int componentHandle);
    }
}
