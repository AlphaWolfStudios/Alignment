﻿using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Inventory))]
public class Entity : MonoBehaviour, IDamageable
{
    public Action<DamageInfo> OnEntityTakeDamage;
    public Action<DamageInfo> OnDeath;
    public Action OnUpdateStats;
    public Action OnStatsLoaded;

    [SerializeField] AttackComponent attackComponent = null;

    [HideInInspector] public CharacterStats entityStats;
    [HideInInspector] public float multiplyRun = 2;
    
    private Inventory inventory;

    private bool isAlive = true;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        entityStats = GetComponent<CharacterStats>();
        OnStatsLoaded?.Invoke();
    }
    private void AddCurrentStat(Stat stat, float value)
    {
        stat.AddCurrent(value);
        OnUpdateStats?.Invoke();
    }
    public void RefreshAttackStats(ref DamageInfo info)
    {
        if (attackComponent != null)
        {
            attackComponent.SetAttackStrenght(entityStats.GetStat(StatType.Damage).GetCurrent());
            attackComponent.SetAttackSpeed(entityStats.GetStat(StatType.AttackSpeed).GetCurrent());
            info.amount = entityStats.GetStat(StatType.Damage).GetCurrent();
        }
    }
    public void TakeDamage(DamageInfo damageInfo) 
    {
        if (!isAlive) return;
        OnEntityTakeDamage?.Invoke(damageInfo);
        Stat statToChange;
        if (damageInfo.type == DamageType.Armor)
        {
            damageInfo.amount -= entityStats.GetStat(StatType.Defense).GetCurrent();
            statToChange = entityStats.GetStat(StatType.Armor);
        }
        else// if (info.type == DamageType.Energy)
        {
            statToChange = entityStats.GetStat(StatType.Energy);
        }

        if (damageInfo.amount > 0)
        {
            AddCurrentStat(statToChange, -damageInfo.amount);
            if (statToChange.GetCurrent() < Mathf.Epsilon)
            {
                inventory.BlowUpInventory();
                OnDeath?.Invoke(damageInfo);
                isAlive = false;
            }
        }
        OnUpdateStats?.Invoke();
    }
}