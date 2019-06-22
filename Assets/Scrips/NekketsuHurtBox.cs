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
        DamagePattern otherPlayerDmgPtn = DamagePattern.None;

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
                otherPlayerDmgPtn = NAct.Nmng.Player2.NowDamage;
                otherPlayerX = NAct.Nmng.Player2.X;
                otherPlayerZ = NAct.Nmng.Player2.Z;
                otherPlayerPunch = NAct.Nmng.Player2.st_punch;
                otherPlayerKick = NAct.Nmng.Player2.st_kick;
                otherPlayerAttack = NAct.Nmng.Player2.NowAttack;
                otherHitBox = NAct.Nmng.Player2.hitBox;
                break;

            case "Player2":
                otherPlayerDmgPtn = NAct.Nmng.Player1.NowDamage;
                otherPlayerX = NAct.Nmng.Player1.X;
                otherPlayerZ = NAct.Nmng.Player1.Z;
                otherPlayerPunch = NAct.Nmng.Player1.st_punch;
                otherPlayerKick = NAct.Nmng.Player1.st_kick;
                otherPlayerAttack = NAct.Nmng.Player1.NowAttack;
                otherHitBox = NAct.Nmng.Player1.hitBox;
                break;
        }

        // ダウン状態の時()
        if (NAct.NowDamage == DamagePattern.UmaTaore
            || NAct.NowDamage == DamagePattern.UmaTaoreUp)
        {
            // まだ失格していない
            if (NAct.DeathFlag == DeathPattern.None)
            {
                // ダウンした瞬間、体力0なら失格
                if (NAct.st_life <= 0)
                {
                    if (NAct.DeathFlag == DeathPattern.None)
                    {
                        NSound.SEPlay(SEPattern.death);
                    }

                    NAct.DeathFlag = DeathPattern.deathNow;
                }
                else
                {
                    // たいりょくがまだあるので起き上がり

                    //現在計測中のダウン時間が、ダウン時間(各キャラのステータス値)を超えた場合
                    if (NAct.st_downTime < NAct.nowDownTime)
                    {
                        NAct.NowDamage = DamagePattern.SquatGetUp; //起き上がり(しゃがみ)
                        NAct.squatFlag = true;

                        NAct.nowDownTime = 0;

                    }
                    NAct.nowDownTime += Time.deltaTime;
                }
            }
        }
        else
        {
            //ふっとばされ中
            if (NAct.BlowUpFlag)
            {
                // ふっとび処理
                if (NAct.BlowUpNowTime <= NAct.BlowUpInitalVelocityTime)
                {
                    NAct.Y += 0.1f;

                    if (otherPlayerAttack == AttackPattern.DosukoiBack)
                    {
                        NAct.Z += 0.1f;
                    }

                    if (otherPlayerAttack == AttackPattern.DosukoiFront)
                    {
                        NAct.Z -= 0.1f;
                    }

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

                    //ふっとび状態から地面についたら
                    if (NAct.Y <= 0)
                    {
                        NAct.Y = 0;
                        NAct.BlowUpFlag = false; ;
                        NAct.downDamage = 0;

                        //ふっとびパターンによってダウン状態(ドット絵)を変更
                        if (NAct.NowDamage == DamagePattern.UmaBARF)
                        {
                            NAct.NowDamage = DamagePattern.UmaTaoreUp;
                        }
                        else if (NAct.NowDamage == DamagePattern.UmaOttotto)
                        {
                            NAct.NowDamage = DamagePattern.UmaTaore;
                        }

                        NAct.BlowUpNowTime = 0;
                    }
                }

                NAct.BlowUpNowTime += Time.deltaTime;
            }
            else
            {
                // プレイヤー1～4の喰らい判定
                if ((otherPlayerZ - 0.4f <= NAct.Z && NAct.Z <= otherPlayerZ + 0.4f)
                    && NAct.hurtBox.Overlaps(otherHitBox)
                    && otherHitBox != Settings.Instance.Attack.AttackNone)
                {
                    NAct.NowAttack = AttackPattern.None;
                    NAct.dashFlag = false;  //被弾したらダッシュ解除

                    if (NAct.NowDamage == DamagePattern.None)
                    {
                        // 喰らったダメージを計算
                        switch (otherPlayerAttack)
                        {
                            case AttackPattern.JumpKick:
                                NAct.downDamage += otherPlayerKick;
                                NAct.st_life -= otherPlayerKick;

                                if (NAct.st_life <= 0)
                                {
                                    NAct.st_life = 0;
                                }
                                break;

                            default:
                                NAct.downDamage += otherPlayerPunch;
                                NAct.st_life -= otherPlayerPunch;

                                if (NAct.st_life <= 0)
                                {
                                    NAct.st_life = 0;
                                }
                                break;
                        }
                    }

                    // 自キャラが空中にいるか？
                    if (!NAct.jumpFlag)
                    {
                        //自キャラが地上にいるので通常のダメージ計算

                        // ダメージ喰らい状態に変更
                        if (NAct.downDamage < 20)
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

                            //ダメージ受けた瞬間から、蓄積ダメージリセットまでの時間を計測
                            NAct.nowHogeTime = 0;
                        }
                        else if (NAct.NowDamage != DamagePattern.UmaHoge
                                 && 20 <= NAct.downDamage)
                        {
                            NAct.NowDamage = DamagePattern.UmaHoge;

                            //ダメージ受けた瞬間から、蓄積ダメージリセットまでの時間を計測
                            NAct.nowHogeTime = 0;
                        }
                        else if (NAct.NowDamage == DamagePattern.UmaHoge
                                 && 30 <= NAct.downDamage)
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
                    }
                    else
                    {
                        //自分がジャンプ状態のときに、
                        //攻撃を受けた際は蓄積ダメージを考慮せず、固定でダウン状態へ

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

                        //ジャンプ中にダウンしたので、ジャンプ周りをリセット
                        NAct.vy = 0; 
                        NAct.jumpFlag = false;
                        NAct.jumpSpeed = 0;
                    }

                    if (NAct.NowDamage == DamagePattern.UmaHitBack
                        || NAct.NowDamage == DamagePattern.UmaHitFront)
                    {
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
                            }
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

                // ★デバッグ用 うにうにくんダメージ★
                if ((NAct.Nmng.uni.Z - 0.4f <= NAct.Z && NAct.Z <= NAct.Nmng.uni.Z + 0.4f)
                    && NAct.hurtBox.Overlaps(NAct.Nmng.uni.hitBoxTEST))
                {
                    NAct.NowDamage = DamagePattern.UmaHoge;
                    if (!NSound.audioSource.isPlaying)
                    {
                        NAct.BlowUpFlag = true;
                        NAct.NowDamage = DamagePattern.UmaBARF;
                        NSound.SEPlay(SEPattern.hit);
                    }

                    //ジャンプ中にダウンを考慮して、ジャンプ周りをリセット
                    NAct.vy = 0;
                    NAct.jumpFlag = false;
                }
            }
        }
    }
}
