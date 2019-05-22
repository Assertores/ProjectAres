using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC
{
    public class ModellRefHolder : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] public DragonBones.UnityArmatureComponent m_modelAnim;
        [SerializeField] public Transform m_weaponPos;

        
    }
}
