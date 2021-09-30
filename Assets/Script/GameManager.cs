using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum DUNGEON_THEME
    {
        CRYSTAL,
        GRASS,
        WHITE,
        ROCK
    }

    [SerializeField] private DUNGEON_THEME m_DungeonTheme;
    public DUNGEON_THEME DungeonTheme
    {
        get { return m_DungeonTheme; }
        set { m_DungeonTheme = value; }
    }

    [SerializeField] private BattleStatus.NAME m_LeaderName;
    public BattleStatus.NAME Name
    {
        get { return m_LeaderName; }
    }

    private void Awake()
    {
        m_LeaderName = BattleStatus.NAME.BOXMAN;

        SoundManager.Instance.NormalBGM.Play();
        //SoundManager.Instance.BossBGM.Play();
        //SoundManager.Instance.KD.Play();

        DungeonTerrain.Instance.DeployDungeonTerrain();
        DungeonContents.Instance.DeployDungeonContents();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void DeployAll()
    {
        
    }

}
