using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC
{
    public class ModellRefHolder : Sauerbraten
    {

        public Image m_icon;
        public SkeletonAnimation m_modelAnim;
        public Transform m_weaponRot;
        public SMGRefHolder m_sMG;
        public RocketLauncherRefHolder m_rocketLauncher;

    }
}
