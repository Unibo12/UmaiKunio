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
        // 地面(Y座標)めりこみ補正は着地したタイミングで行う
        if (NAct.NVariable.Y < NAct.NVariable.mapY)
        {
            NAct.NVariable.Y = NAct.NVariable.mapY; // マイナス値は入れないようにする
        }


        //// 壁(X・Z座標)めりこみ補正
        //if (X・Z条件
        //    && NAct.NVariable.Y < NAct.NVariable.mapY) //壁なのでY座標は最大値？(着地できない高さ)
        //{
        //    NAct.NVariable.X = // X・Zを押し戻す
        //}


        // ★ここではなく適切な処理場所へ移動すること★
        // 高いところから低いところへ降りた場合
        if (!NAct.NJumpV.jumpFlag
            && NAct.NVariable.Y != NAct.NVariable.mapY)
        {
            NAct.NJumpV.jumpFlag = true;
        }
    }
}
