using System;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitResources : MonoBehaviour
{
    public static InitResources Instance;

    public ProjectileManager projectile_manager;
    public EntityManagerBehaviour entity_manager;



    void Awake()
    {
        InitResources.Instance = this;
        projectile_manager = GetComponent<ProjectileManager>();
        entity_manager = GetComponent<EntityManagerBehaviour>();

    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadSceneAsync(1);
    }

    public static ProjectileManager GetProjectileManager => Instance.projectile_manager;
    public static EntityManagerBehaviour GetEntityManagerBehaviour => Instance.entity_manager;
}
