using UnityEngine;
using UnityEngine.Animations.Rigging;

// A manager class for the player, with easy access to any relevant references.
public class PlayerManager : MonoBehaviour
{
    public PlayerLocomotionManager PlayerLocomotionManager { get; private set; }
    public PlayerGameplayActionsManager PlayerGameplayActionManager { get; private set; }
    public SwayAndBop PlayerSwayAndBopManager { get; private set; }
    public PlayerSurvivalPointsManager PlayerSurvivalPointsManager { get; private set; }
    public PlayerWeaponManager PlayerWeaponManager { get; private set; }
    public PlayerAnimationManager PlayerAnimationManager { get; private set; }
    public RigBuilder RigBuilder { get; private set; }

    [SerializeField]
    private UIManager _UIManager;
    public UIManager UIManager { get => _UIManager; private set => _UIManager = value; }

    private void Awake()
    {
        PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        PlayerSwayAndBopManager = GetComponent<SwayAndBop>();
        PlayerGameplayActionManager = GetComponent<PlayerGameplayActionsManager>();
        PlayerSurvivalPointsManager = GetComponent<PlayerSurvivalPointsManager>();
        PlayerWeaponManager = GetComponent<PlayerWeaponManager>();
        PlayerAnimationManager = GetComponent<PlayerAnimationManager>();
        RigBuilder = GetComponentInChildren<RigBuilder>();
    }

    private void Update()
    {
        PlayerLocomotionManager.HandleAllMovement();
        PlayerGameplayActionManager.HandleAllActionInput();
        PlayerSwayAndBopManager.HandleAllSwayAndBop();
    }

    private void LateUpdate()
    {
        RigBuilder.Build();
        PlayerCameraManager.instance.HandleAllCameraMovement(PlayerWeaponManager.activeGun.shootConfig.recoilRecoveryTime);
    }
}
