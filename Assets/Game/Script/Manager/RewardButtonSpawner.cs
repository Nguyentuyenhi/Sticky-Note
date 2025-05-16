using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class RewardButtonSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public float spawnInterval = 10f;
    public float buttonLifetime = 15f;
    public List<GameObject> rewardButtonPrefab;
    public Transform buttonParent;
    public List<RewardData> rewardList;
    private Coroutine spawnCoroutine;
    private void Start()
    {
    }

   public IEnumerator SpawnRoutine()
    {
       // buttonParent.gameObject.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (buttonParent.childCount < rewardList.Count)
            {
                GameObject obj;

                RewardData reward = rewardList[Random.Range(0, rewardList.Count)];
                if(reward.rewardName == "+3 Income")
                {
                     obj = GameManager.Instance.objectPooler.SpawnFromPool("IncomeRewardBtnn", buttonParent.transform.position, buttonParent.transform.rotation);

                }
                else if (reward.rewardName == "+5 Hands")
                {
                    obj = GameManager.Instance.objectPooler.SpawnFromPool("HandsRewardBtn", buttonParent.transform.position, buttonParent.transform.rotation);
                }
                else
                {
                    obj = GameManager.Instance.objectPooler.SpawnFromPool("HandsRewardBtn", buttonParent.transform.position, buttonParent.transform.rotation);
                }
                obj.transform.parent = buttonParent.transform;
                //GameObject obj = Instantiate(rewardButtonPrefab, buttonParent);
                obj.GetComponent<RewardButtonController>().Setup(reward, buttonLifetime);

            }
        }
    }
    public void SpawnStop()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = null;

        foreach (Transform child in buttonParent)
        {
            RewardButtonController controller = child.GetComponent<RewardButtonController>();
            if (controller != null)
                controller.ResetReward();
        }

        buttonParent.gameObject.SetActive(false);
    }

    public void StartSpawn()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnRoutine());
        buttonParent.gameObject.SetActive(true);
    }

}
