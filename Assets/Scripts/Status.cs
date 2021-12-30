using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private int life;

    [SerializeField] private int maximumLife;

    [SerializeField] private bool hasSword;

    [SerializeField] private Animator stateMachine;

    public int Life
    {
        get => life;
        set
        {
            life = Math.Clamp(value, 0, maximumLife);
            stateMachine.SetBool("isDead", IsDead);
        }
    }

    public int MaximumLife
    {
        get => maximumLife;
        set
        {
            if (value > 0)
            {
                maximumLife = value;
                life = Math.Clamp(life, 0, maximumLife);
            }
            
        }
    }

    public bool HasSword
    {
        get => hasSword;
        set
        {
            hasSword = value;
            stateMachine.SetBool("hasSword", value);
        }
    }

    public bool IsDead => (Life == 0);
}
