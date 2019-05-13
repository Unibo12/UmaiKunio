using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuAttack
{
    NekketsuAction NAct; //NekketsuActionが入る変数
    public NekketsuAttack(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }
    public void AttackMain()
    {

        //★★★　値は適当なので再度調整すべし

        switch (NAct.NowAttack)
        {
            case AttackPattern.Hiji:
                NAct.hitBox = new Rect(NAct.X -0.5f, NAct.Y, 0.8f, 0.5f);
                break;

            case AttackPattern.DosukoiSide:
                NAct.hitBox = new Rect(NAct.X +0.5f, NAct.Y, 0.8f, 0.5f);
                break;

            default:
                NAct.hitBox = new Rect(NAct.X, NAct.Y, 0, 0);
                break;
        }


    }
}
