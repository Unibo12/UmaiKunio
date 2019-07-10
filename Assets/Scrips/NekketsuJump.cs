using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ジャンプを管理するクラス
/// </summary>
public class NekketsuJump
{
    // GameObject GObj; //ゲームオブジェクトそのものが入る変数
    NekketsuAction NAct; //NekketsuActionが入る変数

    bool miniJumpFlag = false; // 小ジャンプ
    bool jumpAccelerate = false;    //ジャンプ加速度の計算を行うフラグ
    VectorX JumpX = 0; // ジャンプの瞬間に入力していた疑似Xの方向(左、右、ニュートラル)
    VectorZ JumpZ = 0; // ジャンプの瞬間に入力していた疑似Zの方向(手前、奥、ニュートラル)
    bool leftJumpFlag = false; // ジャンプの瞬間に向いていた方向。左向きかどうか
    float nowTimesquat = 0f; //しゃがみ状態硬直時間を計測

    public NekketsuJump(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    /// <summary>
    /// ジャンプ・空中制御・しゃがみ状態を管理
    /// </summary>
    public void JumpMain(NekketsuSound NSound)
    {
        #region 空中制御

        if (!NAct.NJumpV.squatFlag 
            && NAct.NJumpV.jumpFlag
            && NAct.NAttackV.NowDamage == DamagePattern.None)
        {
            if (!NAct.NMoveV.dashFlag)
            {
                // 空中制御 疑似X軸
                switch (JumpX)
                {
                    case VectorX.None:

                        if (JumpZ == VectorZ.None)
                        {
                            if (!leftJumpFlag)
                            {
                                // 右向き時の垂直ジャンプ中に右キーが押されたら
                                if (NAct.NMoveV.XInputState == XInputState.XRightPushMoment
                                    || NAct.NMoveV.XInputState == XInputState.XRightPushButton)
                                {
                                    //垂直ジャンプ時の加速を設定
                                    NAct.NJumpV.jumpSpeed += 
                                        NAct.NVariable.st_speed * Settings.Instance.Move.VerticalJumpSpeed;
                                    NAct.NVariable.vx += NAct.NJumpV.jumpSpeed; // 右に進む移動量を入れる
                                    NAct.NMoveV.leftFlag = false;
                                }
                            }
                            else
                            {
                                // 左向き時の垂直ジャンプ中に左キーが押されたら ★else if でもキーボード同時押し対策NG★
                                if (NAct.NMoveV.XInputState == XInputState.XLeftPushMoment
                                    || NAct.NMoveV.XInputState == XInputState.XLeftPushButton)
                                {
                                    //垂直ジャンプ時の加速を設定
                                    NAct.NJumpV.jumpSpeed +=
                                        NAct.NVariable.st_speed * Settings.Instance.Move.VerticalJumpSpeed;
                                    NAct.NVariable.vx += -NAct.NJumpV.jumpSpeed; // 左に進む移動量を入れる
                                    NAct.NMoveV.leftFlag = true;
                                }
                            }
                        }
                        break;

                    case VectorX.Right:
                        // もし、右空中移動中に、左キーが押されたら  ★else if でもキーボード同時押し対策NG★
                        if (NAct.NMoveV.XInputState == XInputState.XLeftPushMoment
                            || NAct.NMoveV.XInputState == XInputState.XLeftPushButton)
                        {
                            //シャンプスピードを弱めていく
                            NAct.NJumpV.jumpSpeed *= Settings.Instance.Move.OppositeJumpSpeed;
                            NAct.NMoveV.leftFlag = true;
                        }

                        NAct.NVariable.vx += NAct.NJumpV.jumpSpeed; // 右に進む移動量を入れる

                        break;

                    case VectorX.Left:
                        // もし、左空中移動中に、右キーが押されたら
                        if (NAct.NMoveV.XInputState == XInputState.XRightPushMoment
                            || NAct.NMoveV.XInputState == XInputState.XRightPushButton)
                        {
                            //シャンプスピードを弱めていく
                            NAct.NJumpV.jumpSpeed *= Settings.Instance.Move.OppositeJumpSpeed;
                            NAct.NMoveV.leftFlag = false;
                        }

                        NAct.NVariable.vx += -NAct.NJumpV.jumpSpeed; // 左に進む移動量を入れる

                        break;
                }
            }

            // 空中制御 疑似Z軸
            switch (JumpZ)
            {
                case VectorZ.None:
                    //FC・再っぽく、垂直ジャンプからのZ入力は受け付けない　とりあえず。
                    break;

                case VectorZ.Up:
                    // もし、奥へ空中移動中に、下キー(手前)が押されたら  ★else if でもキーボード同時押し対策NG★
                    if (NAct.NMoveV.ZInputState == ZInputState.ZFrontPushMoment
                        || NAct.NMoveV.ZInputState == ZInputState.ZFrontPushButton)
                    {
                        //シャンプスピードを弱めていく
                        NAct.NJumpV.jumpSpeed *= Settings.Instance.Move.OppositeJumpSpeed;
                    }

                    NAct.NVariable.vz += NAct.NJumpV.jumpSpeed * 0.4f; // 上に進む移動量を入れる(熱血っぽく奥行きは移動量小)

                    break;

                case VectorZ.Down:
                    // もし、手前へ空中移動中に、奥キー(上)が押されたら  ★else if でもキーボード同時押し対策NG★
                    if (NAct.NMoveV.ZInputState == ZInputState.ZBackPushMoment
                        || NAct.NMoveV.ZInputState == ZInputState.ZBackPushButton)
                    {
                        //シャンプスピードを弱めていく
                        NAct.NJumpV.jumpSpeed *= Settings.Instance.Move.OppositeJumpSpeed;
                    }

                    NAct.NVariable.vz += -NAct.NJumpV.jumpSpeed * 0.4f; // 下に進む移動量を入れる(熱血っぽく奥行きは移動量小)

                    break;
            }
        }

        #endregion

        #region ジャンプ処理

        if (!NAct.NJumpV.squatFlag 
            && !NAct.NMoveV.brakeFlag
            && NAct.NAttackV.NowDamage == DamagePattern.None)
        {
            // ジャンプした瞬間
            if (!NAct.NJumpV.jumpFlag
                && NAct.NJumpV.JumpButtonState == JumpButtonPushState.PushMoment)
            {
                // 着地状態
                if (NAct.NVariable.Y <= NAct.NVariable.mapY)
                {
                    NAct.NJumpV.jumpFlag = true; // ジャンプの準備
                    miniJumpFlag = false; // 小ジャンプ
                    NAct.NVariable.vy += NAct.NMoveV.InitalVelocity; // ジャンプした瞬間に初速を追加
                    jumpAccelerate = true; // ジャンプ加速度の計算を行う

                    if (NAct.NMoveV.XInputState == XInputState.XNone
                        && NAct.NMoveV.ZInputState == ZInputState.ZNone)
                    {
                        // 垂直ジャンプ
                        NAct.NJumpV.jumpSpeed = 0;
                    }
                    else
                    {
                        // 垂直ジャンプ以外(横・奥移動ジャンプ)
                        NAct.NJumpV.jumpSpeed = NAct.NVariable.st_speed;
                    }


                    // 空中制御用に、ジャンプした瞬間に入力していたキーを覚えておく(X軸)
                    if (NAct.NMoveV.XInputState == XInputState.XRightPushMoment
                        || NAct.NMoveV.XInputState == XInputState.XRightPushButton
                        || (NAct.NMoveV.dashFlag && !NAct.NMoveV.leftFlag))
                    {
                        JumpX = VectorX.Right;
                    }
                    else if (NAct.NMoveV.XInputState == XInputState.XLeftPushMoment
                            || NAct.NMoveV.XInputState == XInputState.XLeftPushButton
                            || (NAct.NMoveV.dashFlag && NAct.NMoveV.leftFlag))
                    {
                        JumpX = VectorX.Left;
                    }
                    else
                    {
                        // 垂直ジャンプであれば、向いていた方向を覚えておく。
                        JumpX = VectorX.None;
                        leftJumpFlag = NAct.NMoveV.leftFlag;
                    }

                    // 空中制御用に、ジャンプした瞬間に入力していたキーを覚えておく(Z軸)
                    if (NAct.NMoveV.ZInputState == ZInputState.ZBackPushMoment
                        || NAct.NMoveV.ZInputState == ZInputState.ZBackPushButton)
                    {
                        JumpZ = VectorZ.Up;
                    }
                    else if (NAct.NMoveV.ZInputState == ZInputState.ZFrontPushMoment
                            || NAct.NMoveV.ZInputState == ZInputState.ZFrontPushButton)
                    {
                        JumpZ = VectorZ.Down;
                    }
                    else
                    {
                        JumpZ = VectorZ.None;
                    }
                }
                NSound.SEPlay(SEPattern.jump);
            }

            // ジャンプ状態
            if (NAct.NJumpV.jumpFlag)
            {
                // ジャンプボタンが離されたかつ、上昇中かつ、小ジャンプフラグが立ってない
                // 小ジャンプ処理
                if (NAct.NJumpV.JumpButtonState == JumpButtonPushState.ReleaseButton 
                    && NAct.NVariable.vy > 0 
                    && !miniJumpFlag)
                {
                    // 小ジャンプ用に、現在の上昇速度を半分にする
                    NAct.NVariable.vy = NAct.NVariable.vy * 0.5f;

                    // 小ジャンプフラグTrue
                    miniJumpFlag = true;
                }

                // ジャンプ中の重力加算(重力は変化せず常に同じ値が掛かる)
                NAct.NVariable.vy += NAct.NJumpV.Gravity;

                //座標へのジャンプ力反映
                NAct.NVariable.Y += NAct.NVariable.vy;

                // 着地判定
                if (NAct.NVariable.Y <= NAct.NVariable.mapY)
                {
                    NAct.NJumpV.jumpFlag = false;
                    NAct.NJumpV.JumpButtonState = JumpButtonPushState.None; // ジャンプボタン非押下状態とする。
                    NAct.NMoveV.dashFlag = false; //ダッシュ中であれば、着地時にダッシュ解除
                    NAct.NJumpV.squatFlag = true; //しゃがみ状態
                    NAct.NJumpV.jumpSpeed = 0;    //ジャンプ速度初期化

                    if (NAct.NMoveV.InitalVelocity != 0)
                    {
                        // 内部Y軸変数を初期値に戻す
                        NAct.NVariable.vy = 0;
                    }
                }
            }
        }

        // しゃがみ状態の処理
        if (NAct.NJumpV.squatFlag)
        {
            // しゃがみ状態の時間計測
            nowTimesquat += Time.deltaTime;

            // しゃがみ状態解除
            if (nowTimesquat > Settings.Instance.Move.SquatTime)
            {
                NAct.NJumpV.squatFlag = false;
                nowTimesquat = 0;
            }
        }
        #endregion
    }
}
