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

    // *****共通変数*****
    public float speed = 0.08f;                 // スピード
    public float jumpSpeed = 0f;                // ジャンプスピード管理変数
    public float Gravity = -0.006f;             // 内部での重力
    public float InitalVelocity = 0.188f;        // 内部での初速
    public float nextButtonDownTimeDash = 1f;   // ダッシュを受け付ける時間

    public float X = 0;    //内部での横
    public float Y = 0;    //内部での高さ
    public float Z = 0;    //内部での奥行き
    public float vx = 0;   //内部X値用変数
    public float vy = 0;   //内部Y値用変数
    public float vz = 0;   //内部Z値用変数

    public Rect hurtBox = new Rect(0, 0, 0.7f, 1.6f);
    public Rect hitBox = new Rect(0, 0, 0, 0);

    public bool leftFlag = false;   //左向きかどうか
    public bool jumpFlag = false;   //ジャンプして空中にいるか
    public bool dashFlag = false;   //走っているか否か
    public bool squatFlag = false;  //しゃがみ状態フラグ
    public bool brakeFlag = false;  //ブレーキフラグ
    public bool AttackMomentFlag = false;  //攻撃し始めフラグ(空中攻撃出し始め判定)
    public bool BlowUpFlag = false; //吹っ飛び状態か否か

    public JumpButtonPushState JumpButtonState; //ジャンプボタン押下ステータス
    public XInputState XInputState = 0; //疑似Xに対する入力ステータス
    public ZInputState ZInputState = 0; //疑似Zに対する入力ステータス
    public AttackPattern NowAttack = 0; //現在の攻撃パターン格納変数
    public DamagePattern NowDamage = 0; //現在の攻撃喰らいパターン格納変数

    public float BlowUpNowTime = 0;               //吹っ飛んでいる時間計測
    public float BlowUpInitalVelocityTime = 0.2f; //きめ攻撃等で吹っ飛んだ際の吹っ飛び時間

    public float downDamage = 0;  //ダウンまでの蓄積ダメージ
    public float nowDownTime = 0; //ダウン時間計測

    public ItemPattern haveItem = ItemPattern.None; //所持アイテム

    //♡♡♡♡♡すてーたす♡♡♡♡♡
    public float st_life = 0;       //たいりょく
    public float st_punch = 0;      //ぱんち
    public float st_kick = 0;       //きっく
    public float st_downTime = 1;   //おきあがりじかん
    public float st_brake = 0.5f;   //ぶれーき
    //♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡

    // *****共通変数*****

    #endregion

    void Start()
    {
        // 最初に行う
        pos = transform.position;
        animator = this.GetComponent<Animator>();

        gameObjct = this.gameObject;
        NSound = gameObjct.GetComponent<NekketsuSound>();

        gameObjct = GameObject.Find("NekketsuManager");
        Nmng = gameObjct.GetComponent<NekketsuManager>();

        // 生成（コンストラクタ）の引数にNekketsuActionを渡してやる
        NMove = new NekketsuMove(this);
        NJump = new NekketsuJump(this);
        NInput = new NekketsuInput(this);
        NHurtBox = new NekketsuHurtBox(this);
        NStateChange = new NekketsuStateChange(this);
    }

    void Update()
    { // ずっと行う

        vx = 0;
        vz = 0;

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

        #region 画面への描画
        // 入力された内部XYZをtransformに設定する。

        // 基本的に、描画位置はジャンプなどのキャラ状態かかわらず、同じように内部座標を描画座標に適用する
        // （適用できるように、必要ならば内部座標の段階で調整をしておく）

        //座標への速度反映
        X += vx;
        //Y += vy;
        Z += vz;

        pos.x = X;
        pos.y = Z + Y;

        transform.position = pos;

        #region 所持アイテムの位置調整
        if (haveItem != ItemPattern.None)
        {
            Transform ItemTransform = GameObject.Find(haveItem.ToString()).transform;
            Vector3 ItemScale = ItemTransform.localScale;
            Vector3 ItemPos = pos;
            if (leftFlag)
            {
                ItemPos.x = pos.x - 0.4f;
                ItemScale.x = -1;
            }
            else
            {
                ItemPos.x = pos.x + 0.4f;
                ItemScale.x = 1;
            }
            ItemPos.y = pos.y + 0.25f;
            ItemTransform.localScale = ItemScale;

            ItemTransform.position = ItemPos;
        }

        #endregion


        // 喰らい判定の移動
        if (NowDamage == DamagePattern.UmaTaore
            || NowDamage == DamagePattern.UmaTaoreUp)
        {
            //倒れ状態の当たり判定(アイテム化)
            hurtBox = new Rect(X, Y+Z, 1.6f, 0.7f);
        }
        else
        {
            //通常当たり判定
            hurtBox = new Rect(X, Y+Z, 0.7f, 1.6f);
        }


        #endregion

        #region スプライト反転処理
        Vector3 scale = transform.localScale;
        if (leftFlag)
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
        Transform shadeTransform = GameObject.Find(this.gameObject.name + "_Shade").transform;
         
        pos.y = Z - 0.8f;

        if (!squatFlag)
        {
            shadeTransform.position = pos;
        }
        #endregion

        #region アニメ処理

        if (NowDamage != DamagePattern.None)
        {
            // ダメージアニメ処理
            animator.Play(NowDamage.ToString());
        }
        else
        {
            if (!squatFlag && !brakeFlag)
            {
                if (NowAttack != AttackPattern.None)
                {
                    if (haveItem == ItemPattern.None)
                    {
                        // 現在の攻撃状態をアニメーションさせる。
                        animator.Play(NowAttack.ToString());
                    }
                    else
                    {
                        
                    }

                }
                else
                {
                    //  攻撃以外のアニメーション
                    if (vx == 0 && vz == 0)
                    {
                        animator.SetBool("Walk", false);
                    }
                    else
                    {
                        animator.SetBool("Walk", true);
                    }

                    if (jumpFlag
                        && NowDamage == DamagePattern.None)
                    {
                        animator.Play("Jump");
                    }
                }
            }
            else
            {
                if (brakeFlag)
                {
                    animator.Play("Brake");
                }

                if (squatFlag)
                {
                    animator.Play("Squat");
                }
            }
        }

        #endregion
    }

    void OnDrawGizmos()
    {
        // 喰らい判定のギズモを表示
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(hurtBox.width, hurtBox.height, 0));

        // 攻撃判定のギズモを表示
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(hitBox.x, Z + hitBox.y), new Vector3(hitBox.width, hitBox.height, 0.1f));
    }
}