using System;
using GameUtilities.Sprites;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteAtlas _player_atlas;
    [SerializeField] InputBehaviour input_behaviour;

    private SpriteRenderer _sprite_renderer;
    private Sprite[] _idle_frames;
    private Sprite[] _walking_frames;



    enum AnimationState
    {
        IDLE,
        WALK,
        ATTACK,
    }

    AnimationState animation_state;
    float animation_FPS;
    float animation_timer;
    int current_index;
    int frame_count;

    void Awake()
    {
        frame_count = 4;
        _sprite_renderer = GetComponent<SpriteRenderer>();
        _idle_frames = SpriteHelper.LoadAnimationFrames(_player_atlas, "Player_Idle_Scepter_Defence0", frame_count);
        _walking_frames = SpriteHelper.LoadAnimationFrames(_player_atlas, "Player_Walk_Scepter_Defence0", frame_count);

        animation_state = AnimationState.IDLE;
        animation_FPS = 4;
        animation_timer = 0f;
        current_index = 0;



        input_behaviour.OnMove += UpdateDirection;
        input_behaviour.OnMoveStop += SetIdle;
    }

    void Update()
    {
        animation_timer += Time.deltaTime;
        switch (animation_state)
        {
            case AnimationState.IDLE:
                current_index = (int)(animation_timer * animation_FPS) % frame_count;
                _sprite_renderer.sprite = _idle_frames[current_index];
                break;
            case AnimationState.WALK:
                current_index = (int)(animation_timer * animation_FPS) % frame_count;
                _sprite_renderer.sprite = _walking_frames[current_index];
                break;

            default:
                break;
        }
    }

    public void UpdateDirection()
    {
        animation_state = AnimationState.WALK;
        _sprite_renderer.flipX = input_behaviour.flip;
    }
    public void SetIdle()
    {
        animation_state = AnimationState.IDLE;
    }
}
