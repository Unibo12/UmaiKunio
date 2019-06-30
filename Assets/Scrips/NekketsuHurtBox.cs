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

    // 喰らい判定で必要な相手の情報
    DamagePattern otherPlayerDmgPtn = DamagePattern.None;
    float otherPlayerX = 0;
    float otherPlayerZ = 0;
    bool otherPlayerLeftFlag = false;
    float otherPlayerPunch = 0;
    float otherPlayerKick = 0;
    AttackPattern otherPlayerAttack = AttackPattern.None;
    Rect otherHitBox = new Rect(0, 0, 0, 0);

    public NekketsuHurtBox(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    /// @@@Main関数が長くなってしまっているので、
    /// 処理ごとに関数を切るのが良いでしょう
    /// regionで切るのも良いですが、
    /// regionは変数のスコープが長いままにもなりますし
    /// 今後さらに膨れてきた際にバグの発見が難しくなることや
    /// チームで仕事をする際に、流れを知らない他の人が目的箇所にたどり着くため
    /// 上から延々と処理を追うことになってしまうので、
    /// 処理の見出しを作るような感覚で、可能な限り関数で小分けにしておくのが良いかと思います
    /// 自分はVSCODEを使っていますが、
    /// VisualStudioにもメソッドの抽出があると思いますので、
    /// https://docs.microsoft.com/ja-jp/visualstudio/ide/reference/extract-method?view=vs-2019
    /// 積極的に使ってみてください。
    public void HurtBoxMain(NekketsuSound NSound)
    {
        //他プレイヤーの攻撃情報取得
        getOtherPlayerInfo();

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
                        NAct.NJumpV.squatFlag = true;

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
                    // @@@相手の位置ではなく向きで攻撃方向をとった方がいいですね
                    // 旋風脚のように相対位置で飛び方向を決めたほうがいいものもあるのでそれは技にフラグをもたせるのが良いです
                    if (otherPlayerX <= NAct.X)
                    {
                        NAct.X += 0.01f;
                    }
                    else
                    {
                        NAct.X -= 0.01f;
                    }

                    //TODO:
                    //相手の向いている方向で吹っ飛びを考慮した下記修正だと、
                    //攻撃終わり(=otherPlayerAttackがNone)のときNG
                    //相手のキメ攻撃を覚えて置く必要があるのか？
                    //NAct.X += GetSign(otherPlayerLeftFlag, otherPlayerAttack) * 0.01f;
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
                    NAct.NMoveV.dashFlag = false;  //被弾したらダッシュ解除

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
                    if (!NAct.NJumpV.jumpFlag)
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
                            //吹っ飛び状態に切り替え
                            changeBlowUp();
                        }
                    }
                    else
                    {
                        //自分がジャンプ状態のときに、
                        //攻撃を受けた際は蓄積ダメージを考慮せず、固定でダウン状態へ

                        //吹っ飛び状態に切り替え
                        changeBlowUp();

                        //ジャンプ中にダウンしたので、ジャンプ周りをリセット
                        NAct.vy = 0;
                        NAct.NJumpV.jumpFlag = false;
                        NAct.NJumpV.jumpSpeed = 0;
                    }

                    if (NAct.NowDamage == DamagePattern.UmaHitBack
                        || NAct.NowDamage == DamagePattern.UmaHitFront)
                    {
                        //ノックバック処理
                        knockBack();
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
                    NAct.NJumpV.jumpFlag = false;
                }
            }
        }
    }

    /// <summary>
    /// ノックバック処理
    /// </summary>
    private void knockBack()
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

    /// <summary>
    /// 吹っ飛び状態へ切り替え
    /// </summary>
    private void changeBlowUp()
    {
        // 他プレイヤーの攻撃が前か後ろか(おおよそ)と、
        // 自キャラの向きから吹っ飛びアニメを切り替え
        if (otherPlayerX <= NAct.X)
        {
            if (NAct.NMoveV.leftFlag)
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
            if (NAct.NMoveV.leftFlag)
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

    /// <summary>
    /// 他プレイヤーの情報取得
    /// </summary>
    private void getOtherPlayerInfo()
    {
        // 自分から見て、他プレイヤーの情報を取得
        switch (NAct.gameObject.name)
        {
            case "Player1":
                otherPlayerDmgPtn = NAct.Nmng.Player[1].NowDamage;
                otherPlayerX = NAct.Nmng.Player[1].X;
                otherPlayerZ = NAct.Nmng.Player[1].Z;
                otherPlayerPunch = NAct.Nmng.Player[1].st_punch;
                otherPlayerKick = NAct.Nmng.Player[1].st_kick;
                otherPlayerAttack = NAct.Nmng.Player[1].NowAttack;
                otherHitBox = NAct.Nmng.Player[1].hitBox;
                otherPlayerLeftFlag = NAct.Nmng.Player[1].NMoveV.leftFlag;
                break;

            case "Player2":
                otherPlayerDmgPtn = NAct.Nmng.Player[0].NowDamage;
                otherPlayerX = NAct.Nmng.Player[0].X;
                otherPlayerZ = NAct.Nmng.Player[0].Z;
                otherPlayerPunch = NAct.Nmng.Player[0].st_punch;
                otherPlayerKick = NAct.Nmng.Player[0].st_kick;
                otherPlayerAttack = NAct.Nmng.Player[0].NowAttack;
                otherHitBox = NAct.Nmng.Player[0].hitBox;
                otherPlayerLeftFlag = NAct.Nmng.Player[0].NMoveV.leftFlag;
                break;
        }
    }

    /// <summary>
    /// 左右の向きの符号を返す(Leftはマイナス、Rightはプラス)
    /// </summary>
    /// <param name="leftFlag"></param>
    /// <param name="otherPlayerAttack"></param>
    /// <returns></returns>
    int GetSign(bool leftFlag, AttackPattern otherPlayerAttack)
    {
        //TODO
        //このままだと、攻撃終わり(None)の挙動がNG


        // 肘攻撃の場合は逆向きの為、ここで向きを調節
        if (otherPlayerAttack == AttackPattern.Hiji
            || otherPlayerAttack == AttackPattern.HijiWalk)
        {
            return leftFlag ? +1 : -1;
        }
        else
        {
            return leftFlag ? -1 : +1;
        }
    }
}
