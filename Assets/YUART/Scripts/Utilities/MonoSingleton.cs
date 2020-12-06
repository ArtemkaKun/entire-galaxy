using System.Linq;
using UnityEngine;

namespace YUART.Scripts.Utilities
{
    /// <summary>
    /// Base class for Mono singleton.
    /// </summary>
    public class MonoSingleton<T>
    where T : MonoBehaviour
    {
        public static T Instance { get; } = GameObject.FindObjectsOfType<T>().FirstOrDefault();
    }
}