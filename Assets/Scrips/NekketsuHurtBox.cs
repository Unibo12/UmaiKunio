using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuHurtBox
{
    NekketsuAction NAct; //NekketsuActionが入る変数
    DamageTest DmgTest;

    public NekketsuHurtBox(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public NekketsuHurtBox(DamageTest damageTest)
    {
        DmgTest = damageTest;
    }

    public void HurtBoxMain()
    {
        //DamageTest.csで指定された座標に移動すると、喰らい判定が発生するテスト

        //★★★
        //ここで正しく取得できていない為、内部の疑似X・Yが0固定になってしまう？
        //(0,0)以外がうまくいかない。
        if (DmgTest == null)
        {
            DmgTest = new DamageTest(this);
        }
        //★★★


        if ((DmgTest.Z - 0.4f <= NAct.Z && NAct.Z <= DmgTest.Z + 0.4f)
            && NAct.hurtBox.Overlaps(DmgTest.hitBoxTEST))
        {
            NAct.NowDamage = DamagePattern.groggy;
        }
        else
        {
            NAct.NowDamage = DamagePattern.None;
        }

    }
}
