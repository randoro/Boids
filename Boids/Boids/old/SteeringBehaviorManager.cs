using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Boids
{
    class SteeringBehaviorManager
    {
        protected List<SteeringBehavior> m_behaviors; //make array?
        protected List<SteeringBehavior> m_active; //make array?
        protected List<float> m_activeForce; //make array?
        protected int m_numBehaviors;
        //protected AIControl* m_parent;
        protected Vector2 m_totalSteeringForce;
        protected float m_maxSteeringForce;


        public SteeringBehaviorManager()//AIControl* parent = NULL)
        {
            //TODO
        }

        public void Update(float dt)
        {
            //don’t do anything if you have no states
            if (m_behaviors.Count == 0)
                return;
            //Clear out debug logs
            m_active.Clear();
            m_activeForce.Clear();
            //reset the steering vector
            m_totalSteeringForce = Vector2.Zero;
            //update all the behaviors
            bool needToClamp = false;

            for (int i = 0; i < m_behaviors.Count; i++)
            {
                Vector2 steeringForce = Vector2.Zero;
                bool didSomething = m_behaviors[i].Update(dt, steeringForce);
                if (didSomething)
                {
//keep track of the behaviors that actually
//did something this tick
                    m_active.Add(m_behaviors[i]);
                    m_activeForce.Add(steeringForce.Length());
//now we want combine the behaviors into
//the total steering force using
//whatever method we decide upon
                    bool keepGoing = false;
//ONLY USE ‘ONE’ COMBINATION METHOD,
//THEY’RE ONLY ALL HERE FOR THE DEMO CODE



//This is for the “Simple weighted combination” method
                    keepGoing = CombineForceWeighted(steeringForce, m_behaviors[i].m_weight);
//Now that we’ve taken all the behaviors into account
//that we want to for each method, we
//must “normalize” our results for the “Simple
//Weighted Combination” method
                    needToClamp = true;
//This is for the “Prioritized Sum” method
                    //---------- keepGoing = CombineForcePrioritySum(steeringForce, m_behaviors[i].m_weight);
//This is for the “Prioritized Dither” method
//----------keepGoing = CombineForcePriorityDithered(steeringForce, m_behaviors[i].m_weight, 0.1f);
// needToClamp = true;
//if we’re done checking behaviors (for
//whatever reason), exit out
                    if (!keepGoing)
                        break;
                }
            }

            if (needToClamp)
            {
                //Vector2.Clamp(m_totalSteeringForce, Vector2.Zero, m_maxSteeringForce); must be wrong? clamping vector not the length

                m_totalSteeringForce.X = MathHelper.Clamp(m_totalSteeringForce.X, 0f, m_maxSteeringForce);
                m_totalSteeringForce.Y = MathHelper.Clamp(m_totalSteeringForce.Y, 0f, m_maxSteeringForce);

                //Vector2.Clamp(m_totalSteeringForce, Vector2.Zero, new Vector2(m_maxSteeringForce, m_maxSteeringForce)); might be correct?
            }



    }

        public void AddBehavior(SteeringBehavior behavior)
        {
            //TODO
        }

        public void DisableBehavior(int index)
        {
            m_behaviors[index].m_disable = true;
        }

        public void SetupBehavior(int behaviorIndex, float weight, float probability, bool disable = false)
        {
            //TODO
        }

        public void Reset()
        {
            //TODO
        }

        public Vector2 GetFinalSteeringVector()
        {
            return m_totalSteeringForce;
        }

        public void Draw()
        {
            //TODO
        }

        public bool CombineForceWeighted(Vector2 steeringForce, float weight)
        {
            m_totalSteeringForce += steeringForce * weight;
            return true;
        }

        public bool CombineForcePrioritySum(Vector2 steeringForce, float weight)
        {
            bool retVal = false;
            float totalForce = m_totalSteeringForce.Length();
            float forceLeft = m_maxSteeringForce - totalForce;
            if (forceLeft > 0.0f)
            {
                float newForce = steeringForce.Length();
                if (newForce < forceLeft)
                    m_totalSteeringForce += steeringForce;
                else
                    m_totalSteeringForce += Vector2.Normalize(steeringForce) * forceLeft;
                //if there’s anything left over, say so
                if ((forceLeft - newForce) > 0)
                    retVal = true;
            }
            return retVal;
        }


        public bool CombineForcePriorityDithered(Vector2 steeringForce, float weight, float randChance)
        {
            bool retVal = true;
            if (Globals.rand.NextDouble() < randChance)
            {
                if (steeringForce.Length() != 0)
                {
                    m_totalSteeringForce = steeringForce;
                    retVal = false;
                }
            }
            return retVal;
        }



    }
}
