using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [CreateAssetMenu(menuName = "Map")]
    public class MapDATA : ScriptableObject {
        [System.Serializable]
        public struct d_mapData {
            public int index;
            public Vector2 position;
            public float rotation;
            public Vector2 scale;
        }
        [System.Serializable]
        public struct d_mapLights {
            public Vector2 position;
            public Color color;
        }
        [System.Serializable]
        public struct d_mapPlayerStart {
            public Vector2 position;
            public int team;
        }

        public Sprite[] p_background;
        public GameObject[] p_props;
        public Sprite[] p_stage;
        public Sprite[] p_forground;
        public GameObject p_light;
        public GameObject p_laserBariar;

        public Vector2 m_size;
        public int m_background;
        public Color m_globalLight;
        public d_mapData[] m_props;
        public d_mapData[] m_stage;
        public d_mapData[] m_forground;
        public d_mapLights[] m_lights;
        public d_mapPlayerStart[] m_playerStarts;
        public d_mapData[] m_border = new d_mapData[4];
        public AudioClip m_backgroundMusic;
    }
}