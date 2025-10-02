using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    [SerializeField] VfxAnimation animator_ref;

    const int InitialAllocSize = 50;

    List<VfxAnimation> animator_pool;

    void Awake()
    {
        animator_pool = new List<VfxAnimation>(InitialAllocSize);
    }

    public void SpawnAnimation(float3 position, Sprite[] frames, float frame_rate)
    {
        if (animator_pool.Count == 0)
        {
            VfxAnimation vfx = Instantiate(animator_ref);
            vfx.SetPosition(position);
            vfx.gameObject.SetActive(true);
            vfx.StartAnimation(frames, frame_rate);
        }
        else
        {
            VfxAnimation vfx = animator_pool[0];
            animator_pool.RemoveAtSwapBack(0);
            vfx.SetPosition(position);
            vfx.gameObject.SetActive(true);
            vfx.StartAnimation(frames, frame_rate);
        }
    }

    public void ReturnToPool(VfxAnimation vfx)
    {
        animator_pool.Add(vfx);
    }
}
