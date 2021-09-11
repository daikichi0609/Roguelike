using UnityEngine;

//https://note.com/motibe_tsukuru/n/nbe75bb690bcc

public class DeployDungeon: SingletonMonoBehaviour<DeployDungeon>
{
    private int[,] m_Map;
    public int[,] Map
    {
        get { return m_Map; }
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

    ↑
    ↑
    ↑
    z
     x → → →
    */

    public void DeployNewDungeon()
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
                        Instantiate(m_Wall, new Vector3(i, 0, j), Quaternion.identity);
                        break;

                    case 1:
                        Instantiate(m_PathWay, new Vector3(i, 0, j), Quaternion.identity);
                        break;

                    case 2:
                        AroundGrid aroundGrid = CheckAroundGrid(m_Map, i, j);
                        if (CheckGateWay(aroundGrid) == true)
                        {
                            m_Map[i, j] = 3;
                            Instantiate(m_Gate, new Vector3(i, 0, j), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(m_Room, new Vector3(i, 0, j), Quaternion.identity);
                        }
                        break;
                }
            }
        }
    }

    private AroundGrid CheckAroundGrid(int[,] map, int i, int j)
    {
        return new AroundGrid(map, i, j);
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
}

public struct AroundGrid
{
    public int m_UpGrid { get; }
    public int m_UnderGrid { get; }
    public int m_LeftGrid { get; }
    public int m_RightGrid { get; }

    public AroundGrid(int[,] map, int x, int z)
    {
        m_UpGrid = map[x, z + 1];
        m_UnderGrid = map[x, z - 1];
        m_LeftGrid = map[x - 1, z];
        m_RightGrid = map[x + 1, z];
    }
}