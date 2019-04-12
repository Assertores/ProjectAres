using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    [CreateAssetMenu(menuName = "Character")]
    public class CharacterData: ScriptableObject {
        public GameObject m_model;
        public Sprite m_icon;
        public string m_name;
        public GameObject m_sMG;
        public GameObject m_rocked;

        [HideInInspector]
        public IWeapon[] m_weapons = new IWeapon[2];
    }
}