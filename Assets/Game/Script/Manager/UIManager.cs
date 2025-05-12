using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private GameObject SkillUI;
    public GameObject NextLevelPanel;
    private void Start()
    {
        StartReset();   
    }
    public int coin { get;  set; }
    public void StartReset()
    {
        GameManager.Instance.stickyNoteManager.nextLV += CoinReset;
    }
    public void CoinReset()
    {
        coin = 0;
        UpdateCoinText(coin);
    }

    public void UpdateCoinText(int coin)
    {
        coinText.text = "Coin: " + coin;
    }

    public void OnClick()
    {
        Invoke(nameof(StartTearing), 0.1f);
    }

    private void StartTearing()
    {
        StartCoroutine(GameManager.Instance.rewardButtonSpawner.SpawnRoutine());
        GameManager.Instance.ChangeState(GameState.Tearing);
        SkillUI.SetActive(true);
        gameObject.transform.GetComponentInChildren<Button>().gameObject.SetActive(false);
    }
    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateCoinText(coin);
    }
    public void SpendCoin(int amount)
    {
        coin -= amount;
        UpdateCoinText(coin);
    }
    



}
