using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ずっと、カメラが追いかける（水平に）
public class Forever_ChaseCameraH : MonoBehaviour {

	Vector3 base_pos;
    Vector3 pos;

    GameObject playerObjct;
    NekketsuManager Nmng;
    float TopPlayerX;

    void Start() { // 最初に行う
		// カメラの元の位置を覚えておく
		base_pos = Camera.main.gameObject.transform.position;

        playerObjct = GameObject.Find("NekketsuManager");
        Nmng = playerObjct.GetComponent<NekketsuManager>();
    }

    void LateUpdate() { // ずっと行う（いろいろな処理の最後に）

        //何プレイヤーが先頭を走っているか取得
        int topPlayerNum = TopPlayerNumber(Nmng);

        //カメラは先頭を走るプレイヤーを追いかける
        TopPlayerX = Nmng.Player[topPlayerNum].NVariable.X;
        pos.x = Nmng.Player[topPlayerNum].NVariable.X + Settings.Instance.Game.TopPlayerCameraX;

        pos.z = -10; // カメラ位置なのでプレイヤーよりやや手前に移動させる
		pos.y = base_pos.y; // カメラの元の高さを使う
		Camera.main.gameObject.transform.position = pos;
	}

    /// <summary>
    /// 先頭を走るプレイヤーのプレイヤー番号(1～4)を返す
    /// </summary>
    /// <param name="Nmng"></param>
    /// <returns>TopPlayerNum</returns>
    int TopPlayerNumber(NekketsuManager Nmng)
    {
        //★TODO★
        //LINQ使えると思うので再度ためす
        //インデックスを返したい

        float[] playersX;
        int TopPlayerNum = 0;

        playersX = new float[2];

        playersX[0] = Nmng.Player[0].NVariable.X;
        playersX[1] = Nmng.Player[1].NVariable.X;
        //playersX[2] = Nmng.Player[2].NVariable.X;
        //playersX[3] = Nmng.Player[3].NVariable.X;

        if (playersX[1] < playersX[0]
            && (Nmng.Player[0].NVariable.DeathFlag != DeathPattern.death
                || Nmng.Player[1].NVariable.DeathFlag == DeathPattern.death))
        {
            TopPlayerNum = 0;
        }
        else if (playersX[0] < playersX[1]
                && (Nmng.Player[1].NVariable.DeathFlag != DeathPattern.death
                    || Nmng.Player[0].NVariable.DeathFlag == DeathPattern.death))
        {
            TopPlayerNum = 1;
        }

        return TopPlayerNum;
    }

}