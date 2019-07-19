using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PPBC {
    [CustomEditor(typeof(BackgroundMaker))]
    public class BackgroundMakerEditor : Editor {

        public override void OnInspectorGUI() {

            BackgroundMaker myTarget = (BackgroundMaker)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Create Background")) {
                if (myTarget.m_col == null || myTarget.m_sprite == null) {
                    Debug.Log("sprite or collider not set");
                    return;
                }
                if (myTarget.m_name == "") {
                    Debug.Log("no name");
                    return;
                }

                //--> references are set <--

                BackgroundData value = ScriptableObject.CreateInstance<BackgroundData>();

                value.name = myTarget.m_name;
                value.m_image = myTarget.m_sprite.sprite;
                value.m_position = myTarget.m_col.offset;
                value.m_size = myTarget.m_col.size;

                AssetDatabase.CreateAsset(value, "Assets/Prefabs/Map/b_" + value.name + ".asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = value;
            }
        }
    }
}