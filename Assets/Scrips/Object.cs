﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public float X = 0;    //内部での横
    public float Y = 0;    //内部での高さ
    public float Z = 0;    //内部での奥行き

    public Rect Box = new Rect(0, 0, 0, 0); //障害物全体の大きさ

    public float topBoxY = 0;    //天板(足場となるスペース)の高さ
    public Rect TopBoxSetting = new Rect(0, 0, 0, 0); //障害物の天板を設定する変数(インスペクターで設定 固定値とする)
    public Rect TopBox = new Rect(0, 0, 0, 0); //障害物の天板を設定する変数(画面上で描画するときに使用)

    public float myObjectWidth = 0; //障害物オブジェクトの幅
    public float myObjectHeight = 0; //障害物オブジェクトの高さ
    // 変数にしなくてもtransformで取れるのでは？


    //障害物も動きだしたくなるかもしれない
    //public float vx = 0;   //内部X値用変数
    //public float vy = 0;   //内部Y値用変数
    //public float vz = 0;   //内部Z値用変数

    void Start()
    {
        myObjectWidth = this.gameObject.GetComponent<RectTransform>().rect.width;
        myObjectHeight = this.gameObject.GetComponent<RectTransform>().rect.height;

        Box.x = transform.position.x - (myObjectWidth / 2); //Rectの左端の値
        Box.y = transform.position.y;
        Box.width = myObjectWidth;
        Box.height = myObjectHeight;
    }

    void Update()
    {
        //天板の位置調整
        TopBox.x = transform.position.x + (myObjectWidth - TopBoxSetting.width);
        TopBox.y = transform.position.y + (myObjectHeight - TopBoxSetting.height) / 2 ; //頂点に合わせるには÷2が必要
        TopBox.width = myObjectWidth - (myObjectWidth - TopBoxSetting.width);
        TopBox.height = myObjectHeight - (myObjectHeight - TopBoxSetting.height);
    }


    void OnDrawGizmos()
    {
        // 天板のギズモを表示
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(TopBox.x, TopBox.y), new Vector3(TopBox.width, TopBox.height));
    }
}
