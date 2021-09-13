using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonContents : SingletonMonoBehaviour<DungeonContents>
{
    [SerializeField] private GameObject m_Stairs;
    [SerializeField] private GameObject m_PlayerObject;

    private int m_InitX;
    private int m_InitZ;

    public void DeployDungeonContents()
    {
        DeployStairs();
        DeployPlayer();
    }

    private void DeployStairs()
    {
        int[,] map = DungeonTerrain.Instance.Map;
        int stairsNum = 1;
        while (stairsNum != 2)
        {
            m_InitX = Random.Range(0, map.GetLength(0));
            m_InitZ = Random.Range(0, map.GetLength(1));
            stairsNum = map[m_InitX, m_InitZ];
        }
        DungeonTerrain.Instance.SetValueInList(4, m_InitX, m_InitZ);
        GameObject @object = Instantiate(m_Stairs, new Vector3(m_InitX, 0, m_InitZ), Quaternion.identity);
        DungeonTerrain.Instance.SetObjectInListInstead(@object, m_InitX, m_InitZ);
    }

    private void DeployPlayer()
    {
        int[,] map = DungeonTerrain.Instance.Map;
        int playerNum = 1;
        while (playerNum != 2)
        {
            m_InitX = Random.Range(0, map.GetLength(0));
            m_InitZ = Random.Range(0, map.GetLength(1));
            playerNum = map[m_InitX, m_InitZ];
        }
        Instantiate(m_PlayerObject, new Vector3(m_InitX, 1, m_InitZ), Quaternion.identity);
    }
    
    
}

