using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/AttackSettings", fileName = "AttackSettings")]
public sealed class AttackSettings : ScriptableObject
{
    public Rect AttackNone = new Rect(0, 0, 0, 0);
    public Rect DosukoiSide = new Rect(0, 0, 0, 0);

    public float Damage1Time = 0.2f;    //非凹み状態の硬直時間
    public float Damage2Time = 0.8f;    //凹み状態の硬直時間

}