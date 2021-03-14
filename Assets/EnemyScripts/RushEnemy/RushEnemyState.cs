using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RushEnemyState
{
    public abstract void Act(RushEnemySC enemy);
    public virtual void OnStateEnter(RushEnemySC enemy) { }
    public virtual void OnStateExit(RushEnemySC enemy) { }
}
