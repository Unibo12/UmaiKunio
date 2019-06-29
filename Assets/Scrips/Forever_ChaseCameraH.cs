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

        // カメラは先頭を走るプレイヤーを追いかける
        if (Nmng.Player[1].X < Nmng.Player[0].X)
        {
            TopPlayerX = Nmng.Player[0].X;
            pos.x = Nmng.Player[0].X + Settings.Instance.Game.TopPlayerCameraX;
        }
        else
        {
            TopPlayerX = Nmng.Player[1].X;
            pos.x = Nmng.Player[1].X + Settings.Instance.Game.TopPlayerCameraX;
        }

        pos.z = -10; // カメラなので手前に移動させる
		pos.y = base_pos.y; // カメラの元の高さを使う
		Camera.main.gameObject.transform.position = pos;
	}
}