using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharaBattle : MonoBehaviour
{ 
    private CharaMove m_CharaMove;
    protected CharaMove CharaMove
    {
        get { return m_CharaMove; }
        set { m_CharaMove = value; }
    }

    private BattleStatus m_BattleStatus;
    public BattleStatus BattleStatus
    {
        get { return m_BattleStatus; }
        set { m_BattleStatus = value; }
    }

    protected CharaCondition m_Condition;
    protected CharaCondition Condition
    {
        get { return m_Condition; }
        set { m_Condition = value; }
    }

    protected GameObject TargetObject
    {
        get;
        set;
    }

    public bool IsAttacking
    {
        get;
        protected set;
    }

    public int Lv
    {
        private get; set;
    }

    public virtual void Initialize()
    {
        CharaMove = this.gameObject.GetComponent<CharaMove>();
        Condition = this.gameObject.GetComponent<CharaCondition>();
    }

    public void Damage(int power)
    {
        int damage = Calculator.CalculateDamage(power, BattleStatus.Def);
        BattleStatus.Hp = Calculator.CalculateRemainingHp(BattleStatus.Hp, damage);

        SoundManager.Instance.Damage_Small.Play();

        if(BattleStatus.Hp <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        int num = ObjectManager.Instance.EnemyList.IndexOf(ObjectManager.Instance.SpecifiedPositionEnemyObject(CharaMove.Position));
        ObjectManager.Instance.EnemyList.RemoveAt(num);
        Destroy(this.gameObject);
    }

    protected void PlayAnimation(string name, float actFrame) //アニメーション一回流す
    {
        CharaMove.CharaAnimator.SetBool(name, true);
        StartCoroutine(Coroutine.DelayCoroutine(actFrame, () =>
        {
            CharaMove.CharaAnimator.SetBool(name, false);
        }));
    }

    protected void SwitchIsAttacking(float actFrame) //攻撃の全体フレーム
    {
        IsAttacking = true;
        
        StartCoroutine(Coroutine.DelayCoroutine(actFrame, () =>
        {
            IsAttacking = false;
        }));
    }

    protected void PlaySound(bool hit, float hitFrame, AudioSource sound) //攻撃サウンド
    {
        if (hit == false)
        {
            StartCoroutine(Coroutine.DelayCoroutine(hitFrame, () =>
            {
                SoundManager.Instance.Miss.Play();
            }));
        }
        else if (hit == true)
        {
            StartCoroutine(Coroutine.DelayCoroutine(hitFrame, () =>
            {
                sound.Play();
            }));
        }
    }

    protected void FinishTurn()
    {
        CharaMove.FinishTurn();
    }

    public virtual void DecideAndExcuteAction()
    {

    }

    public virtual void Attack()
    {
        FinishTurn();
    }

    public virtual void Skill()
    {
        FinishTurn();
    }
}

public abstract class PlayerBattle : CharaBattle
{
    public int Ex
    {
        private get; set;
    }

    public int NormalAttackLv
    {
        protected get; set;
    }
    public int SkillLv
    {
        protected get; set;
    }
    public int AbilityLv
    {
        protected get; set;
    }

    public override void Initialize()
    {
        base.Initialize();

        NormalAttackLv = 1;
        SkillLv = 1;
        AbilityLv = 1;
    }

    override public void Attack()
    {
        FinishTurn();
    }

    override public void Skill()
    {
        FinishTurn();
    }

    virtual protected void Chase()
    {
        FinishTurn();
    }

    virtual protected void Search()
    {
        FinishTurn();
    }
}