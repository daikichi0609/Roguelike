﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonContents : SingletonMonoBehaviour<DungeonContents>
{
    public void DeployDungeonContents()
    {
        DeployStairs();
        DeployPlayer();
        DeployEnemy(5);
    }

    // 4 -> 階段
    private void DeployStairs() //階段配置
    {
        int[,] map = DungeonTerrain.Instance.Map; //マップ取得
        int[] coord = ChooseEmptyRandomRoomGrid(map); //何もない部屋座標を取得

        DungeonTerrain.Instance.SetValueInTerrainList((int)DungeonTerrain.GRID_ID.STAIRS, coord[0], coord[1]); //マップに階段を登録
        GameObject gridObject = Instantiate(DungeonContentsHolder.Instance.Stairs, new Vector3(coord[0], 0, coord[1]), Quaternion.identity); //オブジェクト生成
        DungeonTerrain.Instance.SetObjectInTerrainListInstead(gridObject, coord[0], coord[1]); //既存のオブジェクトを破壊して代わりに代入

        gridObject.GetComponent<Grid>().IsOnId = Grid.ISON_ID.STAIRS;
    }

    private void DeployPlayer() //プレイヤー配置
    {
        int[,] map = DungeonTerrain.Instance.Map; //マップ取得
        int[] coord = ChooseEmptyRandomRoomGrid(map); //何もない部屋座標を取得

        GameObject player = Instantiate(PlayerObject(), new Vector3(coord[0], 0.51f, coord[1]), Quaternion.identity);
        BattleManager.Instance.PlayerList.Add(player);

        GameObject grid = DungeonTerrain.Instance.GetTerrainListObject(coord[0], coord[1]);
        grid.GetComponent<Grid>().IsOnId = Grid.ISON_ID.PLAYER;
        grid.GetComponent<Grid>().IsOnObject = player;
    }

    private GameObject PlayerObject()
    {
        switch(GameManager.Instance.Name)
        {
            case BattleStatus.NAME.BOXMAN:
                return DungeonContentsHolder.Instance.Boxman;
        }
        return null;
    }

    public void DeployEnemy(int enemyNum) //敵配置
    {
        if (enemyNum <= 0)
            return;

        int[,] map = DungeonTerrain.Instance.Map;

        for (int num = 1; num <= enemyNum; num++)
        {
            int[] coord = ChooseEmptyRandomRoomGrid(map);
            GameObject enemy = Instantiate(EnemyObject(), new Vector3(coord[0], 0.51f, coord[1]), Quaternion.identity);
            BattleManager.Instance.EnemyList.Add(enemy);
            GameObject grid = DungeonTerrain.Instance.GetTerrainListObject(coord[0], coord[1]);
            grid.GetComponent<Grid>().IsOnId = Grid.ISON_ID.ENEMY;
            grid.GetComponent<Grid>().IsOnObject = enemy;
        }
    }

    private GameObject EnemyObject()
    {
        return DungeonContentsHolder.Instance.Mashroom;
        /*
        switch (GameManager.Instance.Theme)
        {
            case GameManager.DUNGEON_THEME.GRASS:
                return DungeonContentsHolder.Instance.Mashroom;
        }
        return null;
        */
    }

    private int[] ChooseRandamRoomGrid(int[,] map) //ランダムな部屋座標を返す
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

    private int[] ChooseEmptyRandomRoomGrid(int[,] map) //ランダムな何も乗っていない部屋座標を返す
    {
        int[] coord = ChooseRandamRoomGrid(map);
        bool isEmpty = false;
        while(isEmpty == false)
        {
            coord = ChooseRandamRoomGrid(map);
            Grid grid = DungeonTerrain.Instance.GetTerrainListObject(coord[0], coord[1]).GetComponent<Grid>();
            if(grid.IsOnId == Grid.ISON_ID.NOTHING)
            {
                isEmpty = true;
            }
        }
        return coord;
    }
}