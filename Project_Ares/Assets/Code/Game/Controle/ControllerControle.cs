﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace ProjectAres {
    public class ControllerControle : MonoBehaviour, IControl {

        [Header("Balancing")]
        [SerializeField] float _shootThreshold = 0.9f;

        public int _controlerIndex = int.MinValue;

        public Vector2 _dir { get; set; }
        public Action StartShooting { get; set; }
        public Action StopShooting { get; set; }
        public Action Dash { get; set; }
        public Action<int> SelectWeapon { get; set; }
        public Action<int> ChangeWeapon { get; set; }
        public Action<int> UseItem { get; set; }
        public Action Disconnect { get; set; }

        GamePadState _state;
        GamePadState _lastState;

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
            
            _dir = new Vector2(_state.ThumbSticks.Right.X, _state.ThumbSticks.Right.Y).normalized;
            if(_dir == Vector2.zero) {
                _dir = new Vector2(_state.ThumbSticks.Left.X, _state.ThumbSticks.Left.Y).normalized;
            }

            if (_state.Triggers.Right > _shootThreshold && _lastState.Triggers.Right <= _shootThreshold) {
                StartShooting?.Invoke();
            }else if(_state.Triggers.Right <= _shootThreshold && _lastState.Triggers.Right > _shootThreshold) {
                StopShooting?.Invoke();
            }

            if(_lastState.DPad.Down == ButtonState.Pressed && _state.DPad.Down == ButtonState.Released) {
                ChangeWeapon?.Invoke(0);
            }else if(_lastState.DPad.Left == ButtonState.Pressed && _state.DPad.Left == ButtonState.Released) {
                ChangeWeapon?.Invoke(1);
            }

            if(_state.Triggers.Left > _shootThreshold && _lastState.Triggers.Left <= _shootThreshold) {
                Dash?.Invoke();
            }

            if(_lastState.Buttons.Start == ButtonState.Pressed && _state.Buttons.Start == ButtonState.Released) {
                Disconnect?.Invoke();
            }
        }
    }
}