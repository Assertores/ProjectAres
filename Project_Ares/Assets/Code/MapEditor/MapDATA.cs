using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PPBC {
    public enum e_objType { BACKGROUND, PROP, STAGE, PLAYERSTART, LIGHT, FORGROUND, BORDER, GLOBALLIGHT, MUSIC, SIZE }

    [System.Serializable]
    public struct d_mapData {
        public e_objType type;
        public int index;
        public Vector2 position;
        public float rotation;
        public Vector2 scale;
    }

    [System.Serializable]
    public struct d_prop {
        public Sprite m_sprite;
        public Vector2[] m_collider;
    }

    [CreateAssetMenu(menuName = "Map")]
    public class MapDATA : ScriptableObject {
        
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

        public Vector2[] p_size;
        public Sprite[] p_background;
        public Color[] p_colors;
        public AudioClip[] p_music;

        public d_prop[] p_props;
        public Sprite[] p_stage;
        public Sprite[] p_forground;

        public int m_size;
        public int m_background;
        public int m_globalLight;
        public int m_music;

        public d_mapData[] m_data;
        
    }
}