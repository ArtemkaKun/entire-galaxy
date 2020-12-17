using System;
using UnityEngine;

namespace YUART.Scripts.Galaxy_Manager.DataContainers.Templates.StarTemplates
{
    /// <summary>
    /// Struct, that store star's type template data.
    /// </summary>
    [Serializable]
    public struct StarTypeTemplate : ISpaceObjectTemplate
    {
        public Vector2 TemperatureRange => temperatureRange;

        public Vector2 MassRange => massRange;
        
        public Vector2 SizeRange => sizeRange;

        public Color32 Color => color;

        public bool CanHaveSystem => canHavePlanets;
        
        [SerializeField] private Vector2 temperatureRange;
        [SerializeField] private Vector2 massRange;
        [SerializeField] private Vector2 sizeRange;
        [SerializeField] private Color32 color;
        [SerializeField] private bool canHavePlanets;
    }
}