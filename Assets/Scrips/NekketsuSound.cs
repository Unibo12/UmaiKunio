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
    public AudioClip hijiHit;
    public AudioClip death;
    public AudioSource audioSource;

    private void Start()
    {
        playerObjct = GameObject.Find("NekketsuManager");
        Nmng = playerObjct.GetComponent<NekketsuManager>();

        audioSource = gameObject.GetComponent<AudioSource>();
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
        else if (se == SEPattern.hijiHit)
        {
            audioSource.clip = hijiHit;
        }
        else if (se == SEPattern.death)
        {
            audioSource.clip = death;
        }
        else
        {

        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
       
    }


}


