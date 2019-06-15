using UnityEngine;

/// <summary>
/// 各オブジェクト(プレイヤー・アイテム)等が
/// それぞれを参照できるよう一元管理するクラス
/// </summary>
public class NekketsuManager : MonoBehaviour
{
    private GameObject playerObjct;
    public NekketsuAction Player1;
    public NekketsuAction Player2;
    public NekketsuAction Player3;
    public NekketsuAction Player4;

    private GameObject ItemObjct;
    public item Item1;

    //　★デバッグ用★
    public UmaiboSandbag sandbag;
    public DamageTest uni;
    public DamageTest uni2;
    //　★デバッグ用★

    public NekketsuAction NAct; //NekketsuActionが入る変数
    public UmaiboSandbag UmaSnd; //NekketsuActionが入る変数

    void Start()    
    {
        // 各オブジェクトの変数を参照できるようにする。
        playerObjct = GameObject.Find("Player1");
        Player1 = playerObjct.GetComponent<NekketsuAction>();

        playerObjct = GameObject.Find("Player2");
        Player2 = playerObjct.GetComponent<NekketsuAction>();

        //playerObjct = GameObject.Find("Player3");
        //Player3 = playerObjct.GetComponent<NekketsuAction>();

        //playerObjct = GameObject.Find("Player4");
        //Player4 = playerObjct.GetComponent<NekketsuAction>();


        ItemObjct = GameObject.Find("bokutou");
        Item1 = ItemObjct.GetComponent<item>();


        //　★デバッグ用★
        playerObjct = GameObject.Find("UmaiboSandbag");
        sandbag = playerObjct.GetComponent<UmaiboSandbag>();

        playerObjct = GameObject.Find("uni");
        uni = playerObjct.GetComponent<DamageTest>();
        //　★デバッグ用★
    }

    void Update()
    {

    }
}
