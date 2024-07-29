using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Serializer
{
    internal class MyCachedSerializator : ICustomSerializator
    {
        private const string _delimeter = ";";
        Dictionary<string, TypeConverter[]> converters_cache = new Dictionary<string, TypeConverter[]>();


        /// <summary>
        /// Object to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string? Serialize<T>(T obj) where T : class
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string delimiter = "";
                // process properties
                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    object? val = property.GetValue(obj);
                    sb.Append(delimiter);
                    sb.Append(val is null ? "" : val.ToString());
                    delimiter = _delimeter;
                }
                // process fields
                FieldInfo[] fields = typeof(T).GetFields();
                foreach (FieldInfo field in fields)
                {
                    object? val = field.GetValue(obj);
                    sb.Append(delimiter);
                    sb.Append(val is null ? "" : val.ToString());
                    delimiter = _delimeter;
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// string to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strObj"></param>
        /// <returns></returns>
        public T? Deserialize<T>(string strObj) where T : class, new()
        {
            try
            {
                // cache
                string? type_name = typeof(T).FullName;
                TypeConverter[]? type_converters = null;
                if (type_name is not null)
                    converters_cache.TryGetValue(type_name, out type_converters);
                if (type_converters is null)
                    type_converters = new TypeConverter[typeof(T).GetFields().Length + typeof(T).GetProperties().Length];
                if (type_converters is null)
                    throw new Exception("Erorr creating cache!");

                converters_cache[type_name ?? ""] = type_converters;
                // cache

                string[] strObjPFs = strObj.Split(_delimeter);

                PropertyInfo[] properties = typeof(T).GetProperties();
                FieldInfo[] fields = typeof(T).GetFields();
                // first check
                if (strObjPFs.Length != (fields.Length + properties.Length))
                    throw new Exception("Не совпадает количество свойств+полей объекта и количество полей в строке. Выходим.");

                T t = new();
                int index = 0;
                foreach (PropertyInfo property in properties)
                {
                    TypeConverter typeConverter;
                    if (type_converters is not null && type_converters[index] == null)
                    {
                        typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                        type_converters[index] = typeConverter;
                    }
                    typeConverter = type_converters[index];
                    property.SetValue(t, typeConverter.ConvertFromString(strObjPFs[index++]));
                }
                foreach (FieldInfo field in fields)
                {
                    TypeConverter typeConverter;
                    if (type_converters is not null && type_converters[index] == null)
                    {
                        typeConverter = TypeDescriptor.GetConverter(field.FieldType);
                        type_converters[index] = typeConverter;
                    }
                    typeConverter = type_converters[index];
                    field.SetValue(t, typeConverter.ConvertFromString(strObjPFs[index++]));
                }
                return t;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

    }
}
