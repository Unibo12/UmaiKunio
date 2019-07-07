using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuAttackVariable : MonoBehaviour
{
    //攻撃
    public Rect hurtBox = new Rect(0, 0, 0.7f, 1.6f);
    public Rect hitBox = new Rect(0, 0, 0, 0);
    public bool AttackMomentFlag = false;  //攻撃し始めフラグ(空中攻撃出し始め判定)
    public bool BlowUpFlag = false; //吹っ飛び状態か否か  
    public AttackPattern NowAttack = 0; //現在の攻撃パターン格納変数
    public DamagePattern NowDamage = 0; //現在の攻撃喰らいパターン格納変数
    public float BlowUpNowTime = 0;               //吹っ飛んでいる時間計測
    public float BlowUpInitalVelocityTime = 0.2f; //きめ攻撃等で吹っ飛んだ際の吹っ飛び時間
    public float downDamage = 0;  //ダウンまでの蓄積ダメージ
    public float nowDownTime = 0; //ダウン時間計測
    public float nowHogeTime = 0; //凹み状態時間計測
    public bool DamageRigidityFlag = false; //攻撃喰らい硬直状態
    public DamagePattern NowDmgRigidity; //現在硬直中のダメージパターン
    public float RigidityDmgTime; //硬直ダメージ時間
    public bool MyAttackHit; //自身の出した攻撃がヒット済みか

    //アイテム関連
    public ItemPattern haveItem = ItemPattern.None; //所持アイテム
}
