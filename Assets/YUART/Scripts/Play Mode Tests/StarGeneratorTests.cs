using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;
using YUART.Scripts.Galaxy_Manager.DataContainers;
using YUART.Scripts.Galaxy_Manager.Systems;
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

        [Test]
        public async void Generate1000StarsAndCheckCount()
        {
            await PrepareDataForTest();
            
            var starGenerator = new StarsGenerator(1000, 1000, _starEntity, _starTemplates);
            
            starGenerator.GenerateStars();
            
            var starQueryDesc = new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    typeof(Star.Components.Star),
                }
            };

            var stars = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(starQueryDesc).ToEntityArray(Allocator.TempJob);
            
            Assert.AreEqual(1000, stars.Length);

            stars.Dispose();
        }

        private async Task PrepareDataForTest()
        {
            if (_starEntity == Entity.Null)
            {
                _starEntity = await GetStarEntity();
            }

            if (!_starTemplates)
            {
                _starTemplates = await GetStarTemplates();
            }
        }
        
        private async Task<Entity> GetStarEntity()
        {
            _blobAssetStore = new BlobAssetStore();

            var starItem = await Addressables.LoadAssetAsync<GameObject>("StarPrefab").Task;
            
            return starItem.ConvertGameObjectIntoEntity(_blobAssetStore);
        }

        private async Task<StarTemplatesData> GetStarTemplates()
        {
            return await Addressables.LoadAssetAsync<StarTemplatesData>("StarTemplates").Task;
        }

        ~StarGeneratorTests()
        {
            _blobAssetStore.Dispose();
        }
    }
}
