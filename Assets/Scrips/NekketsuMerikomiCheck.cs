using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁・地面のめり込み状態をチェックし、必要であれば補正を行う。
/// </summary>
public class NekketsuMerikomiCheck
{
    NekketsuAction NAct; //NekketsuActionが入る変数

    public NekketsuMerikomiCheck(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public void MerikomiMain()
    {
        // 地面めりこみ補正は着地したタイミングで行う
        if (NAct.NVariable.Y < NAct.NVariable.mapY)
        {
            NAct.NVariable.Y = NAct.NVariable.mapY; // マイナス値は入れないようにする
        }
    }
}
