using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キーを押すと、移動する（熱血風対応版）
public class NekketsuAction : MonoBehaviour
{
    Vector3 pos;    // 最終的な描画で使用

    public float speed = 0.08f;             // スピード
    public float jumppower = 0.003f;        // ジャンプ力
    public float Gravity = -0.011f;         // 内部での重力
    public float InitalVelocity = 0.2f;     // 内部での初速

    float vx = 0;   //内部X値用変数
    float vy = 0;   //内部Y値用変数
    float vz = 0;   //内部Z値用変数

    bool leftFlag = false; // 左向きかどうか
    bool pushFlag = false; // ジャンプキーを押しっぱなしかどうか
    bool jumpFlag = false; // ジャンプして空中にいるか

    float X = 0;    //内部での横
    float Y = 0;    //内部での高さ
    float Z = 0;    //内部での奥行き

    float gravity;         //内部での重力
    float initalVelocity;  //内部での初速

    bool jumpAccelerate = false;    //ジャンプ加速度の計算を行うフラグ


    void Start()
    {   
        // 最初に行う
        gravity = Gravity;
        initalVelocity = InitalVelocity;

        pos = transform.position;

    }

    void Update()
    { // ずっと行う

        vx = 0;
        vz = 0;

        #region 十字キー移動
        // もし、右キーが押されたら
        if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
        {
            vx = speed; // 右に進む移動量を入れる
            leftFlag = false;

            X += vx;
        }
        // もし、左キーが押されたら
        if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)
        {
            vx = -speed; // 左に進む移動量を入れる
            leftFlag = true;

            X += vx;
        }
        // もし、上キーが押されたら
        if (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
        {
            vz = speed * 0.4f; // 上に進む移動量を入れる(熱血っぽく奥行きは移動量小)

            Z += vz;
        }
        if (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
        { // もし、下キーが押されたら
            vz = speed * 0.4f; // 下に進む移動量を入れる(熱血っぽく奥行きは移動量小)

            Z += -vz;
        }

        #endregion  移動

        #region ジャンプ処理
        // もし、ジャンプキーが押されたとき
        if (Input.GetKey("a") || Input.GetKey("joystick button 2"))
        {
            // 着地済みかつ、ジャンプキー押しっぱなしでなければ
            if (Y <= 0 && pushFlag == false)
            { 
                jumpFlag = true; // ジャンプの準備
                pushFlag = true; // 押しっぱなし状態

                // ジャンプした瞬間に初速を追加
                vy += initalVelocity;

                // ジャンプ加速度の計算を行う
                jumpAccelerate = true;
            }
        }
        else
        {
            pushFlag = false; // 押しっぱなし解除
        }

        // ジャンプ加速度の計算
        if (jumpFlag && jumpAccelerate)
        {
            // ジャンプ時、横移動の初速を考慮
            if (jumpFlag &&
                (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                || (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0))
            {
                if (leftFlag)
                {
                    X += -(initalVelocity * 0.08f);
                }
                else
                {
                    X += initalVelocity * 0.08f;
                }
            }
            else
            {
                jumpAccelerate = false;
            }
        }

        // ジャンプ状態
        if (jumpFlag)
        {
            vy += jumppower; 
            Y += vy;           // ジャンプ力を加算
            Y += gravity;      // ジャンプ中の重力加算

            gravity += Gravity; // ジャンプ中にかかる重力を増加させる

            // 着地判定
            if (Y <= 0)
            {
                jumpFlag = false;
                pushFlag = false;

                if (initalVelocity != 0)
                {
                    // 重力変数・内部Y軸変数を初期値に戻す
                    gravity = Gravity;
                    vy = 0;
                }
            }
        }
        #endregion

        #region 画面への描画
        // 入力された内部XYZをtransformに設定する。
        pos.x = X;

        if (jumpFlag)
        {
            if (Y < 0)
            {
                Y = 0;  // マイナス値は入れないようにする
            }

            //ジャンプ中の場合は内部Yを加える。
            pos.y = Z + Y;
        }
        else
        {
            pos.y = Z;
        }

        transform.position = pos;
        #endregion

        #region スプライト反転処理
        Vector3 scale = transform.localScale;
        if (leftFlag)
        {
            scale.x = -1; // 反転する（左向き）
        }
        else
        {
            scale.x = 1; // そのまま（右向き）
        }

        transform.localScale = scale;
        #endregion

        #region キャラクターの影の位置描画処理
        var shadeTransform = GameObject.Find("shade").transform;
        pos.y = Z - 0.8f;

        shadeTransform.position = pos;
        #endregion
    }

    void FixedUpdate() { } // ずっと行う（一定時間ごとに）
}