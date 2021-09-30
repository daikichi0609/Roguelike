using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject in_IsOnObject;
    public GameObject IsOnObject
    {
        get { return in_IsOnObject; }
        set { in_IsOnObject = value; }
    }

    [SerializeField] private int m_RoomID;
    public int RoomID
    {
        set { m_RoomID = value; }
    }

    public enum ISON_ID
    {
        NOTHING = 0,
        PLAYER = 1,
        ENEMY = 2,
        TRAP = 3,
        STAIRS = 4,
        ITEM = 5
    }

    [SerializeField] private ISON_ID m_IsOnId = ISON_ID.NOTHING;
    public ISON_ID IsOnId
    {
        get { return m_IsOnId; }
        set { m_IsOnId = value; }
    }

    public void Initialize()
    {
        IsOnObject = null;
        IsOnId = ISON_ID.NOTHING;
    }

    public void InformAttack(int power)
    {
        if (in_IsOnObject == null)
        {
            Debug.LogError("このマスには何もないです");
            return;
        }

        CharaBattle chara = in_IsOnObject.GetComponent<CharaBattle>();
        if (chara == null)
        {
            Debug.LogError("このマスにはキャラがいません");
        }
            
        chara.Damage(power);
    }
}
