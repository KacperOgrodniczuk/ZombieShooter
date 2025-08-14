using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [HideInInspector] public PlayerManager PlayerManager;

    public Animator Animator { get; private set; }

    private void Awake()
    {
        PlayerManager = GetComponent<PlayerManager>();
        Animator = GetComponentInChildren<Animator>();
    }

    public void PlayTargetAnimation(string targetAnimation, bool ispPerformingAction)
    {
        Animator.CrossFade(targetAnimation, 0.15f);
        PlayerManager.PlayerGameplayActionManager.isPerformingAction = ispPerformingAction;
    }
}
