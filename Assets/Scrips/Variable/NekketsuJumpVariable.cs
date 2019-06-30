using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuJumpVariable : MonoBehaviour
{
    //ジャンプ
    public float jumpSpeed = 0f;                //ジャンプスピード管理変数
    public float Gravity = -0.006f;             //内部での重力
    public bool jumpFlag = false;               //ジャンプして空中にいるか
    public bool squatFlag = false;              //しゃがみ状態フラグ    
    public JumpButtonPushState JumpButtonState; //ジャンプボタン押下ステータス
}
