using Unity.Entities;

namespace YUART.Scripts.Galaxy_Manager.DataContainers
{
    /// <summary>
    /// Readonly struct, that stores entities for the galaxy;
    /// </summary>
    public readonly struct GalaxyEntities
    {
        public Entity StarEntity { get; }
        
        public Entity PlanetEntity { get; }

        public GalaxyEntities(Entity starEntity, Entity planetEntity)
        {
            StarEntity = starEntity;
            PlanetEntity = planetEntity;
        }
    }
}