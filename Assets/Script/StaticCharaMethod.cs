using UnityEngine;

static class Calculator
{
    public static float CalculateNormalAttackMag(int lv, float mag)
    {
        switch (lv)
        {
            case 1:
                return mag * 1.0f;

            case 2:
                return mag * 1.5f;

            case 3:
                return mag * 2.0f;

            case 4:
                return mag * 2.5f;

            case 5:
                return mag * 3.5f;
        }
        return 0f;
    }
}

static class BoxmanMethod
{
    static public void Attack(int lv, Vector3 pos, Vector3 direction, int atk)
    {
        float mag = 1.0f;
        mag = Calculator.CalculateNormalAttackMag(lv, mag);
        Vector3 attackPos = pos + direction;
        int power = BattleManager.Instance.CalculatePower(atk, mag);
        bool isHit = PositionManager.Instance.InformEnemyOfAttacking(attackPos, power);
        if(isHit == true)
        {
            SoundManager.Instance.Attack_Sword.Play();
        }
        else
        {
            SoundManager.Instance.Miss.Play();
        }
    }

    static public void Skill(int Lv, ref CharaCondition condition)
    {
        condition.BoxmanSkillBuffTime = 3;
    }

    static public void Ability(int Lv)
    {

    }
}