using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Prince
{
    /// <summary>
    /// Component to assert that there is only one instance of a given prefab.
    ///
    /// This component is only used in tests scenes to remove repeated instances of GameManagers. Usual scenes
    /// don't have a GameManagers instance, but inherit from previous levels and those from first level. Test
    /// scenes instead have their own GameManager because those scenes are not executed in an specific order,
    /// so some deduplication is needed.
    /// </summary>
    public class Deduplicator : MonoBehaviour
    {
        [Tooltip("Tag of game object that should be instanced just once in the scene.")]
        [SerializeField] private string prefabTagThatShouldBeUnique;
        
        private void Start()
        {
            DestroyRepeatedInstances();
        }

        private void DestroyRepeatedInstances()
        {
            List<GameObject> instancedObjects =
                new List<GameObject>(GameObject.FindGameObjectsWithTag(prefabTagThatShouldBeUnique));
            if (instancedObjects.Count > 1)
            {
                // Be aware that we skip first instance to let it live.
                List<GameObject> repeatedInstancesToRemove = instancedObjects.GetRange(1, instancedObjects.Count - 1);
                foreach (GameObject instance in repeatedInstancesToRemove)
                {
                    Destroy(instance);
                }
            }
        }
    }
}