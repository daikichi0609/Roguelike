using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : CharaBattle
{
    public enum ENEMY_ACTION
    {
        SEARCHING,
        CHASING,
        ATTACKING
    }

    protected ENEMY_ACTION CurrentAction
    {
        get;
        set;
    }

    protected Vector3 DestinationPos
    {
        get;
        set;
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected EnemyActionAndTarget DecideActionAndTarget(List<Vector3> attackPosList)
    {
        List<GameObject> targetList = CreateTargetList_Attack(attackPosList);
        if (targetList.Count >= 1)
        {
            return CreateActionAndTarget(ENEMY_ACTION.ATTACKING, targetList);
        }

        targetList = CreateTargetList_Chase();
        if(targetList.Count >= 1)
        {
            return CreateActionAndTarget(ENEMY_ACTION.CHASING, targetList);
        }

        targetList = CreateTargetList_Search();
        return CreateActionAndTarget(ENEMY_ACTION.SEARCHING, targetList);
    }

    private List<GameObject> CreateTargetList_Attack(List<Vector3> attackPosList)
    {
        List<GameObject> targetList = new List<GameObject>();
        foreach (Vector3 attackPos in attackPosList)
        {
            GameObject player = ObjectManager.Instance.SpecifiedPositionPlayerObject(attackPos);
            if (player != null)
            {
                targetList.Add(player);
            }
        }
        return targetList;
    }

    private List<GameObject> CreateTargetList_Chase()
    {
        List<GameObject> targetList = new List<GameObject>();
        int roomId = PositionManager.Instance.IsOnRoomID(CharaMove.Position);
        if (roomId == 0)
        {
            return targetList;
        }
        targetList = ObjectManager.Instance.SpecifiedRoomPlayerObjectList(roomId);

        return targetList;
    }

    private List<GameObject> CreateTargetList_Search()
    {
        return new List<GameObject>();
    }

    protected EnemyActionAndTarget CreateActionAndTarget(ENEMY_ACTION action, List<GameObject> targetList)
    {
        return new EnemyActionAndTarget(action, targetList);
    }

    virtual public void Attack(List<GameObject> targetList)
    {
        FinishTurn();
    }

    virtual public void Skill(List<GameObject> targetList)
    {
        FinishTurn();
    }

    virtual protected void Chase(List<GameObject> targetList)
    {
        FinishTurn();
    }

    virtual protected void Search(List<GameObject> targetList)
    {
        FinishTurn();
    }
}

public struct EnemyActionAndTarget
{
    public EnemyBattle.ENEMY_ACTION ACTION
    {
        get;
    }
    public List<GameObject> TargetList
    {
        get;
    }

    public EnemyActionAndTarget(EnemyBattle.ENEMY_ACTION action, List<GameObject> targetList)
    {
        ACTION = action;
        TargetList = targetList;
    }
}
