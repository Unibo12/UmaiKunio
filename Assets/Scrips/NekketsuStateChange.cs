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
    DamagePattern NowDmgRigidity; //現在硬直中のダメージパターン
    float RigidityDmgTime; //硬直ダメージ時間

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

        //被ダメ硬直処理
        //被ダメ中で、硬直時間時間未計測の場合、硬直時間計測する
        if (NAct.NAttackV.DamageRigidityFlag == false
            && (NAct.NAttackV.NowDamage == DamagePattern.UmaHoge
            || NAct.NAttackV.NowDamage == DamagePattern.UmaHitBack
            || NAct.NAttackV.NowDamage == DamagePattern.UmaHitFront))
        {
            NAct.NAttackV.DamageRigidityFlag = true;
            NowDmgRigidity = NAct.NAttackV.NowDamage;
        }

        //被ダメ硬直計測
        if (NAct.NAttackV.DamageRigidityFlag == true)
        {
            if (NowDmgRigidity == DamagePattern.UmaHitBack
                || NowDmgRigidity == DamagePattern.UmaHitFront)
            {
                //被ダメ状態１(ダメージ軽)
                RigidityDmgTime = Settings.Instance.Attack.Damage1Time;
            }
            else if (NowDmgRigidity == DamagePattern.UmaHoge)
            {
                //被ダメ状態２(ダメージ重 凹み状態)
                RigidityDmgTime = Settings.Instance.Attack.Damage2Time;
            }


            //★１回目のたいりょく０のダウンで失格にならない？
            //★１回目のたいりょく０のダウンで失格にならない？
            //★１回目のたいりょく０のダウンで失格にならない？

            //硬直解除
            if (RigidityDmgTime < NAct.NAttackV.nowHogeTime)
            {
                NAct.NAttackV.nowHogeTime = 0;
                NAct.NAttackV.DamageRigidityFlag = false;

                if (!NAct.NAttackV.BlowUpFlag)
                {
                    NAct.NAttackV.NowDamage = DamagePattern.None;
                }
            }
            else
            {
                //硬直継続
                NAct.NAttackV.nowHogeTime += Time.deltaTime;

                if (!NAct.NAttackV.BlowUpFlag)
                {
                    NAct.NAttackV.NowDamage = NowDmgRigidity;
                }

                NAct.NVariable.vx = 0;
                NAct.NVariable.vz = 0;
            }
        }

        //自動ダッシュ中(十字入力なし)に肘打した場合、ここで歩き肘打ちに補完
        if (NAct.NMoveV.dashFlag
            && NAct.NAttackV.NowAttack == AttackPattern.Hiji)
        {
            NAct.NAttackV.NowAttack = AttackPattern.HijiWalk;
        }

    }
}

