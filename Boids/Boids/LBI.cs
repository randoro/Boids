using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boids
{
    class LBI
    {
        private int gridXPrecision;
        private int gridYPrecision;

        float[] informationGrid;

        public LBI(int gridXPrecision, int gridYPrecision)
        {
            this.gridXPrecision = gridXPrecision;
            this.gridYPrecision = gridYPrecision;

            informationGrid = new float[gridXPrecision * gridYPrecision];


            //for (int i = 400; i < 440; i++)
            //{
            //    informationGrid[i] = 0.4f;
            //}
            for (int i = 3840; i < 3968; i++)
            {
                informationGrid[i] = 1.5f;
            }
            //for (int i = 1600; i < 1680; i++)
            //{
            //    informationGrid[i] = 1f;
            //}
        }


        public Point gridPosFromPosition(Vector2 position)
        {
            float xPart = position.X/Globals.xScreen;
            float yPart = position.Y/Globals.yScreen;

            float xGrid = xPart * gridXPrecision;
            float yGrid = yPart * gridYPrecision;

            int xPos = (int)xGrid;
            int yPos = (int)yGrid;


            return new Point(xPos, yPos);
        }


        public Vector2 vectPosFromPoint(Point pos)
        {
            float xPart = Globals.xScreen / gridXPrecision;
            float yPart = Globals.yScreen / gridYPrecision;

            float xGrid = xPart * pos.X;
            float yGrid = yPart * pos.Y;

            float xPos = xGrid % Globals.xScreen;
            float yPos = yGrid % Globals.yScreen;


            return new Vector2(xPos, yPos);
        }



        public Vector2 AvoidFBINodes(Boid currentBoid)
        {
            return getDangerAngleFromPosition(currentBoid.position);
        }


        public Vector2 getDangerAngleFromPosition(Vector2 position)
        {
            Point closest = gridPosFromPosition(position);

            //gridpos
            Point leftUp = new Point(closest.X - 1, closest.Y - 1);
            Point leftMiddle = new Point(closest.X - 1, closest.Y);
            Point leftDown = new Point(closest.X - 1, closest.Y + 1);

            Point middleUp = new Point(closest.X, closest.Y - 1);
            Point middleDown = new Point(closest.X, closest.Y + 1);

            Point rightUp = new Point(closest.X + 1, closest.Y - 1);
            Point rightMiddle = new Point(closest.X + 1, closest.Y);
            Point rightDown = new Point(closest.X + 1, closest.Y + 1);

            
            //data
            float leftUpFloat = getGridInformation(leftUp);
            float leftMiddleFloat = getGridInformation(leftMiddle);
            float leftDownFloat = getGridInformation(leftDown);

            float middleUpFloat = getGridInformation(middleUp);
            float middleDownFloat = getGridInformation(middleDown);

            float rightUpFloat = getGridInformation(rightUp);
            float rightMiddleFloat = getGridInformation(rightMiddle);
            float rightDownFloat = getGridInformation(rightDown);


            //dangervect
            Vector2 leftUpVect = new Vector2(leftUpFloat, leftUpFloat);
            Vector2 leftMiddleVect = new Vector2(leftMiddleFloat, 0);
            Vector2 leftDownVect = new Vector2(leftDownFloat, -leftDownFloat);

            Vector2 middleUpVect = new Vector2(0, middleUpFloat);
            Vector2 middleDownVect = new Vector2(0, -middleDownFloat);

            Vector2 rightUpVect = new Vector2(-rightUpFloat, rightUpFloat);
            Vector2 rightMiddleVect = new Vector2(-rightMiddleFloat, 0);
            Vector2 rightDownVect = new Vector2(-rightDownFloat, -rightDownFloat);


            //pos
            Vector2 leftUpPos = vectPosFromPoint(leftUp);
            Vector2 leftMiddlePos = vectPosFromPoint(leftMiddle);
            Vector2 leftDownPos = vectPosFromPoint(leftDown);

            Vector2 middleUpPos = vectPosFromPoint(middleUp);
            Vector2 middleDownPos = vectPosFromPoint(middleDown);

            Vector2 rightUpPos = vectPosFromPoint(rightUp);
            Vector2 rightMiddlePos = vectPosFromPoint(rightMiddle);
            Vector2 rightDownPos = vectPosFromPoint(rightDown);

            //distance
            float leftUpDist = Vector2.Distance(position, leftUpPos);
            float leftMiddleDist = Vector2.Distance(position, leftMiddlePos);
            float leftDownDist = Vector2.Distance(position, leftDownPos);

            float middleUpDist = Vector2.Distance(position, middleUpPos);
            float middleDownDist = Vector2.Distance(position, middleDownPos);

            float rightUpDist = Vector2.Distance(position, rightUpPos);
            float rightMiddleDist = Vector2.Distance(position, rightMiddlePos);
            float rightDownDist = Vector2.Distance(position, rightDownPos);

            if (leftUpDist != 0.0f)
                leftUpVect = Vector2.Divide(leftUpVect, leftUpDist);

            if (leftMiddleDist != 0.0f)
                leftMiddleVect = Vector2.Divide(leftMiddleVect, leftMiddleDist);

            if (leftDownDist != 0.0f)
                leftDownVect = Vector2.Divide(leftDownVect, leftDownDist);


            if (middleUpDist != 0.0f)
                middleUpVect = Vector2.Divide(middleUpVect, middleUpDist);

            if (middleDownDist != 0.0f)
                middleDownVect = Vector2.Divide(middleDownVect, middleDownDist);


            if (rightUpDist != 0.0f)
                rightUpVect = Vector2.Divide(rightUpVect, rightUpDist);

            if (rightMiddleDist != 0.0f)
                rightMiddleVect = Vector2.Divide(rightMiddleVect, rightMiddleDist);

            if (rightDownDist != 0.0f)
                rightDownVect = Vector2.Divide(rightDownVect, rightDownDist);


            Vector2 dangerAngle = leftUpVect + leftMiddleVect + leftDownVect + middleUpVect + middleDownVect +
                                  rightUpVect + rightMiddleVect + rightDownVect;


            


            return dangerAngle;

        }


        public float getGridInformation(Point gridPos)
        {
            int modXplus = gridPos.X + gridXPrecision;
            int modYplus = gridPos.Y + gridYPrecision;
            int modX = modXplus % gridXPrecision;
            int modY = modYplus % gridYPrecision;

            return informationGrid[modX + modY*gridXPrecision];
        }
    }
}
