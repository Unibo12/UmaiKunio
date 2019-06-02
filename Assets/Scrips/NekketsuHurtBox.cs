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
        float otherPlayerZ = 0;
        Rect otherHitBox = new Rect(0, 0, 0, 0);
        switch (NAct.gameObject.name)
        {
            case "Player1":
                otherPlayerZ = NAct.Nmng.Player2.Z;
                otherHitBox = NAct.Nmng.Player2.hitBox;
                break;

            case "Player2":
                otherPlayerZ = NAct.Nmng.Player1.Z;
                otherHitBox = NAct.Nmng.Player1.hitBox;
                break;
        }

        // プレイヤー1～4の喰らい判定
        if ((otherPlayerZ - 0.4f <= NAct.Z && NAct.Z <= otherPlayerZ + 0.4f)
            && NAct.hurtBox.Overlaps(otherHitBox))
        {
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
