using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace PPBC {
    [CustomEditor(typeof(MapMaker))]
    public class MapMakerEditor : Editor {

        bool h_inStartUp = false;
        public override void OnInspectorGUI() {
            MapMaker myTarget = (MapMaker)target;

            DrawDefaultInspector();

            if (!EditorApplication.isPlaying && !h_inStartUp) {
                if(GUILayout.Button("Start Editing")) {
                    h_inStartUp = true;
                    myTarget.m_editMap = true;
                    EditorApplication.ExecuteMenuItem("Edit/Play");
                    
                }
                return;
            }
            h_inStartUp = false;

            if (GUILayout.Button("Add Obj")) {
                MapHandler.s_singelton.LoadNewObj(myTarget.m_newObj, true);
            }

            if(GUILayout.Button("Save Map")) {
                MapData value = MapHandler.s_singelton.CreateMapData(myTarget.m_name);

                AssetDatabase.CreateAsset(value, "Assets/Prefabs/Map/m_" + value.name + ".asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = value;
            }
        }
    }
}