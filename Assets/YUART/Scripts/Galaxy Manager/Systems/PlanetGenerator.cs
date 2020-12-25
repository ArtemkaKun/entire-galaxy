using System.Collections.Generic;
using System.Linq;
using FastEnumUtility;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.Data_Containers.Templates;
using YUART.Scripts.Planet.Enums;
using YUART.Scripts.Space_Objects.Components;
using YUART.Scripts.Utilities;
using YUART.Scripts.Utilities.Template_Data_Constructor;

namespace YUART.Scripts.Galaxy_Manager.Systems
{
    /// <summary>
    /// Class, that handles planets generation.
    /// </summary>
    public sealed class PlanetGenerator : SpaceObjectGenerator
    {
        private const int StarMassForOnePlanet = 10000;
        private const PlanetType ExoPlanetType = PlanetType.E;
        private const int MinDistanceFromStarForExoPlanet = 35;
        private const int MaxDistanceFromStarForExoPlanet = 350;
        private const int MinMassForExoPlanet = 3;

        private readonly Stack<Entity> _stars;
        private readonly GalaxyManager _galaxyManager;

        private readonly PlanetType[] _planetTypes = FastEnum.GetValues<PlanetType>().ToArray();

        private EntityManager _entityManager;

        public PlanetGenerator(GalaxyManager galaxyManager, Stack<Entity> stars, float areaSize, Vector2 yAxisRange) : base(areaSize, yAxisRange)
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

            SetSpaceObjectTransforms(planet, template, parentPosition);

            _entityManager.SetComponentData(planet, PreparePlanetComponents(type));

            _entityManager.SetComponentData(planet, PrepareSpaceObjectComponent(type.ToName(), template, parentPosition));

            templateDataConstructor.SetSpaceObjectColor(template, planet);

            if (CheckIfPlanetIsExoPlanet(type, planet, parentPosition))
            {
                AddNewExoPlanetToData();
            }

            return planet;
        }

        private void UpdateGalaxyData()
        {
            _galaxyManager.IncrementPlanetsCount();
        }

        private Planet.Components.Planet PreparePlanetComponents(PlanetType type)
        {
            return CreateTypeDataFromTemplate(type);
        }

        private Planet.Components.Planet CreateTypeDataFromTemplate(PlanetType type)
        {
            return new Planet.Components.Planet
            {
                type = type
            };
        }

        private bool CheckIfPlanetIsExoPlanet(PlanetType type, Entity planet, Vector3 parentPosition)
        {
            if (type != ExoPlanetType) return false;

            var distanceFromStar = Vector3.Distance(_entityManager.GetComponentData<Translation>(planet).Value, parentPosition);

            if (distanceFromStar < MinDistanceFromStarForExoPlanet || distanceFromStar > MaxDistanceFromStarForExoPlanet) return false;

            return _entityManager.GetComponentData<SpaceObject>(planet).mass > MinMassForExoPlanet;
        }

        private void AddNewExoPlanetToData()
        {
            _galaxyManager.IncrementExoPlanetsCount();
        }
    }
}