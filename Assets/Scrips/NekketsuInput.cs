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
            NAct.NJumpV.JumpButtonState = JumpButtonPushState.PushMoment;
        }
        else if (Input.GetKey("a") || Input.GetKey("joystick button 2")
                || (Input.GetKey("z") || Input.GetKey("joystick button 0"))
                && (Input.GetKey("x") || Input.GetKey("joystick button 1")))
        {
            NAct.NJumpV.JumpButtonState = JumpButtonPushState.PushButton;
        }
        else if (Input.GetKeyUp("a") || Input.GetKeyUp("joystick button 2")
                || (Input.GetKeyUp("z") || Input.GetKeyUp("joystick button 0"))
                && (Input.GetKeyUp("x") || Input.GetKeyUp("joystick button 1")))
        {
            NAct.NJumpV.JumpButtonState = JumpButtonPushState.ReleaseButton;
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
        NAct.NAttackV.AttackMomentFlag = false;

        if ((Input.GetKeyDown("z") || Input.GetKeyDown("joystick button 0")))
        {
            if (NAct.NMoveV.leftFlag)
            {
                DosukoiVector();
            }
            else
            {
                if (NAct.NJumpV.jumpFlag && NAct.NVariable.Y >= 0)
                {
                    if (!NAct.NMoveV.leftFlag)
                    {
                        NAct.NAttackV.NowAttack = AttackPattern.JumpKick;
                        NAct.NAttackV.AttackMomentFlag = true;
                    }
                }
                else
                {
                    if (NAct.NMoveV.XInputState == XInputState.XNone
                        && NAct.NMoveV.ZInputState == ZInputState.ZNone)
                    {
                        NAct.NAttackV.NowAttack = AttackPattern.Hiji;
                        NAct.NAttackV.AttackMomentFlag = true;
                    }
                    else
                    {
                        NAct.NAttackV.NowAttack = AttackPattern.HijiWalk;
                        NAct.NAttackV.AttackMomentFlag = true;
                    }
                }
            }
        }
        else if ((Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 1")))
        {
            if (NAct.NMoveV.leftFlag)
            {
                if (NAct.NJumpV.jumpFlag && NAct.NVariable.Y >= 0)
                {
                    if (NAct.NMoveV.leftFlag)
                    {
                        NAct.NAttackV.NowAttack = AttackPattern.JumpKick;
                        NAct.NAttackV.AttackMomentFlag = true;
                    }
                }
                else
                {
                    if (NAct.NMoveV.XInputState == XInputState.XNone
                        && NAct.NMoveV.ZInputState == ZInputState.ZNone)
                    {
                        NAct.NAttackV.NowAttack = AttackPattern.Hiji;
                        NAct.NAttackV.AttackMomentFlag = true;
                    }
                    else
                    {
                        NAct.NAttackV.NowAttack = AttackPattern.HijiWalk;
                        NAct.NAttackV.AttackMomentFlag = true;
                    }
                }
            }
            else
            {
                DosukoiVector();
                NAct.NAttackV.AttackMomentFlag = true;
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
            if (NAct.NMoveV.XInputState == XInputState.XNone
                || NAct.NMoveV.XInputState == XInputState.XLeftPushMoment
                || NAct.NMoveV.XInputState == XInputState.XLeftPushButton
                || NAct.NMoveV.XInputState == XInputState.XLeftReleaseButton)
            {
                NAct.NMoveV.XInputState = XInputState.XRightPushMoment;
            }
            else if (NAct.NMoveV.XInputState == XInputState.XRightPushMoment)
            {
                NAct.NMoveV.XInputState = XInputState.XRightPushButton;
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (NAct.NMoveV.XInputState == XInputState.XNone
                || NAct.NMoveV.XInputState == XInputState.XRightPushMoment
                || NAct.NMoveV.XInputState == XInputState.XRightPushButton
                || NAct.NMoveV.XInputState == XInputState.XRightReleaseButton)
            {
                NAct.NMoveV.XInputState = XInputState.XLeftPushMoment;
            }
            else if (NAct.NMoveV.XInputState == XInputState.XLeftPushMoment)
            {
                NAct.NMoveV.XInputState = XInputState.XLeftPushButton;
            }
        }
        else
        {
            if (NAct.NMoveV.XInputState == XInputState.XRightPushButton)
            {
                NAct.NMoveV.XInputState = XInputState.XRightReleaseButton;
            }
            else if (NAct.NMoveV.XInputState == XInputState.XLeftPushButton)
            {
                NAct.NMoveV.XInputState = XInputState.XLeftReleaseButton;
            }
            else if (NAct.NMoveV.XInputState == XInputState.XRightReleaseButton
                || NAct.NMoveV.XInputState == XInputState.XLeftReleaseButton)
            {
                NAct.NMoveV.XInputState = XInputState.XNone;
            }
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            if (NAct.NMoveV.ZInputState == ZInputState.ZNone
                || NAct.NMoveV.ZInputState == ZInputState.ZFrontPushMoment
                || NAct.NMoveV.ZInputState == ZInputState.ZFrontPushButton
                || NAct.NMoveV.ZInputState == ZInputState.ZFrontReleaseButton)
            {
                NAct.NMoveV.ZInputState = ZInputState.ZBackPushMoment;
            }
            else if (NAct.NMoveV.ZInputState == ZInputState.ZBackPushMoment)
            {
                NAct.NMoveV.ZInputState = ZInputState.ZBackPushButton;
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            if (NAct.NMoveV.ZInputState == ZInputState.ZNone
                || NAct.NMoveV.ZInputState == ZInputState.ZBackPushMoment
                || NAct.NMoveV.ZInputState == ZInputState.ZBackPushButton
                || NAct.NMoveV.ZInputState == ZInputState.ZBackReleaseButton)
            {
                NAct.NMoveV.ZInputState = ZInputState.ZFrontPushMoment;
            }
            else if (NAct.NMoveV.ZInputState == ZInputState.ZFrontPushMoment)
            {
                NAct.NMoveV.ZInputState = ZInputState.ZFrontPushButton;
            }
        }
        else
        {
            if (NAct.NMoveV.ZInputState == ZInputState.ZBackPushButton)
            {
                NAct.NMoveV.ZInputState = ZInputState.ZBackReleaseButton;
            }
            else if (NAct.NMoveV.ZInputState == ZInputState.ZFrontPushButton)
            {
                NAct.NMoveV.ZInputState = ZInputState.ZFrontReleaseButton;
            }
            else if (NAct.NMoveV.ZInputState == ZInputState.ZBackReleaseButton
                     || NAct.NMoveV.ZInputState == ZInputState.ZFrontReleaseButton)
            {
                NAct.NMoveV.ZInputState = ZInputState.ZNone;
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
            NAct.NMoveV.XInputState = XInputState.XRightPushMoment;
        }
        else if (Input.GetKey("right"))
        {
            NAct.NMoveV.XInputState = XInputState.XRightPushButton;
        }
        else if (Input.GetKeyUp("right"))
        {
            NAct.NMoveV.XInputState = XInputState.XRightReleaseButton;
        }

        if (Input.GetKeyDown("left"))
        {
            NAct.NMoveV.XInputState = XInputState.XLeftPushMoment;
        }
        else if (Input.GetKey("left"))
        {
            NAct.NMoveV.XInputState = XInputState.XLeftPushButton;
        }
        else if (Input.GetKeyUp("left"))
        {
            NAct.NMoveV.XInputState = XInputState.XLeftReleaseButton;
        }

        if (Input.GetKeyDown("up"))
        {
            NAct.NMoveV.ZInputState = ZInputState.ZBackPushMoment;
        }
        else if (Input.GetKey("up"))
        {
            NAct.NMoveV.ZInputState = ZInputState.ZBackPushButton;
        }
        else if (Input.GetKeyUp("up"))
        {
            NAct.NMoveV.ZInputState = ZInputState.ZBackReleaseButton;
        }

        if (Input.GetKeyDown("down"))
        {
            NAct.NMoveV.ZInputState = ZInputState.ZFrontPushMoment;
        }
        else if (Input.GetKey("down"))
        {
            NAct.NMoveV.ZInputState = ZInputState.ZFrontPushButton;
        }
        else if (Input.GetKeyUp("down"))
        {
            NAct.NMoveV.ZInputState = ZInputState.ZFrontReleaseButton;
        }
    }

    /// <summary>
    ///　キー入力より、どすこい張り手の奥・手前・横の状態を判断する。
    /// </summary>
    void DosukoiVector()
    {
        if (NAct.NJumpV.jumpFlag)
        {
            NAct.NAttackV.NowAttack = AttackPattern.UmaHariteJump;
            NAct.NAttackV.AttackMomentFlag = true;
        }
        else if ((NAct.NMoveV.ZInputState == ZInputState.ZBackPushMoment
            || NAct.NMoveV.ZInputState == ZInputState.ZBackPushButton)
            && !NAct.NJumpV.jumpFlag)
        {
            NAct.NAttackV.NowAttack = AttackPattern.DosukoiBack;
        }
        else if ((NAct.NMoveV.ZInputState == ZInputState.ZFrontPushMoment
                 || NAct.NMoveV.ZInputState == ZInputState.ZFrontPushButton)
                 && !NAct.NJumpV.jumpFlag)
        {
            NAct.NAttackV.NowAttack = AttackPattern.DosukoiFront;
        }
        else
        {
            if (NAct.NMoveV.XInputState == XInputState.XLeftPushButton
                || NAct.NMoveV.XInputState == XInputState.XLeftPushMoment
                || NAct.NMoveV.XInputState == XInputState.XRightPushButton
                || NAct.NMoveV.XInputState == XInputState.XRightPushMoment)
            {
                NAct.NAttackV.NowAttack = AttackPattern.DosukoiWalk;
            }
            else
            {
                NAct.NAttackV.NowAttack = AttackPattern.Dosukoi;
            }
        }
    }
}
