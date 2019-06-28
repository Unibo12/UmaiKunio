using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/GameSettings", fileName = "GameSettings")]
public sealed class GameSettings : ScriptableObject
{
    public bool isGamePadSetting; //ゲームパッドorキーボードの設定

    public float TopPlayerCameraX; //クロカンで先頭を走るキャラクタのカメラ位置調整
    public float DeathTime; //失格時にキャラクターが消えるまでの時間

}