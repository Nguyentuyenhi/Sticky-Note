using UnityEngine;
using UnityEngine.UI;

public class NextLvUI : MonoBehaviour
{
    public Button nextLVBtn;
    private void Start()
    {
    }
    public void OnClickNextLvBtn()
    {
        GameManager.Instance.stickyNoteManager.NextLevel();
        GameManager.Instance.ChangeState(GameState.Tearing);
        GameManager.Instance.uiManager.NextLevelPanel.SetActive(false);
    }

}
