using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コントローラ(キーボード)の入力を管理するクラス
/// </summary>
public class NekketsuInput
{
    // GameObject GObj; //ゲームオブジェクトそのものが入る変数s
    NekketsuAction NAct; //NekketsuActionが入る変数

    public NekketsuInput(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    /// <summary>
    /// キー入力の状態を常に監視し、入力状態を切り替える。
    /// </summary>
    public void InputMain()
    {
        //NStateChange = new NekketsuStateChange(this);

        //ボタン入力はInput.GetButton Input.GetButtonDown InputGetButtonUp から判断する。

        #region ジャンプステータス判定

        //★TODO:AB同時押しジャンプは別で処理いれること
        //★例：既にA押した状態でB→ジャンプなど...
        if (Input.GetKeyDown("a") || Input.GetKeyDown("joystick button 2")
                || (Input.GetKeyDown("z") || Input.GetKeyDown("joystick button 0"))
                && (Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 1")))
        {
            NAct.JumpButtonState = JumpButtonPushState.PushMoment;
        }
        else if (Input.GetKey("a") || Input.GetKey("joystick button 2")
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

        #region 移動入力

        if (Settings.Instance.Game.isGamePadSetting)
        {
            //ゲームパッドのアナログスティック入力受付
            GamePadInput();
        }
        else
        {
            //キーボード方向キーの入力受付
            KeyboardInput();
        }

        #endregion

        #region 攻撃処理
        NAct.AttackMomentFlag = false;

        if ((Input.GetKeyDown("z") || Input.GetKeyDown("joystick button 0")))
        {
            if (NAct.leftFlag)
            {
                DosukoiVector();
            }
            else
            {
                if (NAct.jumpFlag && NAct.Y >= 0)
                {
                    if (!NAct.leftFlag)
                    {
                        NAct.NowAttack = AttackPattern.JumpKick;
                        NAct.AttackMomentFlag = true;
                    }
                }
                else
                {
                    if (NAct.XInputState == XInputState.XNone
                        && NAct.ZInputState == ZInputState.ZNone)
                    {
                        NAct.NowAttack = AttackPattern.Hiji;
                    }
                    else
                    {
                        NAct.NowAttack = AttackPattern.HijiWalk;
                    }
                }
            }
        }
        else if ((Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 1")))
        {
            if (NAct.leftFlag)
            {
                if (NAct.jumpFlag && NAct.Y >= 0)
                {
                    if (NAct.leftFlag)
                    {
                        NAct.NowAttack = AttackPattern.JumpKick;
                        NAct.AttackMomentFlag = true;
                    }
                }
                else
                {
                    if (NAct.XInputState == XInputState.XNone
                        && NAct.ZInputState == ZInputState.ZNone)
                    {
                        NAct.NowAttack = AttackPattern.Hiji;
                    }
                    else
                    {
                        NAct.NowAttack = AttackPattern.HijiWalk;
                    }
                }
            }
            else
            {
                DosukoiVector();
            }
        }
        else if ((Input.GetKeyDown("s") || Input.GetKeyDown("joystick button 3")))
        {
            //animator.Play("UmaThrow");
        }
        else
        {
            //NAct.NowAttack = AttackPattern.None;
        }
        #endregion
    }

    /// <summary>
    /// ゲームパッドの入力受付(推奨：Xbox360コン)
    /// </summary>
    private void GamePadInput()
    {
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
    }

    /// <summary>
    /// キーボードの方向キー入力受付
    /// </summary>
    private void KeyboardInput()
    {
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
    }

    /// <summary>
    ///　キー入力より、どすこい張り手の奥・手前・横の状態を判断する。
    /// </summary>
    void DosukoiVector()
    {
        if (NAct.jumpFlag)
        {
            NAct.NowAttack = AttackPattern.UmaHariteJump;
            NAct.AttackMomentFlag = true;
        }
        else if ((NAct.ZInputState == ZInputState.ZBackPushMoment
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
            if (NAct.XInputState == XInputState.XLeftPushButton
                || NAct.XInputState == XInputState.XLeftPushMoment
                || NAct.XInputState == XInputState.XRightPushButton
                || NAct.XInputState == XInputState.XRightPushMoment)
            {
                NAct.NowAttack = AttackPattern.DosukoiWalk;
            }
            else
            {
                NAct.NowAttack = AttackPattern.Dosukoi;
            }
        }
    }
}
