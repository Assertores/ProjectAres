using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Animation))]
    public class TrailerInput : MonoBehaviour, IControl {

        Animation m_anim;
        [SerializeField] Transform r_weaponRot;

        private void Start() {
            m_anim = GetComponent<Animation>();
        }

        private void Update() {
            m_dir = r_weaponRot.TransformDirection(Vector3.right);
        }

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
            Disconnect?.Invoke();
        }

        #endregion

        public float StartAnim() {
            print("1");
            m_anim.Play();
            return m_anim.clip.length;
        }

        public void DoTriggerDown() {
            TriggerDown?.Invoke();
        }

        public void DoTriggerUp() {
            TriggerUp?.Invoke();
        }

        public void DoChangeWeapon() {
            ChangeWeapon?.Invoke();
        }

        public void DoChangeCharacterOnce() {
            ChangeCharacter?.Invoke(true);
        }

        public void DoChangeCharacterTwice() {
            ChangeCharacter?.Invoke(true);
            ChangeCharacter?.Invoke(true);
        }
    }
}