using N8Engine.Mathematics;
using N8Engine.Rendering;

namespace SampleProject
{
    public class PlayerJumpAnimation : Animation
    {
        protected override Sprite[] Frames => new Sprite[]
        {
            new(SpritesFolder.Path + "player_5.n8sprite", new Vector(0f, -3f), ShouldFlipHorizontally)
        };
        protected override float TimeBetweenFrames => 0f;
        
        protected virtual bool ShouldFlipHorizontally => false;
    }
}