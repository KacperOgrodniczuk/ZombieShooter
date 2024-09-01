using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void WaitState();
    void ChaseState();
    void AttackState();
    void DieState();
}
