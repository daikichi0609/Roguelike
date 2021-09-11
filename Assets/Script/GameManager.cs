using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private string m_FileName = "Assets/Resources/TestJson";

    private int[,] m_Map;
    private int m_InitX;
    private int m_InitZ;

    private bool m_PlayerTurn;
    public bool PlayerTurn
    {
       get { return m_PlayerTurn; } 
    }

    [SerializeField] private GameObject m_PlayerObject;

    private void Awake()
    {
        DeployDungeon.Instance.DeployNewDungeon();
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_Map = DeployDungeon.Instance.Map;

        int num = 1;
        while (num != 2)
        {
            m_InitX = Random.Range(0, m_Map.GetLength(0));
            m_InitZ = Random.Range(0, m_Map.GetLength(1));
            num = m_Map[m_InitX, m_InitZ];
        }

        Instantiate(m_PlayerObject, new Vector3(m_InitX, 1, m_InitZ), Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void CreateNewDungeon()
    {

    }

    public void SwitchTurn()
    {
        m_PlayerTurn = !m_PlayerTurn;
    }


}
