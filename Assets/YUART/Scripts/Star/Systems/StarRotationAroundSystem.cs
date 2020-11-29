using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace YUART.Scripts.Star.Systems
{
    /// <summary>
    /// System, that controls stars' rotation around center of the galaxy.
    /// </summary>
    public sealed class StarRotationAroundSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, ref Rotation rotation, in LocalToWorld localToWorld, in Components.Star starData) =>
            {
                var rotationStep = Quaternion.AngleAxis(20 * deltaTime / starData.mass, localToWorld.Up);
                
                translation.Value = rotationStep * translation.Value;
                rotation.Value = rotationStep;
                
            }).WithBurst().ScheduleParallel();
        }
    }
}