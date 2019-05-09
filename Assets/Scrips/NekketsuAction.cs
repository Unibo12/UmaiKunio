using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 熱血風アクション
public class NekketsuAction : MonoBehaviour
{
    #region 変数定義

    Vector3 pos;        // 最終的な描画で使用
    Animator animator;  // アニメ変更用
    private NekketsuMove NDash; //NekketsuDashを呼び出す際に使用
    private NekketsuJump NJump; //NekketsuJumpを呼び出す際に使用
    private NekketsuInput NInput; //NekketsuInputを呼び出す際に使用

    // *****共通変数*****
    public float speed = 0.08f;                 // スピード
    public float Gravity = -0.006f;             // 内部での重力
    public float InitalVelocity = 0.19f;        // 内部での初速
    public float nextButtonDownTimeDash = 1f;   // ダッシュを受け付ける時間

    public float X = 0;    //内部での横
    public float Y = 0;    //内部での高さ
    public float Z = 0;    //内部での奥行き
    public float vx = 0;   //内部X値用変数
    public float vy = 0;   //内部Y値用変数
    public float vz = 0;   //内部Z値用変数

    public bool leftFlag = false; // 左向きかどうか
    public bool jumpFlag = false; // ジャンプして空中にいるか
    public bool dashFlag = false;   //走っているか否か
    public bool squatFlag = false;  //しゃがみ状態フラグ
    public bool brakeFlag = false;  //ブレーキフラグ

    public NekketsuTypeDefinition.JumpButtonPushState JumpButtonState; //ジャンプボタン押下ステータス
    public NekketsuTypeDefinition.XInputState XInputState = 0; //疑似Xに対する入力ステータス
    public NekketsuTypeDefinition.ZInputState ZInputState = 0; //疑似Zに対する入力ステータス

    // *****共通変数*****

    #endregion

    void Start()
    {   
        // 最初に行う
        pos = transform.position;
        animator = this.GetComponent<Animator>();

        // 生成（コンストラクタ）の引数にNekketsuActionを渡してやる
        NDash = new NekketsuMove(this); 
        NJump = new NekketsuJump(this);
        NInput = new NekketsuInput(this);
    }

void Update()
    { // ずっと行う

        vx = 0;
        vz = 0;

        // インプット処理呼び出し
        NInput.InputMain();

        // 移動処理呼び出し
        NDash.MoveMain();

        // ジャンプ処理呼び出し
        NJump.JumpMain();

        #region 画面への描画
        // 入力された内部XYZをtransformに設定する。

        // 基本的に、描画位置はジャンプなどのキャラ状態かかわらず、同じように内部座標を描画座標に適用する
        // （適用できるように、必要ならば内部座標の段階で調整をしておく）
        pos.x = X;
        pos.y = Z + Y;

        transform.position = pos;
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

        #region アニメ処理(ここでやるかは仮)
        if (!jumpFlag && !squatFlag && !brakeFlag)
        {
            if ((Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                || (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)
                || (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
                || (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
                || dashFlag)
            {
                animator.Play("Walk");
            }
            else if ((Input.GetKey("z") || Input.GetKey("joystick button 0")))
            {
                animator.Play("Hiji");
            }
            else if ((Input.GetKey("x") || Input.GetKey("joystick button 1")))
            {
                animator.Play("Dosukoi");
            }
            else if ((Input.GetKey("s") || Input.GetKey("joystick button 3")))
            {
                animator.Play("Throw");
            }
            else
            {
                animator.Play("Standing");
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

            if (jumpFlag)
            {
                animator.Play("Jump");
            }
        }

        #endregion

    }


}