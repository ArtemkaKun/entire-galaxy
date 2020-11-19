using Unity.Entities;
using UnityEngine;
using YUART.Scripts.Component;

namespace YUART.Scripts.Systems
{
   /// <summary>
   /// Class, that manages galaxy.
   /// </summary>
   public sealed class GalaxyManager : MonoBehaviour
   {
      [SerializeField] private GameObject starPrefab;
   
      private readonly GalaxyData _data = new GalaxyData();
   
      private GalaxyEntities _entities;
      private BlobAssetStore _entityAssetStore;

      private void ConvertPrefabsToEntities()
      {
         var starEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(starPrefab, GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _entityAssetStore));
      
         _entities = new GalaxyEntities(starEntity);
      }
   
      private void OnDestroy()
      {
         _entityAssetStore.Dispose();
      }
   }
}
