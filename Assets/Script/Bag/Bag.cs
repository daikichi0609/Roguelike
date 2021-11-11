using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag 
{
    public List<Item> ItemList
    {
        get;
    } = new List<Item>();

    public int Maximum
    {
        get; set;
    } = 20;
}
