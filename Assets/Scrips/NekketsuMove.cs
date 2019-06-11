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
        if (!NAct.squatFlag
            && NAct.NowDamage == DamagePattern.None)
        {
            #region 歩き

            // もし、右キーが押されたら
            if (NAct.XInputState == XInputState.XRightPushMoment
                || NAct.XInputState == XInputState.XRightPushButton)
            {
                if (!NAct.brakeFlag)
                {
                    NAct.leftFlag = false;
                }

                if (!NAct.dashFlag && !NAct.jumpFlag && !NAct.brakeFlag)
                {
                    NAct.vx = NAct.speed; // 右に歩く移動量を入れる
                }
            }
            // もし、左キーが押されたら ★else if でもキーボード同時押し対策NG★
            else if (NAct.XInputState == XInputState.XLeftPushMoment
                    || NAct.XInputState == XInputState.XLeftPushButton)
            {
                if (!NAct.brakeFlag)
                {
                    NAct.leftFlag = true;
                }

                if (!NAct.dashFlag && !NAct.jumpFlag && !NAct.brakeFlag)
                {
                    NAct.vx = -NAct.speed; // 左に歩く移動量を入れる
                }
            }

            // もし、上キーが押されたら
            if (NAct.ZInputState == ZInputState.ZBackPushMoment
                || NAct.ZInputState == ZInputState.ZBackPushButton)
            {             
                if (!NAct.jumpFlag)
                {
                    NAct.vz = NAct.speed * Settings.Instance.Move.ZWalkSpeed; // 上に進む移動量を入れる(熱血っぽく奥行きは移動量小)
                }
            }
            // もし、下キーが押されたら
            else if (NAct.ZInputState == ZInputState.ZFrontPushMoment
                    || NAct.ZInputState == ZInputState.ZFrontPushButton)
            {
                if (!NAct.jumpFlag)
                {
                    NAct.vz = -NAct.speed * Settings.Instance.Move.ZWalkSpeed; // 下に進む移動量を入れる(熱血っぽく奥行きは移動量小)
                }
            }

            #endregion

            #region ダッシュ

            if (!NAct.dashFlag)
            {
                // 非ダッシュ状態で、横移動し始めた瞬間か？
                if (NAct.XInputState == XInputState.XRightPushMoment
                    || NAct.XInputState == XInputState.XLeftPushMoment)
                {
                    if (!pushMove)
                    {
                        //ダッシュしたい方向と同じ方向キーが押されている
                        if (XInputDashVector == NAct.XInputState)
                        {
                            //ダッシュの準備をする
                            pushMove = true;
                            leftDash = NAct.leftFlag;
                            nowTimeDash = 0;
                        }

                        //ダッシュしようとしている方向を覚えておく
                        XInputDashVector = NAct.XInputState;
                    }
                    else
                    {
                        // ダッシュ準備済なので、ダッシュしてよい状態か判断
                        if (canDash && !NAct.jumpFlag
                            && leftDash == NAct.leftFlag
                            && nowTimeDash <= NAct.nextButtonDownTimeDash)
                        {
                            NAct.dashFlag = true;
                        }
                    }
                }
                else
                {
                    // 非ダッシュ状態で、ダッシュ準備済か？
                    // 1度左右キーが押された状態で、ダッシュ受付時間内にもう一度左右キーが押された時
                    if (pushMove
                        && !NAct.brakeFlag)
                    {
                        //　時間計測
                        nowTimeDash += Time.deltaTime;

                        if (nowTimeDash > NAct.nextButtonDownTimeDash)
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

                if (!NAct.brakeFlag)
                {
                    // ダッシュ中に逆方向を押した場合
                    if (leftDash != NAct.leftFlag)
                    {
                        if (leftDash)
                        {
                            // 左ダッシュの移動量を入れる
                            NAct.vx = -NAct.speed * Settings.Instance.Move.DashSpeed; 
                        }
                        else
                        {
                            // 右ダッシュの移動量を入れる
                            NAct.vx = NAct.speed * Settings.Instance.Move.DashSpeed; ;
                        }

                        NAct.dashFlag = false;
                        pushMove = false;
                        canDash = false;

                        // ブレーキ状態
                        if (!NAct.jumpFlag)
                        {
                            NAct.brakeFlag = true;
                            NSound.SEPlay(SEPattern.brake);
                        }
                    }
                    else
                    {
                        // ダッシュ中の加速を計算する。
                        // ダッシュ中は方向キー入力なしで自動で進む。(クロカン・障害ふう)
                        if (NAct.leftFlag)
                        {
                            // 左ダッシュの移動量を入れる
                            NAct.vx = -NAct.speed * Settings.Instance.Move.DashSpeed;
                        }
                        else
                        {
                            // 右ダッシュの移動量を入れる
                            NAct.vx = NAct.speed * Settings.Instance.Move.DashSpeed; ;
                        }
                    }
                }
            }

            // ブレーキ処理
            if (!NAct.jumpFlag && NAct.brakeFlag)
            {
                if (NAct.leftFlag)
                {
                    NAct.vx = NAct.speed * NAct.st_brake; // 右に進む移動量を入れる
                }
                else
                {
                    NAct.vx = -NAct.speed * NAct.st_brake; // 左に進む移動量を入れる
                }
                // ブレーキ状態の時間計測
                nowTimebrake += Time.deltaTime;

                // ブレーキ状態解除
                if (nowTimebrake > Settings.Instance.Move.BrakeTime)
                {
                    NAct.brakeFlag = false;
                    nowTimebrake = 0;
                }
            }

            // ダッシュ入力受付中
            if (pushMove)
            {
                //　時間計測
                nowTimeDash += Time.deltaTime;

                if (nowTimeDash > NAct.nextButtonDownTimeDash)
                {
                    pushMove = false;
                    canDash = false;
                    XInputDashVector = XInputState.XNone;
                }
            }

            //座標への速度反映
            //NAct.X += NAct.vx;
            //NAct.Z += NAct.vz;
            #endregion
        }
    }
}
