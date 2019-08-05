using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

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
                    EditorUtility.SetDirty(myTarget);
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
                bool mapExists = true;

                MapData value = (MapData)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Map/m_" + myTarget.m_name + ".asset", typeof(MapData));
                if (!value) {
                    mapExists = false;
                }
                value = MapHandler.s_singelton.CreateMapData(myTarget.m_name);

                if(myTarget.m_name != "") {
                    value.m_name = myTarget.m_name;
                }

                byte[] pngShot = ScreenshotCam.TakeScreenShot();
                File.WriteAllBytes("Assets/Prefabs/Map/m_" + value.m_name + "_icon.png", pngShot);

                if(!mapExists)
                    AssetDatabase.CreateAsset(value, "Assets/Prefabs/Map/m_" + value.m_name + ".asset");

                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = value;
            }
        }
    }
}