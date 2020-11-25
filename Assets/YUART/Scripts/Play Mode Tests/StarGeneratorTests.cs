using System.Collections;
using NUnit.Framework;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TestTools;
using YUART.Scripts.Galaxy_Manager.DataContainers;
using YUART.Scripts.Galaxy_Manager.Systems;
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
            GenerateTestWorld();

            var stars = GetAllStars();

            Assert.AreEqual(1000, stars.Length);

            yield return null;
            
            stars.Dispose();
        }

        [UnityTest]
        public IEnumerator Generate1000StarsAndCheckIfAllNeuron()
        {
            GenerateTestWorld();
            
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

        private void GenerateTestWorld()
        {
            var starGenerator = new StarsGenerator(1000, 1000, _starEntity, _starTemplates);

            starGenerator.GenerateStars();
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
