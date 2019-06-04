﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステータスの変更を行うクラス
/// </summary>
public class NekketsuStateChange
{
    NekketsuAction NAct; //NekketsuActionが入る変数
    NekketsuInput NInput;
    public NekketsuStateChange(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public void StateChangeMain()
    {
        // インプットされた内容から、攻撃の状態を変化させる。
        AttackStateChange();
    }

    void AttackStateChange()
    {
        if((NAct.NowAttack == AttackPattern.JumpKick
           || NAct.NowAttack == AttackPattern.UmaHariteJump)
            && NAct.Y <= 0)
        {
            NAct.NowAttack = AttackPattern.None;
        }

        // ダウン時に攻撃状態(攻撃ボタンを押している)なら、クイックスタンディングとする。
        if ((NAct.NowDamage == DamagePattern.UmaTaore
            || NAct.NowDamage == DamagePattern.UmaTaoreUp)
            && NAct.Y == 0)
        {
            if (NAct.NowAttack != AttackPattern.None)
            {
                NAct.nowDownTime += NAct.downTime / 100;
            }
        }

    }
}

