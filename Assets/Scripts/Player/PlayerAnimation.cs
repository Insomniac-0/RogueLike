using System;
using GameUtilities.Sprites;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteAtlas _player_atlas;
    [SerializeField] InputReader input_reader;

    // MAGE
    [SerializeField] private Sprite[] _idle_frames_S;
    [SerializeField] private Sprite[] _walk_frames_S;
    [SerializeField] private Sprite[] _attack_frames_S;

    [SerializeField] private Sprite[] _idle_frames_SW;
    [SerializeField] private Sprite[] _walk_frames_SW;
    [SerializeField] private Sprite[] _attack_frames_SW;


    [SerializeField] private Sprite[] _idle_frames_W;
    [SerializeField] private Sprite[] _walk_frames_W;
    [SerializeField] private Sprite[] _attack_frames_W;

    [SerializeField] private Sprite[] _idle_frames_NW;
    [SerializeField] private Sprite[] _walk_frames_NW;
    [SerializeField] private Sprite[] _attack_frames_NW;

    [SerializeField] private Sprite[] _idle_frames_N;
    [SerializeField] private Sprite[] _walk_frames_N;
    [SerializeField] private Sprite[] _attack_frames_N;

    [SerializeField] private Sprite[] _idle_frames_NE;
    [SerializeField] private Sprite[] _walk_frames_NE;
    [SerializeField] private Sprite[] _attack_frames_NE;

    [SerializeField] private Sprite[] _idle_frames_E;
    [SerializeField] private Sprite[] _walk_frames_E;
    [SerializeField] private Sprite[] _attack_frames_E;

    [SerializeField] private Sprite[] _idle_frames_SE;
    [SerializeField] private Sprite[] _walk_frames_SE;
    [SerializeField] private Sprite[] _attack_frames_SE;


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
        animation_FPS = 12;
        animation_timer = 0f;
        current_index = 0;



        input_reader.OnMove += UpdateDirection;
        input_reader.OnMoveStop += SetIdle;
    }

    void Update()
    {
        animation_timer += Time.deltaTime;
        switch (input_reader.current_direction)
        {
            case InputReader.PlayerMoveDirection.E:
                switch (animation_state)
                {
                    case AnimationState.IDLE:
                        current_index = (int)(animation_timer * animation_FPS) % 5;
                        _sprite_renderer.sprite = _idle_frames_E[current_index];
                        break;
                    case AnimationState.WALK:
                        current_index = (int)(animation_timer * animation_FPS) % 6;
                        _sprite_renderer.sprite = _walk_frames_E[current_index];
                        break;
                    case AnimationState.ATTACK:
                        break;
                    default:
                        break;
                }
                break;

            case InputReader.PlayerMoveDirection.NE:
                switch (animation_state)
                {
                    case AnimationState.IDLE:
                        current_index = (int)(animation_timer * animation_FPS) % 5;
                        _sprite_renderer.sprite = _idle_frames_NE[current_index];
                        break;
                    case AnimationState.WALK:
                        current_index = (int)(animation_timer * animation_FPS) % 6;
                        _sprite_renderer.sprite = _walk_frames_NE[current_index];
                        break;
                    case AnimationState.ATTACK:
                        break;
                    default:
                        break;
                }
                break;

            case InputReader.PlayerMoveDirection.N:
                switch (animation_state)
                {
                    case AnimationState.IDLE:
                        current_index = (int)(animation_timer * animation_FPS) % 5;
                        _sprite_renderer.sprite = _idle_frames_N[current_index];
                        break;
                    case AnimationState.WALK:
                        current_index = (int)(animation_timer * animation_FPS) % 6;
                        _sprite_renderer.sprite = _walk_frames_N[current_index];
                        break;
                    case AnimationState.ATTACK:
                        break;
                    default:
                        break;
                }
                break;

            case InputReader.PlayerMoveDirection.NW:
                switch (animation_state)
                {
                    case AnimationState.IDLE:
                        break;
                    case AnimationState.WALK:
                        break;
                    case AnimationState.ATTACK:
                        break;
                    default:
                        break;
                }
                break;

            case InputReader.PlayerMoveDirection.W:
                switch (animation_state)
                {
                    case AnimationState.IDLE:
                        break;
                    case AnimationState.WALK:
                        break;
                    case AnimationState.ATTACK:
                        break;
                    default:
                        break;
                }
                break;

            case InputReader.PlayerMoveDirection.SW:
                switch (animation_state)
                {
                    case AnimationState.IDLE:
                        break;
                    case AnimationState.WALK:
                        break;
                    case AnimationState.ATTACK:
                        break;
                    default:
                        break;
                }
                break;

            case InputReader.PlayerMoveDirection.S:
                switch (animation_state)
                {
                    case AnimationState.IDLE:
                        break;
                    case AnimationState.WALK:
                        break;
                    case AnimationState.ATTACK:
                        break;
                    default:
                        break;
                }
                break;

            case InputReader.PlayerMoveDirection.SE:
                switch (animation_state)
                {
                    case AnimationState.IDLE:
                        break;
                    case AnimationState.WALK:
                        break;
                    case AnimationState.ATTACK:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }


    }

    public void UpdateDirection()
    {
        animation_state = AnimationState.WALK;
    }
    public void SetIdle()
    {
        animation_state = AnimationState.IDLE;
    }
}
