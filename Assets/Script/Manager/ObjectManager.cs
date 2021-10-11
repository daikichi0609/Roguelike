using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager>
{
    [SerializeField] private List<GameObject> m_PlayerList = new List<GameObject>();
    public List<GameObject> PlayerList
    {
        get { return m_PlayerList; }
    }
    public GameObject PlayerObject(int i)
    {
        return m_PlayerList[i];
    }

    public GameObject SpecifiedPositionPlayerObject(Vector3 pos)
    {
        foreach (GameObject player in PlayerList)
        {
            Chara charaMove = player.GetComponent<Chara>();
            if (charaMove.Position.x == pos.x && charaMove.Position.z == pos.z)
            {
                return player;
            }
        }
        return null;
    }

    public List<GameObject> SpecifiedRoomPlayerObjectList(int roomId)
    {
        if(roomId <= 0)
        {
            return null;
        }

        List<GameObject> playerList = new List<GameObject>();
        List<GameObject> roomList = DungeonTerrain.Instance.GetRoomList(roomId);
        foreach (GameObject player in PlayerList)
        {
            Chara chara = player.GetComponent<Chara>();
            foreach(GameObject grid in roomList)
            {
                if(chara.Position.x == grid.transform.position.x && chara.Position.z == grid.transform.position.z)
                {
                    playerList.Add(player);
                }
            }
        }

        return playerList;
    }

    [SerializeField] private List<GameObject> m_EnemyList = new List<GameObject>();
    public List<GameObject> EnemyList
    {
        get { return m_EnemyList; }
    }
    public GameObject EnemyObject(int i)
    {
        return EnemyList[i];
    }
    public GameObject SpecifiedPositionEnemyObject(Vector3 pos)
    {
        foreach (GameObject enemy in EnemyList)
        {
            Chara charaMove = enemy.GetComponent<Chara>();
            if (charaMove.Position.x == pos.x && charaMove.Position.z == pos.z)
            {
                return enemy;
            }
        }
        return null;
    }

    public List<GameObject> GateWayObjectList(int roomId)
    {
        List<GameObject> roomList = DungeonTerrain.Instance.GetRoomList(roomId);
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject gridObject in roomList)
        {
            Grid grid = gridObject.GetComponent<Grid>();
            if (grid.GridID == DungeonTerrain.GRID_ID.GATE)
            {
                list.Add(gridObject);
            }
        }
        return list;
    }
}
