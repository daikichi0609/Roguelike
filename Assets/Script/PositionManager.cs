﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager :SingletonMonoBehaviour<PositionManager>
{
    public bool IsPossibleToMove(Vector3 pos, Vector3 direction)
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

        int num = DungeonTerrain.Instance.DestinationGridInfo(map, pos_x, pos_z, direction_x, direction_z);

        if(num == 1 || num == 2 || num == 3 || num == 4)
        {
            return true;
        }
        return false;
    }
}
