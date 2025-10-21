using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;




[BurstCompile]
public unsafe class ProjectileManager : MonoBehaviour
{

    [BurstCompile]
    [MonoPInvokeCallback(typeof(ProcessFloatsDelegate))]
    public static float AddFloats(float a, float b) => a + b;

    [BurstCompile]
    [MonoPInvokeCallback(typeof(ProcessFloatsDelegate))]
    public static float MultiplyFloats(float a, float b) => a * b;

    public delegate float ProcessFloatsDelegate(float a, float b);

    [SerializeField] Projectile projectile_ref;
    [SerializeField] private List<ProjectileDataSO> _projectile_templates;

    private bool _initialized;


    Player player;

    float delta_time;
    float[] randoms;

    float3 mouse_pos;

    const int InitialAllocSize = 100;
    const int ProjectileJobBatchSize = 16;


    private bool disposed;

    // ARRAYS

    NativeList<ProjectileData> projectiles;

    List<Projectile> projectile_objects;
    List<Projectile> projectile_pool;


    // Function Pointers
    //FunctionPointer<FunnyFunction>;


    FunctionPointer<ProcessFloatsDelegate> MultFuncPtr;
    FunctionPointer<ProcessFloatsDelegate> AddFuncPtr;
    void Start()
    {
        mouse_pos = InitResources.GetInputReader.GetMousePositionWS();
        MultFuncPtr = BurstCompiler.CompileFunctionPointer<ProcessFloatsDelegate>(MultiplyFloats);
        AddFuncPtr = BurstCompiler.CompileFunctionPointer<ProcessFloatsDelegate>(AddFloats);
        randoms = new float[2];
    }

    void OnDestroy()
    {
        if (!disposed) projectiles.Dispose();
    }


    public void Init()
    {
        projectiles = new NativeList<ProjectileData>(InitialAllocSize, Allocator.Persistent);
        projectile_objects = new List<Projectile>(InitialAllocSize);
        projectile_pool = new List<Projectile>(InitialAllocSize);
        disposed = false;
    }
    public void HardCleanUp()
    {
        projectiles.Dispose();
        projectile_objects.Clear();
        projectile_pool.Clear();
        disposed = true;
    }

    public void SoftCleanUp()
    {
        for (int i = 0; i < projectile_objects.Count; i++)
        {
            Destroy(projectile_objects[i].gameObject);
        }
        projectiles.Clear();
        projectile_objects.Clear();
        projectile_pool.Clear();
    }


    public void FixedProjectileUpdate()
    {

        delta_time = Time.deltaTime;
        randoms[0] = UnityEngine.Random.Range(-1f, 1f);
        randoms[1] = UnityEngine.Random.Range(-1f, 1f);

        ProjectileMovementJob projectile_job = new ProjectileMovementJob
        {
            AddFuncPtr = AddFuncPtr,
            MultFuncPtr = MultFuncPtr,
            projectiles = projectiles.AsDeferredJobArray(),
            delta_time = delta_time,
            x_rand = randoms[0],
            y_rand = randoms[1],
        };

        JobHandle handle = projectile_job.Schedule(projectile_objects.Count, ProjectileJobBatchSize);

        handle.Complete();

        UpdateProjectiles();
    }

    [BurstCompile]
    struct ProjectileMovementJob : IJobParallelFor
    {
        public FunctionPointer<ProcessFloatsDelegate> AddFuncPtr;
        public FunctionPointer<ProcessFloatsDelegate> MultFuncPtr;
        public NativeArray<ProjectileData> projectiles;
        public float delta_time;
        public float x_rand;
        public float y_rand;

        public void Execute(int index)
        {
            ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[index];
            if (!ptr->active) return;
            ptr->speed = AddFuncPtr.Invoke(ptr->speed, -1f);
            ptr->transform.position += ptr->direction * ptr->speed * delta_time;
        }
    }

    public void TakeDMG(int ID)
    {
        ProjectileData* ptr = &((ProjectileData*)projectiles.GetUnsafePtr())[ID];
        ptr->HP--;
    }

    public void SpawnProjectile(ProjectileDataSO data, TransformData src, float3 direction, float dmg_multiply = 1.0f)
    {

        int newID = projectile_objects.Count;

        if (projectile_pool.Count == 0)
        {


            // Objects
            Projectile p = Instantiate(projectile_ref);

            p.ID = newID;
            p.SetPosition(src.position);
            p.SetRotation(src.rotation);
            p.collided = false;
            p._prevID = -1;
            p.DMG = data.BaseDMG * dmg_multiply;
            p.gameObject.SetActive(true);
            projectile_objects.Add(p);

            projectiles.Add(new ProjectileData(data, src, direction, newID));
        }
        else
        {


            // Object

            Projectile p = projectile_pool[0];
            projectile_pool.RemoveAtSwapBack(0);


            p.ID = newID;
            p.SetPosition(src.position);
            p.SetRotation(src.rotation);
            p.collided = false;
            p._prevID = -1;
            p.DMG = data.BaseDMG * dmg_multiply;
            p.gameObject.SetActive(true);
            projectile_objects.Add(p);

            projectiles.Add(new ProjectileData(data, src, direction, newID));
        }
    }

    void UpdateProjectiles()
    {
        if (projectile_objects.Count <= 0) return;
        for (int i = projectile_objects.Count - 1; i >= 0; i--)
        {
            ProjectileData* ptr = GetProjectilePtr(i);
            ProjectileData p = projectiles[i];
            if (!ptr->active) continue;

            if (ptr->lifetime > 0f && ptr->HP > 0f)
            {
                projectile_objects[i].SetPosition(ptr->transform.position);
                ptr->lifetime -= delta_time;
            }
            else
            {
                int last_index = projectile_objects.Count - 1;
                ptr->active = false;

                projectile_objects[i].gameObject.SetActive(false);
                projectile_pool.Add(projectile_objects[i]);

                projectile_objects[i] = projectile_objects[last_index];
                projectile_objects.RemoveAt(last_index);
                if (last_index < i) projectile_objects[i].ID = i;


                *ptr = projectiles[last_index];
                ProjectileData pr = projectiles[last_index];
                pr.active = false;
                projectiles.RemoveAt(last_index);
                ptr->ID = i;
                //if (last_index < i) projectiles[i] = p;
            }
        }
    }

    // HELPERS
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ProjectileData* GetProjectilePtr(int i) => &((ProjectileData*)projectiles.GetUnsafePtr())[i];
}


