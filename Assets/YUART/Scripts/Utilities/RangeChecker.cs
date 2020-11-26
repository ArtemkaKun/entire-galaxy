using Unity.Mathematics;
using UnityEngine;

namespace YUART.Scripts.Utilities
{
    /// <summary>
    /// Class, that provides methods to check if the value in range.
    /// </summary>
    public static class RangeChecker
    {
        public static bool CheckIfValueInRange(this float value, Vector2 range)
        {
            return value >= range.x && value <= range.y;
        }
        
        public static bool CheckIfValueInRange(this float3 value, Vector2 range)
        {
            return value.x >= range.x && value.y >= range.x && value.z >= range.x
                   && value.x <= range.y && value.y <= range.y && value.z <= range.y;
        }
    }
}