﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キーを押すと、移動する（熱血風対応版）
public class OnKeyPress_MoveGravity : MonoBehaviour {

	public float speed = 1;      // スピード：Inspectorで指定
	public float jumppower = 8;  // ジャンプ力：Inspectorで指定

	float vx = 0;
	bool leftFlag = false; // 左向きかどうか
	bool pushFlag = false; // スペースキーを押しっぱなしかどうか
	    
    Vector3 pos;

    void Start () { } // 最初に行う
	
	void Update () { // ずっと行う
		vx = 0;

        // もし、右キーが押されたら
        if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
        { 
			vx = speed; // 右に進む移動量を入れる
			leftFlag = false;

            pos = transform.position;
            pos.x += speed;
            transform.position = pos;
        }
        // もし、左キーが押されたら
        if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)
        { 
			vx = -speed; // 左に進む移動量を入れる
			leftFlag = true;

            pos = transform.position;
            pos.x += -1 * speed;
            transform.position = pos;
        }
        // もし、上キーが押されたら
        if (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
        { 
            vx = speed * 0.4f; // 上に進む移動量を入れる(熱血っぽく奥行きは移動量小)

            pos = transform.position;
            pos.y += vx;
            transform.position = pos;
        }
        if (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
        { // もし、下キーが押されたら
            vx = speed * 0.4f; // 下に進む移動量を入れる(熱血っぽく奥行きは移動量小)

            pos = transform.position;
            pos.y += -vx;
            transform.position = pos;
        }

        //// JOYPAD対応
        //if (Input.GetAxis("Horizontal") > 0)
        //{
        //    leftFlag = false;
        //}

        //// JOYPAD対応
        //if (Input.GetAxis("Horizontal") < 0)
        //{
        //    leftFlag = true;
        //}
	}

	void FixedUpdate() { } // ずっと行う（一定時間ごとに）
}