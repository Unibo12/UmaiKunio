using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/MoveSettings", fileName = "MoveSettings")]
public sealed class MoveSettings : ScriptableObject
{
    public float ZWalkSpeed;
    public float DashSpeed;
    public float BrakeTime;

    public float OppositeJumpSpeed;
    public float VerticalJumpSpeed;
    public float SquatTime;
}