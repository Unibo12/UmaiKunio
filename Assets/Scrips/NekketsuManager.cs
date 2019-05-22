using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各オブジェクト(プレイヤー・アイテム)等が
/// それぞれを参照できるよう一元管理するクラス
/// </summary>
public class NekketsuManager : MonoBehaviour
{
    private GameObject playerObjct;
    public NekketsuAction Umaibou;
    public UmaiboSandbag sandbag;
    public DamageTest uni;
    public DamageTest uni2;

    public NekketsuAction NAct; //NekketsuActionが入る変数
    public UmaiboSandbag UmaSnd; //NekketsuActionが入る変数

    // Start is called before the first frame update
    void Start()
    {
        playerObjct = GameObject.Find("Umaibou");
        Umaibou = playerObjct.GetComponent<NekketsuAction>();

        playerObjct = GameObject.Find("UmaiboSandbag");
        sandbag = playerObjct.GetComponent<UmaiboSandbag>();

        playerObjct = GameObject.Find("uni");
        uni = playerObjct.GetComponent<DamageTest>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
