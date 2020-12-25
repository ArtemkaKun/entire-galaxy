using System.Collections.Generic;
using UnityEngine;
using YUART.Scripts.Star.Enums;

namespace YUART.Scripts.Galaxy_Manager.Data_Containers.Templates.Star_Templates
{
    /// <summary>
    /// Scriptable object, that stores star types' data.
    /// </summary>
    [CreateAssetMenu(fileName = "ScriptableObject/StartTemplatesData", menuName = "ScriptableObject/StartTemplatesData", order = 0)]
    public sealed class StarTemplatesData : TemplatesDataContainer<StarTypeTemplate, StarType>
    {
        [SerializeField] private StarTypeTemplate typeO;
        [SerializeField] private StarTypeTemplate typeB;
        [SerializeField] private StarTypeTemplate typeA;
        [SerializeField] private StarTypeTemplate typeF;
        [SerializeField] private StarTypeTemplate typeG;
        [SerializeField] private StarTypeTemplate typeK;
        [SerializeField] private StarTypeTemplate typeM;
        [SerializeField] private StarTypeTemplate typeT;
        [SerializeField] private StarTypeTemplate typeW;
        [SerializeField] private StarTypeTemplate typeR;
        [SerializeField] private StarTypeTemplate typeN;

        /// <summary>
        /// Initialize ScriptableObject's variables.
        /// </summary>
        public override void Initialize()
        {
            _spaceObjectsTemplates = new Dictionary<StarType, StarTypeTemplate>
            {
                {StarType.O, typeO},
                {StarType.B, typeB},
                {StarType.A, typeA},
                {StarType.F, typeF},
                {StarType.G, typeG},
                {StarType.K, typeK},
                {StarType.M, typeM},
                {StarType.T, typeT},
                {StarType.W, typeW},
                {StarType.R, typeR},
                {StarType.N, typeN}
            };
        }
    }
}