using UnityEngine;

// A manager class for the player, with easy access to any relevant references.
public class PlayerManager : MonoBehaviour
{
    public Animator PlayerAnimator { get; private set; }
    public PlayerLocomotionManager PlayerLocomotionManager { get; private set; }
    public PlayerActions PlayerActions { get; private set; }
    public SwayAndBop PlayerSwayAndBop { get; private set; }
    public PlayerSurvivalPointsManager PlayerSurvivalPointsManager { get; private set; }

    [SerializeField]
    private UIManager _UIManager;
    public UIManager UIManager { get => _UIManager; private set => _UIManager = value; }

    private void Awake()
    {
        PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        PlayerSwayAndBop = GetComponent<SwayAndBop>();
        PlayerActions = GetComponent<PlayerActions>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        PlayerSurvivalPointsManager = GetComponent<PlayerSurvivalPointsManager>();
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
