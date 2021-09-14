using GTA;
using System;

namespace RageComponent
{
    /// <summary>
    /// Shows script that component will be attached to this entity.
    /// </summary>
    public class EntityAttribute : Attribute
    {
        /// <summary>
        /// Entity to attach component to.
        /// </summary>
        public string EntityProperty { get; set; }
    }
}
