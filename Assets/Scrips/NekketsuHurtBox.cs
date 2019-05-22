using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃喰らい判定を管理するクラス
/// </summary>
public class NekketsuHurtBox
{
    NekketsuAction NAct; //NekketsuActionが入る変数
    DamageTest DmgTest;

    public NekketsuHurtBox(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public void HurtBoxMain()
    {
        //DamageTest.csで指定された座標に移動すると、喰らい判定が発生するテスト

        if ((NAct.Nmng.uni.Z - 0.4f <= NAct.Z && NAct.Z <= NAct.Nmng.uni.Z + 0.4f)
            && NAct.hurtBox.Overlaps(NAct.Nmng.uni.hitBoxTEST))
        {
            NAct.NowDamage = DamagePattern.groggy;
        }
        else
        {
            NAct.NowDamage = DamagePattern.None;
        }

    }
}
