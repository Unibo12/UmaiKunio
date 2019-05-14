using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuManager : MonoBehaviour
{
    private GameObject playerObjct;
    public NekketsuAction Umaibou;
    public UmaiboSandbag sandbag;

    public NekketsuAction NAct; //NekketsuActionが入る変数
    public UmaiboSandbag UmaSnd; //NekketsuActionが入る変数

    //public NekketsuManager(NekketsuAction nekketsuAction)
    //{
    //    NAct = nekketsuAction;
    //}

    //public NekketsuManager(UmaiboSandbag umaiboSandbag)
    //{
    //    playerObjct = GameObject.Find("UmaiboSandbag");
    //    UmaSnd = umaiboSandbag;
    //}

    // Start is called before the first frame update
    void Start()
    {
        playerObjct = GameObject.Find("Umaibou");
        Umaibou = playerObjct.GetComponent<NekketsuAction>();

        playerObjct = GameObject.Find("UmaiboSandbag");
        sandbag = playerObjct.GetComponent<UmaiboSandbag>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
