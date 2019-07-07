using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動・ダッシュ・ブレーキの状態を管理するクラス
/// </summary>
public class NekketsuMove
{
    NekketsuAction NAct; //NekketsuActionが入る変数
    NekketsuSound NSound;

    bool pushMove = false;   //ダッシュする事前準備として、左右移動ボタンが既に押されているか否か
    bool leftDash = false;   //ダッシュする方向(左を正とする)
    bool canDash = false;    //ダッシュが出来る状態か
    float nowTimeDash = 0f;  //最初に移動ボタンが押されてからの経過時間
    float nowTimebrake = 0f; //ブレーキ時間を計測
    XInputState XInputDashVector = XInputState.XNone; //ダッシュ入力受付方向保持変数

    public NekketsuMove(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    /// <summary>
    /// 熱血インプットで入力された値を元に、
    /// 移動・ダッシュ・ブレーキの状態を管理
    /// </summary>
    public void MoveMain(NekketsuSound NSound)
    {
        if (!NAct.NJumpV.squatFlag
            && NAct.NAttackV.NowDamage == DamagePattern.None
            && NAct.NAttackV.DamageRigidityFlag == false)
        {
            #region 歩き

            // もし、右キーが押されたら
            if (NAct.NMoveV.XInputState == XInputState.XRightPushMoment
                || NAct.NMoveV.XInputState == XInputState.XRightPushButton)
            {
                if (!NAct.NMoveV.brakeFlag)
                {
                    NAct.NMoveV.leftFlag = false;
                }

                if (!NAct.NMoveV.dashFlag && !NAct.NJumpV.jumpFlag && !NAct.NMoveV.brakeFlag)
                {
                    NAct.NVariable.vx = NAct.NVariable.st_speed; // 右に歩く移動量を入れる
                }
            }
            // もし、左キーが押されたら ★else if でもキーボード同時押し対策NG★
            else if (NAct.NMoveV.XInputState == XInputState.XLeftPushMoment
                    || NAct.NMoveV.XInputState == XInputState.XLeftPushButton)
            {
                if (!NAct.NMoveV.brakeFlag)
                {
                    NAct.NMoveV.leftFlag = true;
                }

                if (!NAct.NMoveV.dashFlag && !NAct.NJumpV.jumpFlag && !NAct.NMoveV.brakeFlag)
                {
                    NAct.NVariable.vx = -NAct.NVariable.st_speed; // 左に歩く移動量を入れる
                }
            }

            // もし、上キーが押されたら
            if (NAct.NMoveV.ZInputState == ZInputState.ZBackPushMoment
                || NAct.NMoveV.ZInputState == ZInputState.ZBackPushButton)
            {             
                if (!NAct.NJumpV.jumpFlag)
                {
                    NAct.NVariable.vz = NAct.NVariable.st_speed * Settings.Instance.Move.ZWalkSpeed; // 上に進む移動量を入れる(熱血っぽく奥行きは移動量小)
                }
            }
            // もし、下キーが押されたら
            else if (NAct.NMoveV.ZInputState == ZInputState.ZFrontPushMoment
                    || NAct.NMoveV.ZInputState == ZInputState.ZFrontPushButton)
            {
                if (!NAct.NJumpV.jumpFlag)
                {
                    NAct.NVariable.vz = -NAct.NVariable.st_speed * Settings.Instance.Move.ZWalkSpeed; // 下に進む移動量を入れる(熱血っぽく奥行きは移動量小)
                }
            }

            #endregion

            #region ダッシュ

            if (!NAct.NMoveV.dashFlag)
            {
                // 非ダッシュ状態で、横移動し始めた瞬間か？
                if (NAct.NMoveV.XInputState == XInputState.XRightPushMoment
                    || NAct.NMoveV.XInputState == XInputState.XLeftPushMoment)
                {
                    if (!pushMove)
                    {
                        //ダッシュしたい方向と同じ方向キーが押されている
                        if (XInputDashVector == NAct.NMoveV.XInputState)
                        {
                            //ダッシュの準備をする
                            pushMove = true;
                            leftDash = NAct.NMoveV.leftFlag;
                            nowTimeDash = 0;
                        }

                        //ダッシュしようとしている方向を覚えておく
                        XInputDashVector = NAct.NMoveV.XInputState;
                    }
                    else
                    {
                        // ダッシュ準備済なので、ダッシュしてよい状態か判断
                        if (canDash && !NAct.NJumpV.jumpFlag
                            && leftDash == NAct.NMoveV.leftFlag
                            && nowTimeDash <= NAct.NMoveV.nextButtonDownTimeDash)
                        {
                            NAct.NMoveV.dashFlag = true;
                        }
                    }
                }
                else
                {
                    // 非ダッシュ状態で、ダッシュ準備済か？
                    // 1度左右キーが押された状態で、ダッシュ受付時間内にもう一度左右キーが押された時
                    if (pushMove
                        && !NAct.NMoveV.brakeFlag)
                    {
                        //　時間計測
                        nowTimeDash += Time.deltaTime;

                        if (nowTimeDash > NAct.NMoveV.nextButtonDownTimeDash)
                        {
                            pushMove = false;
                            canDash = false;
                            XInputDashVector = XInputState.XNone;
                        }
                        else
                        {
                            canDash = true;
                        }
                    }
                }
            }
            else
            {   //ダッシュ済の場合
                if (!NAct.NMoveV.brakeFlag)
                {
                    // ダッシュ中に逆方向を押した場合
                    if (leftDash != NAct.NMoveV.leftFlag)
                    {
                        // 逆方向の移動量を入れる
                        NAct.NVariable.vx = GetSign(NAct.NMoveV.leftFlag) * NAct.NVariable.st_speed * Settings.Instance.Move.DashSpeed;

                        NAct.NMoveV.dashFlag = false;
                        pushMove = false;
                        canDash = false;

                        // ブレーキ状態
                        if (!NAct.NJumpV.jumpFlag)
                        {
                            NAct.NMoveV.brakeFlag = true;
                            NSound.SEPlay(SEPattern.brake);
                        }
                    }
                    else
                    {
                        // ダッシュの移動量を入れる
                        NAct.NVariable.vx = GetSign(NAct.NMoveV.leftFlag) * NAct.NVariable.st_speed * Settings.Instance.Move.DashSpeed;
                    }
                }
            }

            // ブレーキ処理
            if (!NAct.NJumpV.jumpFlag && NAct.NMoveV.brakeFlag)
            {
                // ブレーキ中の移動量を入れる
                NAct.NVariable.vx = GetSign(!NAct.NMoveV.leftFlag) * NAct.NVariable.st_speed * NAct.NVariable.st_brake; 
                
                // ブレーキ状態の時間計測
                nowTimebrake += Time.deltaTime;

                // ブレーキ状態解除
                if (nowTimebrake > Settings.Instance.Move.BrakeTime)
                {
                    NAct.NMoveV.brakeFlag = false;
                    nowTimebrake = 0;
                }
            }

            // ダッシュ入力受付中
            if (pushMove)
            {
                //　時間計測
                nowTimeDash += Time.deltaTime;

                if (nowTimeDash > NAct.NMoveV.nextButtonDownTimeDash)
                {
                    pushMove = false;
                    canDash = false;
                    XInputDashVector = XInputState.XNone;
                }
            }
            #endregion
        }
    }

    /// <summary>
    /// 左右の向きの符号を返す(Leftはマイナス、Rightはプラス)
    /// </summary>
    /// <param name="leftDash"></param>
    /// <returns></returns>
    int GetSign(bool leftDash) { return leftDash ? -1 : +1; }


}
