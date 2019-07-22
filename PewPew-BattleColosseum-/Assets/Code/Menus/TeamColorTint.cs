using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(SpriteRenderer))]
    public class TeamColorTint : MonoBehaviour {

        [SerializeField] int m_teamIndex;

        void Start() {
            SpriteRenderer ren = GetComponent<SpriteRenderer>();
            float alpha = ren.color.a;
            ren.color = DataHolder.s_teamColors[m_teamIndex];
            ren.color = new Color(ren.color.r, ren.color.g, ren.color.b, alpha);
            Destroy(this);
        }
    }
}