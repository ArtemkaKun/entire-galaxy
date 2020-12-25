using System.Collections.Generic;
using UnityEngine;
using YUART.Scripts.Planet.Enums;

namespace YUART.Scripts.Galaxy_Manager.Data_Containers.Templates.Planet_Templates
{
    [CreateAssetMenu(fileName = "ScriptableObject/PlanetTemplatesData", menuName = "ScriptableObject/PlanetTemplatesData", order = 0)]
    public sealed class PlanetTemplatesData : TemplatesDataContainer<SpaceObjectTemplate, PlanetType>
    {
        [SerializeField] private SpaceObjectTemplate typeE;
        [SerializeField] private SpaceObjectTemplate typeG;

        public override void Initialize()
        {
            _spaceObjectsTemplates = new Dictionary<PlanetType, SpaceObjectTemplate>
            {
                {PlanetType.E, typeE},
                {PlanetType.G, typeG}
            };
        }
    }
}