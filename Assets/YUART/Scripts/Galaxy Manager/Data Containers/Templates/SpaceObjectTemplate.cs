using System;
using UnityEngine;

namespace YUART.Scripts.Galaxy_Manager.Data_Containers.Templates
{
    /// <summary>
    /// Class, that handles template data for SpaceObject.
    /// </summary>
    [Serializable]
    public class SpaceObjectTemplate
    {
        public Vector2 MassRange => massRange;

        public Vector2 SizeRange => sizeRange;

        public Color32 Color => color;

        public bool CanHaveSystem => canHaveSystem;

        [SerializeField] private Vector2 massRange;
        [SerializeField] private Vector2 sizeRange;
        [SerializeField] private Color32 color;
        [SerializeField] private bool canHaveSystem;
    }
}