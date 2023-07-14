using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Hypercasual.Editor
{
    public class SceneSwitcher : EditorWindow
    {
        private  List<string> scenes = new List<string>();

        private ulong iterator;

  
        [MenuItem("Window/General/SceneSwitcher")]
        private static void OpenWindow()
        {
            GetWindow<SceneSwitcher>().Show();
        }


        private void CheckForScenesInBuildSettings()
        {

            foreach (var editorScene in EditorBuildSettings.scenes)
            {

                SceneAsset scn = AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
                scenes.Add(scn.name);
            }

        }

        private void OnEnable()
        {
            CheckForScenesInBuildSettings();
        }
        void OnInspectorUpdate()
        {
            Repaint();
        }

        private Color GetButtonColor()
        {
            iterator++;
            return Color.HSVToRGB(Mathf.Sin(iterator) * .225f + .325f, 1, 1);
        }


        protected  void OnGUI()
        {
       
            DrawSceneButtons();
        }


        private void DrawSceneButtons()
        {
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {

                Rect rt = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(30));

                if (GUI.Button(rt, scenes[i]))
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path);
                }
            }
        }
    }
}
