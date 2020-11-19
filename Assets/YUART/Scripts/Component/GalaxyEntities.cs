using Unity.Entities;

namespace YUART.Scripts.Component
{
    /// <summary>
    /// Readonly struct, that stores entities for the galaxy;
    /// </summary>
    public readonly struct GalaxyEntities
    {
        public Entity StarEntity { get; }

        public GalaxyEntities(Entity starEntity)
        {
            StarEntity = starEntity;
        }
    }
}