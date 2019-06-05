using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/MoveSettings", fileName = "MoveSettings")]
public sealed class MoveSettings : ScriptableObject
{
    public float ZWalkSpeed;
    public float DashSpeed;
}