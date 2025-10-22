using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameUtilities.Sprites;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;


    private SpriteRenderer _sprite_renderer;



    private int _sprite_count;
    float animation_FPS;
    float animation_timer;
    int current_index;

    void Awake()
    {
        _sprite_renderer = GetComponent<SpriteRenderer>();
        animation_FPS = 6;
        animation_timer = 0f;
        current_index = 0;
    }
    void Start()
    {
        _sprite_count = sprites.Count;
    }

    public void AnimationUpdate()
    {
        animation_timer += Time.deltaTime;
        current_index = (int)(animation_timer * animation_FPS) % _sprite_count;
        _sprite_renderer.sprite = sprites[current_index];
    }
}
