using UnityEngine;

// A manager class for the player, with easy access to any relevant references.
public class PlayerManager : MonoBehaviour
{
    public PlayerLocomotionManager PlayerLocomotionManager { get; private set; }
    public PlayerActionsManager PlayerActions { get; private set; }
    public SwayAndBop PlayerSwayAndBop { get; private set; }
    public PlayerSurvivalPointsManager PlayerSurvivalPointsManager { get; private set; }
    public PlayerWeaponManager PlayerWeaponManager { get; private set; }
    public PlayerAnimationManager PlayerAnimationManager { get; private set; }

    [SerializeField]
    private UIManager _UIManager;
    public UIManager UIManager { get => _UIManager; private set => _UIManager = value; }

    private void Awake()
    {
        PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        PlayerSwayAndBop = GetComponent<SwayAndBop>();
        PlayerActions = GetComponent<PlayerActionsManager>();
        PlayerSurvivalPointsManager = GetComponent<PlayerSurvivalPointsManager>();
        PlayerWeaponManager = GetComponent<PlayerWeaponManager>();
        PlayerAnimationManager = GetComponent<PlayerAnimationManager>();
    }

    private void Update()
    {
        PlayerLocomotionManager.HandleAllMovement();
        PlayerActions.HandleAllActionInput();
        PlayerSwayAndBop.HandleAllSwayAndBop();
    }

    private void LateUpdate()
    {
        PlayerCameraManager.instance.HandleAllCameraMovement();
    }
}
