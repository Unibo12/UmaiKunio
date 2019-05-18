using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        #region 攻撃処理

        if ((Input.GetKey("z") || Input.GetKey("joystick button 0")))
        {
            if (NAct.leftFlag)
            {
                DosukoiVector();
            }
            else
            {
                if (NAct.jumpFlag)
                {
                    NAct.NowAttack = AttackPattern.JumpKick;
                }
                else
                {
                    NAct.NowAttack = AttackPattern.Hiji;
                }
            }
        }
        else if ((Input.GetKey("x") || Input.GetKey("joystick button 1")))
        {
            if (NAct.leftFlag)
            {
                if (NAct.jumpFlag)
                {
                    NAct.NowAttack = AttackPattern.JumpKick;
                }
                else
                {
                    NAct.NowAttack = AttackPattern.Hiji;
                }
            }
            else
            {
                DosukoiVector();
            }
        }
        else if ((Input.GetKey("s") || Input.GetKey("joystick button 3")))
        {
            //animator.Play("UmaThrow");
        }
        else
        {
            NAct.NowAttack = AttackPattern.None;
        }
        #endregion
    }

    void DosukoiVector()
    {
        if ((NAct.ZInputState == ZInputState.ZBackPushMoment
            || NAct.ZInputState == ZInputState.ZBackPushButton)
            && !NAct.jumpFlag)
        {
            NAct.NowAttack = AttackPattern.DosukoiBack;
        }
        else if ((NAct.ZInputState == ZInputState.ZFrontPushMoment
                 || NAct.ZInputState == ZInputState.ZFrontPushButton)
                 && !NAct.jumpFlag)
        {
            NAct.NowAttack = AttackPattern.DosukoiFront;
        }
        else
        {
            NAct.NowAttack = AttackPattern.DosukoiSide;
        }
    }

}

