using Unity.Entities;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.Systems;

namespace YUART.Scripts.Star.Systems
{
    /// <summary>
    /// System, that handles planets generate algorithm for stars.
    /// </summary>
    public class PlanetsGenerationSystem : SystemBase
    {
        private const int StarMassForOnePlanet = 10000;
        
        private EndSimulationEntityCommandBufferSystem _endSimCommandBufferSystem;

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            _endSimCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
    
        protected override void OnUpdate()
        {
            var commandBuffer = _endSimCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            
            var planetEntity = GalaxyManagerSingleton.Instance.PlanetEntity;
            
            Entities.WithNone<HavePlanetsTag>().ForEach((Entity entity, int entityInQueryIndex, in Components.Star starData) =>
            {
                if (!starData.canHavePlanets)
                {
                    commandBuffer.AddComponent<HavePlanetsTag>(entityInQueryIndex, entity);
                
                    return;
                }

                for (int i = 0; i < Mathf.RoundToInt(starData.spaceBodyData.mass / StarMassForOnePlanet); i++)
                {
                    commandBuffer.Instantiate(entityInQueryIndex, planetEntity);
                }

            }).WithBurst().ScheduleParallel();
        
            _endSimCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
