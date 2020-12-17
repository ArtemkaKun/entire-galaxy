using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using YUART.Scripts.Space_Objects.Components;

namespace YUART.Scripts.Space_Objects.Systems
{
    /// <summary>
    /// System, that handles self rotation of the star.
    /// </summary>
    public sealed class SpaceObjectSelfRotationSystem : SystemBase
    {
        private const float RotationAngle = 0.005f;

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.WithAll<SpaceObject>().ForEach((ref Rotation rotation) =>
            {
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(RotationAngle * deltaTime)));
            }).WithBurst().ScheduleParallel();
        }
    }
}