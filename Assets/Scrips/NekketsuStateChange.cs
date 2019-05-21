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
        // インプットされた内容から、攻撃の状態を変化させる。
        AttackStateChange();
    }

    void AttackStateChange()
    {
        #region 攻撃処理

        //★押した瞬間に攻撃発生するだけで良いので、
        //★GetKeyDownDownにしたいが、変更すると当たり判定がうまくいかなくなる。

        if ((Input.GetKeyDown("z") || Input.GetKeyDown("joystick button 0")))
        {
            if (NAct.leftFlag)
            {
                DosukoiVector();
            }
            else
            {
                if (NAct.jumpFlag)
                {
                    if (!NAct.leftFlag)
                    {
                        NAct.NowAttack = AttackPattern.JumpKick;
                    }
                }
                else
                {
                    if (NAct.vx == 0 && NAct.vz == 0)
                    {
                        NAct.NowAttack = AttackPattern.Hiji;
                    }
                    else
                    {
                        NAct.NowAttack = AttackPattern.HijiWalk;
                    }
                }
            }
        }
        else if ((Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 1")))
        {
            if (NAct.leftFlag)
            {
                if (NAct.jumpFlag)
                {
                    if (NAct.leftFlag)
                    {
                        NAct.NowAttack = AttackPattern.JumpKick;
                    }
                }
                else
                {
                    if (NAct.vx == 0 && NAct.vz == 0)
                    {
                        NAct.NowAttack = AttackPattern.Hiji;
                    }
                    else
                    {
                        NAct.NowAttack = AttackPattern.HijiWalk;
                    }
                }
            }
            else
            {
                DosukoiVector();
            }
        }
        else if ((Input.GetKeyDown("s") || Input.GetKeyDown("joystick button 3")))
        {
            //animator.Play("UmaThrow");
        }
        else
        {
            //NAct.NowAttack = AttackPattern.None;
        }
        #endregion
    }

    void DosukoiVector()
    {
        if (NAct.jumpFlag)
        {
            NAct.NowAttack = AttackPattern.JumpDosukoiSide;
        }
        else if ((NAct.ZInputState == ZInputState.ZBackPushMoment
            || NAct.ZInputState == ZInputState.ZBackPushButton)
            && !NAct.jumpFlag)
        {
            NAct.NowAttack = AttackPattern.DosukoiBack;
        }
        else if ((NAct.ZInputState == ZInputState.ZFrontPushMoment
                 || NAct.ZInputState == ZInputState.ZFrontPushButton)
                 && !NAct.jumpFlag)
        {
            NAct.NowAttack = AttackPattern.DosukoiFront;
        }
        else
        {
            NAct.NowAttack = AttackPattern.DosukoiSide;
        }
    }

}

