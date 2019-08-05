using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC {

    [System.Serializable]
    public struct d_CharRefs {
        public SpriteRenderer[] r_backgrounds;
        public SpriteRenderer r_character;
        public TextMeshProUGUI r_charName;
    }

    public class GameTransitionRefHolder : MonoBehaviour {

        public Animator r_anim;

        public d_CharRefs[] r_chars;

        public TextMeshProUGUI r_name;
        public TextMeshProUGUI r_flavour;
    }
}