using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace ProjectAres {
    public class ControllerControl : MonoBehaviour, IControl {

        #region Variables

        [Header("Balancing")]
        [SerializeField] float _shootThreshold = 0.9f;

        public int _controlerIndex = int.MinValue;

        GamePadState _state;
        GamePadState _lastState;

        Vector2 _tmpDir;

        #endregion
        #region MonoBehaviour

        // Start is called before the first frame update
        void Start() {
            _state = GamePad.GetState((PlayerIndex)_controlerIndex);
        }

        // Update is called once per frame
        void Update() {
            if (_controlerIndex == int.MinValue)
                return;

            _lastState = _state;
            _state = GamePad.GetState((PlayerIndex)_controlerIndex);
            //_dir = new Vector2 (_state.ThumbSticks.Right.X * Mathf.Sqrt(1 - (_state.ThumbSticks.Right.Y * _state.ThumbSticks.Right.Y) / 2), _state.ThumbSticks.Right.Y * Mathf.Sqrt(1 - (_state.ThumbSticks.Right.X * _state.ThumbSticks.Right.X) / 2));//unnötig http://mathproofs.blogspot.com/2005/07/mapping-square-to-circle.html
            

            _tmpDir = new Vector2(_state.ThumbSticks.Right.X, _state.ThumbSticks.Right.Y).normalized;
            if(_tmpDir == Vector2.zero) {
                _tmpDir = new Vector2(_state.ThumbSticks.Left.X, _state.ThumbSticks.Left.Y).normalized;//damit die waffe nicht nach rechts zurück springt, wenn man die sticks loslässt
            }
            if(_tmpDir != Vector2.zero) {
                m_dir = _tmpDir;
            }

            if (_state.Triggers.Right > _shootThreshold && _lastState.Triggers.Right <= _shootThreshold) {
                StartShooting?.Invoke();
            }else if(_state.Triggers.Right <= _shootThreshold && _lastState.Triggers.Right > _shootThreshold) {
                StopShooting?.Invoke();
            }

            if(_lastState.Buttons.LeftShoulder == ButtonState.Pressed && _state.Buttons.LeftShoulder == ButtonState.Released) {
                ChangeWeapon?.Invoke(-1, true);
            }else if(_lastState.Buttons.RightShoulder == ButtonState.Pressed && _state.Buttons.RightShoulder == ButtonState.Released) {
                ChangeWeapon?.Invoke(1, true);
            }else if(_lastState.DPad.Down == ButtonState.Pressed && _state.DPad.Down == ButtonState.Released) {
                ChangeWeapon?.Invoke(0, false);
            } else if (_lastState.DPad.Left == ButtonState.Pressed && _state.DPad.Left == ButtonState.Released) {
                ChangeWeapon?.Invoke(1, false);
            }

            if (_state.Triggers.Left > _shootThreshold && _lastState.Triggers.Left <= _shootThreshold) {
                Dash?.Invoke();
            }

            if(_lastState.Buttons.Start == ButtonState.Pressed && _state.Buttons.Start == ButtonState.Released) {
                Disconnect?.Invoke();
            }
        }

        #endregion
        #region IControl

        public Vector2 m_dir { get; set; }
        public Action StartShooting { get; set; }
        public Action StopShooting { get; set; }
        public Action Dash { get; set; }
        public Action<int> SelectWeapon { get; set; }
        public Action<int, bool> ChangeWeapon { get; set; }
        public Action<int> UseItem { get; set; }
        public Action Disconnect { get; set; }

        #endregion
    }
}