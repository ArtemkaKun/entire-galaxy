using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using YUART.Scripts.Space_Objects.Components;

namespace YUART.Scripts.Space_Objects.Systems
{
    /// <summary>
    /// System, that controls space object's rotation around center of the gravity.
    /// </summary>
    public sealed class SpaceObjectRotationAroundSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, ref Rotation rotation, in LocalToWorld localToWorld, in SpaceObject objectData) =>
            {
                translation.Value = math.mul(Quaternion.AngleAxis(100 * deltaTime / objectData.mass, localToWorld.Up), translation.Value - objectData.gravityCenter) + objectData.gravityCenter;
            }).WithBurst().ScheduleParallel();
        }
    }
}