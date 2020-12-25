using UnityEngine;
using YUART.Scripts.Utilities;
using YUART.Scripts.Utilities.Template_Data_Constructor;

namespace YUART.Scripts.Galaxy_Manager.Systems
{
    /// <summary>
    /// Base class for space objects generator system.
    /// </summary>
    public class SpaceObjectGenerator
    {
        protected readonly TemplateDataConstructor _templateDataConstructor;

        public SpaceObjectGenerator()
        {
            _templateDataConstructor = TemplateDataConstructorSingleton.Instance;
        }

        protected Vector3 GetRandomPositionInGalaxy(float areaSize, Vector3 parentPosition, Vector2 yAxisRange)
        {
            var position = Random.insideUnitCircle * areaSize + new Vector2(parentPosition.x, parentPosition.z);
            return new Vector3(position.x, parentPosition.y + yAxisRange.GetRandomValueFromRange(), position.y);
        }

        protected Vector3 GetRandomRotation()
        {
            return new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }
    }
}