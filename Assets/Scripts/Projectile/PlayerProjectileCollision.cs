using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerProjectileCollision : ProjectileCollision {
    protected override void Awake() {
        int increases = 0;
        if (PlayerState.Instance.HasUpgrade(PlayerState.PlayerUpgrade.TomeOfWisdom)) {
            increases = PlayerState.Instance.UpgradesCollection[PlayerState.PlayerUpgrade.TomeOfWisdom];
        }

        this.Damage = (int) (this.Damage + 2 * (increases));
    }
}