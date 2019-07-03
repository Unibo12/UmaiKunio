using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステータスの変更を行うクラス
/// </summary>
public class NekketsuStateChange
{
    NekketsuAction NAct; //NekketsuActionが入る変数
    NekketsuInput NInput;

    float NowDeathTime = 0; //プレイヤーが失格してから消えるまでの時間計測

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
        //ジャンプ攻撃中から着地したとき
        if((NAct.NAttackV.NowAttack == AttackPattern.JumpKick
           || NAct.NAttackV.NowAttack == AttackPattern.UmaHariteJump)
            && NAct.NVariable.Y <= 0)
        {
            NAct.NAttackV.NowAttack = AttackPattern.None;
        }

        // ダウン時に攻撃状態(攻撃ボタンを押している)なら、クイックスタンディングとする。
        if ((NAct.NAttackV.NowDamage == DamagePattern.UmaTaore
            || NAct.NAttackV.NowDamage == DamagePattern.UmaTaoreUp)
            && NAct.NVariable.Y == 0)
        {
            if (NAct.NAttackV.NowAttack != AttackPattern.None)
            {
                NAct.NAttackV.nowDownTime += NAct.NVariable.st_downTime / 100;
                NAct.NAttackV.NowAttack = AttackPattern.None;
            }
        }

        // 蓄積ダメージはXX秒経てば、リセット
        if (0 < NAct.NAttackV.downDamage)
        {
            if (2 < NAct.NAttackV.nowHogeTime)
            {
                NAct.NAttackV.nowHogeTime = 0;
                NAct.NAttackV.downDamage = 0;
            }
            else
            {
                NAct.NAttackV.nowHogeTime += Time.deltaTime;
            }
        }

        if (NAct.NVariable.DeathFlag == DeathPattern.deathNow)
        {
            if(Settings.Instance.Game.DeathTime < NowDeathTime)
            {
                NAct.NVariable.DeathFlag = DeathPattern.death;
            }

            NowDeathTime += Time.deltaTime;
        }

        //// TODO：凹み時間正しく計測できていない。
        //if (NAct.NowDamage == DamagePattern.UmaHoge)
        //{
        //    if (2 < NAct.nowHogeTime)
        //    {
        //        NAct.nowHogeTime = 0;
        //        NAct.NowDamage = DamagePattern.None;
        //    }
        //    else
        //    {
        //        NAct.nowHogeTime += Time.deltaTime;
        //        NAct.NowDamage = DamagePattern.UmaHoge;
        //    }
        //}

        //自動ダッシュ中(十字入力なし)に肘打した場合、ここで歩き肘打ちに補完
        if (NAct.NMoveV.dashFlag
            && NAct.NAttackV.NowAttack == AttackPattern.Hiji)
        {
            NAct.NAttackV.NowAttack = AttackPattern.HijiWalk;
        }

    }
}

