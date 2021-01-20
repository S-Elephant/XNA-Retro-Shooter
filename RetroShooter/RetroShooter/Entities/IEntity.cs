using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace RetroShooter
{
    public interface IEntity : IXNADispose
    {
        Rectangle AABB { get; }
        List<Rectangle2> CollisionRects { get; }
        void Update(GameTime gameTime);
        void DebugDraw();
        void Draw();
        void DrawShadow();
        SpriteEffects SpriteEffects { get; }
    }
}
