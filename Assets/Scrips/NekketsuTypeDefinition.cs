using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キー入力状態や、ステータスを定義するクラス
/// </summary>

#region 列挙体 Enum

#region Enum ジャンプボタン押下ステータス
public enum JumpButtonPushState
{
    None,           //Buttonを押していない状態
    PushMoment,     //Buttonを押した瞬間
    PushButton,     //Buttonを押している状態
    ReleaseButton,  //Buttonを離した瞬間
}
#endregion

#region Enum 十字入力ステータス（疑似X、左右）
public enum XInputState
{
    XNone,           //Buttonを押していない状態
    XLeftPushMoment,     //Buttonを押した瞬間
    XLeftPushButton,     //Buttonを押している状態
    XLeftReleaseButton,  //Buttonを離した瞬間
    XRightPushMoment,     //Buttonを押した瞬間
    XRightPushButton,     //Buttonを押している状態
    XRightReleaseButton,  //Buttonを離した瞬間
}
#endregion

#region Enum 十字入力ステータス（疑似Z、手前・奥）
public enum ZInputState
{
    ZNone,           //Buttonを押していない状態
    ZFrontPushMoment,     //Buttonを押した瞬間
    ZFrontPushButton,     //Buttonを押している状態
    ZFrontReleaseButton,  //Buttonを離した瞬間
    ZBackPushMoment,     //Buttonを押した瞬間
    ZBackPushButton,     //Buttonを押している状態
    ZBackReleaseButton,  //Buttonを離した瞬間
}
#endregion

#region Enum ジャンプの瞬間のX・Z軸入力方向(空中制御用)
public enum VectorX
{
    None,
    Left,
    Right,
}

public enum VectorZ
{
    None,
    Up,
    Down,
}
#endregion

#region Enum 攻撃パターン(とりあえずクロカン障害のみ)
public enum AttackPattern
{
    None,
    Dosukoi,
    DosukoiBack,
    DosukoiFront,
    DosukoiWalk,
    Hiji,
    HijiWalk,
    JumpKick,
    UmaHariteJump,
}
#endregion

#region Enum 攻撃喰らいパターン
public enum DamagePattern
{
    None,
    UmaBARF,
    UmaHitBack,
    UmaHitFront,
    UmaHoge,
    UmaHogeWalk,
    UmaOttotto,
    UmaTaore,
    UmaTaoreUp,
    SquatGetUp
}
#endregion

#region Enum あいてむ種類
public enum ItemPattern
{
    None,
    bokutou
}
#endregion

#region Enum 効果音パターン
public enum SEPattern
{
    None,
    brake,
    dosukoiHit,
    hijiHit,
    hit,
    jump,
    attack,
    death
}
#endregion

#region Enum 死亡パターン
public enum DeathPattern
{
    None,
    deathNow,
    death,
}
#endregion

//#region Enum 攻撃喰らい状態
//public enum HaveAttackedPattern
//{
//    None,
//    deathNow,
//    death,
//}
//#endregion


#region Enum めり込みチェック方向
public enum MerikomiCheckPattern
{
    Up,
    Down,
    Left,
    Right,
    None
}
#endregion


#endregion

public class NekketsuTypeDefinition
{

}
