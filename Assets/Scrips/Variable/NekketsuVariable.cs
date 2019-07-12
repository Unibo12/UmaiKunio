using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuVariable : MonoBehaviour
{
    //♡♡♡♡♡すてーたす♡♡♡♡♡
    public float st_life = 0;       //たいりょく
    public float st_punch = 0;      //ぱんち
    public float st_kick = 0;       //きっく
    public float st_speed = 0.08f;  //すばやさ
    public float st_downTime = 1;   //おきあがりじかん
    public float st_brake = 0.5f;   //ぶれーき
    //♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡

    // *****共通変数*****
    public float X = 0;    //内部での横
    public float Y = 0;    //内部での高さ
    public float Z = 0;    //内部での奥行き
    public float vx = 0;   //内部X値用変数
    public float vy = 0;   //内部Y値用変数
    public float vz = 0;   //内部Z値用変数
    public DeathPattern DeathFlag = DeathPattern.None; //失格
    // *****共通変数*****

    public float[, ,] map;   //地形判定
    public float mapY = 0;   //マップで定義された高さ(現在地より取得する)

    public float playerX_Legt = 0;    //プレイヤーの左端の座標
    public float playerX_Right = 0;   //プレイヤーの右端の座標
}
