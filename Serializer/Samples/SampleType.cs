using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serializer
{
    [Serializable]
    public class SampleType
    {
        public int i1 { get; set; }
        public int i2 { get; set; }
        public int i3 { get; set; }
        public int i4 { get; set; }
        public int i5 { get; set; }
        public int i6 { get; set; }
        public int i7 { get; set; }
        public SampleType()
        {
        }
        static public SampleType Get() => new SampleType() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5, i6=6, i7=7 };

        public override string ToString()
        {
            return $"i1={i1}, i2={i2}, i3={i3}, i4={i4}, i5={i5}, i6={i6}, i7={i7}";
        }

    }
}
