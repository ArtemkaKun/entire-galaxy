using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YUART.Scripts.Utilities
{
    /// <summary>
    /// Class, that provides methods to operate with range.
    /// </summary>
    public static class RangeTool
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
        
        public static float GetRandomValueFromRange(this Vector2 range)
        {
            return Mathf.Abs(range.x - range.y) <= 0.001f ? range.x : Random.Range(range.x, range.y);
        }
    }
}