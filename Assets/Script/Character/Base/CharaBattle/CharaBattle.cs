using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharaBattle : MonoBehaviour
{
    public enum ACTION
    {
        ATTACK,
        SKILL,
        MOVE,
    }

    private CharaMove m_CharaMove;
    protected CharaMove CharaMove
    {
        get { return m_CharaMove; }
        set { m_CharaMove = value; }
    }

    [SerializeField] private BattleStatus.Parameter m_Parameter;
    public BattleStatus.Parameter Parameter
    {
        get { return m_Parameter; }
        set { m_Parameter = value; }
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

    public int MaxHp
    {
        get;
        private set;
    }

    public virtual void Initialize()
    {
        CharaMove = this.gameObject.GetComponent<CharaMove>();
        Condition = this.gameObject.GetComponent<CharaCondition>();
        MaxHp = Parameter.Hp;
    }

    protected virtual AttackInfo AttackInfo { get; }

    protected virtual void NormalAttack()
    {
        CharaMove.FinishTurn();
        //ステート操作＆アニメーション再生
        SwitchIsAttacking(AttackInfo.ActFrame);
        PlayAnimation("IsAttacking");
    }

    public enum TARGET
    {
        PLAYER,
        ENEMY,
        NONE
    }

    protected virtual void Attack(Vector3 attackPos, TARGET target)
    {
        if (AttackInfo.IsPossibleToDiagonal == false)
        {
            if(DungeonTerrain.Instance.IsPossibleToMoveDiagonal((int)CharaMove.Position.x, (int)CharaMove.Position.z, (int)CharaMove.Direction.x, (int)CharaMove.Direction.z) == false)
            {
                PlaySound(true, AttackInfo.AnimFrame, SoundManager.Instance.Miss);
                return;
            }
        }

        //攻撃範囲に攻撃対象がいるか確認
        switch (target)
        {
            case TARGET.PLAYER:
                if (Positional.IsPlayerOn(attackPos) == false)
                {
                    PlaySound(true, AttackInfo.AnimFrame, SoundManager.Instance.Miss);
                    return;
                }
                break;

            case TARGET.ENEMY:
                if (Positional.IsEnemyOn(attackPos) == false)
                {
                    PlaySound(true, AttackInfo.AnimFrame, SoundManager.Instance.Miss);
                    return;
                }
                break;

            case TARGET.NONE:
                if (Positional.IsCharacterOn(attackPos) == false)
                {
                    PlaySound(true, AttackInfo.AnimFrame, SoundManager.Instance.Miss);
                    return;
                }
                break;
        }

        CharaBattle targetBattle = ObjectManager.Instance.SpecifiedPositionCharacterObject(attackPos).GetComponent<CharaBattle>();
        BattleStatus.Parameter targetParam = targetBattle.Parameter;

        //威力計算
        float mag = Calculator.CalculateNormalAttackMag(AttackInfo.Lv, AttackInfo.Mag);
        int power = Calculator.CalculatePower(Parameter.Atk, mag);

        //音再生は剣のヒット時
        StartCoroutine(Coroutine.DelayCoroutine(AttackInfo.AnimFrame, () =>
        {
            PlaySound(true, AttackInfo.AnimFrame, SoundManager.Instance.Attack_Sword);
        }));

        //ダメージはモーション終わり
        StartCoroutine(Coroutine.DelayCoroutine(AttackInfo.AnimFrame, () =>
        {
            targetBattle.Damage(power, Parameter.Dex);
        }));
    }

    protected virtual void Skill()
    {

    }

    public void Damage(int power, float dex)
    {
        TurnManager.Instance.IsCanAction = false;
        //ヒットorノット判定
        if (Calculator.JudgeHit(dex, Parameter.Eva) == false)
        {
            PlaySound(false, 0f, null);
            return;
        }

        int damage = Calculator.CalculateDamage(power, Parameter.Def);
        Parameter.Hp = Calculator.CalculateRemainingHp(Parameter.Hp, damage);

        SoundManager.Instance.Damage_Small.Play();
        PlayAnimation("IsDamaging");
        StartCoroutine(Coroutine.DelayCoroutine(0.1f, () =>
        {
            TurnManager.Instance.IsCanAction = true;
        }));

        if (Parameter.Hp <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        
    }

    protected void PlayAnimation(string name) //アニメーション一回流す
    {
        CharaMove.CharaAnimator.SetBool(name, true);
        StartCoroutine(Coroutine.DelayCoroutine(0.1f, () =>
        {
            CharaMove.CharaAnimator.SetBool(name, false);
        }));
    }

    protected void SwitchIsAttacking(float actFrame) //攻撃の全体フレーム
    {
        StartCoroutine(Coroutine.DelayCoroutine(actFrame, () =>
        {
            TurnManager.Instance.IsCanAction = true;
        }));
    }

    protected void PlaySound(bool hit, float delayFrame, AudioSource sound) //攻撃サウンド
    {
        if (hit == false)
        {
            StartCoroutine(Coroutine.DelayCoroutine(delayFrame, () =>
            {
                SoundManager.Instance.Miss.Play();
                TurnManager.Instance.IsCanAction = true;
            }));
        }
        else if (hit == true)
        {
            StartCoroutine(Coroutine.DelayCoroutine(delayFrame, () =>
            {
                sound.Play();
                TurnManager.Instance.IsCanAction = true;
            }));
        }
    }
}

public abstract class AttackInfo
{
    public virtual string Name { get; } //技名
    public int Lv { get; set; } = 1; //技レベル

    public virtual float Mag { get; } //攻撃倍率
    public virtual float ActFrame { get; } //モーション全体フレーム
    public virtual float AnimFrame { get; } //アニメーション秒数
    public virtual bool IsPossibleToDiagonal { get; } //斜め壁抜け可能かどうか
}