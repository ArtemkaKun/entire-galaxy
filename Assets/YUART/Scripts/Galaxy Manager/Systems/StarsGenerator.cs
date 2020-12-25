using System.Collections.Generic;
using System.Linq;
using FastEnumUtility;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.Data_Containers.Templates.Star_Templates;
using YUART.Scripts.Space_Objects.Components;
using YUART.Scripts.Star.Components;
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
        private readonly float _maxSizeOfGalaxy;
        private readonly Entity _starEntity;
        private readonly StarType[] _mainStarTypes;
        private readonly GalaxyManager _galaxyManager;

        private readonly Vector2 _yAxisRange = new Vector2(-1000, 1000);

        private EntityManager _entityManager;

        public StarsGenerator(GalaxyManager galaxyManager)
        {
            _maxCountOfStars = galaxyManager.MaxCountOfStars;
            _maxSizeOfGalaxy = galaxyManager.MaxSizeOfGalaxy;
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

            SetStarTransforms(star, template);
            
            var (newStarData, newSpaceObjectData) = PrepareStarComponent(type, template);

            _entityManager.SetComponentData(star, newStarData);
            
            _entityManager.SetComponentData(star, newSpaceObjectData);

            SetStarColor(template, star);

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

        private void SetStarTransforms(Entity star, StarTypeTemplate template)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            entityManager.AddComponent<NonUniformScale>(star);
            
            entityManager.SetComponentData(star, new NonUniformScale
                {
                    Value = template.SizeRange.GetRandomValueFromRange()
                }
            );

            entityManager.SetComponentData(star, new Translation
                {
                    Value = GetRandomPositionInGalaxy(_maxSizeOfGalaxy, Vector3.zero, _yAxisRange)
                }
            );

            entityManager.SetComponentData(star, new Rotation
                {
                    Value = quaternion.Euler(GetRandomRotation())
                }
            );
        }

        private (Star.Components.Star, SpaceObject) PrepareStarComponent(StarType type, StarTypeTemplate template)
        {
            var newStarData = CreateTypeDataFromTemplate(type, template);

            var newSpaceObjectData = TemplateDataConstructorSingleton.Instance.CreateSpaceObjectDataFromTemplate(type.ToName(), template);
            
            return (newStarData, newSpaceObjectData);
        }

        private Star.Components.Star CreateTypeDataFromTemplate(StarType type, StarTypeTemplate template)
        {
            return new Star.Components.Star
            {
                temperature = template.TemperatureRange.GetRandomValueFromRange(),
                type = type
            };
        }

        private void SetStarColor(StarTypeTemplate template, Entity star)
        {
            var starColor = template.Color;

            _entityManager.SetComponentData(star, new StarColor
            {
                value = starColor.ConvertToFloat4()
            });
        }
    }
}