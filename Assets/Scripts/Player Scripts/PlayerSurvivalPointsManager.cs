using System;
using UnityEngine;

public class PlayerSurvivalPointsManager : MonoBehaviour
{
    [SerializeField] 
    UIManager UImanager;

    int survivalPoints = 0;

    public event Action<int> OnSurvivalPointsChange;

}
