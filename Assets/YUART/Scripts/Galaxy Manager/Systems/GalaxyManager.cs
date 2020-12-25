using Unity.Entities;
using UnityEngine;
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
      public int MaxCountOfStars => countOfStars;

      public Entity StarEntity => _entities.StarEntity;

      public Entity PlanetEntity => _entities.PlanetEntity;

      public StarType[] SecondaryStarTypes => secondaryStarTypes;

      [SerializeField] private GameObject starPrefab;
      [SerializeField] private GameObject planetPrefab;
      [SerializeField] private int countOfStars;
      [SerializeField] private float maxSizeOfGalaxy;
      [SerializeField] private StarType[] secondaryStarTypes;
      [SerializeField] private Vector2 starYAxisOffset;
      [SerializeField] private float areaSizeForPlanetSystem;
      [SerializeField] private Vector2 planetYAxisOffset;

      private readonly GalaxyData _data = new GalaxyData();

      private GalaxyEntities _entities;
      private BlobAssetStore _entityAssetStore;

      /// <summary>
      /// Increment count of stars by specified type.
      /// </summary>
      /// <param name="type">Type of a new star.</param>
      public void IncrementStarsCountByType(StarType type)
      {
         _data.IncrementStarsCountByType(type);
      }
      
      /// <summary>
      /// Increment count of main stars.
      /// </summary>
      public void IncrementMainStarsCount()
      {
         _data.IncrementMainStarsCount();
      }

      /// <summary>
      /// Increment count of secondary stars.
      /// </summary>
      public void IncrementSecondaryStarsCount()
      {
         _data.IncrementSecondaryStarsCount();
      }
      
      /// <summary>
      /// Increment count of planets.
      /// </summary>
      public void IncrementPlanetsCount()
      {
         _data.IncrementPlanetsCount();
      }
      
      private void Awake()
      {
         InitializeGalaxyEntities();
      }

      private void InitializeGalaxyEntities()
      {
         _entityAssetStore = new BlobAssetStore();
         _entities = ConvertPrefabsToEntities();
      }

      private GalaxyEntities ConvertPrefabsToEntities()
      {
         return new GalaxyEntities(starPrefab.ConvertGameObjectIntoEntity(_entityAssetStore), planetPrefab.ConvertGameObjectIntoEntity(_entityAssetStore));
      }

      private void Start()
      {
         var stars = new StarsGenerator(this, maxSizeOfGalaxy, starYAxisOffset).GenerateStars();

         new PlanetGenerator(this, stars, areaSizeForPlanetSystem, planetYAxisOffset).GeneratePlanets();
      }

      private void OnDestroy()
      {
         _entityAssetStore?.Dispose();
      }
   }
}