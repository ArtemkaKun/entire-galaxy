using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.Data_Containers.Templates;
using YUART.Scripts.Galaxy_Manager.Data_Containers.Templates.Planet_Templates;
using YUART.Scripts.Galaxy_Manager.Data_Containers.Templates.Star_Templates;
using YUART.Scripts.Planet.Enums;
using YUART.Scripts.Space_Objects.Components;
using YUART.Scripts.Star.Enums;

namespace YUART.Scripts.Utilities.Template_Data_Constructor
{
    /// <summary>
    /// Class, that contains methods to construct data from the template.
    /// </summary>
    public sealed class TemplateDataConstructor : MonoBehaviour
    {
        [SerializeField] private StarTemplatesData starTemplates;
        [SerializeField] private PlanetTemplatesData planetTemplatesData;

        private Dictionary<Type, ScriptableObject> _templatesByType;
        private EntityManager _entityManager;

        private void Awake()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            starTemplates.Initialize();
            planetTemplatesData.Initialize();

            _templatesByType = new Dictionary<Type, ScriptableObject>
            {
                {typeof(StarType), starTemplates},
                {typeof(PlanetType), planetTemplatesData}
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
            where T : class
            where TS : Enum
        {
            var matchedTemplate = _templatesByType[typeOfObject.GetType()];

            return ((TemplatesDataContainer<T, TS>) matchedTemplate).GetTemplate(typeOfObject);
        }

        /// <summary>
        /// Construct SpaceObject object from template.
        /// </summary>
        /// <param name="typeName">Name of type.</param>
        /// <param name="template">Template.</param>
        /// <param name="parentPosition">Position of the parent.</param>
        /// <returns>SpaceObject object.</returns>
        public SpaceObject CreateSpaceObjectDataFromTemplate(string typeName, SpaceObjectTemplate template, float3 parentPosition)
        {
            return new SpaceObject
            {
                name = NameGenerator.GetRandomNameFromType(typeName),
                mass = template.MassRange.GetRandomValueFromRange(),
                gravityCenter = parentPosition,
                canHaveSystem = template.CanHaveSystem
            };
        }

        public void SetSpaceObjectColor(SpaceObjectTemplate template, Entity star)
        {
            _entityManager.SetComponentData(star, new SpaceObjectColor
            {
                value = template.Color.ConvertToFloat4()
            });
        }
    }
}