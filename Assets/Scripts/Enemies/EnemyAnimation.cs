using GameUtilities.Sprites;
using UnityEngine;
using UnityEngine.U2D;


public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private SpriteAtlas _slime_atlas;


    private SpriteRenderer _sprite_renderer;
    private Sprite[] _walking_frames;

    float animation_FPS;
    float animation_timer;
    int current_index;
    int frame_count;


    void Awake()
    {
        frame_count = 4;
        _sprite_renderer = GetComponent<SpriteRenderer>();
        _walking_frames = SpriteHelper.LoadAnimationFrames_0(_slime_atlas, "Slime_Walk", frame_count);

        animation_FPS = 4;
        animation_timer = 0f;
        current_index = 0;
    }

    void Update()
    {
        animation_timer += Time.deltaTime;
        current_index = (int)(animation_timer * animation_FPS) % frame_count;
        _sprite_renderer.sprite = _walking_frames[current_index];
    }
}
