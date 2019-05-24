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

        public GameObject[] p_background;
        public GameObject[] p_props;
        public GameObject[] p_stage;
        public GameObject[] p_forground;
        public GameObject p_light;
        public GameObject p_laserBariar;

        public Vector2 m_size;
        public int m_background;
        public d_mapData[] m_props;
        public d_mapData[] m_stage;
        public d_mapData[] m_forground;
        public d_mapLights[] m_lights;
        public d_mapPlayerStart[] m_playerStarts;
        public d_mapData[] m_border;
        public AudioClip m_backgroundMusic;
    }
}