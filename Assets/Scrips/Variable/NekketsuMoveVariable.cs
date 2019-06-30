using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuMoveVariable : MonoBehaviour
{
    //移動
    public float InitalVelocity = 0.188f;       // 内部での初速
    public float nextButtonDownTimeDash = 1f;   // ダッシュを受け付ける時間
    public bool leftFlag = false;   //左向きかどうか
    public bool dashFlag = false;   //走っているか否か
    public bool brakeFlag = false;  //ブレーキフラグ
    public XInputState XInputState = 0; //疑似Xに対する入力ステータス
    public ZInputState ZInputState = 0; //疑似Zに対する入力ステータス
}
