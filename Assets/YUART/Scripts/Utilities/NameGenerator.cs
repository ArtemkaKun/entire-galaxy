using System.Text;
using Unity.Collections;
using Random = UnityEngine.Random;

namespace YUART.Scripts.Utilities
{
    /// <summary>
    /// Class, that provides method to generate random name.
    /// </summary>
    public static class NameGenerator
    {
        private const int MaxNameLength = 16;
        
        private static readonly StringBuilder NameBuilder = new StringBuilder();
        
        public static FixedString32 GetRandomNameFromType(string type)
        {
            NameBuilder.Clear();

            NameBuilder.Append(type);
            
            var charCount = Random.Range(1, MaxNameLength);

            for (var i = charCount; i >= 0; i--)
            {
                NameBuilder.Append(Random.Range(0, 9));
            }
            
            return NameBuilder.ToString();
        }
    }
}