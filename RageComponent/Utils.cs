using System;
using System.Reflection;

namespace RageComponent
{
    /// <summary>
    /// Various help functions.
    /// </summary>
    internal class Utils
    {
        /// <summary>
        /// Executes action for all fields of given type.
        /// </summary>
        /// <typeparam name="T">Type of field.</typeparam>
        /// <param name="obj">Class object.</param>
        /// <param name="action">Action to execute.</param>
        internal static void ProcessAllClassFieldsByBaseType<T>(object obj, Action<FieldInfo> action)
        {
            var fields = obj.GetType().GetFields();

            // Go through each field and find fields of given type
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];

                if (field.FieldType.BaseType == typeof(T))
                {
                    action(field);
                }
            }
        }

        /// <summary>
        /// Gets property value of given object.
        /// </summary>
        /// <param name="obj">Object to look for value in.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Value of property if found.</returns>
        /// <exception cref="ArgumentException">If property not found.</exception>
        internal static object GetClassPropertyValueByName(object obj, string propertyName)
        {
            var fields = obj.GetType().GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];

                if (field.Name == propertyName)
                    return field.GetValue(obj);
            }
            throw new ArgumentException($"Property {propertyName} not found!");
        }
    }
}
