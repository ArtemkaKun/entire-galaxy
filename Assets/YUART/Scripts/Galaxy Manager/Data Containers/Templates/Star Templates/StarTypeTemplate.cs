using System;
using UnityEngine;

namespace YUART.Scripts.Galaxy_Manager.Data_Containers.Templates.Star_Templates
{
    /// <summary>
    /// Struct, that store star's type template data.
    /// </summary>
    [Serializable]
    public class StarTypeTemplate : SpaceObjectTemplate
    {
        public Vector2 TemperatureRange => temperatureRange;

        [SerializeField] private Vector2 temperatureRange;
    }
}