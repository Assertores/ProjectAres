using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class PropMaker : MonoBehaviour {

#if UNITY_EDITOR

        public string m_name;
        public SpriteRenderer m_sprite;
        public PolygonCollider2D m_col;

#endif
    }
}