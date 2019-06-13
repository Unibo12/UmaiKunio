using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/GameSettings", fileName = "GameSettings")]
public sealed class GameSettings : ScriptableObject
{
    public float TopPlayerCameraX; //クロカンで先頭を走るキャラクタのカメラ位置調整
}