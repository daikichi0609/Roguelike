using UnityEngine;
using System.Collections.Generic;

//https://note.com/motibe_tsukuru/n/nbe75bb690bcc

public class DungeonTerrain: SingletonMonoBehaviour<DungeonTerrain>
{
    private int[,] m_Map;
    public int[,] Map
    {
        get { return m_Map; }
        private set { m_Map = value; }
    }
    private List<List<GameObject>> m_TerrainList = new List<List<GameObject>>();

    public GameObject GetTerrainListObject(int x, int z)
    {
        return m_TerrainList[x][z];
    }

    public void SetValueInTerrainList(int value, int x, int z)
    {
        Map[x, z] = value;
    }

    public void SetObjectInTerrainListInstead(GameObject @object, int x, int z)
    {
        GameObject removeObject = m_TerrainList[x][z];
        m_TerrainList[x].RemoveAt(z);
        Destroy(removeObject);
        m_TerrainList[x].Insert(z, @object);
    }

    private List<List<GameObject>> m_RoomList = new List<List<GameObject>>();
    public List<List<GameObject>> RoomList
    {
        get { return m_RoomList; }
        set { m_RoomList = value; }
    }

    //注意
    public List<GameObject> GetRoomList(int roomId)
    {
        return RoomList[roomId - 1];
    }

    public GameObject GetRoomListObject(int id, int num)
    {
        return RoomList[id][num];
    }

    [SerializeField] private List<Range> m_RangeList = new List<Range>();
    public List<Range> RangeList
    {
        get { return m_RangeList; }
        set { m_RangeList = value; }
    }

    private int m_MapSizeX = 32;
    private int m_MapSizeZ = 32;
    private int m_MaxRoom = 8;

    public enum GRID_ID
    {
        WALL = 0,
        PATH_WAY = 1,
        ROOM = 2,
        GATE = 3,
        STAIRS = 4,
        NONE = -1
    }

    /*
    0 -> 壁
    1 -> 通路
    2 -> 部屋
    3 -> 出入り口
    4 -> 階段
    -1 -> ダンジョン外

    ↑
    ↑
    ↑
    z
     x → → →
    */

    public void DeployDungeonTerrain()
    {
        Map = MapGenerator.Instance.GenerateMap(m_MapSizeX, m_MapSizeZ, m_MaxRoom);

        for (int i = 0; i < Map.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < Map.GetLength(1) - 1; j++)
            {
                int id = Map[i, j];
                switch (id)
                {
                    case (int)GRID_ID.WALL: //0
                        m_TerrainList.Add(new List<GameObject>());
                        GameObject @object = Instantiate(DungeonContentsHolder.Instance.Wall, new Vector3(i, 0, j), Quaternion.identity);
                        @object.GetComponent<Grid>().GridID = GRID_ID.WALL;
                        m_TerrainList[i].Add(@object);
                        break;

                    case (int)GRID_ID.PATH_WAY: //1
                        @object = Instantiate(PathWayGrid(), new Vector3(i, 0, j), Quaternion.identity);
                        @object.GetComponent<Grid>().GridID = GRID_ID.PATH_WAY;
                        m_TerrainList[i].Add(@object);
                        break;

                    case (int)GRID_ID.ROOM: //2
                        AroundGridID aroundGrid = CreateAroundGrid(i, j);
                        if (CheckGateWay(aroundGrid) == true)
                        {
                            Map[i, j] = (int)GRID_ID.GATE; //3
                            @object = Instantiate(RoomGrid(), new Vector3(i, 0, j), Quaternion.identity);
                            @object.GetComponent<Grid>().GridID = GRID_ID.GATE;
                            m_TerrainList[i].Add(@object);
                        }
                        else
                        {
                            @object = Instantiate(RoomGrid(), new Vector3(i, 0, j), Quaternion.identity);
                            @object.GetComponent<Grid>().GridID = GRID_ID.ROOM;
                            m_TerrainList[i].Add(@object);
                        }
                        break;
                }
            }
        }

        DungeonContents.Instance.DeployStairs();
        AddRoomObjectToList();
        RegisterRoomID();
    }

    public void HideObject()
    {

    }

    public void AddRoomObjectToList()
    {
        RoomList = new List<List<GameObject>>();
        foreach (Range range in RangeList)
        {
            List<GameObject> list = new List<GameObject>();

            for (int x = range.Start.X; x <= range.End.X; x++)
            {
                for(int z = range.Start.Y; z <= range.End.Y; z++)
                {
                    list.Add(GetTerrainListObject(x, z));
                }
            }
            RoomList.Add(list);
        }
    }

    public void RegisterRoomID()
    {
        for(int id = 0; id <= RoomList.Count - 1; id++)
        {
            for(int num = 0; num <= RoomList[id].Count - 1; num++)
            {
                Grid grid = GetRoomListObject(id, num).GetComponent<Grid>();
                grid.RoomID = id + 1;
            }
        }
    }

    private GameObject PathWayGrid()
    {
        switch(GameManager.Instance.DungeonTheme)
        {
            case GameManager.DUNGEON_THEME.GRASS:
                return DungeonContentsHolder.Instance.Grass_C;

            case GameManager.DUNGEON_THEME.ROCK:
                return DungeonContentsHolder.Instance.Rock_C;

            case GameManager.DUNGEON_THEME.WHITE:
                return DungeonContentsHolder.Instance.White_C;

            case GameManager.DUNGEON_THEME.CRYSTAL:
                return DungeonContentsHolder.Instance.CrystalRock_C;
        }
        return null;
    }

    private GameObject RoomGrid()
    {
        switch (GameManager.Instance.DungeonTheme)
        {
            case GameManager.DUNGEON_THEME.GRASS:
                return DungeonContentsHolder.Instance.Grass_A;

            case GameManager.DUNGEON_THEME.ROCK:
                return DungeonContentsHolder.Instance.Rock_A;

            case GameManager.DUNGEON_THEME.WHITE:
                return DungeonContentsHolder.Instance.White_A;

            case GameManager.DUNGEON_THEME.CRYSTAL:
                return DungeonContentsHolder.Instance.CrystalRock_A;
        }
        return null;
    }

    public AroundGridID CreateAroundGrid(int x, int z)
    {
        return new AroundGridID(Map, x, z);
    }

    private bool CheckGateWay(AroundGridID aroundGrid)
    {
        if (aroundGrid.UpGrid == (int)GRID_ID.PATH_WAY)
        {
            return true;
        }
        if (aroundGrid.UnderGrid == (int)GRID_ID.PATH_WAY)
        {
            return true;
        }
        if (aroundGrid.LeftGrid == (int)GRID_ID.PATH_WAY)
        {
            return true;
        }
        if (aroundGrid.RightGrid == (int)GRID_ID.PATH_WAY)
        {
            return true;
        }
        return false;
    }

    public int GridID(int pos_x, int pos_z)
    {
        return Map[pos_x, pos_z];
    }

    public int DestinationGridID(int pos_x, int pos_z, int direction_x, int direction_z)
    {
        return Map[pos_x + direction_x, pos_z + direction_z];
    }

    public bool IsPossibleToMoveDiagonal(int pos_x, int pos_z, int direction_x, int direction_z)
    {
        if(direction_x == 0 || direction_z == 0)
        {
            return true;
        }
        if (Map[pos_x + direction_x, pos_z] == (int)GRID_ID.WALL || Map[pos_x, pos_z + direction_z] == (int)GRID_ID.WALL)
        {
            return false;
        }
        return true;
    }
}

public struct AroundGridID
{
    public int UpGrid { get; } //上
    public int UnderGrid { get; } //下
    public int LeftGrid { get; } //左
    public int RightGrid { get; } //右

    public int UpperLeft { get; } //左上
    public int UpperRight { get; } //右上
    public int LowerLeft { get; } //左下
    public int LowerRight { get; } //右下

    public AroundGridID(int[,] map, int x, int z)
    {
        UpGrid = map[x, z + 1];
        UnderGrid = map[x, z - 1];
        LeftGrid = map[x - 1, z];
        RightGrid = map[x + 1, z];

        UpperLeft = map[x - 1, z + 1];
        UpperRight = map[x + 1, z + 1];
        LowerLeft = map[x - 1, z - 1];
        LowerRight = map[x + 1, z - 1];
    }
}