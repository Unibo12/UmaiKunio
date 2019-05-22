using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステータスの変更を行うクラス
/// </summary>
public class NekketsuStateChange
{
    NekketsuAction NAct; //NekketsuActionが入る変数
    NekketsuInput NInput;
    public NekketsuStateChange(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public void StateChangeMain()
    {
        // インプットされた内容から、攻撃の状態を変化させる。
        AttackStateChange();
    }

    void AttackStateChange()
    {

    }
}

