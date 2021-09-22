using UnityEngine;

public class BattleManager : SingletonMonoBehaviour<BattleManager>
{
    public int CalculatePower(int atk, float mag)
    {
        return (int)(atk * mag);
    }
}