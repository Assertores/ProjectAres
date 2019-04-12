using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class PlayerGUIHandler : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_icon;
        [SerializeField] Canvas m_canvas;

        #endregion
        #region MonoBehaviour

        void Start() {

        }
        
        void Update() {

        }

        #endregion

        public void Init(float position) {
            print("new Position is " + position + " (" + m_canvas.pixelRect.width * position + ")");
            transform.position = new Vector2 (m_canvas.pixelRect.width * position,transform.position.y);
        }
    }
}