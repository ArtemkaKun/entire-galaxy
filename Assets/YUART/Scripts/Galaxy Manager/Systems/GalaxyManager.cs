using Unity.Entities;
using UnityEngine;
using YUART.Scripts.Component;
using YUART.Scripts.Galaxy_Manager.DataContainers;
using YUART.Scripts.Star.Enums;
using YUART.Scripts.Utilities;

namespace YUART.Scripts.Galaxy_Manager.Systems
{
   /// <summary>
   /// Class, that manages galaxy.
   /// </summary>
   public sealed class GalaxyManager : MonoBehaviour
   {
      [SerializeField] private GameObject starPrefab;
      [SerializeField] private int countOfStars;
      [SerializeField] private float maxSizeOfGalaxy;
      [SerializeField] private StarTemplatesData templatesData;
      [SerializeField] private StarType[] secondaryStarTypes;
      
      private readonly GalaxyData _data = new GalaxyData();

      private GalaxyEntities _entities;
      private BlobAssetStore _entityAssetStore;

      private void Awake()
      {
         templatesData.Initialize();
         
         InitializeGalaxyEntities();

         new StarsGenerator(countOfStars, maxSizeOfGalaxy, _entities.StarEntity, templatesData, secondaryStarTypes).GenerateStars();
      }

      private void InitializeGalaxyEntities()
      {
         _entityAssetStore = new BlobAssetStore();
         _entities = ConvertPrefabsToEntities();
      }

      private GalaxyEntities ConvertPrefabsToEntities()
      {
         return new GalaxyEntities(starPrefab.ConvertGameObjectIntoEntity(_entityAssetStore));
      }

      private void OnDestroy()
      {
         _entityAssetStore.Dispose();
      }
   }
}
