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
    public UmaiboSandbag sandbag;
    public DamageTest uni;
    public DamageTest uni2;

    public NekketsuAction NAct; //NekketsuActionが入る変数
    public UmaiboSandbag UmaSnd; //NekketsuActionが入る変数

    void Start()
    {
        playerObjct = GameObject.Find("Player1");
        Player1 = playerObjct.GetComponent<NekketsuAction>();

        playerObjct = GameObject.Find("Player2");
        Player2 = playerObjct.GetComponent<NekketsuAction>();

        //playerObjct = GameObject.Find("Player3");
        //Player3 = playerObjct.GetComponent<NekketsuAction>();

        //playerObjct = GameObject.Find("Player4");
        //Player4 = playerObjct.GetComponent<NekketsuAction>();

        playerObjct = GameObject.Find("UmaiboSandbag");
        sandbag = playerObjct.GetComponent<UmaiboSandbag>();

        playerObjct = GameObject.Find("uni");
        uni = playerObjct.GetComponent<DamageTest>();
    }

    void Update()
    {

    }
}
