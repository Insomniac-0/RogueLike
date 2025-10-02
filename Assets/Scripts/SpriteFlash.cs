using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;

public class SpriteFlash : MonoBehaviour
{
    [SerializeField] Material flash_material;
    SpriteRenderer sprite_renderer;
    Material original_material;
    public Color flash_color;
    public float flash_duration;
    private Coroutine flash_routine;
    void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        original_material = sprite_renderer.material;
    }

    public void Flash()
    {
        if (flash_routine != null)
        {
            StopCoroutine(flash_routine);
        }

        flash_routine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        sprite_renderer.material = flash_material;

        yield return new WaitForSeconds(flash_duration);

        sprite_renderer.material = original_material;
        flash_routine = null;
    }
}
