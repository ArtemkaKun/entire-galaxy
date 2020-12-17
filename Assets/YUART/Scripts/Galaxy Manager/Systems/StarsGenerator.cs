﻿using System.Linq;
using FastEnumUtility;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.DataContainers;
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
    public sealed class StarsGenerator
    {
        private readonly int _maxCountOfStars;
        private readonly float _maxSizeOfGalaxy;
        private readonly Entity _starEntity;
        private readonly StarType[] _mainStarTypes;
        private readonly GalaxyManager _galaxyManager;

        private const float ChanceToSpawnNeutronStar = 0.999f;

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
        public void GenerateStars()
        {
            for (var i = 0; i < _maxCountOfStars; i++)
            {
                SpawnStar();
            }
        }

        private void SpawnStar()
        {
            SpawnStarObject(Random.Range(0f, 1f) > ChanceToSpawnNeutronStar ? StarType.N : _mainStarTypes.GetRandomElement());
        }

        private void SpawnStarObject(StarType type)
        {
            UpdateGalaxyData(type);
            
            var star = _entityManager.Instantiate(_starEntity);
            
            var template = TemplateDataConstructorSingleton.Instance.GetTemplateForType<StarTypeTemplate, StarType>(type);

            SetStarTransforms(star, template);
            
            var (newStarData, newSpaceObjectData) = PrepareStarComponent(type, template);

            _entityManager.SetComponentData(star, newStarData);
            
            _entityManager.SetComponentData(star, newSpaceObjectData);

            SetStarColor(template, star);
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
                    Value = GetRandomPositionInGalaxy()
                }
            );

            entityManager.SetComponentData(star, new Rotation
                {
                    Value = quaternion.Euler(GetRandomRotation())
                }
            );
        }

        private Vector3 GetRandomPositionInGalaxy()
        {
            var position = Random.insideUnitCircle * _maxSizeOfGalaxy;
            return new Vector3(position.x, Random.Range(-1000f, 1000f), position.y);
        }

        private Vector3 GetRandomRotation()
        {
            return new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }

        private (Star.Components.Star, SpaceObject) PrepareStarComponent(StarType type, StarTypeTemplate template)
        {
            var newStarData = CreateTypeDataFromTemplate(type, template);

            var newSpaceObjectData = CreateSpaceObjectDataFromTemplate(type, template);
            
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

        private SpaceObject CreateSpaceObjectDataFromTemplate(StarType type, StarTypeTemplate template)
        {
            return new SpaceObject
            {
                name = NameGenerator.GetRandomNameFromType(type.ToName()),
                mass = template.MassRange.GetRandomValueFromRange(),
                gravityCenter = float3.zero,
                canHaveSystem = template.CanHavePlanets
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