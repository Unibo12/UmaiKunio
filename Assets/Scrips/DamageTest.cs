using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    Vector3 pos;        // 最終的な描画で使用
    Animator animator;  // アニメ変更用

    private NekketsuHurtBox NHurtBox;

    public float X;    //内部での横
    public float Y;    //内部での高さ
    public float Z;    //内部での奥行き

    public Rect hitBoxTEST;


    void Start()
    {
        // 最初に行う
        pos = transform.position;
        animator = this.GetComponent<Animator>();

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
        hitBoxTEST = 
            new Rect(X, Y,
                    this.gameObject.GetComponent<RectTransform>().rect.width,
                    this.gameObject.GetComponent<RectTransform>().rect.height);

        transform.position = pos;
        #endregion

    }

    // void OnDrawGizmos()
    // {
    //     // 喰らい判定のギズモを表示
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(transform.position, new Vector3(hitBoxTEST.width, hitBoxTEST.height, 0));
    // }
}
