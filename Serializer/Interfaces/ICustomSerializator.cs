using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serializer
{
    internal interface ICustomSerializator
    {
        string? Serialize<T>(T obj) where T : class;
        T? Deserialize<T>(string strObj) where T : class, new();
    }
}
