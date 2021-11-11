using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public enum NAME
    {
        APPLE
    }

    public virtual NAME Name
    {
        get;
    }

    public Vector3 Position
    {
        get; set;
    }
}
