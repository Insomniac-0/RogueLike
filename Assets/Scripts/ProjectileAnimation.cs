using UnityEngine;

public class ProjectileAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] _frames;

    private SpriteRenderer _sprite_renderer;


    float animation_FPS;
    float timer;

    int current_index;
    int frame_count;

    void Awake()
    {
        _sprite_renderer = GetComponent<SpriteRenderer>();

        frame_count = 4;
        current_index = 0;

        animation_FPS = 4f;
        timer = 0f;


    }

    void Update()
    {
        timer += Time.deltaTime;
        current_index = (int)(timer * animation_FPS) % frame_count;
        _sprite_renderer.sprite = _frames[current_index];
    }
}
