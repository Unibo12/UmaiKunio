using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/ItemSettings", fileName = "ItemSettings")]
public sealed class ItemSettings : ScriptableObject
{
    public Rect bokutouRect = new Rect(0,0,1,1);

    public float test = 10f;

}