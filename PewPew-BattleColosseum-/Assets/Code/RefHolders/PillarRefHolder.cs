using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC {
    public class PillarRefHolder : MonoBehaviour {

        public TextMeshProUGUI r_points;
        public SpriteRenderer r_light;

        //calculation variables
        [HideInInspector] public Vector3 m_holderPos;
        [HideInInspector] public Vector3 m_middlePos;
        [HideInInspector] public Vector3 m_targetPos;
        [HideInInspector] public int m_previousMatchPoints;
    }
}