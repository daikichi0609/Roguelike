using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public virtual Define.ITEM_NAME Name
    {
        get;
    }

    public Vector3 Position
    {
        get; set;
    }

    //アイテム効果
    public virtual void Execute()
    {

    }
}
