using System;
using System.Collections.Generic;
using UnityEngine;

namespace YUART.Scripts.Galaxy_Manager.Data_Containers.Templates
{
    /// <summary>
    /// Base class for templates scriptable object.
    /// </summary>
    public abstract class TemplatesDataContainer<TTemplateType, TType> : ScriptableObject
        where TTemplateType : class
        where TType : Enum
    {
        protected Dictionary<TType, TTemplateType> _spaceObjectsTemplates;

        /// <summary>
        /// Initialize ScriptableObject's variables.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Get template for this type.
        /// </summary>
        /// <param name="spaceObjectType">Type of space object.</param>
        /// <returns>Template for specified type.</returns>
        public TTemplateType GetTemplate(TType spaceObjectType)
        {
            return _spaceObjectsTemplates[spaceObjectType];
        }
    }
}