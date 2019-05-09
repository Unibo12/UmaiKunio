using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTD = NekketsuTypeDefinition;

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
            NAct.JumpButtonState = NTD.JumpButtonPushState.PushMoment;
        }
        else if(Input.GetKey("a") || Input.GetKey("joystick button 2")
                || (Input.GetKey("z") || Input.GetKey("joystick button 0"))
                && (Input.GetKey("x") || Input.GetKey("joystick button 1")))
        {
            NAct.JumpButtonState = NTD.JumpButtonPushState.PushButton;
        }
        else if (Input.GetKeyUp("a") || Input.GetKeyUp("joystick button 2")
                || (Input.GetKeyUp("z") || Input.GetKeyUp("joystick button 0"))
                && (Input.GetKeyUp("x") || Input.GetKeyUp("joystick button 1")))
        {
            NAct.JumpButtonState = NTD.JumpButtonPushState.ReleaseButton;
        }

        #endregion

        #region 十字キー入力

        if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
        {
            if (NAct.XInputState == NTD.XInputState.XNone
                || NAct.XInputState == NTD.XInputState.XLeftPushMoment
                || NAct.XInputState == NTD.XInputState.XLeftPushButton
                || NAct.XInputState == NTD.XInputState.XLeftReleaseButton)
            {
                NAct.XInputState = NTD.XInputState.XRightPushMoment;
            }
            else if(NAct.XInputState == NTD.XInputState.XRightPushMoment)
            {
                NAct.XInputState = NTD.XInputState.XRightPushButton;
            }
        }
        else if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)
        {
            if (NAct.XInputState == NTD.XInputState.XNone
                || NAct.XInputState == NTD.XInputState.XRightPushMoment
                || NAct.XInputState == NTD.XInputState.XRightPushButton
                || NAct.XInputState == NTD.XInputState.XRightReleaseButton)
            {
                NAct.XInputState = NTD.XInputState.XLeftPushMoment;
            }
            else if (NAct.XInputState == NTD.XInputState.XLeftPushMoment)
            {
                NAct.XInputState = NTD.XInputState.XLeftPushButton;
            }
        }
        else
        {
            if (NAct.XInputState == NTD.XInputState.XRightPushButton)
            {
                NAct.XInputState = NTD.XInputState.XRightReleaseButton;
            }
            else if (NAct.XInputState == NTD.XInputState.XLeftPushButton)
            {
                NAct.XInputState = NTD.XInputState.XLeftReleaseButton;
            }
            else if (NAct.XInputState == NTD.XInputState.XRightReleaseButton
                || NAct.XInputState == NTD.XInputState.XLeftReleaseButton)
            {
                NAct.XInputState = NTD.XInputState.XNone;
            }
        }

        if (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
        {
            if (NAct.ZInputState == NTD.ZInputState.ZNone
                || NAct.ZInputState == NTD.ZInputState.ZFrontPushMoment
                || NAct.ZInputState == NTD.ZInputState.ZFrontPushButton
                || NAct.ZInputState == NTD.ZInputState.ZFrontReleaseButton)
            {
                NAct.ZInputState = NTD.ZInputState.ZBackPushMoment;
            }
            else if (NAct.ZInputState == NTD.ZInputState.ZBackPushMoment)
            {
                NAct.ZInputState = NTD.ZInputState.ZBackPushButton;
            }
        }
        else if (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
        {
            if (NAct.ZInputState == NTD.ZInputState.ZNone
                || NAct.ZInputState == NTD.ZInputState.ZBackPushMoment
                || NAct.ZInputState == NTD.ZInputState.ZBackPushButton
                || NAct.ZInputState == NTD.ZInputState.ZBackReleaseButton)
            {
                NAct.ZInputState = NTD.ZInputState.ZFrontPushMoment;
            }
            else if (NAct.ZInputState == NTD.ZInputState.ZFrontPushMoment)
            {
                NAct.ZInputState = NTD.ZInputState.ZFrontPushButton;
            }
        }
        else
        {
            if (NAct.ZInputState == NTD.ZInputState.ZBackPushButton)
            {
                NAct.ZInputState = NTD.ZInputState.ZBackReleaseButton;
            }
            else if (NAct.ZInputState == NTD.ZInputState.ZFrontPushButton)
            {
                NAct.ZInputState = NTD.ZInputState.ZFrontReleaseButton;
            }
            else if (NAct.ZInputState == NTD.ZInputState.ZBackReleaseButton
                     || NAct.ZInputState == NTD.ZInputState.ZFrontReleaseButton)
            {
                NAct.ZInputState = NTD.ZInputState.ZNone;
            }
        }
        #endregion
    }
}
