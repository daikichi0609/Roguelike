using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utility
{
    public static Vector3 Direction(Vector3 direction)
    {
        int x = (int)direction.x;
        int z = (int)direction.z;

        if (x < 0)
        {
            x = -1;
        }
        else if (x > 0)
        {
            x = 1;
        }

        if (z < 0)
        {
            z = -1;
        }
        else if (z > 0)
        {
            z = 1;
        }

        return new Vector3(x, 0, z);
    }
}

public class Calculator
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

    public static int CalculatePower(int atk, float mag)
    {
        return (int)(atk * mag);
    }

    public static int CalculateDamage(int power, int def)
    {
        int damage = power - def;
        if (damage < 1)
        {
            damage = 1;
        }
        return damage;
    }

    public static int CalculateRemainingHp(int hp, int damage)
    {
        int remainingHp = hp - damage;
        if (remainingHp < 0)
        {
            remainingHp = 0;
        }
        return remainingHp;
    }

    public static bool JudgeHit(float dex, float eva)
    {
        if(UnityEngine.Random.Range(1,101) >= dex * 100)
        {
            Debug.Log("外す");
            return false;
        }
        if(UnityEngine.Random.Range(1, 101) <= eva * 100)
        {
            Debug.Log("回避");
            return false;
        }
        return true;
    }
}

public class Coroutine
{
    public static IEnumerator DelayCoroutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}