using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonContents : SingletonMonoBehaviour<DungeonContents>
{
    //ダンジョンコンテンツ配置
    public void DeployDungeonContents()
    {
        DeployPlayer();
        DeployEnemy(5);
    }

    //ダンジョンコンテンツ撤去
    public void RemoveDungeonContents()
    {
        //RemoveAllPlayerObject();
        RemoveAllEnemyObject();
    }

    public void RedeployDungeonContents()
    {
        RedeployPlayer();
        DeployEnemy(5);
    }

    //敵オブジェクト取得用
    private GameObject CharaObject(BattleStatus.NAME name)
    {
        GameObject chara = ObjectPool.Instance.PoolObject(name.ToString());
        if (chara == null)
        {
            chara = Instantiate(CharaHolder.Instance.CharaObject(name));
        }
        return chara;
    }

    //プレイヤー配置
    private void DeployPlayer()
    {
        int[,] map = DungeonTerrain.Instance.Map; //マップ取得
        int[] coord = ChooseEmptyRandomRoomGrid(map); //何もない部屋座標を取得

        GameObject player = CharaObject(GameManager.Instance.LeaderName);
        player.transform.position = new Vector3(coord[0], 0.51f, coord[1]);
        ObjectManager.Instance.PlayerList.Add(player);
        player.GetComponent<Chara>().Initialize();
        player.GetComponent<CharaBattle>().Initialize();
        UiManager.Instance.GenerateCharacterUi(player);
    }

    private void RedeployPlayer()
    {
        int[,] map = DungeonTerrain.Instance.Map; //マップ取得
        int[] coord = ChooseEmptyRandomRoomGrid(map); //何もない部屋座標を取得

        GameObject player = ObjectManager.Instance.PlayerList[0];
        player.transform.position = new Vector3(coord[0], 0.51f, coord[1]);
        player.GetComponent<Chara>().Position = player.transform.position;
        player.GetComponent<Chara>().Direction = new Vector3(0, 0, -1);
    }

    //敵配置
    public void DeployEnemy(int enemyNum)
    {
        if (enemyNum <= 0)
            return;

        int[,] map = DungeonTerrain.Instance.Map;

        for (int num = 1; num <= enemyNum; num++)
        {
            int[] coord = ChooseEmptyRandomRoomGrid(map);
            GameObject enemy = Instantiate(CharaObject(Utility.RandomEnemyName()), new Vector3(coord[0], 0.51f, coord[1]), Quaternion.identity);
            ObjectManager.Instance.EnemyList.Add(enemy);
            enemy.GetComponent<Chara>().Initialize();
            enemy.GetComponent<CharaBattle>().Initialize();
        }
    }

    //全てのプレイヤーオブジェクトを撤去
    private void RemoveAllPlayerObject()
    {
        foreach(GameObject player in ObjectManager.Instance.PlayerList)
        {
            ObjectManager.Instance.PlayerList.Remove(player);
            string name = player.GetComponent<CharaBattle>().BattleStatus.Name.ToString();
            ObjectPool.Instance.SetObject(name, player);
        }

        ObjectManager.Instance.PlayerList = new List<GameObject>();
    }

    //全ての敵オブジェクトを撤去
    private void RemoveAllEnemyObject()
    {
        foreach (GameObject enemy in ObjectManager.Instance.EnemyList)
        {
            string name = enemy.GetComponent<CharaBattle>().BattleStatus.Name.ToString();
            ObjectPool.Instance.SetObject(name, enemy);
        }
        ObjectManager.Instance.EnemyList = new List<GameObject>();
    }

    //特定の敵オブジェクトを撤去
    public void RemoveEnemyObject(GameObject enemy)
    {
        enemy.SetActive(false);
        ObjectManager.Instance.EnemyList.Remove(enemy);
        string name = enemy.GetComponent<BattleStatus>().Name.ToString();
        ObjectPool.Instance.SetObject(name, enemy);
    }

    //ランダムな部屋座標を返す
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

    //ランダムな何も乗っていない部屋座標を返す
    public int[] ChooseEmptyRandomRoomGrid(int[,] map)
    {
        int[] coord = ChooseRandamRoomGrid(map);
        bool isEmpty = false;
        while(isEmpty == false)
        {
            coord = ChooseRandamRoomGrid(map);
            isEmpty = Positional.IsNoOneThere(new Vector3(coord[0], 0, coord[1]));
        }
        return coord;
    }
}