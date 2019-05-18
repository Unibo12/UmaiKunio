using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuSound : MonoBehaviour
{
    GameObject playerObjct;
    NekketsuManager Nmng;


    public AudioClip Brake;
    public AudioClip Jump;
    public AudioClip audioClip3;
    private AudioSource audioSource;

    public void SoundMain()
    {
        // アニメーション　ループに合わせて音出し
        // が、うまく行かなかったので熱血サウンド作成したがこちらも微妙。

        playerObjct = GameObject.Find("NekketsuManager");
        Nmng = playerObjct.GetComponent<NekketsuManager>();

        audioSource = gameObject.GetComponent<AudioSource>();

        if (!Nmng.Umaibou.jumpFlag &&
            !Nmng.Umaibou.squatFlag &&
            !Nmng.Umaibou.brakeFlag &&
            Nmng.Umaibou.JumpButtonState == JumpButtonPushState.PushMoment)
        {
            audioSource.clip = Jump;
            audioSource.Play();
        }

        //switch (soundName)
        //{
        //    case "Brake":
        //        audioSource.clip = Brake;
        //        audioSource.Play();
        //        break;

        //    case "Jump":
        //        audioSource.clip = Jump;
        //        audioSource.Play();
        //        break;
        //}

    }

}
