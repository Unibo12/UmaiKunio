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


        float hitBoxX = NAct.X;
        float hitBoxY = NAct.Y;

        switch (NAct.NowAttack)
        {
            case AttackPattern.Hiji:
                if (!NAct.leftFlag)
                {
                    NAct.hitBox = new Rect(hitBoxX - 0.5f, hitBoxY, 0.4f, 0.5f);
                }
                else
                {
                    NAct.hitBox = new Rect(hitBoxX + 0.5f, hitBoxY, 0.4f, 0.5f);
                }
                break;

            case AttackPattern.DosukoiSide:
                if (NAct.leftFlag)
                {
                    NAct.hitBox = new Rect(hitBoxX - 0.5f, hitBoxY + 0.2f, 0.4f, 0.5f);
                }
                else
                {
                    NAct.hitBox = new Rect(hitBoxX + 0.5f, hitBoxY + 0.2f, 0.4f, 0.5f);
                }
                break;

            default:
                NAct.hitBox = new Rect(hitBoxX, hitBoxY, 0, 0);
                break;
        }


    }
}
