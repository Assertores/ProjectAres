using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace ProjectAres {
    public class ControllerControl : MonoBehaviour, IControl {

        #region Variables

        [Header("Balancing")]
        [SerializeField] float m_shootThreshold = 0.9f;

        public int m_controlerIndex = int.MinValue;

        GamePadState m_state;
        GamePadState m_lastState;

        Vector2 m_tmpDir;
        float m_oldTimeScale;

        #endregion
        #region MonoBehaviour

        // Start is called before the first frame update
        void Start() {
            m_state = GamePad.GetState((PlayerIndex)m_controlerIndex);
        }

        // Update is called once per frame
        void Update() {
            if (m_controlerIndex == int.MinValue)
                return;

            m_lastState = m_state;
            m_state = GamePad.GetState((PlayerIndex)m_controlerIndex);
            //_dir = new Vector2 (_state.ThumbSticks.Right.X * Mathf.Sqrt(1 - (_state.ThumbSticks.Right.Y * _state.ThumbSticks.Right.Y) / 2), _state.ThumbSticks.Right.Y * Mathf.Sqrt(1 - (_state.ThumbSticks.Right.X * _state.ThumbSticks.Right.X) / 2));//unnötig http://mathproofs.blogspot.com/2005/07/mapping-square-to-circle.html
            

            m_tmpDir = new Vector2(m_state.ThumbSticks.Right.X, m_state.ThumbSticks.Right.Y).normalized;
            if(m_tmpDir == Vector2.zero) {
                m_tmpDir = new Vector2(m_state.ThumbSticks.Left.X, m_state.ThumbSticks.Left.Y).normalized;//damit die waffe nicht nach rechts zurück springt, wenn man die sticks loslässt
            }
            if(m_tmpDir != Vector2.zero) {
                m_dir = m_tmpDir;
            }

            if (m_state.Triggers.Right > m_shootThreshold && m_lastState.Triggers.Right <= m_shootThreshold) {
                StartShooting?.Invoke();
            }else if(m_state.Triggers.Right <= m_shootThreshold && m_lastState.Triggers.Right > m_shootThreshold) {
                StopShooting?.Invoke();
            }

            if(m_lastState.Buttons.LeftShoulder == ButtonState.Pressed && m_state.Buttons.LeftShoulder == ButtonState.Released) {
                ChangeWeapon?.Invoke(-1, true);
            }else if(m_lastState.Buttons.RightShoulder == ButtonState.Pressed && m_state.Buttons.RightShoulder == ButtonState.Released) {
                ChangeWeapon?.Invoke(1, true);
            }else if(m_lastState.DPad.Right == ButtonState.Pressed && m_state.DPad.Right == ButtonState.Released) {
                ChangeCharacter?.Invoke(1, true);
            } else if (m_lastState.DPad.Left == ButtonState.Pressed && m_state.DPad.Left == ButtonState.Released) {
                ChangeCharacter?.Invoke(-1, true);
            }

            if (m_state.Triggers.Left > m_shootThreshold && m_lastState.Triggers.Left <= m_shootThreshold) {
                Dash?.Invoke();
            }

            if(m_lastState.Buttons.Start == ButtonState.Pressed && m_state.Buttons.Start == ButtonState.Released) {
                /*if(Time.timeScale > 0) {
                    m_oldTimeScale = Time.timeScale;
                    Time.timeScale = 0;
                } else {
                    Time.timeScale = m_oldTimeScale;
                }*/
                //Disconnect?.Invoke(); is foll dumm wenn man im spiel disconeckted und nichtmehr hinzu kommt
            }
        }

        #endregion
        #region IControl

        public Vector2 m_dir { get; set; }
        public Action StartShooting { get; set; }
        public Action StopShooting { get; set; }
        public Action Dash { get; set; }
        public Action<int> SelectWeapon { get; set; }
        public Action<int, bool> ChangeCharacter { get; set; }
        public Action<int, bool> ChangeWeapon { get; set; }
        public Action<int> UseItem { get; set; }
        public Action Disconnect { get; set; }

        #endregion
    }
}