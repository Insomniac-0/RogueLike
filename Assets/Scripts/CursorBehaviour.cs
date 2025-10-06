using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    [SerializeField] Sprite[] cursor_sprites;

    Transform cache_transform;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        cache_transform = transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        spriteRenderer.sprite = cursor_sprites[1];
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        cache_transform.position = new float3(InitResources.GetInputReader.GetMousePositionWS().xy, 0f);
    }

    public void SelectCursor(in int index)
    {
        spriteRenderer.sprite = cursor_sprites[index];
    }
}
