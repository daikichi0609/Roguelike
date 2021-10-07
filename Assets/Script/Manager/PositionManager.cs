using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager :SingletonMonoBehaviour<PositionManager>
{
    public bool IsPossibleToMoveGrid(Vector3 pos, Vector3 direction) //指定座標から指定方向へ１マス移動可能かどうか調べる
    {
        int[,] map = DungeonTerrain.Instance.Map;
        int pos_x = (int)pos.x;
        int pos_z = (int)pos.z;
        int direction_x = (int)direction.x;
        int direction_z = (int)direction.z;

        if (direction_x != 0 && direction_z != 0) //斜め移動の場合、壁が邪魔になっていないかどうかチェックする
        {
            if(DungeonTerrain.Instance.IsPossibleToMoveDiagonal(pos_x, pos_z, direction_x, direction_z) == false)
            {
                return false;
            }
        }

        int id = DungeonTerrain.Instance.DestinationGridID(pos_x, pos_z, direction_x, direction_z);

        if(id == (int)DungeonTerrain.GRID_ID.PATH_WAY || id == (int)DungeonTerrain.GRID_ID.ROOM || id == (int)DungeonTerrain.GRID_ID.GATE || id == (int)DungeonTerrain.GRID_ID.STAIRS)
        {
            return true;
        }
        return false;
    }

    public bool EnemyIsOn(Vector3 pos) //指定座標に敵がいるかどうかを調べる
    {
        int pos_x = (int)pos.x;
        int pos_z = (int)pos.z;

        foreach(GameObject enemy in ObjectManager.Instance.EnemyList)
        {
            Chara charaMove = enemy.GetComponent<Chara>();
            if (charaMove.Position.x == pos.x && charaMove.Position.z == pos.z)
            {
                return true;
            }
        }
        return false;
    }

    public bool NoOneIsThere(Vector3 pos) //指定座標に誰もいないかどうかを返す
    {
        foreach(GameObject player in ObjectManager.Instance.PlayerList ?? new List<GameObject>())
        {
            Chara charaMove = player.GetComponent<Chara>();
            if (charaMove.Position.x == pos.x && charaMove.Position.z == pos.z)
            {
                return false;
            }
        }
        foreach (GameObject enemy in ObjectManager.Instance.EnemyList ?? new List<GameObject>())
        {
            Chara charaMove = enemy.GetComponent<Chara>();
            if (charaMove.Position.x == pos.x && charaMove.Position.z == pos.z)
            {
                return false;
            }
        }
        return true;
    }

    public int IsOnRoomID(Vector3 pos) //指定座標の部屋IDを返す
    {
        return DungeonTerrain.Instance.GetTerrainListObject((int)pos.x, (int)pos.z).GetComponent<Grid>().RoomID;
    }

    public bool PlayerIsOnSpecifyRoom(int id) //指定IDの部屋にプレイヤーがいるかどうかを返す
    {
        foreach(GameObject player in ObjectManager.Instance.PlayerList)
        {
            CharaMove charaMove = player.GetComponent<CharaMove>();
            Vector3 playerPos = charaMove.Position;
            if(IsOnRoomID(playerPos) == id)
            {
                return true;
            }
        }
        return false;
    }
}