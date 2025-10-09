using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct WaveEnemy
{
    public EnemyDataSO data;
    public int count;
}


[Serializable]
public struct Wave
{
    public WaveEnemy[] enemies;
    public float spawn_rate;
}

public class Spawner : MonoBehaviour
{

    Transform cache_transform;
    [SerializeField] Spawn[] spawn_points;
    [SerializeField] EnemyDataSO[] enemy_data;
    [SerializeField] List<Wave> waves;


    Player player;
    int index;
    float spawn_rate;
    float count;
    float3 point;
    int max_enemies;
    EnemyType type;

    const float BaseSpawnRate = 2f;

    void Awake()
    {
        InitResources.GetNullableObjects.AssignSpawner(this);
        cache_transform = transform;
        count = 0f;
        spawn_rate = BaseSpawnRate;
        max_enemies = 250;
    }
    void Start()
    {
        InitResources.GetEventChannel.OnLvlUp += UpdateSpawnRate;
        point.z = 0;
        player = InitResources.GetPlayer;


    }

    private void UpdateSpawnRate()
    {
        spawn_rate = BaseSpawnRate + InitResources.GetUpgradeSystem.GetPlayerLvl * 1.1f;
    }

    public void UpdateSpawner()
    {
        if (!InitResources.GetPlayer) return;
        if (count >= 1 && InitResources.GetEnemyManagerBehaviour.GetEnemyCount < max_enemies)
        {
            type = UnityEngine.Random.Range(0f, 1f) < 0.7f ? EnemyType.SKULL : EnemyType.BAT;

            index = UnityEngine.Random.Range(0, 4);
            if (index % 2 == 0)
            {

                point.x = UnityEngine.Random.Range(-15f, 15f);
                point.y = spawn_points[index].GetPosition.y;
                InitResources.GetEnemyManagerBehaviour.SpawnEntity(enemy_data[(int)type], new TransformData(point), type);
            }
            else
            {
                point.y = UnityEngine.Random.Range(-10f, 10f);
                point.x = spawn_points[index].GetPosition.x;
                InitResources.GetEnemyManagerBehaviour.SpawnEntity(enemy_data[(int)type], new TransformData(point), type);

            }
            count = 0;
        }
        count += spawn_rate * Time.deltaTime;
        cache_transform.position = new float3(InitResources.GetPlayer.GetPosition.xy, 0f);
    }

}
