using System.Collections.Generic;
using UnityEngine;

public class Mashroom : EnemyBattle
{
    [SerializeField] private const float NormalAttackMag = 1.0f;
    private const float HitFrame = 0.4f;
    private const float ActFrame = 0.6f;

    public override void Initialize()
    {
        base.Initialize();
        BattleStatus = CharaDataManager.Instance.LoadEnemyScriptableObject(BattleStatus.NAME.MASHROOM);
    }

    public override void DecideAndExcuteAction()
    {
        List<Vector3> attackPosList = AttackPosList(CharaMove.Position);
        EnemyActionAndTarget enemyActionAndTarget = base.DecideActionAndTarget(attackPosList);
        switch (enemyActionAndTarget.ACTION)
        {
            case ENEMY_ACTION.ATTACKING:
                Debug.Log("Attack");
                Attack(enemyActionAndTarget.TargetList);
                break;

            case ENEMY_ACTION.CHASING:
                Debug.Log("Chase");
                Chase(enemyActionAndTarget.TargetList);
                break;

            case ENEMY_ACTION.SEARCHING:
                Debug.Log("Search");
                Search(enemyActionAndTarget.TargetList);
                break;
        }
    }

    public override void Attack(List<GameObject> targetList)
    {
        base.Attack(targetList);

        CurrentAction = ENEMY_ACTION.ATTACKING;

        SwitchIsAttacking(ActFrame);
        PlayAnimation("IsAttacking", HitFrame);

        //ターゲットをランダムに絞って向く
        int num = Random.Range(0, targetList.Count);
        GameObject playerObject = targetList[num];
        Vector3 direction = playerObject.GetComponent<Chara>().Position - CharaMove.Position;
        CharaMove.Face(direction);

        CharaBattle playerBattle = playerObject.GetComponent<CharaBattle>();
        BattleStatus playerStatus = playerBattle.BattleStatus;

        //ヒットorノット判定
        if (Calculator.JudgeHit(BattleStatus.Dex, playerStatus.Eva) == false)
        {
            PlaySound(false, HitFrame, null);
            return;
        }

        //威力計算
        int power = Calculator.CalculatePower(BattleStatus.Atk, NormalAttackMag);

        //音再生は剣のヒット時
        StartCoroutine(Coroutine.DelayCoroutine(HitFrame, () =>
        {
            PlaySound(true, HitFrame, SoundManager.Instance.Attack_Sword);
        }));

        //ダメージはモーション終わり
        StartCoroutine(Coroutine.DelayCoroutine(ActFrame, () =>
        {
            playerObject.GetComponent<CharaBattle>().Damage(power);
        }));
        
    }

    protected override void Chase(List<GameObject> targetList)
    {
        base.Chase(targetList);

        if(CurrentAction != ENEMY_ACTION.CHASING)
        {
            int num = Random.Range(0, targetList.Count);
            TargetObject = targetList[num];
        }

        Chara player = TargetObject.GetComponent<Chara>();
        Vector3 direction = player.Position - CharaMove.Position;
        direction = Utility.Direction(direction);
        CharaMove.Move(direction);
    }

    protected override void Search(List<GameObject> targetList)
    {
        if (CurrentAction != ENEMY_ACTION.CHASING)
        {
            
        }

        return;
    }

    private List<Vector3> AttackPosList(Vector3 pos)
    {
        List<Vector3> list = new List<Vector3>();

        list.Add(pos + new Vector3(0f, 0f, 1f));
        list.Add(pos + new Vector3(1f, 0f, 1f));
        list.Add(pos + new Vector3(1f, 0f, 0f));
        list.Add(pos + new Vector3(1f, 0f, -1f));
        list.Add(pos + new Vector3(0f, 0f, -1f));
        list.Add(pos + new Vector3(-1f, 0f, -1f));
        list.Add(pos + new Vector3(-1f, 0f, 0f));
        list.Add(pos + new Vector3(-1f, 0f, 1f));

        return list;
    }
}
