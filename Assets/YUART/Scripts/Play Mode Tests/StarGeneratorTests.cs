using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.TestTools;
using YUART.Scripts.Galaxy_Manager.DataContainers;
using YUART.Scripts.Galaxy_Manager.Systems;
using YUART.Scripts.Star.Components;
using YUART.Scripts.Star.Enums;
using YUART.Scripts.Utilities;
using Assert = Unity.Assertions.Assert;

namespace YUART.Scripts.Play_Mode_Tests
{
    /// <summary>
    /// Class, that store tests for StarGenerator class.
    /// </summary>
    public class StarGeneratorTests
    {
        private BlobAssetStore _blobAssetStore;
        private Entity _starEntity;
        private StarTemplatesData _starTemplates;

        [UnityTest]
        public IEnumerator Generate1000StarsAndCheckCount()
        {
            var starGenerator = PrepareStarGenerator();
            
            starGenerator.GenerateStars();

            var stars = GetAllStars();

            Assert.AreEqual(1000, stars.Length);

            yield return null;
            
            stars.Dispose();
        }

        [UnityTest]
        public IEnumerator Generate1000StarsAndCheckIfAllNeutron()
        {
            SetChanceToSpawnNeutronStarAndGenerateStars(-1);

            var stars = GetAllStars();

            var foundNotNeuronStar = false;

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            foreach (var star in stars)
            {
                if (entityManager.GetComponentData<Star.Components.Star>(star).type == StarType.N) continue;
                
                foundNotNeuronStar = true;
                
                break;
            }
            
            Assert.AreEqual(false, foundNotNeuronStar);

            yield return null;
            
            stars.Dispose();
        }
        
        [UnityTest]
        public IEnumerator Generate1000NeutronStarsAndCheckIfAllMatchTemplateParameters()
        {
            SetChanceToSpawnNeutronStarAndGenerateStars(-1);

            var stars = GetAllStars();

            var foundNoMatchedStar = false;

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var neutronStarTemplate = _starTemplates.GetTemplate(StarType.N);
            
            foreach (var star in stars)
            {
                var starComponent = entityManager.GetComponentData<Star.Components.Star>(star);
                
                if (starComponent.mass.CheckIfValueInRange(neutronStarTemplate.MassRange) && 
                    entityManager.GetComponentData<NonUniformScale>(star).Value.CheckIfValueInRange(neutronStarTemplate.SizeRange) &&
                    starComponent.temperature.CheckIfValueInRange(neutronStarTemplate.TemperatureRange) &&
                    starComponent.canHavePlanets == neutronStarTemplate.CanHavePlane &&
                    entityManager.GetComponentData<StarColor>(star).value.Equals(neutronStarTemplate.Color.ConvertToFloat4())) continue;

                foundNoMatchedStar = true;
                
                break;
            }
            
            Assert.AreEqual(false, foundNoMatchedStar);

            yield return null;
            
            stars.Dispose();
        }

        [UnityTest]
        public IEnumerator Generate1000StarsAndCheckIfAllNotNeutron()
        {
            SetChanceToSpawnNeutronStarAndGenerateStars(1);

            var stars = GetAllStars();

            var foundNeuronStar = false;

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            foreach (var star in stars)
            {
                if (entityManager.GetComponentData<Star.Components.Star>(star).type != StarType.N) continue;
                
                foundNeuronStar = true;
                
                break;
            }
            
            Assert.AreEqual(false, foundNeuronStar);

            yield return null;
            
            stars.Dispose();
        }
        
        [UnityTest]
        public IEnumerator Generate1000NotNeutronStarsAndCheckIfAllMatchTemplateParameters()
        {
            SetChanceToSpawnNeutronStarAndGenerateStars(1);

            var stars = GetAllStars();

            var foundNoMatchedStar = false;

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            foreach (var star in stars)
            {
                var starComponent = entityManager.GetComponentData<Star.Components.Star>(star);
                var starTemplate = _starTemplates.GetTemplate(starComponent.type);

                if (starComponent.mass.CheckIfValueInRange(starTemplate.MassRange) && 
                    entityManager.GetComponentData<NonUniformScale>(star).Value.CheckIfValueInRange(starTemplate.SizeRange) &&
                    starComponent.temperature.CheckIfValueInRange(starTemplate.TemperatureRange) &&
                    starComponent.canHavePlanets == starTemplate.CanHavePlane &&
                    entityManager.GetComponentData<StarColor>(star).value.Equals(starTemplate.Color.ConvertToFloat4())) continue;

                foundNoMatchedStar = true;
                
                break;
            }
            
            Assert.AreEqual(false, foundNoMatchedStar);

            yield return null;
            
            stars.Dispose();
        }

        private void SetChanceToSpawnNeutronStarAndGenerateStars(float chanceToSpawnNeutronStar)
        {
            var starGenerator = PrepareStarGenerator();

            starGenerator.ChanceToSpawnNeutronStar = chanceToSpawnNeutronStar;

            starGenerator.GenerateStars();
        }

        private StarsGenerator PrepareStarGenerator()
        {
            return new StarsGenerator(1000, 1000, _starEntity, _starTemplates, new [] {StarType.N});
        }

        private NativeArray<Entity> GetAllStars()
        {
            var starQueryDesc = new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    typeof(Star.Components.Star)
                }
            };

            return World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(starQueryDesc).ToEntityArray(Allocator.TempJob);
        }

        [OneTimeSetUp]
        public void Awake()
        {
            PrepareDataForTest();
        }
        
        private void PrepareDataForTest()
        {
            var starGeneratorData = Resources.Load<StarGenerationData>("StarGeneratorData");
            
            _blobAssetStore = new BlobAssetStore();
            _starEntity = starGeneratorData.StarPrefab.ConvertGameObjectIntoEntity(_blobAssetStore);
            
            _starTemplates = starGeneratorData.StarTemplatesData;
            _starTemplates.Initialize();
        }

        [OneTimeTearDown]
        public void OnDestroy()
        {
            _blobAssetStore.Dispose();
        }
    }
}
