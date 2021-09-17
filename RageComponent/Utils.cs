using System;
using System.Collections.Generic;
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
        /// Gets field value of given object.
        /// </summary>
        /// <param name="obj">Object to look for value in.</param>
        /// <param name="propertyName">Name of the field.</param>
        /// <returns>Value of property if found.</returns>
        /// <exception cref="ArgumentException">If property not found.</exception>
        internal static object GetClassFieldValueByName(object obj, string propertyName)
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

        /// <summary>
        /// Gets all field values of given type.
        /// </summary>
        /// <typeparam name="T">Field type.</typeparam>
        /// <param name="instance">Class instance to get values from.</param>
        /// <returns>Field values of given type.</returns>
        public static List<T> GetAllFieldValues<T>(object instance)
        {
            var fields = instance.GetType().GetFields();
            var list = new List<T>();

            // Go through every class field
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];

                // Check if it have type of T
                if (field.FieldType == typeof(T))
                    list.Add((T)field.GetValue(instance));
            }
            return list;
        }
    }
}
