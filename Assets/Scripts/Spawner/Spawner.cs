using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Transform cache_transform;
    [SerializeField] Spawn[] spawn_points;
    Player player;
    int index;
    float spawn_rate;
    float count;
    float3 point;

    void Awake()
    {
        cache_transform = transform;
        count = 0f;
        spawn_rate = 2f;
    }
    void Start()
    {
        point.z = 0;
        player = InitResources.GetPlayer;

    }
    void Update()
    {
        if (!InitResources.GetPlayer) return;
        if (count >= 1)
        {
            index = UnityEngine.Random.Range(0, 4);
            if (index % 2 == 0)
            {

                point.x = UnityEngine.Random.Range(-15f, 15f);
                point.y = spawn_points[index].GetPosition().y;
                InitResources.GetEnemyManagerBehaviour.SpawnEntity(new(point), 10, 2, 5);
            }
            else
            {
                point.y = UnityEngine.Random.Range(-10f, 10f);
                point.x = spawn_points[index].GetPosition().x;
                InitResources.GetEnemyManagerBehaviour.SpawnEntity(new(point), 10, 2, 5);

            }
            count = 0;
        }
        count += spawn_rate * Time.deltaTime;
        cache_transform.position = new float3(player.GetPosition.xy, 0f);
    }
}
