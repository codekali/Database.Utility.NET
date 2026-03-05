using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace GenericFunctions.Extensions
{
    /// <summary>
    /// Extension methods for all objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Used to simplify and beautify casting an object to a type. 
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// Converts given object to a value or enum type using <see cref="Convert.ChangeType(object,TypeCode)"/> or <see cref="Enum.Parse(Type,string)"/> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this object obj)
            where T : struct
        {
            if (typeof(T) == typeof(Guid))
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
            }

            if (typeof(T).IsEnum)
            {
                if (Enum.IsDefined(typeof(T), obj))
                {
                    return (T)Enum.Parse(typeof(T), obj.ToString());
                }
                else
                {
                    throw new ArgumentException($"Enum type undefined '{obj}'.");
                }
            }

            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        public static object ConvertPropertyTypeValue<T>(this T item, string property, object value)
        {
            var properties = item.GetType().GetProperties();
            var prop = properties.ToList().FirstOrDefault(x => x.Name == property);

            if (prop == null)
                return null;

            var stringType = typeof(string);

            var type = prop.PropertyType;

            //Check string
            if (type == typeof(string))
                return value.ToString();

            //check date
            if (type == typeof(DateTime) || typeof(DateTime?).IsAssignableFrom(type))
            {
                if (DateTime.TryParse(value.ToString(), out DateTime result))
                {
                    return result;
                }

                return value.ToString();
            }

            //check integer
            if (type == typeof(int) || typeof(int?).IsAssignableFrom(type))
            {
                if (Int32.TryParse(value.ToString(), out int result))
                {
                    return result;
                }

                return null;
            }

            //check double
            if (type == typeof(double) || typeof(double?).IsAssignableFrom(type))
            {
                if (Double.TryParse(value.ToString(), out double result))
                {
                    return result;
                }

                return value.ToString();
            }


            //check decimal
            if (type == typeof(decimal) || typeof(decimal?).IsAssignableFrom(type))
            {
                if (Decimal.TryParse(value.ToString(), out decimal result))
                {
                    return result;
                }

                return value.ToString();
            }

            //check boolean
            if (type == typeof(bool) || typeof(bool?).IsAssignableFrom(type))
            {
                if (Boolean.TryParse(value.ToString(), out bool result))
                {
                    return result;
                }

                return value.ToString();
            }

            try
            {
                var obj = Enum.Parse(type, value.ToString(), true);

                return obj;
            }
            catch
            {
                return null;
            }
        }
    }
}
