using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 熱血風アクション
/// </summary>
public class NekketsuAction : MonoBehaviour
{
    #region 変数定義

    Vector3 pos;               // 最終的な描画で使用
    public Animator animator;  // アニメ変更用

    GameObject gameObjct;
    private NekketsuSound NSound;
    private NekketsuAttack NAttack;
    public NekketsuManager Nmng;

    private NekketsuMove NMove; //NekketsuMoveを呼び出す際に使用
    private NekketsuJump NJump; //NekketsuJumpを呼び出す際に使用
    private NekketsuInput NInput; //NekketsuInputを呼び出す際に使用
    private NekketsuHurtBox NHurtBox; //NekketsuHurtBoxを呼び出す際に使用
    private NekketsuStateChange NStateChange; //NekketsuStateChangeを呼び出す際に使用
    private NekketsuHaveItem NHaveItem;

    public NekketsuVariable NVariable;
    public NekketsuMoveVariable NMoveV;
    public NekketsuJumpVariable NJumpV;
    public NekketsuAttackVariable NAttackV;

    /// @@@変数が膨れてきたので、そろそろ
    /// ジャンルごとに変数用のクラスを作ったほうが良いかと思います。

    //アイテム
    public ItemPattern haveItem = ItemPattern.None; //所持アイテム

    //影の位置
    Transform shadeTransform;

    #endregion

    void Start()
    {
        // 最初に行う
        pos = transform.position;
        animator = this.GetComponent<Animator>();

        //自分にアタッチされている効果音、変数クラスを取得
        gameObjct = this.gameObject;
        NSound = gameObjct.GetComponent<NekketsuSound>();
        NVariable = gameObjct.GetComponent<NekketsuVariable>();
        NMoveV = gameObjct.GetComponent<NekketsuMoveVariable>();
        NJumpV = gameObjct.GetComponent<NekketsuJumpVariable>();
        NAttackV = gameObjct.GetComponent<NekketsuAttackVariable>();

        //熱血マネージャ取得
        gameObjct = GameObject.Find("NekketsuManager");
        Nmng = gameObjct.GetComponent<NekketsuManager>();

        // 生成（コンストラクタ）の引数にNekketsuActionを渡してやる
        NMove = new NekketsuMove(this);
        NJump = new NekketsuJump(this);
        NInput = new NekketsuInput(this);
        NHurtBox = new NekketsuHurtBox(this);
        NStateChange = new NekketsuStateChange(this);
        NHaveItem = new NekketsuHaveItem(this);

        shadeTransform = GameObject.Find(this.gameObject.name + "_Shade").transform;
    }

    void Update()
    { // ずっと行う

        NVariable.vx = 0;
        NVariable.vz = 0;

        // インプット処理呼び出し
        NInput.InputMain();

        // 入力されたインプット内容でステータスを変更
        NStateChange.StateChangeMain();

        // 攻撃の処理
        //NAttack.AttackMain();

        // 移動処理呼び出し
        NMove.MoveMain(NSound);

        // ジャンプ処理呼び出し
        NJump.JumpMain(NSound);

        // 攻撃喰らい判定
        NHurtBox.HurtBoxMain(NSound);

        // 所持アイテムの管理
        NHaveItem.NekketsuHaveItemMain();

        #region 画面への描画
        // 入力された内部XYZをtransformに設定する。

        // 基本的に、描画位置はジャンプなどのキャラ状態かかわらず、同じように内部座標を描画座標に適用する
        // （適用できるように、必要ならば内部座標の段階で調整をしておく）

        /// @@@Y座標も同じ場所で速度反映を行ったほうがよいです
        /// ここで行うと着地時めり込むということだと思うのですが
        /// いずれ直面する左右の壁めり込みなども同様の現象なので
        /// 「速度を座標に反映する」処理の後に、「地形にぶつかっていたら適切な位置に戻す」
        /// という処理を入れる必要があります。
        /// とりあえずY座標だけでもめり込み判定をNekketsuJumpから取り出し
        /// NekketsuMerikomiCheckみたいなクラスを作り、
        /// 速度を座標に反映する処理の後に、上記クラスの処理を通して適切な位置に戻す流れを作っておくのが良いでしょう

        //座標への速度反映
        NVariable.X += NVariable.vx;
        //Y += vy;
        NVariable.Z += NVariable.vz;

        pos.x = NVariable.X;
        pos.y = NVariable.Z + NVariable.Y;

        transform.position = pos;

        // 喰らい判定の移動
        if (NAttackV.NowDamage == DamagePattern.UmaTaore
            || NAttackV.NowDamage == DamagePattern.UmaTaoreUp)
        {
            //倒れ状態の当たり判定(アイテム化)
            NAttackV.hurtBox = new Rect(NVariable.X, NVariable.Y+NVariable.Z, 1.6f, 0.7f);
        }
        else
        {
            //通常当たり判定
            NAttackV.hurtBox = new Rect(NVariable.X, NVariable.Y+NVariable.Z, 0.7f, 1.6f);
        }

        #endregion

        #region スプライト反転処理
        Vector3 scale = transform.localScale;
        if (NMoveV.leftFlag)
        {
            scale.x = -1; // 反転する（左向き）
        }
        else
        {
            scale.x = 1; // そのまま（右向き）
        }

        transform.localScale = scale;
        #endregion

        #region キャラクターの影の位置描画処理
         
        pos.y = NVariable.Z - 0.8f;

        if (!NJumpV.squatFlag)
        {
            shadeTransform.position = pos;
        }
        #endregion

        #region アニメ処理

        if (NAttackV.NowDamage != DamagePattern.None)
        {
            // ダメージアニメ処理
            animator.Play(NAttackV.NowDamage.ToString());
        }
        else
        {
            if (!NJumpV.squatFlag && !NMoveV.brakeFlag)
            {
                if (NAttackV.NowAttack != AttackPattern.None)
                {
                    if (haveItem == ItemPattern.None)
                    {
                        // 現在の攻撃状態をアニメーションさせる。
                        animator.Play(NAttackV.NowAttack.ToString());
                    }
                    else
                    {
                        
                    }

                }
                else
                {
                    //  攻撃以外のアニメーション
                    if (NVariable.vx == 0 && NVariable.vz == 0)
                    {
                        animator.SetBool("Walk", false);
                    }
                    else
                    {
                        animator.SetBool("Walk", true);
                    }

                    if (NJumpV.jumpFlag
                        && NAttackV.NowDamage == DamagePattern.None)
                    {
                        animator.Play("Jump");
                    }
                }
            }
            else
            {
                if (NMoveV.brakeFlag)
                {
                    animator.Play("Brake");
                }

                if (NJumpV.squatFlag)
                {
                    animator.Play("Squat");
                }
            }
        }

        #endregion

        #region 失格判定(ゲームシーンから削除)

        if (NVariable.DeathFlag == DeathPattern.death)
        {
            Destroy(this.gameObject);
        }

        #endregion
    }

    void OnDrawGizmos()
    {
        // 喰らい判定のギズモを表示
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(NAttackV.hurtBox.width, NAttackV.hurtBox.height, 0));

        // 攻撃判定のギズモを表示
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(NAttackV.hitBox.x, NVariable.Z + NAttackV.hitBox.y), new Vector3(NAttackV.hitBox.width, NAttackV.hitBox.height, 0.1f));
    }
}