using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAres {
    public class PlayerGUIHandler : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] Image m_characterIconRef;
        [SerializeField] Text m_characterNameRef;
        [SerializeField] Image m_weaponIconRef;
        [SerializeField] Text m_playerNameRef;
        [SerializeField] Canvas m_canvas;

        #endregion
        #region MonoBehaviour

        void Start() {

        }
        
        void Update() {

        }

        #endregion

        public void ChangeCharacter(Sprite icon, string name) {
            m_characterIconRef.sprite = icon;
            m_characterNameRef.text = name;
        }

        public void ChangeWeapon(Sprite icon) {
            m_weaponIconRef.sprite = icon;
        }

        public void Reposition(float position) {
            transform.position = new Vector2 (m_canvas.pixelRect.width * position,transform.position.y);
        }


    }
}