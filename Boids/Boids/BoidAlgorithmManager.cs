using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boids
{
    class BoidAlgorithmManager
    {
        private List<Boid> allBoids;

        public BoidAlgorithmManager()
        {
            allBoids = new List<Boid>();

            //allBoids.Add(new Boid(new Vector2(400, 400), new Vector2(0.5f, -0.5f)));
            //allBoids.Add(new Boid(new Vector2(700, 400), new Vector2(-0.5f, -0.5f)));
            //allBoids.Add(new Boid(new Vector2(500, 200), new Vector2(0.5f, 0.4f)));

            //for (int i = 0; i < 100; i++)
            //{
            //    Vector2 randPos = new Vector2(Globals.rand.Next(100, 1100), Globals.rand.Next(100, 600));
            //    Vector2 vel = Vector2.Subtract(new Vector2(Globals.xScreen/2, Globals.yScreen/2), randPos);
            //    vel.Normalize();
            //    Vector2 randVel = Vector2.Multiply(vel, new Vector2((float)(Globals.rand.NextDouble() * 2.0f), (float)(Globals.rand.NextDouble() * 2.0f)));
            //    allBoids.Add(new Boid(randPos, randVel));
            //}

            for (int i = 0; i < 100; i++)
            {
                Vector2 staticPos = new Vector2(Globals.xScreen/2, Globals.yScreen/2);
                Vector2 randVel = new Vector2((float)(Globals.rand.NextDouble() * 2.0f)- 1.0f, (float)(Globals.rand.NextDouble() * 2.0f)- 1.0f);
                allBoids.Add(new Boid(staticPos, randVel));
            }

        }


        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < allBoids.Count; i++)
            {
                AddLocalNeighbours(allBoids[i]);

                // Separation
                Vector2 sepVect = Separate(allBoids[i]);
                sepVect = Vector2.Multiply(sepVect, 1.5f);

                // Alignment
                Vector2 alignVect = Align(allBoids[i]);
                alignVect = Vector2.Multiply(alignVect, 1f);

                // Cohesion
                Vector2 cohesionVect = Cohesion(allBoids[i]);
                cohesionVect = Vector2.Multiply(cohesionVect, 1f);

                Vector2 newForce = sepVect + alignVect + cohesionVect;
                allBoids[i].Update(gameTime, newForce);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < allBoids.Count; i++)
            {
                allBoids[i].Draw(spriteBatch);
            }
        }


        private void AddLocalNeighbours(Boid currentBoid)
        {
            currentBoid.localNeighbours.Clear();

            for (int i = 0; i < allBoids.Count; i++)
            {
                Boid checkingBoid = allBoids[i];
                if (Vector2.Distance(checkingBoid.position, currentBoid.position) > Globals.boidSightLength)
                {
                    //its outside the field
                    continue;
                }

                if (checkingBoid == currentBoid)
                {
                    //its itself
                    continue;
                }

                if (!CheckIfInFieldOfView(currentBoid, checkingBoid))
                {
                    //its in blindspot
                    continue;
                }

                currentBoid.localNeighbours.Add(checkingBoid);

            }
        }


        private bool CheckIfInFieldOfView(Boid currentBoid, Boid checkingBoid)
        {
            float delta_x = checkingBoid.position.X - currentBoid.position.X;
            float delta_y = currentBoid.position.Y - checkingBoid.position.Y;
            double theta_radians = Math.Atan2(delta_y, delta_x);

            double velocity_theta_radians = Math.Atan2(-currentBoid.velocity.Y, currentBoid.velocity.X);
            double velocity_theta_radians_plus_pi = velocity_theta_radians + Math.PI;

            bool inBlindSpot = WithinRange(theta_radians, velocity_theta_radians_plus_pi - Globals.halfOfBoidsBlindSpot,
                velocity_theta_radians_plus_pi + Globals.halfOfBoidsBlindSpot);

            return !inBlindSpot;


        }


        private bool WithinRange(double value, double min, double max)
        {
            return value > min && value < max;
        }


        private Vector2 Separate(Boid currentBoid)
        {
            List<Boid> localboids = currentBoid.localNeighbours;
            float desiredseparation = Globals.boidSeperateLength;//25.0f; //move to globals
            Vector2 steer = new Vector2(0, 0);
            int count = 0;
            // For every boid in the system, check if it's too close
            for (int i = 0; i < localboids.Count; i++)
            {
                Boid other = localboids[i];
                float d = Vector2.Distance(currentBoid.position, other.position);
                // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
                if ((d > 0) && (d < desiredseparation))
                {
                    // Calculate vector pointing away from neighbor
                    Vector2 diff = Vector2.Subtract(currentBoid.position, other.position);
                    diff.Normalize();
                    diff = Vector2.Divide(diff, d); // Weight by distance
                    steer = Vector2.Add(steer, diff);
                    count++; // Keep track of how many
                }
            }
            // Average -- divide by how many
            if (count > 0)
            {
                steer = Vector2.Divide(steer, (float) count);
            }

            // As long as the vector is greater than 0
            if (steer.Length() > 0)
            {
                // First two lines of code below could be condensed with new PVector setMag() method
                // Not using this method until Processing.js catches up
                // steer.setMag(maxspeed);

                // Implement Reynolds: Steering = Desired - Velocity
                steer.Normalize();
                steer = Vector2.Multiply(steer, Globals.boidMaxSpeed);
                steer = Vector2.Subtract(steer, currentBoid.velocity);

                //own
                
                ///steer = Vector2.Clamp(steer, new Vector2(-Globals.boidMaxForce, -Globals.boidMaxForce), new Vector2(Globals.boidMaxForce, Globals.boidMaxForce));

                steer.Normalize();
                steer = Vector2.Multiply(steer, Globals.boidMaxForce);
                //steer = Vector2.Multiply(steer, 1.0f);
            }
            return steer;
        }


        // Alignment
        // For every nearby boid in the system, calculate the average velocity
        private Vector2 Align(Boid currentBoid)
        {
            List<Boid> localboids = currentBoid.localNeighbours;
            float neighbordist = Globals.boidSightLength;
            Vector2 sum = new Vector2(0, 0);
            int count = 0;
            for (int i = 0; i < localboids.Count; i++)
            {
                Boid other = localboids[i];
                float d = Vector2.Distance(currentBoid.position, other.position);
                if ((d > 0) && (d < neighbordist))
                {
                    sum = Vector2.Add(sum, other.velocity);
                    count++;
                }
            }
            if (count > 0)
            {
                sum = Vector2.Divide(sum, (float) count);
                // First two lines of code below could be condensed with new PVector setMag() method
                // Not using this method until Processing.js catches up
                // sum.setMag(maxspeed);

                // Implement Reynolds: Steering = Desired - Velocity
                sum.Normalize();
                sum = Vector2.Multiply(sum, Globals.boidMaxSpeed);
                Vector2 steer = Vector2.Subtract(sum, currentBoid.velocity);

                //own
                

                ///steer = Vector2.Clamp(steer, new Vector2(-Globals.boidMaxForce, -Globals.boidMaxForce), new Vector2(Globals.boidMaxForce, Globals.boidMaxForce));
                //steer.limit(maxforce);
                steer.Normalize();
                steer = Vector2.Multiply(steer, Globals.boidMaxForce);

                //steer = Vector2.Multiply(steer, 0.001f);

                return steer;
            }
            else
            {
                return new Vector2(0, 0);
            }
        }


        private Vector2 Cohesion(Boid currentBoid)
        {
            List<Boid> localboids = currentBoid.localNeighbours;
            float neighbordist = Globals.boidSightLength;
            Vector2 sum = new Vector2(0, 0); // Start with empty vector to accumulate all locations
            int count = 0;
            for (int i = 0; i < localboids.Count; i++)
            {
                Boid other = localboids[i];
                float d = Vector2.Distance(currentBoid.position, other.position);
                if ((d > 0) && (d < neighbordist))
                {
                    sum = Vector2.Add(sum, other.position); // Add location
                    count++;
                }
            }
            if (count > 0)
            {
                sum = Vector2.Divide(sum, count);
                return Seek(currentBoid, sum); // Steer towards the location
            }
            else
            {
                return new Vector2(0, 0);
            }
        }


        private Vector2 Seek(Boid currentBoid, Vector2 target)
        {
            Vector2 desired = Vector2.Subtract(target, currentBoid.position);  // A vector pointing from the location to the target
            // Scale to maximum speed
            desired.Normalize();
            desired = Vector2.Multiply(desired, Globals.boidMaxSpeed);

            // Above two lines of code below could be condensed with new PVector setMag() method
            // Not using this method until Processing.js catches up
            // desired.setMag(maxspeed);

            // Steering = Desired minus Velocity
            Vector2 steer = Vector2.Subtract(desired, currentBoid.velocity);

            //own
            

            //steer = Vector2.Clamp(steer, new Vector2(-Globals.boidMaxForce, -Globals.boidMaxForce), new Vector2(Globals.boidMaxForce, Globals.boidMaxForce));
            //steer.limit(maxforce);  // Limit to maximum steering force
            steer.Normalize();
            steer = Vector2.Multiply(steer, Globals.boidMaxForce);
            //steer = Vector2.Multiply(steer, 0.001f);

            return steer;
        }



    }
}
