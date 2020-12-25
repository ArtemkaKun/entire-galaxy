using System.Collections.Generic;
using System.Linq;
using FastEnumUtility;
using Unity.Entities;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.Data_Containers.Templates.Star_Templates;
using YUART.Scripts.Star.Enums;
using YUART.Scripts.Utilities;
using YUART.Scripts.Utilities.Template_Data_Constructor;
using Random = UnityEngine.Random;

namespace YUART.Scripts.Galaxy_Manager.Systems
{
    /// <summary>
    /// Class, that handles stars generation.
    /// </summary>
    public sealed class StarsGenerator : SpaceObjectGenerator
    {
        private const float ChanceToSpawnNeutronStar = 0.999f;
        
        private readonly int _maxCountOfStars;
        private readonly Entity _starEntity;
        private readonly StarType[] _mainStarTypes;
        private readonly GalaxyManager _galaxyManager;

        private EntityManager _entityManager;

        public StarsGenerator(GalaxyManager galaxyManager, float areaSize, Vector2 yAxisRange) : base(areaSize, yAxisRange)
        {
            _maxCountOfStars = galaxyManager.MaxCountOfStars;
            _starEntity = galaxyManager.StarEntity;
            _galaxyManager = galaxyManager;
            
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _mainStarTypes = FastEnum.GetValues<StarType>().Except(galaxyManager.SecondaryStarTypes).ToArray();
        }

        /// <summary>
        /// Generates stars.
        /// </summary>
        public Stack<Entity> GenerateStars()
        {
            var stars = new Stack<Entity>(_maxCountOfStars);
            
            for (var i = 0; i < _maxCountOfStars; i++)
            {
                stars.Push(SpawnStar());
            }

            return stars;
        }

        private Entity SpawnStar()
        {
            return SpawnStarObject(Random.Range(0f, 1f) > ChanceToSpawnNeutronStar ? StarType.N : _mainStarTypes.GetRandomElement());
        }

        private Entity SpawnStarObject(StarType type)
        {
            UpdateGalaxyData(type);
            
            var star = _entityManager.Instantiate(_starEntity);
            
            var template = TemplateDataConstructorSingleton.Instance.GetTemplateForType<StarTypeTemplate, StarType>(type);

            SetSpaceObjectTransforms(star, template, Vector3.zero);

            _entityManager.SetComponentData(star, PrepareStarComponent(type, template));
            
            _entityManager.SetComponentData(star, PrepareSpaceObjectComponent(type.ToName(), template));

            templateDataConstructor.SetSpaceObjectColor(template, star);

            return star;
        }

        private void UpdateGalaxyData(StarType type)
        {
            _galaxyManager.IncrementStarsCountByType(type);
            
            if (type == StarType.N)
            {
                _galaxyManager.IncrementSecondaryStarsCount();
            }
            else
            {
                _galaxyManager.IncrementMainStarsCount();
            }
        }

        private Star.Components.Star PrepareStarComponent(StarType type, StarTypeTemplate template)
        {
            return CreateTypeDataFromTemplate(type, template);
        }

        private Star.Components.Star CreateTypeDataFromTemplate(StarType type, StarTypeTemplate template)
        {
            return new Star.Components.Star
            {
                temperature = template.TemperatureRange.GetRandomValueFromRange(),
                type = type
            };
        }
    }
}