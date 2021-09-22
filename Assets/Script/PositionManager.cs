﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager :SingletonMonoBehaviour<PositionManager>
{
    public bool IsPossibleToMoveGrid(Vector3 pos, Vector3 direction)
    {
        int[,] map = DungeonTerrain.Instance.Map;
        int pos_x = (int)pos.x;
        int pos_z = (int)pos.z;
        int direction_x = (int)direction.x;
        int direction_z = (int)direction.z;

        if (direction_x != 0 && direction_z != 0) //斜め移動の場合、壁が邪魔になっていないかどうかチェックする
        {
            if(DungeonTerrain.Instance.IsPossibleToMoveDiagonal(map, pos_x, pos_z, direction_x, direction_z) == false)
            {
                return false;
            }
        }

        int id = DungeonTerrain.Instance.DestinationGridID(map, pos_x, pos_z, direction_x, direction_z);

        if(id == (int)DungeonTerrain.GRID_ID.PATH_WAY || id == (int)DungeonTerrain.GRID_ID.ROOM || id == (int)DungeonTerrain.GRID_ID.GATE || id == (int)DungeonTerrain.GRID_ID.STAIRS)
        {
            return true;
        }
        return false;
    }

    public bool EnemyIsOn(Vector3 pos)
    {
        int pos_x = (int)pos.x;
        int pos_z = (int)pos.z;

        GameObject grid = DungeonTerrain.Instance.GetListObject(pos_x, pos_z);
        if (grid.GetComponent<Grid>().IsOnId == Grid.ISON_ID.ENEMY)
        {
            return true;
        }
        return false;
    }

    public void InformAttack(Vector3 attackPos, int power)
    {
        if(EnemyIsOn(attackPos) == false)
        {
            return;
        }
        int pos_x = (int)attackPos.x;
        int pos_z = (int)attackPos.z;

        Grid grid = DungeonTerrain.Instance.GetListObject(pos_x, pos_z).GetComponent<Grid>();
        grid.InformAttack(power);
    }
}