using System;
using System.Collections.Generic;
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
                // process fields
                FieldInfo[] fields = typeof(T).GetFields(/*BindingFlags.Public | BindingFlags.NonPublic*/);
                foreach (FieldInfo field in fields)
                {
                    object? val = field.GetValue(obj);
                    sb.Append(delimiter);
                    sb.Append(val is null ? "" : val.ToString());
                    delimiter = _delimeter;
                }
                // process properties
                PropertyInfo[] properties = typeof(T).GetProperties(/*BindingFlags.Public | BindingFlags.NonPublic*/);
                foreach (PropertyInfo property in properties)
                {
                    object? val = property.GetValue(obj);
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

                string[] strObjPFs = strObj.Split(';');

                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.NonPublic);
                FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic);
                // first check
                if (strObjPFs.Length != (fields.Length + properties.Length))
                    throw new Exception("Не совпадает количество свойств+полей объекта и количество полей в строке. Выходим.");

                T t = new();
                if (properties.Any())
                    foreach (PropertyInfo property in properties.Take(new Range(0, properties.Length - 1)))
                    {
                        //???
                        //property.SetValue(t, )
                    }
                if (fields.Any())
                    foreach (FieldInfo field in fields.Take(new Range(properties.Length, (properties.Length+ fields.Length) - 1)))
                    { 

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
