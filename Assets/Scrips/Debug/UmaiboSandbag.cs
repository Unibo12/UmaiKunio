using System.Collections;
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

    GameObject PlayerObjct;
    NekketsuManager Nmng;

    public AudioClip hit;
    public AudioClip dosukoiHit;
    public AudioClip audioClip3;
    private AudioSource audioSource;

    void Start()
    {
        // 最初に行う
        pos = transform.position;
        animator = this.GetComponent<Animator>();

        //Nmng = new NekketsuManager(this);

        PlayerObjct = GameObject.Find("NekketsuManager");
        Nmng = PlayerObjct.GetComponent<NekketsuManager>();

        audioSource = gameObject.GetComponent<AudioSource>();

    }

    void Update()
    {
        Vector3 scale = transform.localScale;

        // どすこい奥手前は、Z -0.4で処理できない。今後のためにも改良すべし
        if ((Z - 0.4f <= Nmng.Player[0].NVariable.Z && Nmng.Player[0].NVariable.Z <= Z + 0.4f)
            && Nmng.Player[0].NAttackV.hitBox.Overlaps(UmaiboSandbagHitBox))
        {
            animator.Play("UmaHitFrontWh");

            // ノックバックもどき仮
            if (UmaiboSandbagHitBox.x < Nmng.Player[0].NAttackV.hitBox.x)
            {

                if (Nmng.Player[0].NAttackV.NowAttack == AttackPattern.DosukoiBack)
                {
                    Z += 0.01f;
                }
                else if (Nmng.Player[0].NAttackV.NowAttack == AttackPattern.DosukoiFront)
                {
                    Z -= 0.02f;
                }
                else
                {
                    X -= 0.02f;
                    scale.x = 1; // そのまま（右向き）
                }


                if (!audioSource.isPlaying)
                {
                    if (Nmng.Player[0].NAttackV.NowAttack == AttackPattern.Dosukoi
                        || Nmng.Player[0].NAttackV.NowAttack == AttackPattern.DosukoiBack
                        || Nmng.Player[0].NAttackV.NowAttack == AttackPattern.DosukoiFront)
                    {
                        audioSource.clip = dosukoiHit;
                        audioSource.Play();

                    }
                    else
                    {
                        audioSource.clip = hit;
                        audioSource.Play();
                    }
                }
            }
            else
            {
                if (Nmng.Player[0].NAttackV.NowAttack == AttackPattern.DosukoiBack)
                {
                    Z += 0.02f;
                }
                else if(Nmng.Player[0].NAttackV.NowAttack == AttackPattern.DosukoiFront)
                {
                    Z -= 0.02f;
                }
                else
                {
                    X += 0.01f;
                    scale.x = -1; // 反転する（左向き）
                }


                if (!audioSource.isPlaying)
                {
                    if (Nmng.Player[0].NAttackV.NowAttack == AttackPattern.Dosukoi
                        || Nmng.Player[0].NAttackV.NowAttack == AttackPattern.DosukoiBack
                        || Nmng.Player[0].NAttackV.NowAttack == AttackPattern.DosukoiFront)
                    {
                        audioSource.clip = dosukoiHit;
                        audioSource.Play();
                    }
                    else
                    {
                        audioSource.clip = hit;
                        audioSource.Play();
                    }
                }

            }

            transform.localScale = scale;
        }
        else
        {
            animator.Play("UmaStamdingWh");
        }


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
    }

    // void OnDrawGizmos()
    // {
    //     // 喰らい判定のギズモを表示
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireCube(transform.position, new Vector3(UmaiboSandbagHitBox.width, UmaiboSandbagHitBox.height, 0));
    // }
}
