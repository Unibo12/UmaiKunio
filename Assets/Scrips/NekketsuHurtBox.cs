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

        float otherPlayerPunch = 0;
        float otherPlayerKick = 0;

        AttackPattern otherPlayerAttack = AttackPattern.None;
        Rect otherHitBox = new Rect(0, 0, 0, 0);

        // 自分から見て、他プレイヤーの情報を取得
        switch (NAct.gameObject.name)
        {
            case "Player1":
                otherPlayerX = NAct.Nmng.Player2.X;
                otherPlayerZ = NAct.Nmng.Player2.Z;
                otherPlayerPunch = NAct.Nmng.Player2.punchPow;
                otherPlayerKick = NAct.Nmng.Player2.kickPow;
                otherPlayerAttack = NAct.Nmng.Player2.NowAttack;
                otherHitBox = NAct.Nmng.Player2.hitBox;
                break;

            case "Player2":
                otherPlayerX = NAct.Nmng.Player1.X;
                otherPlayerZ = NAct.Nmng.Player1.Z;
                otherPlayerPunch = NAct.Nmng.Player1.punchPow;
                otherPlayerKick = NAct.Nmng.Player1.kickPow;
                otherPlayerAttack = NAct.Nmng.Player1.NowAttack;
                otherHitBox = NAct.Nmng.Player1.hitBox;
                break;
        }

        if (NAct.BlowUpFlag)
        {
            // 吹っ飛び処理
            if (NAct.BlowUpNowTime <= NAct.BlowUpInitalVelocityTime)
            {
                NAct.Y += 0.1f;

                // 他プレイヤーの攻撃が前か後ろか(おおよそ)で、前後に吹っ飛ぶか分ける
                if (otherPlayerX <= NAct.X)
                {
                    NAct.X += 0.01f;
                }
                else
                {
                    NAct.X -= 0.01f;
                }
            }
            else
            {
                //固定吹っ飛び時間終了なので、落下させる
                NAct.Y -= 0.08f;

                //地面についたら～
                if (NAct.Y <= 0)
                {
                    NAct.Y = 0;
                    NAct.BlowUpFlag = false;
                    NAct.NowDamage = DamagePattern.None;
                    NAct.downDamage = 0;

                    NAct.BlowUpNowTime = 0;
                }
            }

            NAct.BlowUpNowTime += Time.deltaTime;
        }
        else
        {
            // プレイヤー1～4の喰らい判定
            if ((otherPlayerZ - 0.4f <= NAct.Z && NAct.Z <= otherPlayerZ + 0.4f)
                && NAct.hurtBox.Overlaps(otherHitBox))
            {
                NAct.NowAttack = AttackPattern.None;

                // 喰らったダメージを計算
                switch (otherPlayerAttack)
                {
                    case AttackPattern.JumpKick:
                        NAct.downDamage += otherPlayerKick;
                        break;

                    default:
                        NAct.downDamage += otherPlayerPunch;
                        break;
                }

                // ダメージ喰らい状態に変更
                if (NAct.downDamage <= 100)
                {
                    if (NAct.vx == 0 && NAct.vz == 0)
                    {
                        if (otherPlayerX <= NAct.X)
                        {
                            NAct.NowDamage = DamagePattern.UmaHitFront;
                        }
                        else
                        {
                            NAct.NowDamage = DamagePattern.UmaHitBack;
                        }
                    }
                    else
                    {
                        NAct.NowDamage = DamagePattern.UmaHogeWalk;
                    }
                }
                else if (NAct.downDamage <= 200)
                {
                    NAct.NowDamage = DamagePattern.groggy;
                }
                else if (NAct.downDamage <= 300)
                {
                    // 他プレイヤーの攻撃が前か後ろか(おおよそ)と、
                    // 自キャラの向きから吹っ飛びアニメを切り替え
                    if (otherPlayerX <= NAct.X)
                    {
                        if (NAct.leftFlag)
                        {
                            NAct.NowDamage = DamagePattern.UmaBARF;
                        }
                        else
                        {
                            NAct.NowDamage = DamagePattern.UmaOttotto;
                        }
                    }
                    else
                    {
                        if (NAct.leftFlag)
                        {
                            NAct.NowDamage = DamagePattern.UmaOttotto;
                        }
                        else
                        {
                            NAct.NowDamage = DamagePattern.UmaBARF;
                        }
                    }

                    NAct.BlowUpFlag = true;

                    NAct.downDamage = 0;
                }
                else
                {

                }

                // ノックバック処理
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

                // 攻撃効果音
                if (otherPlayerAttack == AttackPattern.Dosukoi
                    || otherPlayerAttack == AttackPattern.DosukoiBack
                    || otherPlayerAttack == AttackPattern.DosukoiFront)
                {
                    NSound.SEPlay(SEPattern.hit);
                }
                else
                {
                    NSound.SEPlay(SEPattern.hijiHit);
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
                    NAct.BlowUpFlag = true;
                    NAct.NowDamage = DamagePattern.UmaBARF;
                    NSound.SEPlay(SEPattern.hit);
                }
            }
        }
    }
}
