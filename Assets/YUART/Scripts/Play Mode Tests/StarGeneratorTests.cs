using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.DataContainers;
using YUART.Scripts.Utilities;

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
