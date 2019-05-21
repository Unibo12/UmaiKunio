using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuSound : MonoBehaviour
{
    GameObject playerObjct;
    NekketsuManager Nmng;

    public AudioClip Brake;
    public AudioClip Jump;
    public AudioClip punch;
    private AudioSource audioSource;

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


        // ジャンプ
        if (!Nmng.Umaibou.jumpFlag &&
            !Nmng.Umaibou.squatFlag &&
            !Nmng.Umaibou.brakeFlag &&
            Nmng.Umaibou.JumpButtonState == JumpButtonPushState.PushMoment)
        {
            audioSource.clip = Jump;
            audioSource.Play();
        }

        // ブレーキ
        if (!audioSource.isPlaying 
            && Nmng.Umaibou.brakeFlag)
        {
            audioSource.clip = Brake;
            audioSource.Play();
        }


        //★★★★★★★
        // ジャンプ中攻撃
        // 本来ここでキー入力を参照しないつもりだが、一旦これで。
        // ジャンプ攻撃中、再度攻撃ボタンで音がなってしまう。。。
        //★★★★★★★
        if ((Input.GetKeyDown("z") || Input.GetKeyDown("joystick button 0")
                || Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 1"))
                    && (Nmng.Umaibou.NowAttack == AttackPattern.JumpKick
                        || Nmng.Umaibou.NowAttack == AttackPattern.JumpDosukoiSide))
        {
            audioSource.clip = punch;
            audioSource.Play();
        }
    }
}
