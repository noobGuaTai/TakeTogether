using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttribute : PlayerAttribute
{

    void Awake()
    {
        HP = 100f;
        ATK = 10f;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

    }

    
    void Update()
    {
        
    }

    public override void ChangeHP(float value)
    {
        HP += value;
    }
}
