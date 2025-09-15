using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField]
    private int pool_size;

    [SerializeField]
    private T prefab;

    private Queue<T> pool;

    public void Initialize(T prefab, int pool_size)
    {
        this.pool_size = pool_size;
        this.prefab = prefab;

        pool = new Queue<T>();

        for (int i = 0; i < pool_size; i++)
        {
            T obj = Instantiate(prefab);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

    }

    public T Get()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T obj = Instantiate(prefab);
            obj.gameObject.SetActive(false);
            return obj;
        }
    }
    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
