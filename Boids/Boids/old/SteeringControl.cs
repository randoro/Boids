using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boids
{
    class SteeringControl
    {

        //perception data
        //(public so that states can share it)
        //public GameObj m_nearestAsteroid;
        public float m_safetyRadius;

        //data
        protected SteeringBehaviorManager m_behaviorManager;
        protected int m_getPowerupIndex;

        //constructor/functions
        public SteeringControl() //Ship* ship = NULL); 
        {

        }

        public void Update(float dt)
        {
            
        }

        public void UpdatePerceptions(float dt)
        {
            
        }

        public void Init()
        {
            
        }

        public void Draw()
        {
            
        }

        public void Reset()
        {
            
        }

    }
}
