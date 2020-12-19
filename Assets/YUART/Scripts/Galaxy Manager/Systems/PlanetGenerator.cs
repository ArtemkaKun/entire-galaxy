using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using YUART.Scripts.Space_Objects.Components;

namespace YUART.Scripts.Galaxy_Manager.Systems
{
    /// <summary>
    /// Class, that handles planets generation.
    /// </summary>
    public sealed class PlanetGenerator
    {
        private const int StarMassForOnePlanet = 10000;
        
        private readonly Stack<Entity> _stars;
        
        private EntityManager _entityManager;

        public PlanetGenerator(Stack<Entity> stars)
        {
            _stars = stars;
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
                    planets.Push(SpawnPlanet());
                }
            }

            return planets;
        }

        private Entity SpawnPlanet()
        {
            return _entityManager.Instantiate(GalaxyManagerSingleton.Instance.PlanetEntity);
        }
    }
}