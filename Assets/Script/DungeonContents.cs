using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonContents : SingletonMonoBehaviour<DungeonContents>
{
    [SerializeField] private GameObject m_Stairs;
    [SerializeField] private GameObject m_PlayerObject;

    public void DeployDungeonContents()
    {
        DeployStairs();
        DeployPlayer();
    }

    // 4 -> 階段
    private void DeployStairs()
    {
        int[,] map = DungeonTerrain.Instance.Map;
        int[] coord = ChooseRandamRoomGrid(map);
        DungeonTerrain.Instance.SetValueInList(4, coord[0], coord[1]);
        GameObject @object = Instantiate(m_Stairs, new Vector3(coord[0], 0, coord[1]), Quaternion.identity);
        DungeonTerrain.Instance.SetObjectInListInstead(@object, coord[0], coord[1]);
    }

    private void DeployPlayer()
    {
        int[,] map = DungeonTerrain.Instance.Map;
        int[] coord = ChooseRandamRoomGrid(map);
        Instantiate(m_PlayerObject, new Vector3(coord[0], 1, coord[1]), Quaternion.identity);
    }

    private int[] ChooseRandamRoomGrid(int[,] map)
    {
        int num = -1;
        int x = -1;
        int z = -1;
        while (num != 2)
        {
            x = Random.Range(0, map.GetLength(0));
            z = Random.Range(0, map.GetLength(1));
            num = map[x, z];
        }
        int[] coord = { x, z };
        return coord;
    }
}