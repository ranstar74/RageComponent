using System;
using System.Reflection;

namespace RageComponent
{
    /// <summary>
    /// Various help functions.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Executes action for all fields of given type.
        /// </summary>
        /// <typeparam name="T">Type of field.</typeparam>
        /// <param name="obj">Class object.</param>
        /// <param name="action">Action to execute.</param>
        public static void ProcessAllClassFieldsByType<T>(object obj, Action<FieldInfo> action)
        {
            var fields = obj.GetType().GetFields();

            // Go through each field and find fields of given type
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];

                if (field.FieldType.BaseType == typeof(T) || field.FieldType == typeof(T))
                {
                    action(field);
                }
            }
        }
    }
}
