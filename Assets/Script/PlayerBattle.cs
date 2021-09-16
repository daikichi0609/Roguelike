using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    private PlayerStatus m_Data;

    private void Start()
    {
        m_Data = new PlayerStatus();
        JsonUtility.FromJsonOverwrite(CharaDataManager.Instance.LoadTest(CharaDataManager.Instance.Datapath), m_Data);
    }

    private void Update()
    {
        
    }
}
