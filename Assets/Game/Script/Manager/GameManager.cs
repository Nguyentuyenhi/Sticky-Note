using UnityEngine.InputSystem.XR;
using UnityEngine;

public enum GameState { Boot, Setup, Tearing, Playing }

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public int incomePerNote = 1;
    public GameState CurrentState { get; private set; }

    public StickyNoteManager stickyNoteManager;
    public ArmController armController;
    public UIManager uiManager;
    public SkillManager skillManager;
    public RewardManager rewardManager;
    public RewardButtonSpawner rewardButtonSpawner;
    public PaperCutter paperCutter;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        ChangeState(GameState.Setup);
    }


    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case GameState.Setup:
                stickyNoteManager.StartLevel(stickyNoteManager.currentLevel);
                break;
            case GameState.Tearing:
                armController.StartTearingSequence(() => ChangeState(GameState.Playing));
                break;
            case GameState.Playing:
                // B?t đ?u gameplay chính
                break;
        }
    }
    
}
