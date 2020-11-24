using Unity.Entities;
using UnityEngine;

namespace YUART.Scripts.Utilities
{
    /// <summary>
    /// Class, that provides static method to convert gameObject into Entity;
    /// </summary>
    public static class EntityConverter
    {
        public static Entity ConvertGameObjectIntoEntity(this GameObject gameObject, BlobAssetStore entityAssetStore)
        {
            return GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObject, GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, entityAssetStore));
        }
    }
}