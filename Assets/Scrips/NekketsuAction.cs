using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キーを押すと、移動する（熱血風対応版）
public class NekketsuAction : MonoBehaviour
{
    public float speed = 0.08f;             // スピード：Inspectorで指定
    public float jumppower = 0.00125f;        // ジャンプ力：Inspectorで指定
    public float MaxJumpHeight = 2.5f;    // ジャンプの最大高さ
    public float Gravity = -0.0035f;     // 内部での重力
    public float InitalVelocity = 0.125f; // 内部での初速

    float vx = 0;
    float vy = 0;
    float vz = 0;

    bool leftFlag = false; // 左向きかどうか
    bool pushFlag = false; // ジャンプキーを押しっぱなしかどうか
    bool jumpFlag = false; // ジャンプして空中にいるか

    Vector3 pos;

    float X = 0;    //内部での横
    float Y = 0;    //内部での高さ
    float Z = 0;    //内部での奥行き

    float gravity;    //内部での重力
    float initalVelocity;  //内部での初速

    void Start()
    {   
        // 最初に行う
        gravity = Gravity;
        initalVelocity = InitalVelocity;
    }

    void Update()
    { // ずっと行う

        vx = 0;
        //vy = 0;
        vz = 0;

        pos = transform.position;

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

        // もし、ジャンプキーが押されたとき
        if (!(MaxJumpHeight <= Y) && (Input.GetKey("a") || Input.GetKey("joystick button 2")))
        {
            // 着地済みかつ、ジャンプキー押しっぱなしでなければ
            if (Y <= 0 && pushFlag == false)
            { 
                jumpFlag = true; // ジャンプの準備
                pushFlag = true; // 押しっぱなし状態

                // ジャンプした瞬間に初速を追加
                vy += initalVelocity;
            }
        }
        else
        {
            pushFlag = false; // 押しっぱなし解除
        }

        // ジャンプ上昇中状態
        if (jumpFlag && pushFlag)
        {
            vy += jumppower; // ジャンプの移動量を入れる

            // ジャンプが頂点に達しているか
            if (Y < MaxJumpHeight)
            {

                Y += vy;

                // ジャンプ時、横移動の初速を考慮
                if ((Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                    || (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0))
                {
                    if (leftFlag)
                    {
                        X += -(initalVelocity * 0.2f);
                    }
                    else
                    {
                        X += initalVelocity * 0.2f;
                    }
                }

                // 決められたジャンプの頂点より高く飛ばないように
                if (MaxJumpHeight < Y)
                {
                    pushFlag = false;
                }
            }
        }

        // ジャンプ下降中状態
        if (jumpFlag && !pushFlag)
        {
            vy = jumppower; // ジャンプの移動量を入れる

            // ジャンプ中なら下降させる。
            if (0 <= Y)
            {
                Y -= vy;
                Y += gravity;
                gravity += Gravity; // 下降中にかかる重力を加算していく
            }

            // 着地判定
            if (Y <= 0)
            {
                jumpFlag = false;

                if (initalVelocity != 0)
                {
                    // 初速を初期値に戻す
                    initalVelocity = InitalVelocity;
                    gravity = Gravity;
                    vy = 0;
                }

            }
        }



        // スプライト反転
        Vector3 scale = transform.localScale;
        if (leftFlag)
        {
            scale.x = -1; // 反転する（左向き）
        }
        else
        {
            scale.x = 1; // そのまま（右向き）
        }

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
        transform.localScale = scale;

        // キャラクターの影の位置を設定。
        var shadeTransform = GameObject.Find("shade").transform;
        pos.y = Z - 0.8f;

        shadeTransform.position = pos;
    }

    void FixedUpdate() { } // ずっと行う（一定時間ごとに）
}