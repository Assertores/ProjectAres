using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
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
    }
}