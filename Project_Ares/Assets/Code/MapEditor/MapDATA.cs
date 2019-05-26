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

        public MapDATA Copy() {
            MapDATA value = new MapDATA();

            value.hideFlags = this.hideFlags;
            value.name = this.name;

            value.m_background = this.m_background;
            value.m_data = new d_mapData[this.m_data.Length];
            for(int i = 0; i < this.m_data.Length; i++) {
                value.m_data[i] = this.m_data[i];
            }
            value.m_globalLight = this.m_globalLight;
            value.m_music = this.m_music;
            value.m_size = this.m_size;

            value.p_background = new Sprite[this.p_background.Length];
            for(int i = 0; i < this.p_background.Length; i++) {
                value.p_background[i] = this.p_background[i];
            }
            value.p_colors = new Color[this.p_colors.Length];
            for(int i = 0; i < this.p_colors.Length; i++) {
                value.p_colors[i] = this.p_colors[i];
            }
            value.p_forground = new Sprite[this.p_forground.Length];
            for(int i = 0; i < this.p_forground.Length; i++) {
                value.p_forground[i] = this.p_forground[i];
            }
            value.p_music = new AudioClip[this.p_music.Length];
            for(int i = 0; i < this.p_music.Length; i++) {
                value.p_music[i] = this.p_music[i];
            }
            value.p_props = new d_prop[this.p_props.Length];
            for(int i = 0; i < this.p_props.Length; i++) {
                value.p_props[i] = this.p_props[i];
            }
            value.p_size = new Vector2[this.p_size.Length];
            for(int i = 0; i < this.p_size.Length; i++) {
                value.p_size[i] = this.p_size[i];
            }
            value.p_stage = new Sprite[this.p_stage.Length];
            for(int i = 0; i < this.p_stage.Length; i++) {
                value.p_stage[i] = this.p_stage[i];
            }

            return value;
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