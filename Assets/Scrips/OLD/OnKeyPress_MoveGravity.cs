using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キーを押すと、移動する（熱血風対応版）
public class OnKeyPress_MoveGravity : MonoBehaviour {

	public float speed = 1;      // スピード：Inspectorで指定
	public float jumppower = 0.1f;  // ジャンプ力：Inspectorで指定

	float vx = 0;
	//bool leftFlag = false; // 左向きかどうか
	bool pushFlag = false; // スペースキーを押しっぱなしかどうか
	    
    Vector3 pos;

    float X = 0;    //内部での横
    float Y = 0;    //内部での高さ
    float Z = 0;    //内部での奥行き
    bool jumpFlag = false;

    void Start ()
    {
        // 最初に行う
        jumpFlag = false;
    }
    
	
	void Update () { // ずっと行う
		vx = 0;
        pos = transform.position;

        // もし、右キーが押されたら
        if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
        { 
			vx = speed; // 右に進む移動量を入れる
			//leftFlag = false;

            pos.x += vx;
        }
        // もし、左キーが押されたら
        if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)
        { 
			vx = -speed; // 左に進む移動量を入れる
			//leftFlag = true;

            pos.x += vx;
        }
        // もし、上キーが押されたら
        if (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
        { 
            vx = speed * 0.4f; // 上に進む移動量を入れる(熱血っぽく奥行きは移動量小)

            pos.y += vx;
        }
        if (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
        { // もし、下キーが押されたら
            vx = speed * 0.4f; // 下に進む移動量を入れる(熱血っぽく奥行きは移動量小)

            pos.y += -vx;
        }


        // もし、ジャンプキーが押されたとき
        if ((Input.GetKey("a") || Input.GetKey("joystick button 2")))
        {
            if (pushFlag == false)
            { // 押しっぱなしでなければ
                jumpFlag = true; // ジャンプの準備
                pushFlag = true; // 押しっぱなし状態

                X = transform.position.x;   //ジャンプ時の横を覚えておく
                Z = transform.position.y;   //ジャンプ時の奥行きを覚えてく
            }
        }
        else
        {
            pushFlag = false; // 押しっぱなし解除
        }


        // ジャンプ上昇中状態
        if (jumpFlag && pushFlag)
        { 
            vx = jumppower; // ジャンプの移動量を入れる

            pos.y += vx;
            Y += vx; // ジャンプした移動量を覚えておく

            transform.position = pos;

        }

        // ジャンプ下降中状態
        if(jumpFlag && !pushFlag)
        {
            pos.y -= Y;    // ジャンプした移動量を戻す(着地)　※重力による落下ではなく、いきなり元の位置に戻ってしまう
            //pos.x = X;

            transform.position = pos;

            Y = 0;  // 着地したので記録用の移動量を0クリア

            jumpFlag = false;

        }

        transform.position = pos;
    }

    void FixedUpdate() { } // ずっと行う（一定時間ごとに）
}