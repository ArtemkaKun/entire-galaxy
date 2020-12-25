using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using YUART.Scripts.Galaxy_Manager.Data_Containers.Templates;
using YUART.Scripts.Space_Objects.Components;
using YUART.Scripts.Utilities;
using YUART.Scripts.Utilities.Template_Data_Constructor;
using Random = UnityEngine.Random;

namespace YUART.Scripts.Galaxy_Manager.Systems
{
    /// <summary>
    /// Base class for space objects generator system.
    /// </summary>
    public class SpaceObjectGenerator
    {
        protected readonly TemplateDataConstructor templateDataConstructor;

        private readonly float _areaSize;
        private readonly Vector2 _yAxisRange;

        protected SpaceObjectGenerator(float areaSize, Vector2 yAxisRange)
        {
            _areaSize = areaSize;
            _yAxisRange = yAxisRange;
            templateDataConstructor = TemplateDataConstructorSingleton.Instance;
        }

        protected void SetSpaceObjectTransforms(Entity spaceObjectEntity, SpaceObjectTemplate template, Vector3 parentPosition)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            entityManager.AddComponent<NonUniformScale>(spaceObjectEntity);

            entityManager.SetComponentData(spaceObjectEntity, new NonUniformScale
                {
                    Value = template.SizeRange.GetRandomValueFromRange()
                }
            );

            entityManager.SetComponentData(spaceObjectEntity, new Translation
                {
                    Value = GetRandomPositionInGalaxy(_areaSize, parentPosition, _yAxisRange)
                }
            );

            entityManager.SetComponentData(spaceObjectEntity, new Rotation
                {
                    Value = quaternion.Euler(GetRandomRotation())
                }
            );
        }

        private Vector3 GetRandomPositionInGalaxy(float areaSize, Vector3 parentPosition, Vector2 yAxisRange)
        {
            var position = Random.insideUnitCircle * areaSize + new Vector2(parentPosition.x, parentPosition.z);
            return new Vector3(position.x, parentPosition.y + yAxisRange.GetRandomValueFromRange(), position.y);
        }

        private Vector3 GetRandomRotation()
        {
            return new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }

        protected SpaceObject PrepareSpaceObjectComponent(string typeName, SpaceObjectTemplate template, Vector3 parentPosition)
        {
            return templateDataConstructor.CreateSpaceObjectDataFromTemplate(typeName, template, parentPosition);
        }
    }
}