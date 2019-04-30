using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キーを押すと、移動する（熱血風対応版）
public class NekketsuAction : MonoBehaviour
{
    #region 変数定義
    Vector3 pos;        // 最終的な描画で使用
    Animator animator;  // アニメ変更用

    public float speed = 0.08f;             // スピード
    public float jumppower = 0.05f;        // ジャンプ力
    public float Gravity = -0.005f;         // 内部での重力
    public float InitalVelocity = 0.15f;     // 内部での初速
    public float nextButtonDownTimeDash = 1f;   // ダッシュを受け付ける時間

    float X = 0;    //内部での横
    float Y = 0;    //内部での高さ
    float Z = 0;    //内部での奥行き
    float vx = 0;   //内部X値用変数
    float vy = 0;   //内部Y値用変数
    float vz = 0;   //内部Z値用変数
    float gravity;  //内部での重力

    bool leftFlag = false; // 左向きかどうか
    bool pushJump = false; // ジャンプキーを押しっぱなしかどうか
    bool jumpFlag = false; // ジャンプして空中にいるか
    bool miniJumpFlag = false; // 小ジャンプ

    bool jumpAccelerate = false;    //ジャンプ加速度の計算を行うフラグ

    bool dashFlag = false;   //走っているか否か
    bool pushMove = false;   //走る事前準備として、左右移動ボタンが既に押されているか否か
    bool leftDash = false;   //走ろうとする方向(左を正とする)
    bool canDash = false;    //走ることが出来る状態か
    float nowTimeDash = 0f;      //最初に移動ボタンが押されてからの経過時間

    #endregion

    void Start()
    {   
        // 最初に行う
        gravity = Gravity;
        pos = transform.position;

        animator = this.GetComponent<Animator>();
    }

    void Update()
    { // ずっと行う

        vx = 0;
        vz = 0;

        #region 十字キー移動
        // もし、右キーが押されたら
        if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
        {
            vx = speed; // 右に進む移動量を入れる
            leftFlag = false;

            if (!dashFlag)
            {
                X += vx;
            }
        }
        // もし、左キーが押されたら
        else if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)   //else if でも同時押し対策NG
        {
            vx = -speed; // 左に進む移動量を入れる
            leftFlag = true;

            if (!dashFlag)
            {
                X += vx;
            }
        }

        // もし、上キーが押されたら
        if (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
        {
            vz = speed * 0.5f; // 上に進む移動量を入れる(熱血っぽく奥行きは移動量小)

            Z += vz;
        }
        if (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
        { // もし、下キーが押されたら
            vz = speed * 0.5f; // 下に進む移動量を入れる(熱血っぽく奥行きは移動量小)

            Z += -vz;
        }


        if (!dashFlag)
        {
            // 非ダッシュ状態で、横移動中か？
            if ((Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                || (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0))
            {
                if (!pushMove)
                {
                    //ダッシュの準備をする
                    pushMove = true;
                    leftDash = leftFlag;
                    nowTimeDash = 0;
                }
                else
                {
                    // ダッシュ準備済なので、ダッシュしてよい状態か判断
                    if (canDash && !jumpFlag
                        && leftDash == leftFlag
                        && nowTimeDash <= nextButtonDownTimeDash)
                    {
                        dashFlag = true;
                    }
                }
            }
            else
            {
                // 非ダッシュ状態で、ダッシュ準備済か？
                // 1度左右キーが押された状態で、ダッシュ受付時間内にもう一度左右キーが押された時
                if (pushMove)
                {
                    //　時間計測
                    nowTimeDash += Time.deltaTime;

                    if (nowTimeDash > nextButtonDownTimeDash)
                    {
                        pushMove = false;
                        canDash = false;
                    }
                    else
                    {
                        canDash = true;
                    }
                }
            }
        }
        else
        {   //ダッシュ済の場合

            // ダッシュ中に逆方向を押した場合
            if(leftDash != leftFlag)
            {
                dashFlag = false;
                pushMove = false;
                canDash = false;
            }
            else
            {
                // ダッシュ中の加速を計算する。
                // ダッシュ中は方向キー入力なしで自動で進む。(クロカン・障害ふう)
                if (leftFlag)
                {
                    vx = -speed; // 左に進む移動量を入れる
                    X += vx * 1.4f;
                }
                else
                {
                    vx = speed; // 右に進む移動量を入れる
                    X += vx * 1.4f;
                }
            }
        }

        // ダッシュ入力受付中
        if (pushMove)
        {
            //　時間計測
            nowTimeDash += Time.deltaTime;

            if (nowTimeDash > nextButtonDownTimeDash)
            {
                pushMove = false;
                canDash = false;
            }
        }


        #endregion  移動

        #region ジャンプ処理
        // もし、ジャンプキーが押されたとき
        if (Input.GetKey("a") || Input.GetKey("joystick button 2"))
        {
            // 着地済みかつ、ジャンプキー押しっぱなしでなければ
            if (Y <= 0 && pushJump == false)
            { 
                jumpFlag = true; // ジャンプの準備
                pushJump = true; // 押しっぱなし状態
                miniJumpFlag = false; // 小ジャンプ
                // ジャンプした瞬間に初速を追加
                vy += InitalVelocity;

                // ジャンプ加速度の計算を行う
                jumpAccelerate = true;
            }
        }
        else
        {
            pushJump = false; // 押しっぱなし解除
        }

        // ジャンプ状態
        if (jumpFlag)
        {
            // ジャンプボタンが押されてない＆上昇中＆小ジャンプフラグが立ってない
            if (!pushJump && vy > 0 && !miniJumpFlag)
            {
                // vy = vy * 0.95f;
                vy = vy * 0.5f;

                // 小ジャンプ
                miniJumpFlag = true;
            }
            //else
            //{
            //    vy += jumppower;
            //}

            // Y += gravity; // ジャンプ中の重力加算
            // gravity += Gravity; // ジャンプ中にかかる重力を増加させる

            // 重力は変化せず常に同じ値が掛かる
            vy += Gravity;

            //vy += jumppower; 

            Y += vy; // ジャンプ力を加算

            // 着地判定
            if (Y <= 0)
            {
                jumpFlag = false;
                pushJump = false;
                dashFlag = false; //ダッシュ中であれば、着地時にダッシュ解除

                // 地面めりこみ補正は着地したタイミングで行う
                if (Y < 0)
                {
                    Y = 0; // マイナス値は入れないようにする
                }

                if (InitalVelocity != 0)
                {
                    // 重力変数・内部Y軸変数を初期値に戻す
                    gravity = Gravity;
                    vy = 0;
                }
            }
        }

        // ジャンプ加速度の計算
        if (jumpFlag && jumpAccelerate)
        {
            // ジャンプ時、横移動の初速を考慮
            if (jumpFlag &&
                (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                || (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0))
            {
                if (leftFlag)
                {
                    X += -(InitalVelocity * speed);
                }
                else
                {
                    X += InitalVelocity * speed;
                }
            }
            else
            {
                jumpAccelerate = false;
            }
        }
        #endregion

        #region 画面への描画
        // 入力された内部XYZをtransformに設定する。

        // 基本的に、描画位置はジャンプなどのキャラ状態かかわらず、同じように内部座標を描画座標に適用する
        // （適用できるように、必要ならば内部座標の段階で調整をしておく）
        pos.x = X;
        pos.y = Z + Y;

        // if (jumpFlag)
        // {
        //     //ジャンプ中の場合は内部Yを加える。
        //     pos.y = Z + Y;
        // }
        // else
        // {
        //     pos.y = Z;
        // }

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

        shadeTransform.position = pos;
        #endregion

        #region アニメ処理(ここでやるかは仮)
        if (!jumpFlag)
        {
            if ((Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                || (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)
                || (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
                || (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
                || dashFlag)
            {
                animator.Play("UmaGr");
            }
            else if ((Input.GetKey("z") || Input.GetKey("joystick button 0")))
            {
                animator.Play("UmaHiji");
            }
            else if ((Input.GetKey("x") || Input.GetKey("joystick button 1")))
            {
                animator.Play("UmaHarite");
            }
            else if ((Input.GetKey("s") || Input.GetKey("joystick button 3")))
            {
                animator.Play("UmaThrow");
            }
            else
            {
                animator.Play("UmaGrTACHI");
            }
        }
        else
        {
            animator.Play("UmaJump");
        }

        #endregion

    }

}