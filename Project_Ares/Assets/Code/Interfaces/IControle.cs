﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public interface IControle {

        Vector2 _dir { get; set; }

        Action StartShooting { get; set; }
        Action StopShooting { get; set; }
        Action Dash { get; set; }

        Action<int> SelectWeapon { get; set; }
        Action<int> ChangeWeapon { get; set; }
        Action<int> UseItem { get; set; }
    }
}