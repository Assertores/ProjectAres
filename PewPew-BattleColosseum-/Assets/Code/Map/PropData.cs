using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [CreateAssetMenu(menuName = "Map/Prop")]
    public class PropData : ScriptableObject {

        public Sprite m_image;
        public Vector2[] m_collider;
    }

    [System.Serializable]
    public class PropJSON {
        public string m_image;
        public Vector2[] m_collider;
    }
}