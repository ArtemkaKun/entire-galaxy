using System;

namespace YUART.Scripts.Utilities
{
    /// <summary>
    /// Class, that provides methods for peaking random element from IEnumerable.
    /// </summary>
    public static class RandomPicker
    {
        public static T GetRandomElement<T>(this T[] array)
        {
           var randomElementIndex = new Random().Next(array.Length);

           return array[randomElementIndex];
        }
    }
}