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
        foreach (GameObject gameObject in EnemyList)
        {
            CharaMove charaMove = gameObject.GetComponent<CharaMove>();
            if (charaMove.Position.x == pos.x && charaMove.Position.z == pos.z)
            {
                return gameObject;
            }
        }
        return null;
    }
}
