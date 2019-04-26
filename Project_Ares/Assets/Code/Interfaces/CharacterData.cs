using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    [CreateAssetMenu(menuName = "Character")]
    public class CharacterData: ScriptableObject {

        /// <summary>
        /// das modell des characters als Prefab
        /// </summary>
        public GameObject m_model;

        /// <summary>
        /// das icon des characters, dass in der GUI angezeigt werden soll
        /// </summary>
        public Sprite m_icon;

        /// <summary>
        /// der name des characters in der GUI
        /// </summary>
        public string m_name;

        /// <summary>
        /// dass Prefab der smg für diesen character
        /// </summary>
        public GameObject m_sMG;

        /// <summary>
        /// das Prefab für den rocked launcher für diesen character
        /// </summary>
        public GameObject m_rocked;

        /// <summary>
        /// fieleicht Legacy, da die interfaces nicht auf die initialisierten waffen zeigen
        /// </summary>
        [HideInInspector]
        public IWeapon[] m_weapons = new IWeapon[2];
    }
}