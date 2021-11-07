using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : CharaBattle
{
    public enum ENEMY_STATE
    {
        NONE,
        SEARCHING,
        CHASING,
        ATTACKING
    }

    protected ENEMY_STATE CurrentState
    {
        get;
        set;
    }

    [SerializeField] protected GameObject m_TargetObject;
    protected GameObject TargetObject
    {
        get { return m_TargetObject; }
        set { m_TargetObject = value; }
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void Death()
    {
        int num = ObjectManager.Instance.EnemyList.IndexOf(ObjectManager.Instance.SpecifiedPositionEnemyObject(CharaMove.Position));
        ObjectManager.Instance.EnemyList.RemoveAt(num);
        ObjectPool.Instance.SetObject(BattleStatus.Name.ToString(), gameObject);
    }

    public void DecideAndExcuteAction()
    {
        List<Vector3> attackPosList = AttackPosList(CharaMove.Position);
        EnemyActionAndTarget enemyActionAndTarget = Utility.DecideActionAndTarget(attackPosList, CharaMove.Position);
        switch (enemyActionAndTarget.NextState)
        {
            case ENEMY_STATE.ATTACKING:
                if(TurnManager.Instance.IsActing == true)
                {
                    return;
                }
                Face(enemyActionAndTarget.TargetList);
                NormalAttack();
                break;

            case ENEMY_STATE.CHASING:
                Chase(enemyActionAndTarget.TargetList);
                CurrentState = ENEMY_STATE.CHASING;
                FinishTurn(); //暫定処置
                break;

            case ENEMY_STATE.SEARCHING:
                Search();
                CurrentState = ENEMY_STATE.SEARCHING;
                FinishTurn();
                break;
        }
    }

    protected override void NormalAttack()
    {
        CurrentState = ENEMY_STATE.ATTACKING;
        base.NormalAttack();
    }

    protected void Face(List<GameObject> targetList)
    {
        //ターゲットをランダムに絞って向く
        Utility.Shuffle<GameObject>(targetList);
        if (AttackInfo.IsPossibleToDiagonal == true)
        {
            Vector3 direction = targetList[0].GetComponent<Chara>().Position - CharaMove.Position;
            CharaMove.Face(direction);
            return;
        }

        foreach (GameObject gameObject in targetList)
        {
            Vector3 direction = gameObject.GetComponent<Chara>().Position - CharaMove.Position;
            if(DungeonTerrain.Instance.IsPossibleToMoveDiagonal((int)CharaMove.Position.x, (int)CharaMove.Position.z, (int)direction.x, (int)direction.z) == true)
            {
                CharaMove.Face(direction);
                return;
            }
        }

        CompromiseMove(targetList[0].GetComponent<Chara>().Position);
    }

    protected virtual void Skill(List<GameObject> targetList)
    {
        CurrentState = ENEMY_STATE.ATTACKING;
    }

    protected void CompromiseMove(Vector3 targetPos)
    {
        Vector3 direction = targetPos - CharaMove.Position;
        direction = Utility.Direction(direction);
        if(direction.x != 0 && direction.z != 0)
        {
            
        }

        if(direction.x == 0)
        {
            
        }

        FinishTurn();
    }

    protected virtual void Chase(List<GameObject> targetList)
    {
        if (CurrentState != ENEMY_STATE.CHASING)
        {
            int num = Random.Range(0, targetList.Count);
            TargetObject = targetList[num];
        }

        Chara player = TargetObject.GetComponent<Chara>();
        Vector3 direction = player.Position - CharaMove.Position;
        direction = Utility.Direction(direction);
        if (CharaMove.Move(direction) == false)
        {
            //移動できなかった場合の処理
            CompromiseMove(player.Position);
        }
    }

    protected virtual void Search()
    {
        int currentRoomId = Positional.IsOnRoomID(CharaMove.Position);
        //通路にいる場合
        if (currentRoomId == 0)
        {
            AroundGridID aroundGridID = DungeonTerrain.Instance.CreateAroundGrid((int)CharaMove.Position.x, (int)CharaMove.Position.z);
            Vector3 myDirection = CharaMove.Direction;
            Vector3 oppositeDirection = CharaMove.Direction * -1;
            List<Vector3> directionList = new List<Vector3>();
            if (aroundGridID.UpGrid > (int)DungeonTerrain.GRID_ID.WALL)
            {
                Vector3 upDirection = new Vector3(0f, 0f, 1f);
                if (myDirection != upDirection * -1)
                {
                    directionList.Add(upDirection);
                }
            }
            if (aroundGridID.UnderGrid > (int)DungeonTerrain.GRID_ID.WALL)
            {
                Vector3 downDirection = new Vector3(0f, 0f, -1f);
                if (myDirection != downDirection * -1)
                {
                    directionList.Add(downDirection);
                }
            }
            if (aroundGridID.LeftGrid > (int)DungeonTerrain.GRID_ID.WALL)
            {
                Vector3 leftDirection = new Vector3(-1f, 0f, 0f);
                if (myDirection != leftDirection * -1)
                {
                    directionList.Add(leftDirection);
                }
            }
            if (aroundGridID.RightGrid > (int)DungeonTerrain.GRID_ID.WALL)
            {
                Vector3 rightDirection = new Vector3(1f, 0f, 0f);
                if (myDirection != rightDirection * -1)
                {
                    directionList.Add(rightDirection);
                }
            }

            int num = Random.Range(0, directionList.Count);
            if(CharaMove.Move(directionList[num]) == false)
            {
                CharaMove.Move(oppositeDirection);
            }
            return;
        }

        //新しくSEARCHINGステートになった場合、目標となる部屋の入り口を設定する
        if (CurrentState != ENEMY_STATE.SEARCHING) 
        {
            List<GameObject> gateWayObjectList = ObjectManager.Instance.GateWayObjectList(Positional.IsOnRoomID(CharaMove.Position));
            int num = Random.Range(0, gateWayObjectList.Count);
            TargetObject = gateWayObjectList[num];
        }

        //入り口についた場合、部屋を出る
        if(CharaMove.Position.x == TargetObject.transform.position.x && CharaMove.Position.z == TargetObject.transform.position.z)
        {
            Debug.Log("部屋を出る");
            AroundGridID aroundGridID = DungeonTerrain.Instance.CreateAroundGrid((int)CharaMove.Position.x, (int)CharaMove.Position.z);
            if(aroundGridID.UpGrid == (int)DungeonTerrain.GRID_ID.PATH_WAY)
            {
                CharaMove.Move(new Vector3(0f, 0f, 1f));
                return;
            }
            if (aroundGridID.UnderGrid == (int)DungeonTerrain.GRID_ID.PATH_WAY)
            {
                CharaMove.Move(new Vector3(0f, 0f, -1f));
                return;
            }
            if (aroundGridID.LeftGrid == (int)DungeonTerrain.GRID_ID.PATH_WAY)
            {
                CharaMove.Move(new Vector3(-1f, 0f, 0f));
                return;
            }
            if (aroundGridID.RightGrid == (int)DungeonTerrain.GRID_ID.PATH_WAY)
            {
                CharaMove.Move(new Vector3(1f, 0f, 0f));
                return;
            }
            return;
        }

        //通路出入り口へ向かう
        Vector3 direction = TargetObject.transform.position - CharaMove.Position;
        direction = Utility.Direction(direction);
        if (CharaMove.Move(direction) == false)
        {
            //移動できなかった場合の処理
            CompromiseMove(TargetObject.transform.position);
        }
    }

    protected virtual List<Vector3> AttackPosList(Vector3 pos)
    {
        return null;
    }
}

public struct EnemyActionAndTarget
{
    public EnemyAI.ENEMY_STATE NextState
    {
        get; set;
    }
    public List<GameObject> TargetList
    {
        get;
    }

    public EnemyActionAndTarget(EnemyAI.ENEMY_STATE state, List<GameObject> targetList)
    {
        NextState = state;
        TargetList = targetList;
    }
}