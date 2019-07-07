using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の状態・攻撃当たり判定を管理するクラス
/// 各攻撃メソッドはアニメーションから、モーションの毎に呼び出される
/// </summary>
public class NekketsuAttack : MonoBehaviour
{
    GameObject gameObjct;
    NekketsuAction NAct;
    NekketsuSound NSound;

    public NekketsuAttack(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    void Start()
    {
        gameObjct = GameObject.Find("NekketsuManager");
        NSound = this.gameObject.GetComponent<NekketsuSound>();
        NAct = this.gameObject.GetComponent<NekketsuAction>();
    }

    void Update()
    {
        if (NAct.NAttackV.NowAttack == AttackPattern.None
            || NAct.NAttackV.NowDamage != DamagePattern.None)
        {
            None();
        }
    }

    void None()
    {
        //攻撃判定初期化
        NAct.NAttackV.hitBox = new Rect(0, 0, 0, 0);

        //攻撃ヒットフラグ初期化
        NAct.NAttackV.MyAttackHit = false;
    }

    void DosukoiSide(float timing)
    {
        if (NAct.NAttackV.NowDamage == DamagePattern.None)
        {
            float hitBoxX = NAct.NVariable.X;
            float hitBoxY = NAct.NVariable.Y;
            // 左方向の場合はマイナス値とする。
            float leftMinusVector = (NAct.NMoveV.leftFlag) ? -1 : 1;

            if (NAct.NVariable.vx != 0
                || NAct.NMoveV.dashFlag)
            {
                NAct.NAttackV.NowAttack = AttackPattern.DosukoiWalk;
            }
            else
            {
                NAct.NAttackV.NowAttack = AttackPattern.Dosukoi;
            }

            switch (timing)
            {
                case 1:
                   
                    NAct.NAttackV.hitBox
                        = new Rect(hitBoxX + (0.6f * leftMinusVector),
                                   hitBoxY + 0.2f,
                                   0.6f, 0.5f);

                    NSound.SEPlay(SEPattern.attack);

                    break;

                case 2:
                    NAct.NAttackV.hitBox
                        = new Rect(hitBoxX + (0.6f * leftMinusVector),
                                   hitBoxY + 0.2f,
                                   0.6f, 0.5f);

                    NAct.NAttackV.NowAttack = AttackPattern.None;

                    break;

                default:
                    break;
            }
        }
    }

    void DosukoiBack(float timing)
    {
        float hitBoxX = NAct.NVariable.X;
        float hitBoxY = NAct.NVariable.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (NAct.NMoveV.leftFlag) ? -1 : 1;

        switch (timing)
        {
            case 1:
                NAct.NAttackV.hitBox
                    = new Rect(hitBoxX + (0.3f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.4f, 0.5f);

                NSound.SEPlay(SEPattern.attack);
                break;

            case 2:
                NAct.NAttackV.hitBox
                    = new Rect(hitBoxX + (0.3f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.4f, 0.5f);

                NAct.NAttackV.NowAttack = AttackPattern.None;

                break;

            default:
                break;
        }

    }

    void DosukoiFront(float timing)
    {
        float hitBoxX = NAct.NVariable.X;
        float hitBoxY = NAct.NVariable.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (NAct.NMoveV.leftFlag) ? -1 : 1;

        switch (timing)
        {
            case 1:
                NAct.NAttackV.hitBox
                    = new Rect(hitBoxX + (0.1f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);

                NSound.SEPlay(SEPattern.attack);
                break;

            case 2:
                NAct.NAttackV.hitBox
                    = new Rect(hitBoxX + (0.1f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);

                NAct.NAttackV.NowAttack = AttackPattern.None;

                break;

            default:
                break;
        }

    }

    void JumpDosukoi(float timing)
    {
        float hitBoxX = NAct.NVariable.X;
        float hitBoxY = NAct.NVariable.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (NAct.NMoveV.leftFlag) ? -1 : 1;

        switch (timing)
        {
            case 1:
                NAct.NAttackV.NowAttack = AttackPattern.UmaHariteJump;
                NAct.NAttackV.hitBox
                    = new Rect(hitBoxX + (0.6f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);

                if (NAct.NAttackV.AttackMomentFlag)
                {
                    NSound.SEPlay(SEPattern.attack);
                }
                break;

            case 2:
                NAct.NAttackV.NowAttack = AttackPattern.UmaHariteJump;
                NAct.NAttackV.hitBox
                    = new Rect(hitBoxX + (0.6f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);

                if (!NAct.NJumpV.jumpFlag
                    || NAct.NVariable.Y <= 0
                    || NAct.NJumpV.squatFlag
                    || NAct.NAttackV.NowDamage != DamagePattern.None)
                {
                    NAct.NAttackV.NowAttack = AttackPattern.None;
                }

                break;

            default:
                break;
        }

    }

    void Hiji(float timing)
    {
        float hitBoxX = NAct.NVariable.X;
        float hitBoxY = NAct.NVariable.Y;
        // 右方向の場合はマイナス値とする。
        float RightMinusVector = (NAct.NMoveV.leftFlag) ? 1 : -1;

        switch (timing)
        {
            case 1:
                NAct.NAttackV.hitBox =
                    new Rect(hitBoxX + (0.6f * RightMinusVector),
                             hitBoxY + 0.2f,
                             0.4f, 0.4f);

                NSound.SEPlay(SEPattern.attack);
                break;

            case 2:
                NAct.NAttackV.hitBox =
                    new Rect(hitBoxX + (0.6f * RightMinusVector),
                             hitBoxY + 0.2f,
                             0.4f, 0.4f);

                break;

            case 3:
                NAct.NAttackV.hitBox =
                    new Rect(hitBoxX + (0.6f * RightMinusVector),
                             hitBoxY + 0.2f,
                             0.4f, 0.4f);

                NAct.NAttackV.NowAttack = AttackPattern.None;

                break;

            default:
                break;
        }
    }

    void JumpKick(float timing)
    {
        float hitBoxX = NAct.NVariable.X;
        float hitBoxY = NAct.NVariable.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (NAct.NMoveV.leftFlag) ? -1 : 1;


        switch (timing)
        {
            case 1:
                NAct.NAttackV.hitBox =
                    new Rect(hitBoxX + ( 0.2f * leftMinusVector),
                             hitBoxY - 0.65f,
                             0.8f, 0.4f);

                if (NAct.NAttackV.AttackMomentFlag)
                {
                    NSound.SEPlay(SEPattern.attack);
                }

                if (!NAct.NJumpV.jumpFlag
                    || NAct.NVariable.Y <= 0
                    || NAct.NJumpV.squatFlag
                    || NAct.NAttackV.NowDamage != DamagePattern.None)
                {
                    NAct.NAttackV.NowAttack = AttackPattern.None;
                }

                break;

            default:
                NAct.NAttackV.hitBox =
                    new Rect(hitBoxX + (0.2f * leftMinusVector),
                             hitBoxY - 0.65f,
                             0.8f, 0.4f);
                break;
        }
    }


}
