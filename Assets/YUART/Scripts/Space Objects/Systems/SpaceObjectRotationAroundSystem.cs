using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using YUART.Scripts.Space_Objects.Components;

namespace YUART.Scripts.Space_Objects.Systems
{
    /// <summary>
    /// System, that controls stars' rotation around center of the galaxy.
    /// </summary>
    public sealed class SpaceObjectRotationAroundSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, ref Rotation rotation, in LocalToWorld localToWorld, in SpaceObject objectData) =>
            {
                var rotationStep = Quaternion.AngleAxis(1000 * deltaTime / objectData.mass, localToWorld.Up);
                
                translation.Value = rotationStep * translation.Value;

            }).WithBurst().ScheduleParallel();
        }
    }
}