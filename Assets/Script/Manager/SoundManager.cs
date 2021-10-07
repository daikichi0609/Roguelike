using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager :  SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] private AudioSource m_NormalBGM;
    public AudioSource NormalBGM
    {
        get { return m_NormalBGM; }
    }

    [SerializeField] private AudioSource m_BossBGM;
    public AudioSource BossBGM
    {
        get { return m_BossBGM; }
    }

    [SerializeField] private AudioSource m_KD;
    public AudioSource KD
    {
        get { return m_KD; }
    }

    [SerializeField] private AudioSource m_Attack_Sword;
    public AudioSource Attack_Sword
    {
        get { return m_Attack_Sword; }
    }

    [SerializeField] private AudioSource m_Damage_Small;
    public AudioSource Damage_Small
    {
        get { return m_Damage_Small; }
    }

    [SerializeField] private AudioSource m_Miss;
    public AudioSource Miss
    {
        get { return m_Miss; }
    }
}
