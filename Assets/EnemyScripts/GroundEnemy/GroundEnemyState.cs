using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundEnemyState
{
    public abstract void Act(GroundEnemySC enemy);
    public virtual void OnStateEnter(GroundEnemySC enemy) { }
    public virtual void OnStateExit(GroundEnemySC enemy) { }
}
