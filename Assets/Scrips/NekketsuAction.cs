using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 熱血風アクション
/// </summary>
public class NekketsuAction : MonoBehaviour
{
    #region 変数定義

    Vector3 pos;        // 最終的な描画で使用
    public Animator animator;  // アニメ変更用

    GameObject gameObjct;
    private NekketsuSound NSound;
    private NekketsuAttack NAttack;
    public NekketsuManager Nmng;

    private NekketsuMove NMove; //NekketsuMoveを呼び出す際に使用
    private NekketsuJump NJump; //NekketsuJumpを呼び出す際に使用
    private NekketsuInput NInput; //NekketsuInputを呼び出す際に使用
    private NekketsuHurtBox NHurtBox; //NekketsuHurtBoxを呼び出す際に使用
    private NekketsuStateChange NStateChange;
    //private NekketsuSound NSound;

    // *****共通変数*****
    public float speed = 0.08f;                 // スピード
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

    public bool leftFlag = false; // 左向きかどうか
    public bool jumpFlag = false; // ジャンプして空中にいるか
    public bool dashFlag = false;   //走っているか否か
    public bool squatFlag = false;  //しゃがみ状態フラグ
    public bool brakeFlag = false;  //ブレーキフラグ
    public bool AttackMomentFlag = false;  //攻撃し始めフラグ(空中攻撃出し始め判定)

    public JumpButtonPushState JumpButtonState; //ジャンプボタン押下ステータス
    public XInputState XInputState = 0; //疑似Xに対する入力ステータス
    public ZInputState ZInputState = 0; //疑似Zに対する入力ステータス
    public AttackPattern NowAttack = 0; // 現在の攻撃パターン格納変数
    public DamagePattern NowDamage = 0; // 現在の攻撃喰らいパターン格納変数

    // *****共通変数*****

    #endregion

    void Start()
    {
        // 最初に行う
        pos = transform.position;
        animator = this.GetComponent<Animator>();

        gameObjct = GameObject.Find("Umaibou");
        NSound = gameObjct.GetComponent<NekketsuSound>();
        NAttack = gameObjct.GetComponent<NekketsuAttack>();

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
        NAttack.AttackMain();

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
        pos.x = X;
        pos.y = Z + Y;

        transform.position = pos;

        // 喰らい判定の移動
        hurtBox = new Rect(X, Y, 0.7f, 1.6f);

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
        var shadeTransform = GameObject.Find("shade").transform;
        pos.y = Z - 0.8f;

        if (!squatFlag)
        {
            shadeTransform.position = pos;
        }
        #endregion

        #region アニメ処理(旧)

        //if (NowDamage == DamagePattern.groggy)
        //{
        //    if ((XInputState != XInputState.XNone
        //        && XInputState != XInputState.XRightReleaseButton
        //        && XInputState != XInputState.XLeftReleaseButton)
        //            || (ZInputState != ZInputState.ZNone
        //            && ZInputState != ZInputState.ZBackReleaseButton
        //            && ZInputState != ZInputState.ZFrontReleaseButton)
        //                || dashFlag)
        //    {
        //        //左右キー押していないor左右キー離した瞬間ではない
        //        animator.Play("UmaHogeWalk");
        //    }
        //    else
        //    {
        //        animator.Play("UmaHoge");
        //    }
        //}
        //else
        //{
        //    if (!squatFlag && !brakeFlag)
        //    {

        //        if (jumpFlag)
        //        {
        //            if (NowAttack == AttackPattern.JumpKick)
        //            {
        //                animator.Play("JumpKick");
        //            }
        //            else if(NowAttack == AttackPattern.DosukoiSide)
        //            {
        //                animator.Play("Dosukoi");
        //            }
        //            else
        //            {
        //                animator.Play("Jump");
        //            }
        //        }
        //        else
        //        {
        //            if ((XInputState != XInputState.XNone
        //                && XInputState != XInputState.XRightReleaseButton
        //                && XInputState != XInputState.XLeftReleaseButton)
        //                    || (ZInputState != ZInputState.ZNone
        //                    && ZInputState != ZInputState.ZBackReleaseButton
        //                    && ZInputState != ZInputState.ZFrontReleaseButton)
        //                        || dashFlag)
        //            {
        //                if ((Input.GetKey("z") || Input.GetKey("joystick button 0")))
        //                {
        //                    if ((ZInputState == ZInputState.ZBackPushMoment
        //                        || ZInputState == ZInputState.ZBackPushButton)
        //                        && leftFlag)
        //                    {
        //                        animator.Play("DosukoiBack");
        //                    }
        //                    else if ((ZInputState == ZInputState.ZFrontPushMoment
        //                             || ZInputState == ZInputState.ZFrontPushButton)
        //                             && leftFlag)
        //                    {
        //                        animator.Play("DosukoiFront");
        //                    }
        //                    else if (XInputState == XInputState.XLeftPushMoment
        //                             || XInputState == XInputState.XLeftPushButton
        //                             || leftFlag)
        //                    {
        //                        animator.Play("DosukoiWalk");
        //                    }
        //                    else
        //                    {
        //                        animator.Play("HjiWalk");
        //                    }
        //                }
        //                else if ((Input.GetKey("x") || Input.GetKey("joystick button 1")))
        //                {
        //                    if ((ZInputState == ZInputState.ZBackPushMoment
        //                        || ZInputState == ZInputState.ZBackPushButton)
        //                        && !leftFlag)

        //                    {
        //                        animator.Play("DosukoiBack");
        //                    }
        //                    else if ((ZInputState == ZInputState.ZFrontPushMoment
        //                             || ZInputState == ZInputState.ZFrontPushButton)
        //                             && !leftFlag)
        //                    {
        //                        animator.Play("DosukoiFront");
        //                    }
        //                    else if (XInputState == XInputState.XRightPushMoment
        //                             || XInputState == XInputState.XRightPushButton
        //                             || !leftFlag)
        //                    {
        //                        animator.Play("DosukoiWalk");
        //                    }
        //                    else
        //                    {
        //                        animator.Play("HjiWalk");
        //                    }
        //                }
        //                else if ((Input.GetKey("s") || Input.GetKey("joystick button 3")))
        //                {
        //                    animator.Play("Throw");
        //                }
        //                else
        //                {
        //                    //左右キー押していないor左右キー離した瞬間ではない
        //                    animator.Play("Walk");
        //                }
        //            }
        //            else if ((Input.GetKey("z") || Input.GetKey("joystick button 0")))
        //            {
        //                if (leftFlag)
        //                {
        //                    animator.Play("Dosukoi");
        //                }
        //                else
        //                {
        //                    animator.Play("Hiji");
        //                }

        //            }
        //            else if ((Input.GetKey("x") || Input.GetKey("joystick button 1")))
        //            {
        //                if (leftFlag)
        //                {
        //                    animator.Play("Hiji");
        //                }
        //                else
        //                {
        //                    animator.Play("Dosukoi");
        //                }
        //            }
        //            else if ((Input.GetKey("s") || Input.GetKey("joystick button 3")))
        //            {
        //                animator.Play("Throw");
        //            }
        //            else
        //            {
        //                animator.Play("Standing");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (brakeFlag)
        //        {
        //            animator.Play("Brake");
        //        }

        //        if (squatFlag)
        //        {
        //            animator.Play("Squat");
        //        }
        //    }
        //}

        #endregion

        #region アニメ処理(仮修正)

        if (NowDamage == DamagePattern.groggy)
        {
            if (vx == 0 && vz == 0)
            {
                //左右キー押していないor左右キー離した瞬間ではない
                animator.Play("UmaHoge");
            }
            else
            {
                animator.Play("UmaHogeWalk");
            }
        }
        else
        {
            if (!squatFlag && !brakeFlag)
            {
                switch (NowAttack)
                {
                    case AttackPattern.DosukoiBack:
                        animator.Play("DosukoiBack");
                        break;

                    case AttackPattern.DosukoiFront:
                        animator.Play("DosukoiFront");
                        break;

                    case AttackPattern.DosukoiSide:

                        if (Y == 0)
                        {
                            if (vx == 0 && vz == 0)
                            {
                                animator.Play("Dosukoi");
                            }
                            else
                            {
                                animator.Play("DosukoiWalk");
                            }
                        }

                        break;

                    case AttackPattern.Hiji:

                        if (vx == 0 && vz == 0)
                        {
                            animator.Play("Hiji");
                        }
                        else
                        {
                            animator.Play("HjiWalk");
                        }

                        break;

                    case AttackPattern.JumpKick:
                        if (0 < Y)
                        {
                            animator.Play("JumpKick");
                        }
                        else
                        {
                            NowAttack = AttackPattern.None;
                        }

                        break;

                    case AttackPattern.JumpDosukoiSide:
                        if (0 < Y)
                        {
                            animator.Play("UmaHariteJump");
                        }
                        else
                        {
                            NowAttack = AttackPattern.None;
                        }
                        break;


                    case AttackPattern.None:

                        if (vx == 0 && vz == 0)
                        {
                            //animator.Play("Stanging");
                            animator.SetBool("Walk", false);

                        }
                        else
                        {
                            //animator.Play("Walk");
                            animator.SetBool("Walk", true);
                        }

                        if (jumpFlag)
                        {
                            //animator.SetBool("Jump", true);
                            animator.Play("Jump");
                        }
                        else
                        {
                            //animator.SetBool("Jump", false);
                        }
                        break;
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