using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.DataContainers;
using YUART.Scripts.Galaxy_Manager.DataContainers.Templates;
using YUART.Scripts.Galaxy_Manager.DataContainers.Templates.StarTemplates;
using YUART.Scripts.Space_Objects.Components;
using YUART.Scripts.Star.Enums;

namespace YUART.Scripts.Utilities.Template_Data_Constructor
{
    /// <summary>
    /// Class, that contains methods to construct data from the template.
    /// </summary>
    public class TemplateDataConstructor : MonoBehaviour
    {
        [SerializeField] private StarTemplatesData starTemplates;

        private Dictionary<Type, ScriptableObject> _templatesByType;
        
        private void Awake()
        {
            starTemplates.Initialize();
            
            _templatesByType = new Dictionary<Type, ScriptableObject>
            {
                {typeof(StarType), starTemplates}
            };
        }

        /// <summary>
        /// Get template data from type of the object.
        /// </summary>
        /// <param name="typeOfObject">Type of object.</param>
        /// <typeparam name="T">Expected type of template.</typeparam>
        /// <typeparam name="TS">Type of object.</typeparam>
        /// <returns>Template data.</returns>
        public T GetTemplateForType<T, TS>(TS typeOfObject)
            where T : struct
            where TS : Enum
        {
            var matchedTemplate = _templatesByType[typeOfObject.GetType()];

            return ((ITemplatesContainer<T, TS>) matchedTemplate).GetTemplate(typeOfObject);
        }
    }
}