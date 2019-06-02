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

    public void HurtBoxMain(NekketsuSound NSound)
    {
        float otherPlayerX = 0;
        float otherPlayerZ = 0;
        AttackPattern otherPlayerAttack = AttackPattern.None;
        Rect otherHitBox = new Rect(0, 0, 0, 0);
        switch (NAct.gameObject.name)
        {
            case "Player1":
                otherPlayerX = NAct.Nmng.Player2.X;
                otherPlayerZ = NAct.Nmng.Player2.Z;
                otherPlayerAttack = NAct.Nmng.Player2.NowAttack;
                otherHitBox = NAct.Nmng.Player2.hitBox;
                break;

            case "Player2":
                otherPlayerX = NAct.Nmng.Player1.X;
                otherPlayerZ = NAct.Nmng.Player1.Z;
                otherPlayerAttack = NAct.Nmng.Player1.NowAttack;
                otherHitBox = NAct.Nmng.Player1.hitBox;
                break;
        }

        // プレイヤー1～4の喰らい判定
        if ((otherPlayerZ - 0.4f <= NAct.Z && NAct.Z <= otherPlayerZ + 0.4f)
            && NAct.hurtBox.Overlaps(otherHitBox))
        {
            if (otherPlayerX <= NAct.X)
            {
                if (otherPlayerAttack == AttackPattern.DosukoiBack)
                {
                    NAct.Z += 0.02f;
                }
                else if (otherPlayerAttack == AttackPattern.DosukoiFront)
                {
                    NAct.Z -= 0.02f;
                }
                else
                {
                    NAct.X += 0.02f;
                    //scale.x = 1; // そのまま（右向き）
                }
            }
            else
            {
                if (otherPlayerAttack == AttackPattern.DosukoiBack)
                {
                    NAct.Z += 0.02f;
                }
                else if (otherPlayerAttack == AttackPattern.DosukoiFront)
                {
                    NAct.Z -= 0.02f;
                }
                else
                {
                    NAct.X -= 0.02f;
                    //scale.x = 1; // そのまま（右向き）
                }
            }

            NAct.NowDamage = DamagePattern.groggy;
            NAct.NowAttack = AttackPattern.None;
            if (!NSound.audioSource.isPlaying)
            {
                NSound.SEPlay(SEPattern.hit);
            }
        }
        else
        {
            NAct.NowDamage = DamagePattern.None;
        }


        // テスト用 うにうにくんダメージ
        if ((NAct.Nmng.uni.Z - 0.4f <= NAct.Z && NAct.Z <= NAct.Nmng.uni.Z + 0.4f)
            && NAct.hurtBox.Overlaps(NAct.Nmng.uni.hitBoxTEST))
        {
            NAct.NowDamage = DamagePattern.groggy;
            if (!NSound.audioSource.isPlaying)
            {
                NSound.SEPlay(SEPattern.hit);
            }
        }
    }
}
