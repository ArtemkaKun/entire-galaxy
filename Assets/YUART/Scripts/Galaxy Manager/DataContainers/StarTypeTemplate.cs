using System;
using UnityEngine;

namespace YUART.Scripts.Galaxy_Manager.DataContainers
{
    /// <summary>
    /// Struct, that store star's type template data.
    /// </summary>
    [Serializable]
    public struct StarTypeTemplate
    {
        public Vector2 TemperatureRange => temperatureRange;

        public Vector2 MassRange => massRange;
        
        public Vector2 SizeRange => sizeRange;

        public Color32 Color => color;

        public bool CanHavePlanets => canHavePlanets;
        
        [SerializeField] private Vector2 temperatureRange;
        [SerializeField] private Vector2 massRange;
        [SerializeField] private Vector2 sizeRange;
        [SerializeField] private Color32 color;
        [SerializeField] private bool canHavePlanets;
    }
}