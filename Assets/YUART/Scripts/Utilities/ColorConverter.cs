using Unity.Mathematics;
using UnityEngine;

namespace YUART.Scripts.Utilities
{
    /// <summary>
    /// Class, that handles color conversion from Color32 to float4.
    /// </summary>
    public static class ColorConverter
    {
        public static float4 ConvertToFloat4(this Color32 color)
        {
            return new float4(color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
        }
    }
}