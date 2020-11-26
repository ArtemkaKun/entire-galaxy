using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.DataContainers;
using YUART.Scripts.Star.Components;
using YUART.Scripts.Star.Enums;
using YUART.Scripts.Utilities;
using Random = UnityEngine.Random;

namespace YUART.Scripts.Galaxy_Manager.Systems
{
    /// <summary>
    /// Class, that handles stars generation.
    /// </summary>
    public sealed class StarsGenerator
    {
        #if UNITY_EDITOR
        public float ChanceToSpawnNeutronStar
        {
            get => _chanceToSpawnNeutronStar;
            set => _chanceToSpawnNeutronStar = value;
        }
        #endif
        
        private readonly int _countOfStars;
        private readonly float _maxSizeOfGalaxy;
        private readonly Entity _starEntity;
        private readonly StarTemplatesData _templatesData;
        private readonly StarType[] _mainStarTypes;

        private float _chanceToSpawnNeutronStar = 0.999f;
        private const int MaxStarNameLength = 16;

        public StarsGenerator(int countOfStars, float maxSizeOfGalaxy, Entity starEntity, StarTemplatesData templatesData, IEnumerable<StarType> secondaryStarTypes)
        {
            _countOfStars = countOfStars;
            _maxSizeOfGalaxy = maxSizeOfGalaxy;
            _starEntity = starEntity;
            _templatesData = templatesData;

            _mainStarTypes = Enum.GetValues(typeof(StarType)).Cast<StarType>().Except(secondaryStarTypes).ToArray();
        }

        /// <summary>
        /// Generates stars.
        /// </summary>
        public void GenerateStars()
        {
            for (var i = 0; i < _countOfStars; i++)
            {
                SpawnStar();
            }
        }

        private void SpawnStar()
        {
            SpawnStarObject(Random.Range(0f, 1f) > _chanceToSpawnNeutronStar ? StarType.N : _mainStarTypes.GetRandomElement());
        }

        private void SpawnStarObject(StarType type)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var star = entityManager.Instantiate(_starEntity);
            
            var template = _templatesData.GetTemplate(type);

            SetStarTransforms(star, template);
            
            var newStarData = PrepareStarComponent(type, template);

            entityManager.SetComponentData(star, newStarData);

            var starColor = template.Color;
            
            entityManager.SetComponentData(star, new StarColor
            {
                value = starColor.ConvertToFloat4()
            });
        }

        private Star.Components.Star PrepareStarComponent(StarType type, StarTypeTemplate template)
        {
            var newStarData = CreateTypeDataFromTemplate(template);
            
            newStarData.type = type;
            
            newStarData.name = GetRandomStarName(type);
            
            return newStarData;
        }

        private Star.Components.Star CreateTypeDataFromTemplate(StarTypeTemplate template)
        {
            return new Star.Components.Star
            {
                temperature = GetRandomValueFromRange(template.TemperatureRange),
                mass = GetRandomValueFromRange(template.MassRange),
                canHavePlanets = template.CanHavePlane
            };
        }

        private float GetRandomValueFromRange(Vector2 range)
        {
            return Mathf.Abs(range.x - range.y) <= 0.001f ? range.x : Random.Range(range.x, range.y);
        }

        private FixedString32 GetRandomStarName(StarType type)
        {
            FixedString32 starName = type.ToString();
            
            var charCount = Random.Range(1, MaxStarNameLength);

            for (var i = charCount; i >= 0; i--)
            {
                starName += Random.Range(0, 9).ToString();
            }
            
            return starName;
        }

        private void SetStarTransforms(Entity star, StarTypeTemplate template)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            entityManager.AddComponent<NonUniformScale>(star);
            
            entityManager.SetComponentData(star, new NonUniformScale
                {
                    Value = GetRandomValueFromRange(template.SizeRange)
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
    }
}