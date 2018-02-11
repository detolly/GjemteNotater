using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace itslearning
{
    static class Extensions
    {
        public static string Concatenate(this string[] array, string combiner = null)
        {
            StringBuilder builder = new StringBuilder();
            int i = 1;
            foreach (string String in array)
            {
                builder.Append(String);
                if (i != array.Length) builder.Append(combiner);
                i++;
            }
            return builder.ToString();
        }
    }
}
