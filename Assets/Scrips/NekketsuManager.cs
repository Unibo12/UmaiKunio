using UnityEngine;

/// <summary>
/// 各オブジェクト(プレイヤー・アイテム)等が
/// それぞれを参照できるよう一元管理するクラス
/// </summary>
public class NekketsuManager : MonoBehaviour
{
    private GameObject playerObjct;

    /// @@@PlayerXのように連番になるようなものはリストか配列で管理するほうが良いです
    /// リストは数が可変するもの、配列は変わらない予定のものという判断で大丈夫です
    /// 配列で管理するとインデックスでキャラを管理、判断できるので
    /// 後々楽になります
    public NekketsuAction[] Player;

    //アイテム
    private GameObject ItemObjct;
    public item Item1;

    //マップ上の障害物
    private GameObject MapObjct;
    public  Object MapObjct1;

    //　★デバッグ用★
    public UmaiboSandbag sandbag;
    public DamageTest uni;
    public DamageTest uni2;
    //　★デバッグ用★

    public NekketsuAction NAct; //NekketsuActionが入る変数
    public UmaiboSandbag UmaSnd; //NekketsuActionが入る変数

    void Start()    
    {
        Player = new NekketsuAction[2];

        // 各オブジェクトの変数を参照できるようにする。
        playerObjct = GameObject.Find("Player1");
        Player[0] = playerObjct.GetComponent<NekketsuAction>();

        playerObjct = GameObject.Find("Player2");
        Player[1] = playerObjct.GetComponent<NekketsuAction>();

        //playerObjct = GameObject.Find("Player3");
        //Player3 = playerObjct.GetComponent<NekketsuAction>();

        //playerObjct = GameObject.Find("Player4");
        //Player4 = playerObjct.GetComponent<NekketsuAction>();


        ItemObjct = GameObject.Find("bokutou");
        Item1 = ItemObjct.GetComponent<item>();

        //障害物取得
        GetMapObject();

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


    void GetMapObject()
    {
        MapObjct = GameObject.Find("table");
        MapObjct1 = MapObjct.GetComponent<Object>();
    }
}
