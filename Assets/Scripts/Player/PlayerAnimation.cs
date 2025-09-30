using System;
using System.Threading;
using GameUtilities.Sprites;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteAtlas _player_atlas;
    [SerializeField] Sprite[] sprites;


    private SpriteRenderer _sprite_renderer;


    AnimationState animation_state;
    float animation_FPS;
    float animation_timer;
    int current_index;
    int frame_count;

    void Awake()
    {
        frame_count = 4;
        _sprite_renderer = GetComponent<SpriteRenderer>();

        animation_FPS = 6;
        animation_timer = 0f;
        current_index = 0;




    }
    void Start()
    {

    }
    void Update()
    {
        animation_timer += Time.deltaTime;
        current_index = (int)(animation_timer * animation_FPS) % 2;
        _sprite_renderer.sprite = sprites[current_index];
    }

}
