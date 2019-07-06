using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PPBC {
    [CustomEditor(typeof(PropMaker))]
    public class PropMakerEditor : Editor {

        public override void OnInspectorGUI() {

            PropMaker myTarget = (PropMaker)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Create Prop")) {
                if(myTarget.m_col == null || myTarget.m_sprite == null) {
                    Debug.Log("sprite or collider not set");
                    return;
                }
                if(myTarget.m_name == "") {
                    Debug.Log("no name");
                    return;
                }

                //--> references are set <--

                PropData value = ScriptableObject.CreateInstance<PropData>();

                value.name = myTarget.m_name;
                value.m_image = myTarget.m_sprite.sprite;
                value.m_collider = myTarget.m_col.GetPath(0);

                AssetDatabase.CreateAsset(value, "Assets/Prefabs/Map/p_" + value.name + ".asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = value;
            }
        }
    }
}