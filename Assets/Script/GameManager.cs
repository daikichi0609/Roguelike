using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private bool m_PlayerTurn;
    public bool PlayerTurn
    {
       get { return m_PlayerTurn; } 
    }

    [SerializeField] private GameObject m_PlayerObject;

    private void Awake()
    {
        DungeonTerrain.Instance.DeployDungeonTerrain();
    }

    // Start is called before the first frame update
    private void Start()
    {
        DungeonContents.Instance.DeployDungeonContents();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void DeployAll()
    {
        
    }

    public void SwitchTurn()
    {
        m_PlayerTurn = !m_PlayerTurn;
    }


}
