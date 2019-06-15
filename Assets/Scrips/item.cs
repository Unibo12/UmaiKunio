using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public float X = 0;    //内部での横
    public float Y = 0;    //内部での高さ
    public float Z = 0;    //内部での奥行き
    public float vx = 0;   //内部X値用変数
    public float vy = 0;   //内部Y値用変数
    public float vz = 0;   //内部Z値用変数
    public ItemPattern itemType;

    public Rect itemBox = new Rect(0, 0, 0, 0);
    public Rect hitBox = new Rect(0, 0, 0, 0);

    GameObject playerObjct;
    NekketsuManager Nmng;

    // Start is called before the first frame update
    void Start()
    {
        playerObjct = GameObject.Find("NekketsuManager");
        Nmng = playerObjct.GetComponent<NekketsuManager>();

        switch (itemType)
        {
            case ItemPattern.bokutou:
                itemBox = new Rect(Settings.Instance.Item.bokutouRect);
                itemBox.x = X;
                itemBox.y = Y+Z;
                break;

            default:
                itemBox = new Rect(0, 0, 0, 0);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Nmng.Player1.hurtBox.Overlaps(Nmng.Item1.itemBox)
            && Nmng.Player1.NowAttack != AttackPattern.None)
        {
            // お試しで アイテム上で攻撃ボタン押下で消えるテスト
            Destroy(this.gameObject);
        }
    }

    void OnDrawGizmos()
    {
        // アイテム拾い判定のギズモを表示
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(itemBox.position, new Vector3(itemBox.width, itemBox.height, 0));
    }

}
