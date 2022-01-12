using UnityEngine;

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
        NormalAttackLv = 1;
        SkillLv = 1;
        AbilityLv = 1;

        base.Initialize();
    }

    public void Act(ACTION action)
    {
        switch (action)
        {
            case ACTION.ATTACK:
                if (TurnManager.Instance.IsCanAttack == false)
                {
                    return;
                }
                TurnManager.Instance.IsCanAttack = true;
                NormalAttack();
                break;

            case ACTION.SKILL:
                if (TurnManager.Instance.IsCanAttack == false)
                {
                    return;
                }
                TurnManager.Instance.IsCanAttack = true;
                Skill();
                break;
        }
    }


}