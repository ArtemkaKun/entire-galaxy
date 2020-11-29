﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace YUART.Scripts.Star.Systems
{
    /// <summary>
    /// System, that handles self rotation of the star.
    /// </summary>
    public sealed class StarSelfRotation : SystemBase
    {
        private const float RotationAngle = 0.005f;

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.WithAll<Components.Star>().ForEach((ref Rotation rotation) =>
            {
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(RotationAngle * deltaTime)));
            }).WithBurst().ScheduleParallel();
        }
    }
}