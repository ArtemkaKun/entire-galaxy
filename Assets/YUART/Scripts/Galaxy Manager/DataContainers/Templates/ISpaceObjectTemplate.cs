using UnityEngine;

namespace YUART.Scripts.Galaxy_Manager.DataContainers.Templates
{
    /// <summary>
    /// Interface, that provides methods for SpaceObject template.
    /// </summary>
    public interface ISpaceObjectTemplate
    {
        public Vector2 MassRange { get; }
        
        public bool CanHaveSystem { get; }
    }
}