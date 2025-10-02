using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class VfxAnimation : MonoBehaviour
{
    SpriteRenderer sprite_renderer;
    Transform cache_transform;

    void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        cache_transform = transform;
    }
    public void SetPosition(float3 pos) => cache_transform.position = pos;

    public void StartAnimation(Sprite[] frames, float frame_rate)
    {
        StartCoroutine(Animate(frames, frame_rate));
    }

    private IEnumerator Animate(Sprite[] frames, float frame_rate)
    {
        float delay = 1f / frame_rate;

        for (int i = 0; i < frames.Length; i++)
        {
            sprite_renderer.sprite = frames[i];
            yield return new WaitForSeconds(delay);
        }

        gameObject.SetActive(false);
        InitResources.GetVfxManager.ReturnToPool(this);
    }
}
