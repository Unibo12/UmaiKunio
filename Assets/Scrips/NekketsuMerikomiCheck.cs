using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁・地面のめり込み状態をチェックし、必要であれば補正を行う。
/// </summary>
public class NekketsuMerikomiCheck
{
    NekketsuAction NAct; //NekketsuActionが入る変数

    MerikomiCheckPattern MeriCheckPtn;

    public NekketsuMerikomiCheck(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public void MerikomiMain()
    {
        //めり込みチェック初期化
        MeriCheckPtn = MerikomiCheckPattern.None;

        // 地面(Y座標)めりこみ補正は着地したタイミングで行う
        // プレイヤーの現在地と障害物が重なっている場合

        if (NAct.NJumpV.squatFlag
            && NAct.NVariable.mapY <= NAct.NVariable.Y)
        {
            NAct.NVariable.Y = NAct.NVariable.mapY; // マイナス値は入れないようにする

            //if (NAct.NVariable.Y < 0)
            //{
            //    NAct.NVariable.Y = 0;
            //}
        }

        //テスト用テーブル地形
        //ベタガキなので要修正

        if (!NAct.NJumpV.jumpFlag
            && !NAct.NJumpV.squatFlag
            && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
            && NAct.Nmng.MapObjct1.TopBox.x - (NAct.Nmng.MapObjct1.myObjectWidth / 2) < NAct.NVariable.X
            && NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.myObjectWidth / 2))
        {
            if (NAct.Nmng.MapObjct1.TopBox.yMax <= NAct.NVariable.Z)
            {
                MeriCheckPtn = MerikomiCheckPattern.Up;
                //NAct.NVariable.Z = NAct.Nmng.MapObjct1.TopBox.yMax;
            }
            if (NAct.NVariable.Z <= NAct.Nmng.MapObjct1.TopBox.yMin)
            {
                MeriCheckPtn = MerikomiCheckPattern.Down;
                //NAct.NVariable.Z = NAct.Nmng.MapObjct1.TopBox.yMin;
            }
            //else
            //{
            //    MeriCheckPtn = MerikomiCheckPattern.None;
            //}
        }


        //if (MeriCheckPtn == MerikomiCheckPattern.None)
        //{

        //}

        // 壁(X座標)めりこみ補正
        if (!NAct.NJumpV.jumpFlag
            && !NAct.NJumpV.squatFlag
            && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
            && NAct.Nmng.MapObjct1.TopBox.yMin < NAct.NVariable.Z
            && NAct.NVariable.Z < NAct.Nmng.MapObjct1.TopBox.yMax)
        {
            if (NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x)
            {
                MeriCheckPtn = MerikomiCheckPattern.Left;
            }
            if (NAct.Nmng.MapObjct1.TopBox.x + NAct.Nmng.MapObjct1.TopBox.width < NAct.NVariable.X)
            {
                MeriCheckPtn = MerikomiCheckPattern.Right;
            }
        }



        switch (MeriCheckPtn)
        {
            case MerikomiCheckPattern.Up:


                if (NAct.Nmng.MapObjct1.TopBox.yMin <= NAct.NVariable.Z
                    && NAct.NVariable.Z <= NAct.Nmng.MapObjct1.TopBox.yMax
                    && !NAct.NJumpV.jumpFlag
                    && !NAct.NJumpV.squatFlag
                    && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
                    && NAct.Nmng.MapObjct1.TopBox.x - (NAct.Nmng.MapObjct1.myObjectWidth / 2) < NAct.NVariable.X
            && NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.myObjectWidth / 2))
                {
                    NAct.NVariable.Z = NAct.Nmng.MapObjct1.TopBox.yMax;
                }


                break;

            case MerikomiCheckPattern.Down:


                if (NAct.Nmng.MapObjct1.TopBox.yMin <= NAct.NVariable.Z
                    && NAct.NVariable.Z <= NAct.Nmng.MapObjct1.TopBox.yMax
                    && !NAct.NJumpV.jumpFlag
                    && !NAct.NJumpV.squatFlag
                    && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
                    && NAct.Nmng.MapObjct1.TopBox.x - (NAct.Nmng.MapObjct1.myObjectWidth / 2) < NAct.NVariable.X
                    && NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.myObjectWidth / 2))
                {
                    NAct.NVariable.Z = NAct.Nmng.MapObjct1.TopBox.yMin;
                }


                break;




            case MerikomiCheckPattern.Left:


                if (NAct.Nmng.MapObjct1.TopBox.yMin <= NAct.NVariable.Z
                    && NAct.NVariable.Z <= NAct.Nmng.MapObjct1.TopBox.yMax
                    && !NAct.NJumpV.jumpFlag
                    && !NAct.NJumpV.squatFlag
                    && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
                    && NAct.Nmng.MapObjct1.TopBox.x - (NAct.Nmng.MapObjct1.myObjectWidth / 2) < NAct.NVariable.X
                    && NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.myObjectWidth / 2))
                {
                    NAct.NVariable.X = NAct.Nmng.MapObjct1.TopBox.xMin;
                }


                break;




            case MerikomiCheckPattern.Right:


                if (NAct.Nmng.MapObjct1.TopBox.yMin <= NAct.NVariable.Z
                    && NAct.NVariable.Z <= NAct.Nmng.MapObjct1.TopBox.yMax
                    && !NAct.NJumpV.jumpFlag
                    && !NAct.NJumpV.squatFlag
                    && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
                    && NAct.Nmng.MapObjct1.TopBox.x - (NAct.Nmng.MapObjct1.myObjectWidth / 2) < NAct.NVariable.X
                    && NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.myObjectWidth / 2))
                {
                    NAct.NVariable.X = NAct.Nmng.MapObjct1.TopBox.xMax;
                }


                break;



                //case MerikomiCheckPattern.None:

                //    break;

        }




        //// 壁(X座標)めりこみ補正
        //if (!NAct.NJumpV.jumpFlag
        //    && !NAct.NJumpV.squatFlag
        //    && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
        //    && NAct.Nmng.MapObjct1.TopBox.yMin < NAct.NVariable.Z
        //    && NAct.NVariable.Z < NAct.Nmng.MapObjct1.TopBox.yMax)
        //{
        //    if (NAct.Nmng.MapObjct1.TopBox.x < NAct.NVariable.X
        //        && NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.TopBox.width / 2))
        //    {
        //        //障害物の左半分にめり込んでいる場合
        //        NAct.NVariable.X = NAct.Nmng.MapObjct1.TopBox.x;
        //    }
        //    if (NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.TopBox.width / 2) < NAct.NVariable.X
        //             && NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + NAct.Nmng.MapObjct1.TopBox.width)
        //    {
        //        //障害物の右半分にめり込んでいる場合
        //        NAct.NVariable.X = NAct.Nmng.MapObjct1.TopBox.x + NAct.Nmng.MapObjct1.TopBox.width;
        //    }
        //}

        //// 壁(Z座標)めりこみ補正
        //if (!NAct.NJumpV.jumpFlag
        //    && !NAct.NJumpV.squatFlag
        //    && NAct.NVariable.Y != NAct.Nmng.MapObjct1.topBoxY
        //    && NAct.Nmng.MapObjct1.TopBox.x - (NAct.Nmng.MapObjct1.myObjectWidth / 2) < NAct.NVariable.X
        //    && NAct.NVariable.X < NAct.Nmng.MapObjct1.TopBox.x + (NAct.Nmng.MapObjct1.myObjectWidth / 2))
        //{
        //    if (NAct.Nmng.MapObjct1.TopBox.y < NAct.NVariable.Z
        //        && NAct.NVariable.Z < NAct.Nmng.MapObjct1.TopBox.y + (NAct.Nmng.MapObjct1.TopBox.height / 2))
        //    {
        //        //障害物の奥半分にめり込んでいる場合
        //        NAct.NVariable.Z = NAct.Nmng.MapObjct1.TopBox.y;
        //    }
        //    else if (NAct.Nmng.MapObjct1.TopBox.y + (NAct.Nmng.MapObjct1.TopBox.height / 2) < NAct.NVariable.Z
        //             && NAct.NVariable.Z < NAct.Nmng.MapObjct1.TopBox.y + NAct.Nmng.MapObjct1.TopBox.height)
        //    {
        //        //障害物の手前半分にめり込んでいる場合
        //        NAct.NVariable.Z = NAct.Nmng.MapObjct1.TopBox.y + NAct.Nmng.MapObjct1.TopBox.height;
        //    }
        //}




        // めり込みチェック確認用に　一旦コメントアウト。

        // ★ここではなく適切な処理場所へ移動すること★
        // 高いところから低いところへ降りた場合
        //if (!NAct.NJumpV.jumpFlag
        //    && NAct.NVariable.Y != NAct.NVariable.mapY)
        //{
        //    NAct.NJumpV.jumpFlag = true;
        //}
    }
}
