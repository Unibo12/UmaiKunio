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
        NAct.hitJudgment[0] = 0;
        NAct.hitJudgment[1] = 0;

        switch (NAct.NowAttack)
        {
            case AttackPattern.Hiji:
                NAct.hitJudgment[0] = NAct.X - 0.5f;
                NAct.hitJudgment[1] = NAct.Z;
                break;

            case AttackPattern.DosukoiSide:
                NAct.hitJudgment[0] = NAct.X + 0.5f;
                NAct.hitJudgment[1] = NAct.Z;
                break;

            default:
                break;
        }
    }
}
