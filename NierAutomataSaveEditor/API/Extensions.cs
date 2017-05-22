using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NierAutomataSaveEditor.API
{
    public static class Extensions
    {
        public static T[] SubArray<T>(this T[] array, int index, int length)
        {
            T[] result = new T[length];

            Array.Copy(array, index, result, 0, length);

            return result;
        }

        public static void Modify<T>(this T[] array, int startIndex, T[] subarray)
        {
            for(int i = 0; i < subarray.Length; i++)
                array[startIndex + i] = subarray[i];
        }
    }
}
