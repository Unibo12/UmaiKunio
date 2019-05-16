using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekketsuAttack : MonoBehaviour
{
    GameObject playerObjct;
    NekketsuManager Nmng;

    // Start is called before the first frame update
    void Start()
    {
        playerObjct = GameObject.Find("NekketsuManager");
        Nmng = playerObjct.GetComponent<NekketsuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Nmng.Umaibou.hitBox = new Rect(hitBoxX, hitBoxY, 0, 0);

    }

    void None()
    {
        float hitBoxX = Nmng.Umaibou.X;
        float hitBoxY = Nmng.Umaibou.Y;

        Nmng.Umaibou.hitBox = new Rect(hitBoxX, hitBoxY, 0, 0);

    }

    void DosukoiSide(float timing)
    {
        float hitBoxX = Nmng.Umaibou.X;
        float hitBoxY = Nmng.Umaibou.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (Nmng.Umaibou.leftFlag) ? -1 : 1;

        switch (timing)
        {
            case 1:
                Nmng.Umaibou.hitBox = new Rect(hitBoxX, hitBoxY, 0, 0);
                break;

            case 2:
                Nmng.Umaibou.hitBox
                    = new Rect(hitBoxX + (0.6f * leftMinusVector),
                               hitBoxY + 0.2f,
                               0.6f, 0.5f);
                break;

            default:
                break;
        }

    }

    void Hiji(float timing)
    {
        float hitBoxX = Nmng.Umaibou.X;
        float hitBoxY = Nmng.Umaibou.Y;
        // 右方向の場合はマイナス値とする。
        float RightMinusVector = (Nmng.Umaibou.leftFlag) ? 1 : -1;

        switch (timing)
        {
            case 1:
                Nmng.Umaibou.hitBox =
                    new Rect(hitBoxX + (0.4f * RightMinusVector),
                             hitBoxY,
                             0.4f, 0.4f);
                break;

            case 2:
                Nmng.Umaibou.hitBox =
                    new Rect(hitBoxX + (0.4f * RightMinusVector),
                             hitBoxY,
                             0.4f, 0.4f);
                break;

            default:
                break;
        }

        if (Nmng.Umaibou.leftFlag)
        {
            Nmng.Umaibou.hitBox = new Rect(hitBoxX + 0.5f, hitBoxY, 0.4f, 0.5f);
        }
        else
        {
            Nmng.Umaibou.hitBox = new Rect(hitBoxX - 0.5f, hitBoxY, 0.4f, 0.5f);
        }
    }

    void JumpKick(float timing)
    {
        float hitBoxX = Nmng.Umaibou.X;
        float hitBoxY = Nmng.Umaibou.Y;
        // 左方向の場合はマイナス値とする。
        float leftMinusVector = (Nmng.Umaibou.leftFlag) ? -1 : 1;


        switch (timing)
        {
            case 1:
                Nmng.Umaibou.hitBox =
                    new Rect(hitBoxX + ( 0.2f * leftMinusVector),
                             hitBoxY - 0.65f,
                             0.8f, 0.4f);
                break;

            default:
                break;
        }
    }

    void OnDrawGizmos()
    {
        // 喰らい判定のギズモを表示
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(Nmng.Umaibou.hurtBox.width, Nmng.Umaibou.hurtBox.height, 0));

        // 攻撃判定のギズモを表示
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(Nmng.Umaibou.hitBox.x, Nmng.Umaibou.Z + Nmng.Umaibou.hitBox.y), new Vector3(Nmng.Umaibou.hitBox.width, Nmng.Umaibou.hitBox.height, 0));

    }
}
