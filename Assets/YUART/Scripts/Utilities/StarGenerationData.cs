using UnityEngine;
using YUART.Scripts.Galaxy_Manager.Data_Containers.Templates.Star_Templates;
using YUART.Scripts.Galaxy_Manager.DataContainers;

namespace YUART.Scripts.Utilities
{
    /// <summary>
    /// Class, that handles data for star generation (needed for test).
    /// </summary>
    [CreateAssetMenu(fileName = "ScriptableObject/StarGenerationData", menuName = "ScriptableObject/StarGenerationData", order = 0)]
    public class StarGenerationData : ScriptableObject
    {
        public GameObject StarPrefab => starPrefab;
        
        public StarTemplatesData StarTemplatesData => starTemplatesData;
        
        [SerializeField] private GameObject starPrefab;
        [SerializeField] private StarTemplatesData starTemplatesData;
    }
}