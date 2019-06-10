using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/AttackSettings", fileName = "AttackSettings")]
public sealed class AttackSettings : ScriptableObject
{
    public Rect DosukoiSide = new Rect(0,0,0,0);
}