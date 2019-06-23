using UnityEngine;

public class NekketsuHaveItem : MonoBehaviour
{
    NekketsuAction NAct; //NekketsuActionが入る変数

    public NekketsuHaveItem(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public void NekketsuHaveItemMain()
    {
        #region 所持アイテムの位置調整
        if (NAct.haveItem != ItemPattern.None)
        {
            Transform ItemTransform = GameObject.Find(NAct.haveItem.ToString()).transform;
            Vector3 ItemScale = ItemTransform.localScale;
            Vector3 ItemPos = new Vector3(0, 0, 0);
            if (NAct.leftFlag)
            {
                ItemPos.x = NAct.X - 0.4f;
                ItemScale.x = -1;
            }
            else
            {
                ItemPos.x = NAct.X + 0.4f;
                ItemScale.x = 1;
            }
            ItemPos.y = NAct.Y + NAct.Z + 0.25f;
            ItemTransform.localScale = ItemScale;

            ItemTransform.position = ItemPos;
        }

        #endregion
    }
}
