using UnityEngine;
using UnityEngine.U2D;
namespace GameUtilities.Sprites
{
    public static class SpriteHelper
    {
        public static Sprite[] LoadAnimationFrames(SpriteAtlas atlas, string base_name, int frame_count)
        {
            Sprite[] frames = new Sprite[frame_count];
            for (int i = 0; i < frame_count; i++)
            {
                string spritename = $"{base_name}_{i}_0";
                frames[i] = atlas.GetSprite(spritename);
            }
            return frames;
        }

        public static Sprite[] LoadAnimationFrames_0(SpriteAtlas atlas, string base_name, int frame_count)
        {
            Sprite[] frames = new Sprite[frame_count];
            frames[0] = atlas.GetSprite("Slime_Walk_0_0");
            for (int i = 1; i < frame_count; i++)
            {
                string spritename = $"{base_name}_{i}_1";
                frames[i] = atlas.GetSprite(spritename);
            }
            return frames;
        }
    }
}