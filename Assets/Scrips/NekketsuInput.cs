using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuInput
{
    // GameObject GObj; //ゲームオブジェクトそのものが入る変数
    NekketsuAction NAct; //NekketsuActionが入る変数
    public NekketsuInput(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public void InputMain()
    {
        //ボタン入力はInput.GetButton Input.GetButtonDown InputGetButtonUp から判断する。

        #region ジャンプステータス判定

        if (Input.GetKeyDown("a") || Input.GetKeyDown("joystick button 2")
                || (Input.GetKeyDown("z") || Input.GetKeyDown("joystick button 0"))
                && (Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 1")))
        {
            NAct.JumpButtonState = JumpButtonPushState.PushMoment;
        }
        else if(Input.GetKey("a") || Input.GetKey("joystick button 2")
                || (Input.GetKey("z") || Input.GetKey("joystick button 0"))
                && (Input.GetKey("x") || Input.GetKey("joystick button 1")))
        {
            NAct.JumpButtonState = JumpButtonPushState.PushButton;
        }
        else if (Input.GetKeyUp("a") || Input.GetKeyUp("joystick button 2")
                || (Input.GetKeyUp("z") || Input.GetKeyUp("joystick button 0"))
                && (Input.GetKeyUp("x") || Input.GetKeyUp("joystick button 1")))
        {
            NAct.JumpButtonState = JumpButtonPushState.ReleaseButton;
        }

        #endregion

        #region 十字キー入力

        #region キーボード方向キー

        if (Input.GetKeyDown("right"))
        {
            NAct.XInputState = XInputState.XRightPushMoment;
        }
        else if (Input.GetKey("right"))
        {
            NAct.XInputState = XInputState.XRightPushButton;
        }
        else if (Input.GetKeyUp("right"))
        {
            NAct.XInputState = XInputState.XRightReleaseButton;
        }

        if (Input.GetKeyDown("left"))
        {
            NAct.XInputState = XInputState.XLeftPushMoment;
        }
        else if (Input.GetKey("left"))
        {
            NAct.XInputState = XInputState.XLeftPushButton;
        }
        else if (Input.GetKeyUp("left"))
        {
            NAct.XInputState = XInputState.XLeftReleaseButton;
        }

        if (Input.GetKeyDown("up"))
        {
            NAct.ZInputState = ZInputState.ZBackPushMoment;
        }
        else if (Input.GetKey("up"))
        {
            NAct.ZInputState = ZInputState.ZBackPushButton;
        }
        else if (Input.GetKeyUp("up"))
        {
            NAct.ZInputState = ZInputState.ZBackReleaseButton;
        }

        if (Input.GetKeyDown("down"))
        {
            NAct.ZInputState = ZInputState.ZFrontPushMoment;
        }
        else if (Input.GetKey("down"))
        {
            NAct.ZInputState = ZInputState.ZFrontPushButton;
        }
        else if (Input.GetKeyUp("down"))
        {
            NAct.ZInputState = ZInputState.ZFrontReleaseButton;
        }

        #endregion

        #region コントロールスティック ※見直し必要

        if (Input.GetAxis("Horizontal") > 0)
        {
            if (NAct.XInputState == XInputState.XNone
                || NAct.XInputState == XInputState.XLeftPushMoment
                || NAct.XInputState == XInputState.XLeftPushButton
                || NAct.XInputState == XInputState.XLeftReleaseButton)
            {
                NAct.XInputState = XInputState.XRightPushMoment;
            }
            else if (NAct.XInputState == XInputState.XRightPushMoment)
            {
                NAct.XInputState = XInputState.XRightPushButton;
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (NAct.XInputState == XInputState.XNone
                || NAct.XInputState == XInputState.XRightPushMoment
                || NAct.XInputState == XInputState.XRightPushButton
                || NAct.XInputState == XInputState.XRightReleaseButton)
            {
                NAct.XInputState = XInputState.XLeftPushMoment;
            }
            else if (NAct.XInputState == XInputState.XLeftPushMoment)
            {
                NAct.XInputState = XInputState.XLeftPushButton;
            }
        }
        else
        {
            if (NAct.XInputState == XInputState.XRightPushButton)
            {
                NAct.XInputState = XInputState.XRightReleaseButton;
            }
            else if (NAct.XInputState == XInputState.XLeftPushButton)
            {
                NAct.XInputState = XInputState.XLeftReleaseButton;
            }
            else if (NAct.XInputState == XInputState.XRightReleaseButton
                || NAct.XInputState == XInputState.XLeftReleaseButton)
            {
                NAct.XInputState = XInputState.XNone;
            }
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            if (NAct.ZInputState == ZInputState.ZNone
                || NAct.ZInputState == ZInputState.ZFrontPushMoment
                || NAct.ZInputState == ZInputState.ZFrontPushButton
                || NAct.ZInputState == ZInputState.ZFrontReleaseButton)
            {
                NAct.ZInputState = ZInputState.ZBackPushMoment;
            }
            else if (NAct.ZInputState == ZInputState.ZBackPushMoment)
            {
                NAct.ZInputState = ZInputState.ZBackPushButton;
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            if (NAct.ZInputState == ZInputState.ZNone
                || NAct.ZInputState == ZInputState.ZBackPushMoment
                || NAct.ZInputState == ZInputState.ZBackPushButton
                || NAct.ZInputState == ZInputState.ZBackReleaseButton)
            {
                NAct.ZInputState = ZInputState.ZFrontPushMoment;
            }
            else if (NAct.ZInputState == ZInputState.ZFrontPushMoment)
            {
                NAct.ZInputState = ZInputState.ZFrontPushButton;
            }
        }
        else
        {
            if (NAct.ZInputState == ZInputState.ZBackPushButton)
            {
                NAct.ZInputState = ZInputState.ZBackReleaseButton;
            }
            else if (NAct.ZInputState == ZInputState.ZFrontPushButton)
            {
                NAct.ZInputState = ZInputState.ZFrontReleaseButton;
            }
            else if (NAct.ZInputState == ZInputState.ZBackReleaseButton
                     || NAct.ZInputState == ZInputState.ZFrontReleaseButton)
            {
                NAct.ZInputState = ZInputState.ZNone;
            }
        }

        #endregion

        #endregion

        #region 攻撃処理
        if (!NAct.squatFlag && !NAct.brakeFlag)
        {
            if ((Input.GetKey("z") || Input.GetKey("joystick button 0")))
            {
                if (NAct.leftFlag)
                {
                    if (NAct.ZInputState == ZInputState.ZBackPushMoment
                        || NAct.ZInputState == ZInputState.ZBackPushButton)
                    {
                        NAct.NowAttack = AttackPattern.DosukoiBack;
                    }
                    else if (NAct.ZInputState == ZInputState.ZFrontPushMoment
                             || NAct.ZInputState == ZInputState.ZFrontPushButton)
                    {
                        NAct.NowAttack = AttackPattern.DosukoiFront;
                    }
                    else
                    {
                        NAct.NowAttack = AttackPattern.DosukoiSide;
                    }
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
                    if (NAct.ZInputState == ZInputState.ZBackPushMoment
                        || NAct.ZInputState == ZInputState.ZBackPushButton)
                    {
                        NAct.NowAttack = AttackPattern.DosukoiBack;
                    }
                    else if (NAct.ZInputState == ZInputState.ZFrontPushMoment
                             || NAct.ZInputState == ZInputState.ZFrontPushButton)
                    {
                            NAct.NowAttack = AttackPattern.DosukoiFront;
                    }
                    else
                    {
                        NAct.NowAttack = AttackPattern.DosukoiSide;
                    }
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
