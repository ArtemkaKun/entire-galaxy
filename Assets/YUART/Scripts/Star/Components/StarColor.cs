using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace YUART.Scripts.Star.Components
{
    [Serializable]
    [MaterialProperty("_StarColor", MaterialPropertyFormat.Float4)]
    [GenerateAuthoringComponent]
    public struct StarColor : IComponentData
    {
        public float4 value;
    }
}
