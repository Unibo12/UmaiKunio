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
    int JumpButtonState = 0; //ジャンプボタン押下ステータス

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

    protected enum VectorY
    {
        None,
        Up,
        Down,
    }
    #endregion

    public NekketsuJump(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction; //UmaGr1の中にあるNekketsuActionを参照する。
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

                        if (JumpZ == (int)VectorY.None)
                        {
                            if (!leftJumpFlag)
                            {
                                // 右向き時の垂直ジャンプ中に右キーが押されたら
                                if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                                {
                                    NAct.vx = NAct.speed; // 右に進む移動量を入れる
                                    NAct.leftFlag = false;

                                    NAct.X += NAct.vx;
                                }
                            }
                            else
                            {
                                // 左向き時の垂直ジャンプ中に左キーが押されたら
                                if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)   //else if でも同時押し対策NG
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

                        // もし、右空中移動中に、左キーが押されたら
                        if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)   //else if でも同時押し対策NG
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
                        if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
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
                    case (int)VectorY.None:
                        //FC・再っぽく、垂直ジャンプからのY入力は受け付けない　とりあえず。
                        break;

                    case (int)VectorY.Up:
                        NAct.vz = NAct.speed * 0.4f; // 上に進む移動量を入れる(熱血っぽく奥行きは移動量小)
                        NAct.Z += NAct.vz;
                        break;

                    case (int)VectorY.Down:
                        NAct.vz = -NAct.speed * 0.4f; // 下に進む移動量を入れる(熱血っぽく奥行きは移動量小)
                        NAct.Z += NAct.vz;
                        break;
                }
            }
        }

        #endregion

        #region ジャンプ処理

        #region ジャンプステータス判定
        if (Input.GetKey("a") || Input.GetKey("joystick button 2")
                || (Input.GetKey("z") || Input.GetKey("joystick button 0"))
                && (Input.GetKey("x") || Input.GetKey("joystick button 1")))
        {
            if (!NAct.jumpFlag
                && JumpButtonState == (int)JumpButtonPushState.None
                && JumpButtonState != (int)JumpButtonPushState.PushButton)
            {
                JumpButtonState = (int)JumpButtonPushState.PushMoment;
            }
            else if (NAct.jumpFlag
                && JumpButtonState == (int)JumpButtonPushState.PushMoment
                && JumpButtonState != (int)JumpButtonPushState.None)
            {
                JumpButtonState = (int)JumpButtonPushState.PushButton;
            }
        }
        else
        {
            if (NAct.jumpFlag
                && (JumpButtonState == (int)JumpButtonPushState.PushButton
                || JumpButtonState == (int)JumpButtonPushState.PushMoment))
            {
                JumpButtonState = (int)JumpButtonPushState.ReleaseButton;
            }
            // ★ジャンプボタン押しっぱなし対策必要★
            //else if (ButtonState == (int)JumpButtonPushState.ReleaseButton)
            //{
            //    ButtonState = (int)JumpButtonPushState.None;
            //}

            //if ( NAct.Y <= 0)
            //{
            //    if (ButtonState != (int)JumpButtonPushState.PushButton)
            //    {
            //        ButtonState = (int)JumpButtonPushState.None;
            //    }
            //}

            // ★ジャンプボタン押しっぱなし対策必要★
        }
        #endregion

        if (!NAct.squatFlag && !NAct.brakeFlag)
        {
            // ジャンプした瞬間
            if (!NAct.jumpFlag
                && JumpButtonState == (int)JumpButtonPushState.PushMoment)
            {
                // 着地状態
                if (NAct.Y <= 0)
                {
                    NAct.jumpFlag = true; // ジャンプの準備
                    miniJumpFlag = false; // 小ジャンプ
                    NAct.vy += NAct.InitalVelocity; // ジャンプした瞬間に初速を追加
                    jumpAccelerate = true; // ジャンプ加速度の計算を行う

                    // 空中制御用に、ジャンプした瞬間に入力していたキーを覚えておく(X軸)
                    if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                    {
                        JumpX = (int)VectorX.Right;
                    }
                    else if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)
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
                    if (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
                    {
                        JumpZ = (int)VectorY.Up;
                    }
                    else if (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
                    {
                        JumpZ = (int)VectorY.Down;
                    }
                    else
                    {
                        JumpZ = (int)VectorY.None;
                    }
                }
            }

            // ジャンプ状態
            if (NAct.jumpFlag)
            {
                // ジャンプボタンが離されたかつ、上昇中かつ、小ジャンプフラグが立ってない
                // 小ジャンプ処理
                if (JumpButtonState == (int)JumpButtonPushState.ReleaseButton 
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
                    JumpButtonState = (int)JumpButtonPushState.None;
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
                    (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                    || (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0))
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
            if (nowTimesquat > 0.15f)
            {
                NAct.squatFlag = false;
                nowTimesquat = 0;
            }
        }
        #endregion
    }
}
