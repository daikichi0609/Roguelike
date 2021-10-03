using UnityEngine;

class Boxman: PlayerBattle
{
    [SerializeField] private const float NormalAttackMag = 1.0f;
    private const float HitFrame = 0.4f;
    private const float ActFrame = 0.6f;

    override public void Attack()
    {
        SwitchIsAttacking(ActFrame);
        PlayAnimation("IsAttacking", HitFrame);

        Vector3 attackPos = CharaMove.Position + CharaMove.Direction;
        if(PositionManager.Instance.EnemyIsOn(attackPos) == false)
        {
            PlaySound(false, HitFrame, null);
            return;
        }

        CharaBattle enemyBattle = ObjectManager.Instance.SpecifiedPositionEnemyObject(attackPos).GetComponent<CharaBattle>();
        BattleStatus enemyStatus = enemyBattle.BattleStatus;

        if(Calculator.JudgeHit(BattleStatus.Dex, enemyStatus.Eva) == false)
        {
            PlaySound(false, HitFrame, null);
            return;
        }

        float mag = Calculator.CalculateNormalAttackMag(NormalAttackLv, NormalAttackMag);
        int power = Calculator.CalculatePower(BattleStatus.Atk, mag);

        StartCoroutine(Coroutine.DelayCoroutine(HitFrame, () =>
        {
            PlaySound(true, HitFrame, SoundManager.Instance.Attack_Sword);
        }));

        StartCoroutine(Coroutine.DelayCoroutine(ActFrame, () =>
        {
            enemyBattle.Damage(power);
        }));
    }

    override public void Skill()
    {
        
    }
}