using System.Linq;
using System.Text;
using FastEnumUtility;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.Components;
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
            set => _chanceToSpawnNeutronStar = value;
        }
        #endif
        
        private readonly int _maxCountOfStars;
        private readonly float _maxSizeOfGalaxy;
        private readonly Entity _starEntity;
        private readonly StarTemplatesData _templatesData;
        private readonly StarType[] _mainStarTypes;
        private readonly GalaxyManager _galaxyManager;
        
        private readonly StringBuilder _starNameBuilder = new StringBuilder();
        
        private float _chanceToSpawnNeutronStar = 0.999f;
        private const int MaxStarNameLength = 16;

        private EntityManager _entityManager;

        public StarsGenerator(GalaxyManager galaxyManager)
        {
            _maxCountOfStars = galaxyManager.MaxCountOfStars;
            _maxSizeOfGalaxy = galaxyManager.MaxSizeOfGalaxy;
            _starEntity = galaxyManager.StarEntity;
            _templatesData = galaxyManager.StarTemplatesData;
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
            SpawnStarObject(Random.Range(0f, 1f) > _chanceToSpawnNeutronStar ? StarType.N : _mainStarTypes.GetRandomElement());
        }

        private void SpawnStarObject(StarType type)
        {
            UpdateGalaxyData(type);
            
            var star = _entityManager.Instantiate(_starEntity);
            
            var template = _templatesData.GetTemplate(type);

            SetStarTransforms(star, template);
            
            var newStarData = PrepareStarComponent(type, template);

            _entityManager.SetComponentData(star, newStarData);

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

        private Star.Components.Star PrepareStarComponent(StarType type, StarTypeTemplate template)
        {
            var newStarData = CreateTypeDataFromTemplate(type, template);
            
            newStarData.type = type;

            return newStarData;
        }

        private Star.Components.Star CreateTypeDataFromTemplate(StarType type, StarTypeTemplate template)
        {
            return new Star.Components.Star
            {
                temperature = GetRandomValueFromRange(template.TemperatureRange),
                spaceBodyData = new SpaceBody
                {
                    gravityCenter = float3.zero,
                    mass = GetRandomValueFromRange(template.MassRange),
                    name = GetRandomStarName(type)
                },
                canHavePlanets = template.CanHavePlane
            };
        }

        private float GetRandomValueFromRange(Vector2 range)
        {
            return Mathf.Abs(range.x - range.y) <= 0.001f ? range.x : Random.Range(range.x, range.y);
        }

        private FixedString32 GetRandomStarName(StarType type)
        {
            _starNameBuilder.Clear();

            _starNameBuilder.Append(type.ToName());
            
            var charCount = Random.Range(1, MaxStarNameLength);

            for (var i = charCount; i >= 0; i--)
            {
                _starNameBuilder.Append(Random.Range(0, 9));
            }
            
            return _starNameBuilder.ToString();
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