using Unity.Collections;
using Unity.Mathematics;

namespace YUART.Scripts.Galaxy_Manager.Components
{
    /// <summary>
    /// Component, that handles space body data.
    /// </summary>
    public struct SpaceBody
    {
        public float mass;
        public float3 gravityCenter;
        public FixedString32 name;
    }
}