using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stats
{
    //const
    [SerializeField]
    private static float MIN_SPEED = 1f;
    private static float MIN_POWER = 0.5f;


    //visible effects - changeable
    [SerializeField]
    private int curr_health = 0;
    [SerializeField]
    private int max_health = 0;


    //the basic stats
    [SerializeField]
    private float speed = 0;
    [SerializeField]
    private float power = 0;

    public Stats(int _curr_health, int _max_health, float _speed, float _power)
    {
        curr_health = _curr_health;
        max_health = _max_health;

        speed = _speed;
        power = _power;
    }

    public Stats(float _power)
    {
        curr_health = max_health = 100;
        speed = 3;
        power = _power;
    }

    public Stats()
    {}

    public void SumStats(Stats newStat)
    {
        max_health += newStat.max_health;
        curr_health += newStat.curr_health;

        speed += newStat.speed;
        power += newStat.power;

        CheckPossibleStats();

        CheckDead();
    }

    public void ReduceStats(Stats newStat)
    {
        curr_health -= newStat.curr_health;
        max_health -= newStat.max_health;

        speed -= newStat.speed;
        power -= newStat.power;

        CheckPossibleStats();

        if (curr_health < 0)
        {
            curr_health = 0;
        }
        CheckDead();
    }

    private void CheckPossibleStats()
    {
        if (power <= MIN_POWER)
        {
            power = MIN_POWER;
        }
        if (speed <= MIN_SPEED)
        {
            speed = MIN_SPEED;
        }

        if (curr_health > max_health)
        { curr_health = max_health; }
    }

    public bool CheckDead()
    {
        if(curr_health <= 0)
        {
            Debug.Log("dead chicken");
            return true;
        }
        return false;
    }

    public float getSpeed()
    {
        return speed;
    }
      
    public float getPower()
    {
        return power;
    }

    public int Max_health { get => max_health; set => max_health = value; }
    public int Curr_health { get => curr_health; set => curr_health = value; }

    public string toString()
    {
        return "Health: " + curr_health + ", MaxHealth: " + max_health + ", Power: " + power + ", Speed: " + speed;
    }
}
