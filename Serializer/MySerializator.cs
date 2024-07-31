using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Serializer
{
    internal class MySerializator : ICustomSerializator
    {
        private const string _delimeter = ";";

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
                string[] strObjPFs = strObj.Split(_delimeter);
                PropertyInfo[] properties = typeof(T).GetProperties();
                FieldInfo[] fields = typeof(T).GetFields();
                // first check
                if (strObjPFs.Length != (fields.Length + properties.Length))
                {
                    Console.WriteLine("Не совпадает количество свойств+полей объекта и количество полей в строке. Выходим.");
                    return null;
                }

                T t = new();
                int index = 0;
                foreach (PropertyInfo property in properties)
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                    property.SetValue(t, typeConverter.ConvertFromString(strObjPFs[index++]));
                }
                foreach (FieldInfo field in fields)
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(field.FieldType);
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
