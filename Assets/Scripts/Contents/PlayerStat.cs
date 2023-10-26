using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] protected int _exp;
    [SerializeField] protected int _gold;
    
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;
        _defense = 5;
        _moveSpeed = 5.0f;
        _gold = 0;

        SetStat(_level);
    }

    private void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dic = Managers.Data.StatDict;
        Data.Stat stat = dic[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }

    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;
            //레벨업 체크

            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if(Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;
                if(_exp < stat.totalExp)
                    break;
                level++;
            }

            if (level != level)
            {
                Debug.Log("Level up");
                Level = level;
                SetStat(Level);
            }
        }
    }
}
