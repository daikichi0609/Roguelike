using UnityEngine;

class Boxman: PlayerBattle
{
    [SerializeField] private const float NormalAttackMag = 1.0f;
    private const float HitFrame = 0.4f;
    private const float ActFrame = 0.6f;

    public override void Initialize()
    {
        base.Initialize();
        BattleStatus = CharaDataManager.Instance.LoadPlayerScriptableObject(BattleStatus.NAME.BOXMAN);
    }

    public override void Attack()
    {
        //ターン終了
        base.Attack();

        //アタッキングブール操作＆アニメーション再生
        SwitchIsAttacking(ActFrame);
        PlayAnimation("IsAttacking", HitFrame);

        //攻撃範囲に敵がいるか確認
        Vector3 attackPos = CharaMove.Position + CharaMove.Direction;
        if(PositionManager.Instance.EnemyIsOn(attackPos) == false)
        {
            PlaySound(false, HitFrame, null);
            return;
        }

        CharaBattle enemyBattle = ObjectManager.Instance.SpecifiedPositionEnemyObject(attackPos).GetComponent<CharaBattle>();
        BattleStatus enemyStatus = enemyBattle.BattleStatus;

        //ヒットorノット判定
        if(Calculator.JudgeHit(BattleStatus.Dex, enemyStatus.Eva) == false)
        {
            PlaySound(false, HitFrame, null);
            return;
        }

        //威力計算
        float mag = Calculator.CalculateNormalAttackMag(NormalAttackLv, NormalAttackMag);
        int power = Calculator.CalculatePower(BattleStatus.Atk, mag);

        //音再生は剣のヒット時
        StartCoroutine(Coroutine.DelayCoroutine(HitFrame, () =>
        {
            PlaySound(true, HitFrame, SoundManager.Instance.Attack_Sword);
        }));

        //ダメージはモーション終わり
        StartCoroutine(Coroutine.DelayCoroutine(ActFrame, () =>
        {
            enemyBattle.Damage(power);
        }));
    }

    public override void Skill()
    {
        
    }
}