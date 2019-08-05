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
        // プレイヤーの現在地と障害物が重なっている場合

        if (NAct.NJumpV.squatFlag
            && NAct.NVariable.Y < NAct.NVariable.mapY)
        {
            NAct.NVariable.Y = NAct.NVariable.mapY; // マイナス値は入れないようにする
        }

        //テスト用テーブル地形
        //ベタガキなので要修正

        // 壁(X座標)めりこみ補正
        if (!NAct.NJumpV.jumpFlag
            && !NAct.NJumpV.squatFlag
            && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
            && NAct.Nmng.MapObjct1.Box.yMin < NAct.NVariable.Z
            && NAct.NVariable.Z < NAct.Nmng.MapObjct1.Box.yMax)
        {
            if (NAct.Nmng.MapObjct1.Box.x - NAct.NVariable.vx < NAct.NVariable.X
                && NAct.NVariable.X < NAct.Nmng.MapObjct1.Box.x + NAct.NVariable.vx)
            {
                //障害物の左半分にめり込んでいる場合
                NAct.NVariable.X = NAct.Nmng.MapObjct1.Box.x;
            }
            if (NAct.Nmng.MapObjct1.Box.x + NAct.Nmng.MapObjct1.Box.width - System.Math.Abs(NAct.NVariable.vx) < NAct.NVariable.X
                     && NAct.NVariable.X < NAct.Nmng.MapObjct1.Box.x + NAct.Nmng.MapObjct1.Box.width + System.Math.Abs(NAct.NVariable.vx))
            {
                //障害物の右半分にめり込んでいる場合
                NAct.NVariable.X = NAct.Nmng.MapObjct1.Box.x + NAct.Nmng.MapObjct1.Box.width;
            }
        }

        // 壁(Z座標)めりこみ補正
        if (!NAct.NJumpV.jumpFlag
            && !NAct.NJumpV.squatFlag
            && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
            && NAct.Nmng.MapObjct1.TopBox.x - (NAct.Nmng.MapObjct1.myObjectWidth / 2) < NAct.NVariable.X
            && NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.myObjectWidth / 2))
        {
            if (NAct.Nmng.MapObjct1.Box.y < NAct.NVariable.Z
                && NAct.NVariable.Z < NAct.Nmng.MapObjct1.Box.y + (NAct.Nmng.MapObjct1.Box.height / 2))
            {
                //障害物の奥半分にめり込んでいる場合
                NAct.NVariable.Z = NAct.Nmng.MapObjct1.Box.y;
            }
            else if (NAct.Nmng.MapObjct1.Box.y + (NAct.Nmng.MapObjct1.Box.height / 2) < NAct.NVariable.Z
                     && NAct.NVariable.Z < NAct.Nmng.MapObjct1.Box.y + NAct.Nmng.MapObjct1.Box.height)
            {
                //障害物の手前半分にめり込んでいる場合
                NAct.NVariable.Z = NAct.Nmng.MapObjct1.Box.y + NAct.Nmng.MapObjct1.Box.height;
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
