using Unity.Entities;
using YUART.Scripts.Planet.Enums;

namespace YUART.Scripts.Planet.Components
{
    /// <summary>
    /// Component, that stores planet data.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct Planet : IComponentData
    {
        public PlanetType type;
    }
}