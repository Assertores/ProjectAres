using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC {
    public class GameTransitionRefHolder : MonoBehaviour {

        public Animator r_anim;

        public SpriteRenderer r_p1;
        public SpriteRenderer r_p2;
        public SpriteRenderer r_p3;
        public SpriteRenderer r_p4;

        [Tooltip("or 1 team")]
        public SpriteRenderer r_p1Background;
        [Tooltip("or 2 team")]
        public SpriteRenderer r_p2Background;
        public SpriteRenderer r_p3Background;
        public SpriteRenderer r_p4Background;

        public TextMeshProUGUI r_name;
        public TextMeshProUGUI r_flavour;
    }
}