﻿using Unity.Collections;
using Unity.Entities;
using YUART.Scripts.Star.Enums;

namespace YUART.Scripts.Star.Components
{
    /// <summary>
    /// Component, that handles star's data.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct Star : IComponentData
    {
        public StarType type;
        public float temperature;
        public float mass;
        public FixedString32 name;
        public bool canHavePlanets;
    }
}