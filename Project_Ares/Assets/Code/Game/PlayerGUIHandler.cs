using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectAres {
    public class PlayerGUIHandler : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] Image m_characterIconRef;
        [SerializeField] TextMeshProUGUI m_characterNameRef;
        [SerializeField] GameObject m_characterSelector;
        [SerializeField] Image m_weaponIconRef;
        [SerializeField] TextMeshProUGUI m_playerNameRef;
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

        public void SetCharChangeActive(bool activate) {
            m_characterSelector.SetActive(activate);
        }
    }
}