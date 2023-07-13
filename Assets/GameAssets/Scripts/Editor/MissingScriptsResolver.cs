using System.Linq;
using UnityEditor;
using UnityEngine;
using static System.StringComparison;

namespace SteampunkChess
{
    public static class MissingScriptsResolver
    {
        [MenuItem("Utility/Find Missing Scripts In Project")]
        public static void FindMissingScriptsInProject()
        {
            string[] paths = AssetDatabase
                .GetAllAssetPaths()
                .Where(path => path.EndsWith(".prefab", OrdinalIgnoreCase))
                .ToArray();

            foreach (var path in paths)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                foreach (var component in prefab.GetComponentsInChildren<Component>())
                {
                    if (component == null)
                    {
                        Debug.Log("Missing script, click at the message to show it!", prefab);
                        break;
                    }
                }
            }
        }

        [MenuItem("Utility/Find Missing Scripts In Scene")]
        public static void FindMissingScriptsInScene()
        { 
            foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
            {
                foreach (Component component in gameObject.GetComponentsInChildren<Component>())
                {
                    if (component == null)
                    {
                        Debug.Log("Missing script, click at the message to show it!", gameObject);
                        break;
                    }
                }
            }
        }

    }
}
