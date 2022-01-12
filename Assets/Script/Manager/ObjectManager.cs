﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager>
{
    /// <summary>
    /// 要素の追加
    /// 要素の削除
    /// 要素数の変化
    /// 要素の上書き
    /// 要素の移動
    /// リストのクリア
    /// </summary>

    protected override void Awake()
    {
        base.Awake();

        GetPlayerObs.Subscribe(_ => MessageBroker.Default.Publish(new Message.IsChangedPlayerList()));
    }

    /// <summary>
    /// Playerのオブジェクト管理リスト
    /// </summary>
    [SerializeField] public ReactiveCollection<GameObject> m_PlayerList = new ReactiveCollection<GameObject>();

    /// <summary>
    /// IObservable取得用
    /// </summary>
    public IObservable<CollectionReplaceEvent<GameObject>> GetPlayerObs => m_PlayerList.ObserveReplace();

    /// <summary>
    /// Playerオブジェクト取得
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public GameObject PlayerObject(int i)
    {
        return m_PlayerList[i];
    }

    public GameObject SpecifiedPositionPlayerObject(Vector3 pos)
    {
        foreach (GameObject player in m_PlayerList)
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
        foreach (GameObject player in m_PlayerList)
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

    /// <summary>
    /// Enemyのオブジェクト管理リスト
    /// </summary>
    [SerializeField] public ReactiveCollection<GameObject> m_EnemyList = new ReactiveCollection<GameObject>();

    /// <summary>
    /// リストの値だけ見れる
    /// </summary>
    //public List<GameObject> EnemyValueList => m_EnemyList.ToList();

    /// <summary>
    /// IObservable取得用
    /// </summary>
    public IObservable<CollectionReplaceEvent<GameObject>> GetEnemyObs => m_EnemyList.ObserveReplace();

    public GameObject EnemyObject(int i)
    {
        return m_EnemyList[i];
    }

    public GameObject SpecifiedPositionEnemyObject(Vector3 pos)
    {
        foreach (GameObject enemy in m_EnemyList)
        {
            Chara charaMove = enemy.GetComponent<Chara>();
            if (charaMove.Position.x == pos.x && charaMove.Position.z == pos.z)
            {
                return enemy;
            }
        }
        return null;
    }

    public GameObject SpecifiedPositionCharacterObject(Vector3 pos)
    {
        foreach (GameObject player in m_PlayerList)
        {
            Chara charaMove = player.GetComponent<Chara>();
            if (charaMove.Position.x == pos.x && charaMove.Position.z == pos.z)
            {
                return player;
            }
        }

        foreach (GameObject enemy in m_EnemyList)
        {
            Chara charaMove = enemy.GetComponent<Chara>();
            if (charaMove.Position.x == pos.x && charaMove.Position.z == pos.z)
            {
                return enemy;
            }
        }
        return null;
    }

    [SerializeField] private List<GameObject> m_ItemList = new List<GameObject>();
    public List<GameObject> ItemList
    {
        get { return m_ItemList; }
        set { m_ItemList = value; }
    }

    public GameObject SpecifiedPositionItemObject(Vector3 pos)
    {
        foreach (GameObject player in m_PlayerList)
        {
            Chara charaMove = player.GetComponent<Chara>();
            if (charaMove.Position.x == pos.x && charaMove.Position.z == pos.z)
            {
                return player;
            }
        }

        foreach (GameObject enemy in m_EnemyList)
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
