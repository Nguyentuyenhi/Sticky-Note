using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private GameObject SkillUI;
    public GameObject NextLevelPanel;
    public Coroutine autoAddCoinCoroutine;
    public float coin { get; set; }
    private IEnumerator AutoAddCoin()
    {
        while (true)
        {
            AddCoin(GameManager.Instance.incomePerNote);
            yield return new WaitForSeconds(1f);
        }
    }
    public void UpdateCoinValue()
    {
        GameManager.Instance.incomePerNote = (1 +  GameManager.Instance.skillManager.incomeSkill.level + GameManager.Instance.skillManager.incomeSkill.temporaryLevel) ;
        AddCoin(GameManager.Instance.incomePerNote);
    }

    private void Start()
    {
        StartReset();   
    }
    public void StartReset()
    {
        GameManager.Instance.stickyNoteManager.nextLV += CoinReset;
    }
    public void CoinReset()
    {
        StopCoroutine(AutoAddCoin());
        coin = 0;
        UpdateCoinText(coin);
        autoAddCoinCoroutine = null;
        GameManager.Instance.incomePerNote = 0;
    }

    public void UpdateCoinText(float coin)
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
    public void AddCoin(float amount)
    {
        coin += amount;
        UpdateCoinText(coin);
    }
    public void StartCoin()
    {
        UpdateCoinValue();
        autoAddCoinCoroutine = StartCoroutine(AutoAddCoin());
    }
    public void SpendCoin(float amount)
    {
        coin -= amount;
        UpdateCoinText(coin);

    }
    



}
