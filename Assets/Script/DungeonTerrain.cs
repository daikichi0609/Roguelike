using UnityEngine;
using System.Collections.Generic;

//https://note.com/motibe_tsukuru/n/nbe75bb690bcc

public class DungeonTerrain: SingletonMonoBehaviour<DungeonTerrain>
{
    private int[,] m_Map;
    public int[,] Map
    {
        get { return m_Map; }
    }
    [SerializeField] private List<List<GameObject>> m_TerrainList = new List<List<GameObject>>();

    public void SetValueInList(int value, int i, int j)
    {
        m_Map[i, j] = value;
    }

    public void SetObjectInListInstead(GameObject @object, int i, int j)
    {
        GameObject removeObject = m_TerrainList[i][j];
        m_TerrainList[i].RemoveAt(j);
        Destroy(removeObject);
        m_TerrainList[i].Insert(j, @object);
    }

    private int m_MapSizeX = 32;
    private int m_MapSizeZ = 32;
    private int m_MaxRoom = 8;

    [SerializeField] private GameObject m_PathWay;
    [SerializeField] private GameObject m_Wall;
    [SerializeField] private GameObject m_Room;
    [SerializeField] private GameObject m_Gate;

    /*
    0 == 壁
    1 == 通路
    2 == 部屋
    3 == 出入り口
    4 == 階段
    -1 == ダンジョン外

    ↑
    ↑
    ↑
    z
     x → → →
    */

    public void DeployDungeonTerrain()
    {
        m_Map = MapGenerator.Instance.GenerateMap(m_MapSizeX, m_MapSizeZ, m_MaxRoom);

        for (int i = 0; i < m_Map.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < m_Map.GetLength(1) - 1; j++)
            {
                int num = m_Map[i, j];
                switch (num)
                {
                    case 0:
                        GameObject @object = Instantiate(m_Wall, new Vector3(i, 0, j), Quaternion.identity);
                        m_TerrainList.Add(new List<GameObject>());
                        m_TerrainList[i].Add(@object);
                        break;

                    case 1:
                        @object = Instantiate(m_PathWay, new Vector3(i, 0, j), Quaternion.identity);
                        m_TerrainList[i].Add(@object);
                        break;

                    case 2:
                        AroundGrid aroundGrid = CheckAroundGrid(m_Map, i, j);
                        if (CheckGateWay(aroundGrid) == true)
                        {
                            m_Map[i, j] = 3;
                            @object = Instantiate(m_Gate, new Vector3(i, 0, j), Quaternion.identity);
                            m_TerrainList[i].Add(@object);
                        }
                        else
                        {
                            @object = Instantiate(m_Room, new Vector3(i, 0, j), Quaternion.identity);
                            m_TerrainList[i].Add(@object);
                        }
                        break;
                }
            }
        }


    }

    public AroundGrid CheckAroundGrid(int[,] map, int x, int z) //
    {
        return new AroundGrid(map, x, z);
    }

    private bool CheckGateWay(AroundGrid aroundGrid)
    {
        if(aroundGrid.m_UpGrid == 1)
        {
            return true;
        }
        if(aroundGrid.m_UnderGrid == 1)
        {
            return true;
        }
        if(aroundGrid.m_LeftGrid == 1)
        {
            return true;
        }
        if(aroundGrid.m_RightGrid == 1)
        {
            return true;
        }
        return false;
    }

    public int DestinationGridInfo(int[,] map, int pos_x, int pos_z, int direction_x, int direction_z)
    {
        return map[pos_x + direction_x, pos_z + direction_z];
    }
}

public struct AroundGrid
{
    public int m_UpGrid { get; } //上
    public int m_UnderGrid { get; } //下
    public int m_LeftGrid { get; } //左
    public int m_RightGrid { get; } //右

    public int m_UpperLeft { get; } //左上
    public int m_UpperRight { get; } //右上
    public int m_LowerLeft { get; } //左下
    public int m_LowerRight { get; } //右下

    public AroundGrid(int[,] map, int x, int z)
    {
        m_UpGrid = map[x, z + 1];
        m_UnderGrid = map[x, z - 1];
        m_LeftGrid = map[x - 1, z];
        m_RightGrid = map[x + 1, z];

        m_UpperLeft = map[x - 1, z + 1];
        m_UpperRight = map[x + 1, z + 1];
        m_LowerLeft = map[x - 1, z - 1];
        m_LowerRight = map[x + 1, z - 1];
    }
}