using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlyingEnemyState
{
    public abstract void Act(FlyingEnemySC enemy);
    public virtual void OnStateEnter(FlyingEnemySC enemy) { }
    public virtual void OnStateExit(FlyingEnemySC enemy) { }
}
