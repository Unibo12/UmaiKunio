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

    #region 列挙体 Enum

    #region Enum ジャンプボタン押下ステータス
    protected enum JumpButtonPushState
    {
        None,           //Buttonを押していない状態
        PushMoment,     //Buttonを押した瞬間
        PushButton,     //Buttonを押している状態
        ReleaseButton,  //Buttonを離した瞬間
    }
    #endregion

    #region Enum 十字入力ステータス（疑似X、左右）
    protected enum XInputState
    {
        XNone,           //Buttonを押していない状態
        XLeftPushMoment,     //Buttonを押した瞬間
        XLeftPushButton,     //Buttonを押している状態
        XLeftReleaseButton,  //Buttonを離した瞬間
        XRightPushMoment,     //Buttonを押した瞬間
        XRightPushButton,     //Buttonを押している状態
        XRightReleaseButton,  //Buttonを離した瞬間
    }
    #endregion

    #region Enum 十字入力ステータス（疑似Z、手前・奥）
    protected enum ZInputState
    {
        ZNone,           //Buttonを押していない状態
        ZFrontPushMoment,     //Buttonを押した瞬間
        ZFrontPushButton,     //Buttonを押している状態
        ZFrontReleaseButton,  //Buttonを離した瞬間
        ZBackPushMoment,     //Buttonを押した瞬間
        ZBackPushButton,     //Buttonを押している状態
        ZBackReleaseButton,  //Buttonを離した瞬間
    }
    #endregion

    #endregion

    public void InputMain()
    {

        //TODO:藤堂
        //Input.GetButton Input.GetButtonDown InputGetButtonUp から判断したほうがよいのでは？

        #region ジャンプステータス判定

        if (Input.GetKeyDown("a") || Input.GetKeyDown("joystick button 2")
                || (Input.GetKeyDown("z") || Input.GetKeyDown("joystick button 0"))
                && (Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 1")))
        {
            NAct.JumpButtonState = (int)JumpButtonPushState.PushMoment;
        }
        else if(Input.GetKey("a") || Input.GetKey("joystick button 2")
                || (Input.GetKey("z") || Input.GetKey("joystick button 0"))
                && (Input.GetKey("x") || Input.GetKey("joystick button 1")))
        {
            NAct.JumpButtonState = (int)JumpButtonPushState.PushButton;
        }
        else if (Input.GetKeyUp("a") || Input.GetKeyUp("joystick button 2")
                || (Input.GetKeyUp("z") || Input.GetKeyUp("joystick button 0"))
                && (Input.GetKeyUp("x") || Input.GetKeyUp("joystick button 1")))
        {
            NAct.JumpButtonState = (int)JumpButtonPushState.ReleaseButton;
        }

        #endregion

        #region 十字キー入力

        if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
        {
            if (NAct.XInputState == (int)XInputState.XNone
                || NAct.XInputState == (int)XInputState.XLeftPushMoment
                || NAct.XInputState == (int)XInputState.XLeftPushButton
                || NAct.XInputState == (int)XInputState.XLeftReleaseButton)
            {
                NAct.XInputState = (int)XInputState.XRightPushMoment;
            }
            else if(NAct.XInputState == (int)XInputState.XRightPushMoment)
            {
                NAct.XInputState = (int)XInputState.XRightPushButton;
            }
        }
        else if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)
        {
            if (NAct.XInputState == (int)XInputState.XNone
                || NAct.XInputState == (int)XInputState.XRightPushMoment
                || NAct.XInputState == (int)XInputState.XRightPushButton
                || NAct.XInputState == (int)XInputState.XRightReleaseButton)
            {
                NAct.XInputState = (int)XInputState.XLeftPushMoment;
            }
            else if (NAct.XInputState == (int)XInputState.XLeftPushMoment)
            {
                NAct.XInputState = (int)XInputState.XLeftPushButton;
            }
        }
        else
        {
            if (NAct.XInputState == (int)XInputState.XRightPushButton)
            {
                NAct.XInputState = (int)XInputState.XRightReleaseButton;
            }
            else if (NAct.XInputState == (int)XInputState.XLeftPushButton)
            {
                NAct.XInputState = (int)XInputState.XLeftReleaseButton;
            }
            else if (NAct.XInputState == (int)XInputState.XRightReleaseButton
                || NAct.XInputState == (int)XInputState.XLeftReleaseButton)
            {
                NAct.XInputState = (int)XInputState.XNone;
            }
        }

        if (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
        {
            if (NAct.ZInputState == (int)ZInputState.ZNone
                || NAct.ZInputState == (int)ZInputState.ZFrontPushMoment
                || NAct.ZInputState == (int)ZInputState.ZFrontPushButton
                || NAct.ZInputState == (int)ZInputState.ZFrontReleaseButton)
            {
                NAct.ZInputState = (int)ZInputState.ZBackPushMoment;
            }
            else if (NAct.ZInputState == (int)ZInputState.ZBackPushMoment)
            {
                NAct.ZInputState = (int)ZInputState.ZBackPushButton;
            }
        }
        else if (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
        {
            if (NAct.ZInputState == (int)ZInputState.ZNone
                || NAct.ZInputState == (int)ZInputState.ZBackPushMoment
                || NAct.ZInputState == (int)ZInputState.ZBackPushButton
                || NAct.ZInputState == (int)ZInputState.ZBackReleaseButton)
            {
                NAct.ZInputState = (int)ZInputState.ZFrontPushMoment;
            }
            else if (NAct.ZInputState == (int)ZInputState.ZFrontPushMoment)
            {
                NAct.ZInputState = (int)ZInputState.ZFrontPushButton;
            }
        }
        else
        {
            if (NAct.ZInputState == (int)ZInputState.ZBackPushButton)
            {
                NAct.ZInputState = (int)ZInputState.ZBackReleaseButton;
            }
            else if (NAct.ZInputState == (int)ZInputState.ZFrontPushButton)
            {
                NAct.ZInputState = (int)ZInputState.ZFrontReleaseButton;
            }
            else if (NAct.ZInputState == (int)ZInputState.ZBackReleaseButton
                     || NAct.ZInputState == (int)ZInputState.ZFrontReleaseButton)
            {
                NAct.ZInputState = (int)ZInputState.ZNone;
            }
        }
        #endregion
    }
}
