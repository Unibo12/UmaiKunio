using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuJump
{
    // GameObject GObj; //ゲームオブジェクトそのものが入る変数
    NekketsuAction NAct; //NekketsuActionが入る変数

    bool pushJump = false; // ジャンプキーを押しっぱなしかどうか
    bool miniJumpFlag = false; // 小ジャンプ
    bool jumpAccelerate = false;    //ジャンプ加速度の計算を行うフラグ
    int JumpX = 0; // ジャンプの瞬間に入力していた疑似Xの方向(左、右、ニュートラル)
    int JumpZ = 0; // ジャンプの瞬間に入力していた疑似Zの方向(手前、奥、ニュートラル)
    bool leftJumpFlag = false; // ジャンプの瞬間に向いていた方向。左向きかどうか
    float nowTimesquat = 0f; //しゃがみ状態硬直時間を計測

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


    // ジャンプ処理を行う前に、熱血アクションの共通変数を取得
    // public void UpdateJump()
    // {
    //     GObj = GameObject.Find("UmaGr1"); //オブジェクトの名前からゲームオブジェクトを取得する。
    //     NAct = GObj.GetComponent<NekketsuAction>(); //UmaGr1の中にあるNekketsuActionを参照する。
    // }

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

        if (!NAct.squatFlag && !NAct.brakeFlag)
        {
            // もし、ジャンプキーが押されたとき
            if (Input.GetKey("a") || Input.GetKey("joystick button 2"))
            {
                // 着地済みかつ、ジャンプキー押しっぱなしでなければ
                if (NAct.Y <= 0 && pushJump == false)
                {
                    NAct.jumpFlag = true; // ジャンプの準備
                    pushJump = true; // 押しっぱなし状態
                    miniJumpFlag = false; // 小ジャンプ
                                          // ジャンプした瞬間に初速を追加
                    NAct.vy += NAct.InitalVelocity;

                    // ジャンプ加速度の計算を行う
                    jumpAccelerate = true;

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
            else
            {
                pushJump = false; // 押しっぱなし解除
            }

            // ジャンプ状態
            if (NAct.jumpFlag)
            {
                // ジャンプボタンが押されてない＆上昇中＆小ジャンプフラグが立ってない
                if (!pushJump && NAct.vy > 0 && !miniJumpFlag)
                {
                    // 小ジャンプ用に、現在の上昇速度を半分にする
                    NAct.vy = NAct.vy * 0.5f;

                    // 小ジャンプ
                    miniJumpFlag = true;
                }

                // ジャンプ中の重力加算(重力は変化せず常に同じ値が掛かる)
                NAct.vy += NAct.Gravity;

                NAct.Y += NAct.vy; // ジャンプ力を加算

                // 着地判定
                if (NAct.Y <= 0)
                {
                    NAct.jumpFlag = false;
                    pushJump = false;
                    NAct.dashFlag = false; //ダッシュ中であれば、着地時にダッシュ解除

                    NAct.squatFlag = true;

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
