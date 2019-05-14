﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmaiboSandbag : MonoBehaviour
{

    //★デバッグ用の　喰らい判定を持ったオブジェクトにアタッチする
    //★サンドバッグとしてつかう。


    Vector3 pos;        // 最終的な描画で使用
    Animator animator;  // アニメ変更用

    public float X;    //内部での横
    public float Y;    //内部での高さ
    public float Z;    //内部での奥行き

    public Rect UmaiboSandbagHitBox;

    GameObject playerObjct;
    NekketsuAction NActScript;

    NekketsuAction NAct; //NekketsuActionが入る変数
    public UmaiboSandbag(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    void Start()
    {
        // 最初に行う
        pos = transform.position;
        animator = this.GetComponent<Animator>();


        playerObjct = GameObject.Find("Umaibou");
        NActScript = playerObjct.GetComponent<NekketsuAction>();
    }

    void Update()
    {
        #region 画面への描画
        // 入力された内部XYZをtransformに設定する。

        // 基本的に、描画位置はジャンプなどのキャラ状態かかわらず、同じように内部座標を描画座標に適用する
        // （適用できるように、必要ならば内部座標の段階で調整をしておく）

        pos.x = X;
        pos.y = Z + Y;

        // 喰らい判定の移動
        UmaiboSandbagHitBox = new Rect(X, Y, 0.7f, 1.6f);

        transform.position = pos;
        #endregion

        if ((Z - 0.4f <= NActScript.Z && NActScript.Z <= Z + 0.4f)
            && NActScript.hitBox.Overlaps(UmaiboSandbagHitBox))
        {
            animator.Play("UmaHitFrontWh");
        }
        else
        {
            animator.Play("UmaStamdingWh");
        }
    }

    void OnDrawGizmos()
    {
        // 喰らい判定のギズモを表示
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(UmaiboSandbagHitBox.width, UmaiboSandbagHitBox.height, 0));
    }
}