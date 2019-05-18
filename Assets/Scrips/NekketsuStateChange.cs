using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        #region 攻撃処理

        if (!NAct.squatFlag && !NAct.brakeFlag)
        {
            // ブレーキ・しゃがみ中は攻撃出来ないつもりの処理だが、
            // アニメーションで当たり判定をつけている為、不要になりそう。
        }
        else
        {
            if (NAct.brakeFlag)
            {

            }

            if (NAct.squatFlag)
            {

            }

            if (NAct.jumpFlag)
            {

            }
        }
        #endregion


    }
}
