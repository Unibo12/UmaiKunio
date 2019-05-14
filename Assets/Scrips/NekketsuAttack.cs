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

        //★値はざっくりなので再度調整すべし
        //★アニメーションのコマに合わせて当たり判定を変える必要があるので、
        //★Unityのアニメーションウインドウからやるべし

        float hitBoxX = NAct.X;
        float hitBoxY = NAct.Y;

        switch (NAct.NowAttack)
        {
            //ひじうち、コマによって当たり判定が変わるのでこのままだとNG
            case AttackPattern.Hiji:
                if (NAct.leftFlag)
                {
                    NAct.hitBox = new Rect(hitBoxX + 0.5f, hitBoxY, 0.4f, 0.5f);
                }
                else
                {
                    NAct.hitBox = new Rect(hitBoxX - 0.5f, hitBoxY, 0.4f, 0.5f);
                }
                break;

            //どすこい、コマによって当たり判定が変わるのでこのままだとNG
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

            //JK、当たり判定は変わらないはずなのでこれでOK？
            case AttackPattern.JumpKick:
                if (NAct.leftFlag)
                {
                    NAct.hitBox = new Rect(hitBoxX - 0.2f, hitBoxY - 0.65f, 0.8f, 0.4f);
                }
                else
                {
                    NAct.hitBox = new Rect(hitBoxX + 0.2f, hitBoxY - 0.65f, 0.8f, 0.4f);
                }
                break;

            default:
                NAct.hitBox = new Rect(hitBoxX, hitBoxY, 0, 0);
                break;
        }


    }
}
