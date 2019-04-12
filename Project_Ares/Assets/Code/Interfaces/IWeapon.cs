using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAres {
    public interface IWeapon {

        [SerializeField] Sprite m_icon { get;}

        void Init(Player player);
        void SetActive(bool activate);
        void StartShooting();
        void StopShooting();
    }
}