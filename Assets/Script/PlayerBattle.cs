using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class CharaBattle : MonoBehaviour
{
    [SerializeField] protected BattleStatus.NAME m_CharaName;
    public bool Turn
    {
        get;
        set;
    }

    private BattleStatus m_BattleStatus;
    public BattleStatus BattleStatus
    {
        get { return m_BattleStatus; }
        set { m_BattleStatus = value; }
    }

    private CharaMove m_CharaMove;
    protected CharaMove CharaMove
    {
        get { return m_CharaMove; }
        set { m_CharaMove = value; }
    }

    protected CharaCondition m_Condition;
    protected CharaCondition Condition
    {
        get { return m_Condition; }
        set { m_Condition = value; }
    }

    public int Lv
    {
        private get; set;
    }

    public void Damage(int power)
    {
        int damage = BattleManager.Instance.CalculateDamage(power, BattleStatus.Def);
        BattleStatus.Hp = BattleManager.Instance.CalculateRemainingHp(BattleStatus.Hp, damage);

        SoundManager.Instance.Damage_Small.Play();

        if(BattleStatus.Hp <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        CharaMove.InitializeBeforeMoveGrid();
        Destroy(this.gameObject);
    }

}

public class PlayerBattle : CharaBattle
{
    public int Ex
    {
        private get; set;
    }

    public int m_NormalAttackLv
    {
        private get; set;
    }
    public int m_SkillLv
    {
        private get; set;
    }
    public int m_AbilityLv
    {
        private get; set;
    }

    private void Start()
    {
        CharaMove = this.gameObject.GetComponent<PlayerMove>();
        Condition = this.gameObject.GetComponent<CharaCondition>();
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Attack();
        }
    }

    private void Initialize()
    {
        BattleStatus = CharaDataManager.Instance.LoadPlayerScriptableObject(m_CharaName);
        m_NormalAttackLv = 1;
        m_SkillLv = 1;
        m_AbilityLv = 1;
    }

    private void Attack()
    {
        if(CharaMove.IsMoving == true)
        {
            return;
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            Skill();
            return;
        }
        switch (m_CharaName)
        {
            case BattleStatus.NAME.BOXMAN:
                CharaMove.CharaAnimator.SetBool("IsAttacking", true);
                StartCoroutine(DelayCoroutine(0.4f, () =>
                {
                    BoxmanMethod.Attack(m_NormalAttackLv, CharaMove.Position, CharaMove.Direction, BattleStatus.Atk);
                    CharaMove.CharaAnimator.SetBool("IsAttacking", false);
                }));
                break;
        }
    }

    private void Skill()
    {
        switch (m_CharaName)
        {
            case BattleStatus.NAME.BOXMAN:
                BoxmanMethod.Skill(m_SkillLv, ref m_Condition);
                break;
        }
    }

    private void Ability()
    {

    }

    private IEnumerator DelayCoroutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}