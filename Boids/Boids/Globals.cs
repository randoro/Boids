using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boids
{
    public static class Globals
    {
        public const int xScreen = 1280;
        public const int yScreen = 720;
        public static Random rand = new Random();

        public const int lbiSpaceBetween = 16;

        public const int boidSightLength = 100;
        public const int boidSeperateLength = 50;
        public const double halfOfBoidsBlindSpot = Math.PI / 8;
        public const int boidAmountTotal = 100;

        public const float boidLBIMaxSpeed = 1f;
        public const float boidMaxSpeed = 2f;
        public const float boidMaxForce = 0.04f;
    }
}
