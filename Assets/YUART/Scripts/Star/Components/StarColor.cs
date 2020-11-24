using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace YUART.Scripts.Star.Components
{
    /// <summary>
    /// Component, that handles star's material color.
    /// </summary>
    [Serializable]
    [MaterialProperty("_StarColor", MaterialPropertyFormat.Float4)]
    [GenerateAuthoringComponent]
    public struct StarColor : IComponentData
    {
        public float4 value;
    }
}
