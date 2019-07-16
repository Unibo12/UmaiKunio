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


        //テスト用テーブル地形
        //ベタガキなので要修正

        // 壁(X・Z座標)めりこみ補正
        if (!NAct.NJumpV.jumpFlag
            && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
            && NAct.Nmng.MapObjct1.TopBox.x - (NAct.Nmng.MapObjct1.myObjectWidth / 2) < NAct.NVariable.X 
            && NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.myObjectWidth / 2)
            && NAct.Nmng.MapObjct1.Box.yMin < NAct.NVariable.Z
            && NAct.NVariable.Z < NAct.Nmng.MapObjct1.Box.yMax)
        {

            // どの面でめり込んでいるかの判断がNG

            if (NAct.Nmng.MapObjct1.TopBox.x - (NAct.Nmng.MapObjct1.myObjectWidth / 2) < NAct.NVariable.X)
            {
                NAct.NVariable.X = NAct.Nmng.MapObjct1.TopBox.x - (NAct.Nmng.MapObjct1.myObjectWidth / 2); // Xを押し戻す

            }

            if (NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.myObjectWidth / 2))
            {
                NAct.NVariable.X = NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.myObjectWidth / 2);
            }

        }


        // ★ここではなく適切な処理場所へ移動すること★
        // 高いところから低いところへ降りた場合
        if (!NAct.NJumpV.jumpFlag
            && NAct.NVariable.Y != NAct.NVariable.mapY)
        {
            NAct.NJumpV.jumpFlag = true;
        }
    }
}
