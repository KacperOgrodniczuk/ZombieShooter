using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void Wait();
    void Chase();
    void Attack();
    void Die();
}
