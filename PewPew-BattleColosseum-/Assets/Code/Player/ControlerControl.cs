using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XInputDotNetPure;

namespace PPBC {
    public class ControlerControl : MonoBehaviour, IControl {

        #region Variables
        
        float m_shootThreshold = 0.9f;
        float m_stickDeadZone = 0.2f;

        GamePadState m_state;
        GamePadState m_lastState;

        Vector2 m_tmpDir;
        float m_oldTimeScale;

        #endregion
        #region MonoBehaviour

        // Start is called before the first frame update
        void Start() {
            m_state = GamePad.GetState((PlayerIndex)m_index);
            m_lastState = m_state;
        }

        // Update is called once per frame
        void Update() {
            if (Time.timeScale <= 0)
                return;

            if (m_index == int.MinValue)
                return;

            m_state = GamePad.GetState((PlayerIndex)m_index);

            if (!m_state.IsConnected) {
                DoDisconnect();
                return;
            }

            m_lastState = m_state;
            
            //_dir = new Vector2 (_state.ThumbSticks.Right.X * Mathf.Sqrt(1 - (_state.ThumbSticks.Right.Y * _state.ThumbSticks.Right.Y) / 2), _state.ThumbSticks.Right.Y * Mathf.Sqrt(1 - (_state.ThumbSticks.Right.X * _state.ThumbSticks.Right.X) / 2));//unnötig http://mathproofs.blogspot.com/2005/07/mapping-square-to-circle.html


            m_tmpDir = new Vector2(m_state.ThumbSticks.Right.X, m_state.ThumbSticks.Right.Y);
            if (m_tmpDir == Vector2.zero) {
                m_tmpDir = new Vector2(m_state.ThumbSticks.Left.X, m_state.ThumbSticks.Left.Y);//damit die waffe nicht nach rechts zurück springt, wenn man die sticks loslässt
            }
            if (m_tmpDir.magnitude > m_stickDeadZone) {//fieleicht besser wenn man x und y seperat abfägt, da der input auf ein quadrat gemapt wird und nicht auf einen kreis
                m_dir = m_tmpDir;
            } else {
                m_dir = m_dir.normalized * 0.0001f;
            }

            if ((m_state.Triggers.Right > m_shootThreshold && m_lastState.Triggers.Right <= m_shootThreshold) ||
                (m_state.Triggers.Left > m_shootThreshold && m_lastState.Triggers.Left <= m_shootThreshold)) {
                TriggerDown?.Invoke();
            } else if ((m_state.Triggers.Right <= m_shootThreshold && m_lastState.Triggers.Right > m_shootThreshold) ||
                       (m_state.Triggers.Left <= m_shootThreshold && m_lastState.Triggers.Left > m_shootThreshold)) {
                TriggerUp?.Invoke();
            }

            if ((m_lastState.Buttons.LeftShoulder == ButtonState.Pressed && m_state.Buttons.LeftShoulder == ButtonState.Released) ||
               (m_lastState.Buttons.RightShoulder == ButtonState.Pressed && m_state.Buttons.RightShoulder == ButtonState.Released)) {
                ChangeWeapon?.Invoke();
            }

            if (m_lastState.DPad.Right == ButtonState.Pressed && m_state.DPad.Right == ButtonState.Released) {
                ChangeCharacter?.Invoke(true);
            } else if (m_lastState.DPad.Left == ButtonState.Pressed && m_state.DPad.Left == ButtonState.Released) {
                ChangeCharacter?.Invoke(false);
            }

            if (m_lastState.DPad.Up == ButtonState.Pressed && m_state.DPad.Up == ButtonState.Released) {
                ChangeType?.Invoke(true);
            } else if (m_lastState.DPad.Down == ButtonState.Pressed && m_state.DPad.Down == ButtonState.Released) {
                ChangeType?.Invoke(false);
            }

            if (m_lastState.Buttons.A == ButtonState.Pressed && m_state.Buttons.A == ButtonState.Released) {
                Accept?.Invoke();
            }

            /*if (m_lastState.Buttons.Start == ButtonState.Pressed && m_state.Buttons.Start == ButtonState.Released) {
                OptionMenu?.Invoke();
            }*/
        }

        #endregion
        #region IControl

        public int m_index { get; set; }

        public Vector2 m_dir { get; private set; }

        public Action TriggerDown { get; set; }
        public Action TriggerUp { get; set; }
        public Action ChangeWeapon { get; set; }
        public Action<bool> ChangeCharacter { get; set; }
        public Action Accept { get; set; }
        public Action<bool> ChangeType { get; set; }
        public Action Disconnect { get; set; }
        public void DoDisconnect() {
            DataHolder.s_players[m_index] = false;
            Disconnect?.Invoke();
        }

        #endregion
    }
}