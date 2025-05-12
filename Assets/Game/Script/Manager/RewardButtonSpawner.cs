using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RewardButtonSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public float spawnInterval = 10f;
    public float buttonLifetime = 15f;
    public GameObject rewardButtonPrefab;
    public Transform buttonParent;
    public List<RewardData> rewardList;

    private void Start()
    {
    }

   public IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (buttonParent.childCount < rewardList.Count)
            {

                RewardData reward = rewardList[Random.Range(0, rewardList.Count)];
                GameObject obj = Instantiate(rewardButtonPrefab, buttonParent);
               obj.GetComponent<RewardButtonController>().Setup(reward, buttonLifetime);
            }
        }
    }

}
