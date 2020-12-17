using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace YUART.Scripts.Space_Objects.Components
{
    /// <summary>
    /// Component, that stores data of space object.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct SpaceObject : IComponentData
    {
        public FixedString32 name;
        public float mass;
        public float3 gravityCenter;
        public bool canHaveSystem;
    }
}
