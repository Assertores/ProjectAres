using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [CreateAssetMenu(menuName = "Map/Background")]
    public class BackgroundData : ScriptableObject {

        public Sprite m_image;
        public Vector2 m_position;
        public Vector2 m_size;

    }

    [System.Serializable]
    public class BackgroundJSON {
        public string m_image;
        public Vector2 m_position;
        public Vector2 m_size;
    }
}