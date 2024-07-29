using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Serializer
{
    internal class JSONSerializator : ICustomSerializator
    {
        public string? Serialize<T>(T obj) where T : class
        {
            try
            {
                return JsonSerializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public T? Deserialize<T>(string strObj) where T : class, new()
        {
            try
            {
                return JsonSerializer.Deserialize<T>(strObj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
