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

        //TODO:藤堂
        //Input.GetButton Input.GetButtonDown InputGetButtonUp から判断したほうがよいのでは？

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

        if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
        {
            if (NAct.XInputState == XInputState.XNone
                || NAct.XInputState == XInputState.XLeftPushMoment
                || NAct.XInputState == XInputState.XLeftPushButton
                || NAct.XInputState == XInputState.XLeftReleaseButton)
            {
                NAct.XInputState = XInputState.XRightPushMoment;
            }
            else if(NAct.XInputState == XInputState.XRightPushMoment)
            {
                NAct.XInputState = XInputState.XRightPushButton;
            }
        }
        else if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)
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

        if (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
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
        else if (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
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
    }
}
