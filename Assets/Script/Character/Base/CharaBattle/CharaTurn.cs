using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaTurn : MonoBehaviour
{
    private bool m_IsFinishTurn;
    public bool IsFinishTurn
    {
        get { return m_IsFinishTurn; }
        set { m_IsFinishTurn = value; }
    }
}
