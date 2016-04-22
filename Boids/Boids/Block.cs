using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boids
{
    class Block
    {
        public Rectangle position { get; private set; }

        public Block(Rectangle position)
        {
            this.position = position;


        }


        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.boidText, position, new Rectangle(10, 10, 1, 1), Color.White,
                0f, new Vector2(0, 0), SpriteEffects.None, 1f);
        }
    }
}
