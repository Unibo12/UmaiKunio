using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuAttack
{
    int nowAttack = 0; // 現在の攻撃パターン格納変数

    NekketsuAction NAct; //NekketsuActionが入る変数
    public NekketsuAttack(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }
    public void AttackMain()
    {
        #region 攻撃処理
        if (!NAct.jumpFlag && !NAct.squatFlag && !NAct.brakeFlag)
        {
            if ((Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                || (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)
                || (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
                || (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
                || NAct.dashFlag)
            {
                //animator.Play("UmaGr");
            }
            else if ((Input.GetKey("z") || Input.GetKey("joystick button 0")))
            {
                //animator.Play("UmaHiji");
            }
            else if ((Input.GetKey("x") || Input.GetKey("joystick button 1")))
            {
                //animator.Play("UmaHarite");
            }
            else if ((Input.GetKey("s") || Input.GetKey("joystick button 3")))
            {
                //animator.Play("UmaThrow");
            }
            else
            {
                //animator.Play("UmaGrTACHI");
            }
        }
        else
        {
            if (NAct.brakeFlag)
            {
                //animator.Play("UmaBrake");
            }

            if (NAct.squatFlag)
            {
                //animator.Play("UmaJumpShagami");
            }

            if (NAct.jumpFlag)
            {
                //animator.Play("UmaJump");
            }
        }

        #endregion
    }
}
