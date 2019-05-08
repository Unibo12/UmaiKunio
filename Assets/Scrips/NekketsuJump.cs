using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuJump
{
    // GameObject GObj; //ゲームオブジェクトそのものが入る変数
    NekketsuAction NAct; //NekketsuActionが入る変数

    bool miniJumpFlag = false; // 小ジャンプ
    bool jumpAccelerate = false;    //ジャンプ加速度の計算を行うフラグ
    int JumpX = 0; // ジャンプの瞬間に入力していた疑似Xの方向(左、右、ニュートラル)
    int JumpZ = 0; // ジャンプの瞬間に入力していた疑似Zの方向(手前、奥、ニュートラル)
    bool leftJumpFlag = false; // ジャンプの瞬間に向いていた方向。左向きかどうか
    float nowTimesquat = 0f; //しゃがみ状態硬直時間を計測

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

    #region Enum ジャンプの瞬間のX・Z軸入力方向(空中制御用)
    protected enum VectorX
    {
        None,
        Left,
        Right,
    }

    protected enum VectorZ
    {
        None,
        Up,
        Down,
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

    public NekketsuJump(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public void JumpMain()
    {
        #region 空中制御

        if (!NAct.squatFlag)
        {

            if (NAct.jumpFlag && !NAct.dashFlag)
            {
                // 空中制御 疑似X軸
                switch (JumpX)
                {
                    case (int)VectorX.None:

                        if (JumpZ == (int)VectorZ.None)
                        {
                            if (!leftJumpFlag)
                            {
                                // 右向き時の垂直ジャンプ中に右キーが押されたら
                                if (NAct.XInputState == (int)XInputState.XRightPushMoment
                                    || NAct.XInputState == (int)XInputState.XRightPushButton)
                                {
                                    NAct.vx = NAct.speed; // 右に進む移動量を入れる
                                    NAct.leftFlag = false;

                                    NAct.X += NAct.vx;
                                }
                            }
                            else
                            {
                                // 左向き時の垂直ジャンプ中に左キーが押されたら ★else if でもキーボード同時押し対策NG★
                                if (NAct.XInputState == (int)XInputState.XLeftPushMoment
                                    || NAct.XInputState == (int)XInputState.XLeftPushButton)
                                {
                                    NAct.vx = -NAct.speed; // 左に進む移動量を入れる
                                    NAct.leftFlag = true;

                                    NAct.X += NAct.vx;
                                }
                            }
                        }
                        break;

                    case (int)VectorX.Right:
                        NAct.vx = NAct.speed; // 右に進む移動量を入れる
                        NAct.X += NAct.vx;

                        // もし、右空中移動中に、左キーが押されたら  ★else if でもキーボード同時押し対策NG★
                        if (NAct.XInputState == (int)XInputState.XLeftPushMoment
                            || NAct.XInputState == (int)XInputState.XLeftPushButton)
                        {
                            NAct.vx = -NAct.speed; // 左に進む移動量を入れる
                            NAct.leftFlag = true;

                            NAct.X += NAct.vx * 0.2f;
                        }

                        break;


                    case (int)VectorX.Left:
                        NAct.vx = -NAct.speed; // 左に進む移動量を入れる
                        NAct.X += NAct.vx;

                        // もし、左空中移動中に、右キーが押されたら
                        if (NAct.XInputState == (int)XInputState.XRightPushMoment
                            || NAct.XInputState == (int)XInputState.XRightPushButton)
                        {
                            NAct.vx = NAct.speed; // 右に進む移動量を入れる
                            NAct.leftFlag = false;

                            NAct.X += NAct.vx * 0.2f;
                        }
                        break;
                }

                // 空中制御 疑似Z軸
                switch (JumpZ)
                {
                    case (int)VectorZ.None:
                        //FC・再っぽく、垂直ジャンプからのZ入力は受け付けない　とりあえず。
                        break;

                    case (int)VectorZ.Up:
                        NAct.vz = NAct.speed * 0.4f; // 上に進む移動量を入れる(熱血っぽく奥行きは移動量小)
                        NAct.Z += NAct.vz;
                        break;

                    case (int)VectorZ.Down:
                        NAct.vz = -NAct.speed * 0.4f; // 下に進む移動量を入れる(熱血っぽく奥行きは移動量小)
                        NAct.Z += NAct.vz;
                        break;
                }
            }
        }

        #endregion

        #region ジャンプ処理

        if (!NAct.squatFlag && !NAct.brakeFlag)
        {
            // ジャンプした瞬間
            if (!NAct.jumpFlag
                && NAct.JumpButtonState == (int)JumpButtonPushState.PushMoment)
            {
                // 着地状態
                if (NAct.Y <= 0)
                {
                    NAct.jumpFlag = true; // ジャンプの準備
                    miniJumpFlag = false; // 小ジャンプ
                    NAct.vy += NAct.InitalVelocity; // ジャンプした瞬間に初速を追加
                    jumpAccelerate = true; // ジャンプ加速度の計算を行う

                    // 空中制御用に、ジャンプした瞬間に入力していたキーを覚えておく(X軸)
                    if (NAct.XInputState == (int)XInputState.XRightPushMoment
                        || NAct.XInputState == (int)XInputState.XRightPushButton)
                    {
                        JumpX = (int)VectorX.Right;
                    }
                    else if (NAct.XInputState == (int)XInputState.XLeftPushMoment
                            || NAct.XInputState == (int)XInputState.XLeftPushButton)
                    {
                        JumpX = (int)VectorX.Left;
                    }
                    else
                    {
                        // 垂直ジャンプであれば、向いていた方向を覚えておく。
                        JumpX = (int)VectorX.None;
                        leftJumpFlag = NAct.leftFlag;
                    }

                    // 空中制御用に、ジャンプした瞬間に入力していたキーを覚えておく(Z軸)
                    if (NAct.ZInputState == (int)ZInputState.ZBackPushMoment
                        || NAct.ZInputState == (int)ZInputState.ZBackPushButton)
                    {
                        JumpZ = (int)VectorZ.Up;
                    }
                    else if (NAct.ZInputState == (int)ZInputState.ZFrontPushMoment
                            || NAct.ZInputState == (int)ZInputState.ZFrontPushButton)
                    {
                        JumpZ = (int)VectorZ.Down;
                    }
                    else
                    {
                        JumpZ = (int)VectorZ.None;
                    }
                }
            }

            // ジャンプ状態
            if (NAct.jumpFlag)
            {
                // ジャンプボタンが離されたかつ、上昇中かつ、小ジャンプフラグが立ってない
                // 小ジャンプ処理
                if (NAct.JumpButtonState == (int)JumpButtonPushState.ReleaseButton 
                    && NAct.vy > 0 
                    && !miniJumpFlag)
                {
                    // 小ジャンプ用に、現在の上昇速度を半分にする
                    NAct.vy = NAct.vy * 0.5f;

                    // 小ジャンプフラグTrue
                    miniJumpFlag = true;
                }

                // ジャンプ中の重力加算(重力は変化せず常に同じ値が掛かる)
                NAct.vy += NAct.Gravity;

                NAct.Y += NAct.vy; // ジャンプ力を加算

                // 着地判定
                if (NAct.Y <= 0)
                {
                    NAct.jumpFlag = false;

                    // ★ジャンプボタン押しっぱなし対策必要★
                    NAct.JumpButtonState = (int)JumpButtonPushState.None;
                    // ★ジャンプボタン押しっぱなし対策必要★

                    NAct.dashFlag = false; //ダッシュ中であれば、着地時にダッシュ解除

                    NAct.squatFlag = true; //しゃがみ状態

                    // 地面めりこみ補正は着地したタイミングで行う
                    if (NAct.Y < 0)
                    {
                        NAct.Y = 0; // マイナス値は入れないようにする
                    }

                    if (NAct.InitalVelocity != 0)
                    {
                        // 内部Y軸変数を初期値に戻す
                        NAct.vy = 0;
                    }
                }
            }

            // ジャンプ加速度の計算
            if (NAct.jumpFlag && jumpAccelerate)
            {
                // ジャンプ時、横移動の初速を考慮
                if (NAct.jumpFlag &&
                    (NAct.XInputState == (int)XInputState.XRightPushMoment
                    || NAct.XInputState == (int)XInputState.XRightPushButton)
                    || (NAct.XInputState == (int)XInputState.XLeftPushMoment
                       || NAct.XInputState == (int)XInputState.XLeftPushButton))
                {
                    if (NAct.leftFlag)
                    {
                        NAct.X += -(NAct.InitalVelocity * NAct.speed);
                    }
                    else
                    {
                        NAct.X += NAct.InitalVelocity * NAct.speed;
                    }
                }
                else
                {
                    jumpAccelerate = false;
                }
            }
        }

        // しゃがみ状態の処理
        if (NAct.squatFlag)
        {
            // しゃがみ状態の時間計測
            nowTimesquat += Time.deltaTime;

            // しゃがみ状態解除
            if (nowTimesquat > 0.1f)
            {
                NAct.squatFlag = false;
                nowTimesquat = 0;
            }
        }
        #endregion
    }
}
