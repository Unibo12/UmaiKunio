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
    NekketsuManager Nmng;
    NekketsuSound NSound;

    void Start()
    {
        gameObjct = GameObject.Find("NekketsuManager");
        Nmng = gameObjct.GetComponent<NekketsuManager>();

        NSound = this.gameObject.GetComponent<NekketsuSound>();
    }

    void Update()
    {
        //Nmng.Umaibou.hitBox = new Rect(hitBoxX, hitBoxY, 0, 0);
    }

    public void AttackMain()
    {
        if (Nmng.Umaibou.NowAttack == AttackPattern.None)
        {
            None();
        }
    }

    void None()
    {
        Nmng.Umaibou.hitBox = new Rect(0, 0, 0, 0);
    }

    void DosukoiSide(float timing)
    {
        float hitBoxX = Nmng.Umaibou.X;
        float hitBoxY = Nmng.Umaibou.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (Nmng.Umaibou.leftFlag) ? -1 : 1;

        switch (timing)
        {
            case 1:
                Nmng.Umaibou.NowAttack = AttackPattern.DosukoiSide;
                Nmng.Umaibou.hitBox
                    = new Rect(hitBoxX + (0.6f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);

                NSound.SEPlay(SEPattern.attack);

                break;

            case 2:
                Nmng.Umaibou.NowAttack = AttackPattern.DosukoiSide;
                Nmng.Umaibou.hitBox
                    = new Rect(hitBoxX + (0.6f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);

                Nmng.Umaibou.NowAttack = AttackPattern.None;

                break;

            default:
                break;
        }

    }

    void DosukoiBack(float timing)
    {
        float hitBoxX = Nmng.Umaibou.X;
        float hitBoxY = Nmng.Umaibou.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (Nmng.Umaibou.leftFlag) ? -1 : 1;

        switch (timing)
        {
            case 1:
                Nmng.Umaibou.hitBox
                    = new Rect(hitBoxX + (0.3f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.4f, 0.5f);

                NSound.SEPlay(SEPattern.attack);
                break;

            case 2:
                Nmng.Umaibou.hitBox
                    = new Rect(hitBoxX + (0.3f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.4f, 0.5f);

                Nmng.Umaibou.NowAttack = AttackPattern.None;

                break;

            default:
                break;
        }

    }

    void DosukoiFront(float timing)
    {
        float hitBoxX = Nmng.Umaibou.X;
        float hitBoxY = Nmng.Umaibou.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (Nmng.Umaibou.leftFlag) ? -1 : 1;

        switch (timing)
        {
            case 1:
                Nmng.Umaibou.hitBox
                    = new Rect(hitBoxX + (0.1f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);

                NSound.SEPlay(SEPattern.attack);
                break;

            case 2:
                Nmng.Umaibou.hitBox
                    = new Rect(hitBoxX + (0.1f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);

                Nmng.Umaibou.NowAttack = AttackPattern.None;

                break;

            default:
                break;
        }

    }

    void JumpDosukoi(float timing)
    {
        float hitBoxX = Nmng.Umaibou.X;
        float hitBoxY = Nmng.Umaibou.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (Nmng.Umaibou.leftFlag) ? -1 : 1;

        switch (timing)
        {
            case 1:
                Nmng.Umaibou.NowAttack = AttackPattern.JumpDosukoiSide;
                Nmng.Umaibou.hitBox
                    = new Rect(hitBoxX + (0.6f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);

                NSound.SEPlay(SEPattern.attack);
                break;

            case 2:
                Nmng.Umaibou.NowAttack = AttackPattern.JumpDosukoiSide;
                Nmng.Umaibou.hitBox
                    = new Rect(hitBoxX + (0.6f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);

                if (!Nmng.Umaibou.jumpFlag
                    || Nmng.Umaibou.Y <= 0
                    || Nmng.Umaibou.squatFlag)
                {
                    Nmng.Umaibou.NowAttack = AttackPattern.None;
                }

                break;

            default:
                break;
        }

    }

    void Hiji(float timing)
    {
        float hitBoxX = Nmng.Umaibou.X;
        float hitBoxY = Nmng.Umaibou.Y;
        // 右方向の場合はマイナス値とする。
        float RightMinusVector = (Nmng.Umaibou.leftFlag) ? 1 : -1;

        switch (timing)
        {
            case 1:
                Nmng.Umaibou.hitBox =
                    new Rect(hitBoxX + (0.6f * RightMinusVector),
                             hitBoxY + 0.2f,
                             0.4f, 0.4f);

                NSound.SEPlay(SEPattern.attack);
                break;

            case 2:
                Nmng.Umaibou.hitBox =
                    new Rect(hitBoxX + (0.6f * RightMinusVector),
                             hitBoxY + 0.2f,
                             0.4f, 0.4f);

                break;

            case 3:
                Nmng.Umaibou.hitBox =
                    new Rect(hitBoxX + (0.6f * RightMinusVector),
                             hitBoxY + 0.2f,
                             0.4f, 0.4f);

                Nmng.Umaibou.NowAttack = AttackPattern.None;

                break;

            default:
                break;
        }
    }

    void JumpKick(float timing)
    {
        float hitBoxX = Nmng.Umaibou.X;
        float hitBoxY = Nmng.Umaibou.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (Nmng.Umaibou.leftFlag) ? -1 : 1;


        switch (timing)
        {
            case 1:
                Nmng.Umaibou.hitBox =
                    new Rect(hitBoxX + ( 0.2f * leftMinusVector),
                             hitBoxY - 0.65f,
                             0.8f, 0.4f);

                //NSound.SEPlay(SEPattern.Attack);

                if (!Nmng.Umaibou.jumpFlag
                    || Nmng.Umaibou.Y <= 0
                    || Nmng.Umaibou.squatFlag)
                {
                    Nmng.Umaibou.NowAttack = AttackPattern.None;
                }

                break;

            default:
                Nmng.Umaibou.hitBox =
                    new Rect(hitBoxX + (0.2f * leftMinusVector),
                             hitBoxY - 0.65f,
                             0.8f, 0.4f);
                break;
        }
    }


}
