using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuMove
{
    // GameObject GObj; //ゲームオブジェクトそのものが入る変数
    NekketsuAction NAct; //NekketsuActionが入る変数

    bool pushMove = false;   //ダッシュする事前準備として、左右移動ボタンが既に押されているか否か
    bool leftDash = false;   //ダッシュする方向(左を正とする)
    bool canDash = false;    //ダッシュが出来る状態か
    float nowTimeDash = 0f;  //最初に移動ボタンが押されてからの経過時間
    float nowTimebrake = 0f; //ブレーキ時間を計測

    public NekketsuMove(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public void MoveMain()
    {
        if (!NAct.squatFlag)
        {
            #region 歩き

            // もし、右キーが押されたら
            if (Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
            {
                NAct.vx = NAct.speed; // 右に進む移動量を入れる
                NAct.leftFlag = false;

                if (!NAct.dashFlag && !NAct.jumpFlag)
                {
                    NAct.X += NAct.vx;
                }
            }
            // もし、左キーが押されたら
            else if (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0)   //else if でも同時押し対策NG
            {
                NAct.vx = -NAct.speed; // 左に進む移動量を入れる
                NAct.leftFlag = true;

                if (!NAct.dashFlag && !NAct.jumpFlag)
                {
                    NAct.X += NAct.vx;
                }
            }

            // もし、上キーが押されたら
            if (Input.GetKey("up") || Input.GetAxis("Vertical") > 0)
            {
                NAct.vz = NAct.speed * 0.5f; // 上に進む移動量を入れる(熱血っぽく奥行きは移動量小)

                if (!NAct.jumpFlag)
                {
                    NAct.Z += NAct.vz;
                }
            }
            if (Input.GetKey("down") || Input.GetAxis("Vertical") < 0)
            { // もし、下キーが押されたら
                NAct.vz = -NAct.speed * 0.5f; // 下に進む移動量を入れる(熱血っぽく奥行きは移動量小)

                if (!NAct.jumpFlag)
                {
                    NAct.Z += NAct.vz;
                }
            }

            #endregion

            #region ダッシュ

            if (!NAct.dashFlag)
            {
                // 非ダッシュ状態で、横移動中か？
                if ((Input.GetKey("right") || Input.GetAxis("Horizontal") > 0)
                    || (Input.GetKey("left") || Input.GetAxis("Horizontal") < 0))
                {
                    if (!pushMove)
                    {
                        //ダッシュの準備をする
                        pushMove = true;
                        leftDash = NAct.leftFlag;
                        nowTimeDash = 0;
                    }
                    else
                    {
                        // ダッシュ準備済なので、ダッシュしてよい状態か判断
                        if (canDash && !NAct.jumpFlag
                            && leftDash == NAct.leftFlag
                            && nowTimeDash <= NAct.nextButtonDownTimeDash)
                        {
                            NAct.dashFlag = true;
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

                        if (nowTimeDash > NAct.nextButtonDownTimeDash)
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
                if (leftDash != NAct.leftFlag)
                {
                    if (leftDash)
                    {
                        NAct.vx = -NAct.speed; // 左に進む移動量を入れる
                        NAct.X += NAct.vx * 1.35f;
                    }
                    else
                    {
                        NAct.vx = NAct.speed; // 右に進む移動量を入れる
                        NAct.X += NAct.vx * 1.35f;
                    }

                    NAct.dashFlag = false;
                    pushMove = false;
                    canDash = false;

                    // ブレーキ状態
                    if (!NAct.jumpFlag)
                    {
                        NAct.brakeFlag = true;
                    }
                }
                else
                {
                    // ダッシュ中の加速を計算する。
                    // ダッシュ中は方向キー入力なしで自動で進む。(クロカン・障害ふう)
                    if (NAct.leftFlag)
                    {
                        NAct.vx = -NAct.speed; // 左に進む移動量を入れる
                        NAct.X += NAct.vx * 1.35f;
                    }
                    else
                    {
                        NAct.vx = NAct.speed; // 右に進む移動量を入れる
                        NAct.X += NAct.vx * 1.35f;
                    }
                }
            }

            // ブレーキ処理
            if (!NAct.jumpFlag && NAct.brakeFlag)
            {
                if (NAct.leftFlag)
                {
                    NAct.vx = NAct.speed; // 右に進む移動量を入れる
                    NAct.X += NAct.vx * 1.25f;
                }
                else
                {
                    NAct.vx = -NAct.speed; // 左に進む移動量を入れる
                    NAct.X += NAct.vx * 1.25f;
                }
                // ブレーキ状態の時間計測
                nowTimebrake += Time.deltaTime;

                // ブレーキ状態解除
                if (nowTimebrake > 0.25f)
                {
                    NAct.brakeFlag = false;
                    nowTimebrake = 0;
                }
            }

            // ダッシュ入力受付中
            if (pushMove)
            {
                //　時間計測
                nowTimeDash += Time.deltaTime;

                if (nowTimeDash > NAct.nextButtonDownTimeDash)
                {
                    pushMove = false;
                    canDash = false;
                }
            }

            #endregion
        }
    }
}
