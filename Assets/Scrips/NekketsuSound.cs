using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 効果音(SE)を管理するクラス
/// </summary>
public class NekketsuSound : MonoBehaviour
{
    GameObject playerObjct;
    NekketsuManager Nmng;

    private NekketsuMove NMove; //NekketsuMoveを呼び出す際に使用
    private NekketsuAction NAct; //NekketsuActionが入る変数

    public AudioClip Brake;
    public AudioClip Jump;
    public AudioClip attack;
    public AudioClip hit;
    public AudioSource audioSource;

    private void Start()
    {

        playerObjct = GameObject.Find("NekketsuManager");
        Nmng = playerObjct.GetComponent<NekketsuManager>();

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void SoundMain()
    {
        //★TODO★
        //アニメーションのスクリプト呼び出しでやるか、
        //熱血サウンドでやるか どちらかにまとめること。
        //地上どすこい等はアニメでやっている。
        //★TODO★
        //NMove.MoveMain();

        //// ジャンプ
        //if (!Nmng.Umaibou.jumpFlag &&
        //    !Nmng.Umaibou.squatFlag &&
        //    !Nmng.Umaibou.brakeFlag &&
        //    Nmng.Umaibou.JumpButtonState == JumpButtonPushState.PushMoment)
        //{
        //    audioSource.clip = Jump;
        //    audioSource.Play();
        //}

        //// ブレーキ
        //if (!audioSource.isPlaying
        //    && Nmng.Umaibou.brakeFlag)
        //{
        //    audioSource.clip = Brake;
        //    audioSource.Play();
        //}


        ////★★★★★★★
        //// ジャンプ中攻撃
        //// 本来ここでキー入力を参照しないつもりだが、一旦これで。
        //// ジャンプ攻撃中、再度攻撃ボタンで音がなってしまう。。。
        ////★★★★★★★
        //if ((Input.GetKeyDown("z") || Input.GetKeyDown("joystick button 0")
        //        || Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 1"))
        //            && (Nmng.Umaibou.NowAttack == AttackPattern.JumpKick
        //                || Nmng.Umaibou.NowAttack == AttackPattern.JumpDosukoiSide))
        //{
        //    audioSource.clip = attack;
        //    audioSource.Play();
        //}

        //// テスト（うにうにくん被弾時ダメージ）
        //if (!audioSource.isPlaying
        //    && Nmng.Umaibou.NowDamage == DamagePattern.groggy)
        //{
        //    audioSource.clip = hit;
        //    audioSource.Play();
        //}
    }

    /// <summary>
    /// 効果音を再生させる。
    /// </summary>
    /// <param name="se"></param>
    public void SEPlay(SEPattern se)
    {
        if (se == SEPattern.brake)
        {
            audioSource.clip = Brake;
        }
        else if (se == SEPattern.attack)
        {
            audioSource.clip = attack;
        }
        else if (se == SEPattern.jump)
        {
            audioSource.clip = Jump;
        }
        else if (se == SEPattern.hit)
        {
            audioSource.clip = hit;
        }
        else
        {

        }

        audioSource.Play();
    }


}


