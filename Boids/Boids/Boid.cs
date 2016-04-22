using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boids
{
    class Boid
    {
        public Vector2 position;
        public bool died { get; private set; }
        public Vector2 velocity { get; set; }
        public List<Boid> localNeighbours { get; private set; }
        private LBI informationGrid;

        public Boid(Vector2 startPosition, Vector2 startVelocity, LBI informationGrid)
        {
            position = startPosition;
            velocity = startVelocity;
            this.informationGrid = informationGrid;
            localNeighbours = new List<Boid>();
        }

        public void Update(GameTime gameTime, Vector2 newForce)
        {
            if (!died)
            {
                velocity = velocity + newForce;
                position += velocity;


                checkIfDied();

                fixOutSideScreen();
            }
        }

        private void checkIfDied()
        {
            for (int i = 0; i < BoidAlgorithmManager.blockList.Count; i++)
            {
                if(BoidAlgorithmManager.blockList[i].position.Contains((int)position.X, (int)position.Y))
                {
                    died = true;
                    informationGrid.AddDot(position);
                    Game1.deaths++;
                    break;
                }
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            if (!died)
            {
                spriteBatch.Draw(Game1.boidText, position, new Rectangle(1, 1, 36, 22), Color.White,
                    (float) Math.Atan2(velocity.Y, velocity.X), new Vector2(18, 11), Vector2.One, SpriteEffects.None, 1f);
            }
        }

        private void fixOutSideScreen()
        {
            if (position.X < -36)
            {
                position.X = Globals.xScreen + 35;
            }
            if (position.X > Globals.xScreen + 36)
            {
                position.X = -35;
            }

            if (position.Y < -36)
            {
                position.Y = Globals.yScreen + 35;
            }
            if (position.Y > Globals.yScreen + 36)
            {
                position.Y = -35;
            }

        }
    }
}
