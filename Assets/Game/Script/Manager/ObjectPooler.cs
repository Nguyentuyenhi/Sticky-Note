using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class Pool
{
    public string name;            // Tên định danh pool
    public GameObject prefab;      // Prefab cần pool
    public int size = 10;          // Số lượng ban đầu
    public bool expandable = true; // Có thể mở rộng khi hết object
}

public class ObjectPooler : MonoBehaviour
{
    public List<Pool> pools;

    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, GameObject> prefabLookup;
    private Dictionary<string, bool> expandableLookup;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        prefabLookup = new Dictionary<string, GameObject>();
        expandableLookup = new Dictionary<string, bool>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.name = pool.name;
                obj.transform.parent = this.transform;

                // Gán component chứa tên pool
                PooledGameObject pooledObj = obj.GetComponent<PooledGameObject>();
                if (pooledObj == null)
                    pooledObj = obj.AddComponent<PooledGameObject>();
                pooledObj.poolName = pool.name;

                objectPool.Enqueue(obj);
            }

            poolDictionary[pool.name] = objectPool;
            prefabLookup[pool.name] = pool.prefab;
            expandableLookup[pool.name] = pool.expandable;
        }
    }

    public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"Pool with name {name} doesn't exist.");
            return null;
        }

        Queue<GameObject> poolQueue = poolDictionary[name];
        GameObject objectToSpawn;

        if (poolQueue.Count == 0 && expandableLookup[name])
        {
            objectToSpawn = Instantiate(prefabLookup[name]);
            objectToSpawn.name = name;

            var pooledObj = objectToSpawn.GetComponent<PooledGameObject>();
            if (pooledObj == null)
                pooledObj = objectToSpawn.AddComponent<PooledGameObject>();
            pooledObj.poolName = name;

            //objectToSpawn.transform.parent = this.transform;
        }
        else if (poolQueue.Count > 0)
        {
            objectToSpawn = poolQueue.Dequeue();
        }
        else
        {
            Debug.LogWarning($"Pool with name {name} is empty and not expandable.");
            return null;
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);

        return objectToSpawn;
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;

        var pooledObj = obj.GetComponent<PooledGameObject>();
        if (pooledObj == null)
        {
            Debug.LogWarning("Object doesn't have a PooledGameObject component!");
            return;
        }

        string name = pooledObj.poolName;

        if (poolDictionary.ContainsKey(name))
        {
            // GỌI RESET TRƯỚC KHI TẮT
            IPoolable poolable = obj.GetComponent<IPoolable>();
            obj.transform.SetParent(this.transform, false);

            //poolable?.OnReturnedToPool();

            obj.SetActive(false);
            poolDictionary[name].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning($"Không thấy pool với tên: {name}");
        }
    }


}
