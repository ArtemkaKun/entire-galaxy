using System.Collections.Generic;
using System.Linq;
using FastEnumUtility;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.Data_Containers.Templates;
using YUART.Scripts.Planet.Enums;
using YUART.Scripts.Space_Objects.Components;
using YUART.Scripts.Utilities;
using YUART.Scripts.Utilities.Template_Data_Constructor;
using Random = UnityEngine.Random;

namespace YUART.Scripts.Galaxy_Manager.Systems
{
    /// <summary>
    /// Class, that handles planets generation.
    /// </summary>
    public sealed class PlanetGenerator
    {
        private const int StarMassForOnePlanet = 10000;
        
        private readonly Stack<Entity> _stars;
        private readonly GalaxyManager _galaxyManager;

        private readonly PlanetType[] _planetTypes = FastEnum.GetValues<PlanetType>().ToArray();

        private EntityManager _entityManager;

        public PlanetGenerator(GalaxyManager galaxyManager, Stack<Entity> stars)
        {
            _stars = stars;
            _galaxyManager = galaxyManager;
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        /// <summary>
        /// Generate planets.
        /// </summary>
        /// <returns>Generated planets.</returns>
        public Stack<Entity> GeneratePlanets()
        {
            var planets = new Stack<Entity>();

            while (_stars.Count > 0)
            {
                var star = _stars.Pop();

                var spaceObjectComponent = _entityManager.GetComponentData<SpaceObject>(star);
                
                if (!spaceObjectComponent.canHaveSystem || spaceObjectComponent.mass < StarMassForOnePlanet) continue;

                var countOfPlanets = Mathf.RoundToInt(spaceObjectComponent.mass / StarMassForOnePlanet);

                for (int i = 0; i < countOfPlanets; i++)
                {
                    planets.Push(SpawnPlanet(_entityManager.GetComponentData<Translation>(star).Value));
                }
            }

            return planets;
        }

        private Entity SpawnPlanet(Vector3 parentPosition)
        {
            return SpawnPlanetObject(_planetTypes.GetRandomElement(), parentPosition);
        }

        private Entity SpawnPlanetObject(PlanetType type, Vector3 parentPosition)
        {
            UpdateGalaxyData();
            
            var planet = _entityManager.Instantiate(GalaxyManagerSingleton.Instance.PlanetEntity);
            
            var template = TemplateDataConstructorSingleton.Instance.GetTemplateForType<SpaceObjectTemplate, PlanetType>(type);

            SetStarTransforms(planet, template, parentPosition);

            var (newStarData, newSpaceObjectData) = PrepareStarComponent(type, template);

            _entityManager.SetComponentData(planet, newStarData);

            _entityManager.SetComponentData(planet, newSpaceObjectData);

            SetStarColor(template, planet);

            return planet;
        }

        private void UpdateGalaxyData()
        {
            _galaxyManager.IncrementPlanetsCount();
        }

        private void SetStarTransforms(Entity star, SpaceObjectTemplate template, Vector3 parentPosition)
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
                    Value = GetRandomPositionInGalaxy(parentPosition)
                }
            );

            entityManager.SetComponentData(star, new Rotation
                {
                    Value = quaternion.Euler(GetRandomRotation())
                }
            );
        }

        private Vector3 GetRandomPositionInGalaxy(Vector3 parentPosition)
        {
            var position = Random.insideUnitCircle * 1000f + new Vector2(parentPosition.x, parentPosition.z);
            return new Vector3(position.x, parentPosition.y + Random.Range(-100f, 100f), position.y);
        }

        private Vector3 GetRandomRotation()
        {
            return new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }

        private (Planet.Components.Planet, SpaceObject) PrepareStarComponent(PlanetType type, SpaceObjectTemplate template)
        {
            var newStarData = CreateTypeDataFromTemplate(type);

            var newSpaceObjectData = TemplateDataConstructorSingleton.Instance.CreateSpaceObjectDataFromTemplate(type.ToName(), template);

            return (newStarData, newSpaceObjectData);
        }

        private Planet.Components.Planet CreateTypeDataFromTemplate(PlanetType type)
        {
            return new Planet.Components.Planet
            {
                type = type
            };
        }

        private void SetStarColor(SpaceObjectTemplate template, Entity star)
        {
            var starColor = template.Color;

            _entityManager.SetComponentData(star, new SpaceObjectColor
            {
                value = starColor.ConvertToFloat4()
            });
        }
    }
}