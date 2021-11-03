using UnityEngine;

/// <summary>
/// 所持アイテムの管理を行うクラス
/// </summary>
public class NekketsuHaveItem
{
    NekketsuAction NAct; //NekketsuActionが入る変数

    public NekketsuHaveItem(NekketsuAction nekketsuAction)
    {
        NAct = nekketsuAction;
    }

    public void NekketsuHaveItemMain()
    {
        #region 所持アイテムの位置調整
        if (NAct.NAttackV.haveItem != ItemPattern.None)
        {
            //★TODO：毎回呼ばれてしまう
            Transform ItemTransform = GameObject.Find(NAct.NAttackV.haveItem.ToString()).transform;

            Vector3 ItemScale = ItemTransform.localScale;
            Vector3 ItemPos = new Vector3(0, 0, 0);

            ItemPos.x = NAct.NVariable.X + (0.4f * GetSign(NAct.NMoveV.leftFlag));
            ItemScale.x = GetSign(NAct.NMoveV.leftFlag);

            ItemPos.y = NAct.NVariable.Y + NAct.NVariable.Z + 0.25f;
            ItemTransform.localScale = ItemScale;

            ItemTransform.position = ItemPos;
        }

        #endregion
    }

    /// <summary>
    /// 左右の向きの符号を返す(Leftはマイナス、Rightはプラス)
    /// </summary>
    /// <param name="leftFlag"></param>
    /// <returns></returns>
    int GetSign(bool leftFlag)
    {
        return leftFlag ? -1 : +1;
    }
}
