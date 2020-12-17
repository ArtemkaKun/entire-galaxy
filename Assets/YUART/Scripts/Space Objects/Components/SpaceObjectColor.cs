using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace YUART.Scripts.Space_Objects.Components
{
    /// <summary>
    /// Component, that handles space object's material color.
    /// </summary>
    [Serializable]
    [MaterialProperty("_Color", MaterialPropertyFormat.Float4)]
    [GenerateAuthoringComponent]
    public struct SpaceObjectColor : IComponentData
    {
        public float4 value;
    }
}