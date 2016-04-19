using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boids
{
    class SteeringBehavior
    {

        //data
        //public AIControl* m_parent;
        public float m_weight;
        public float m_probability;
        public string m_name;
        public bool m_disable;
        public float m_lastForceMagApplied;

        public SteeringBehavior( /*AIControl* parent,*/ string name)
        {
//m_parent = parent;
            m_disable = false;
            m_lastForceMagApplied = 0.0f;
        }

        public virtual bool Update(float dt, Vector2 totalForce)
        {
            return false;
        }

        public virtual void Reset()
        {
        }

        public virtual void Draw()
        {
        }

        public virtual void SteerTowards(Vector2 target, Vector2 result)
        {
//Vector2 desired = target - m_parent->m_ship->m_position;
//float targetDistance = desired.Length();
//if(targetDistance > 0)
//{
//desired = desired.Normalize() *
//m_parent->m_ship->m_maxSpeed;
//result = desired - m_parent->m_ship->m_velocity;
//}
//442 AI Game Engine Programming
//else
//result.SetZero();
        }


        public virtual void SteerAway(Vector2 target, Vector2 result)
        {
//Point3f desired = m_parent->m_ship->m_position - target;
//float targetDistance = desired.Length();
//if(targetDistance > 0)
//{
//desired = desired.Normalize() *
//m_parent->m_ship->m_maxSpeed;
//result = desired - m_parent->m_ship->m_velocity;
//}
//else
//result.SetZero();
        }
    }
}
