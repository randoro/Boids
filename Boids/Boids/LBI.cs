using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Boids
{
    class LBI
    {
        private int gridXPrecision;
        private int gridYPrecision;
        public bool viewLBI { get; set; }

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
            //for (int i = 3840; i < 3968; i++)
            //{
            //    informationGrid[i] = 1.5f;
            //}
            //for (int i = 1600; i < 1680; i++)
            //{
            //    informationGrid[i] = 1f;
            //}
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (viewLBI)
            {
                for (int i = 0; i < informationGrid.Length; i++)
                {
                    spriteBatch.Draw(Game1.boidText,
                        new Rectangle((i%gridXPrecision)*Globals.lbiSpaceBetween,
                            (i / gridXPrecision) * Globals.lbiSpaceBetween, Globals.lbiSpaceBetween, Globals.lbiSpaceBetween), new Rectangle(50, 50, 1, 1),
                        new Color(1f, 0f, 0f, informationGrid[i]),
                        0f, new Vector2(0, 0), SpriteEffects.None, 1f);
                }
            }
        }


        public void AddDot(Vector2 position)
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

            Point outerleftleftUp = new Point(closest.X - 2, closest.Y - 2);
            Point outerleftUp = new Point(closest.X - 1, closest.Y - 2);
            Point outerUp = new Point(closest.X, closest.Y - 2);
            Point outerrightrightUp = new Point(closest.X + 1, closest.Y - 2);
            Point outerrightUp = new Point(closest.X + 2, closest.Y - 2);


            Point outerleftSideUp = new Point(closest.X - 2, closest.Y - 1);
            Point outerleftSide = new Point(closest.X - 2, closest.Y);
            Point outerleftSideDown = new Point(closest.X - 2, closest.Y + 1);

            Point outerrightSideUp = new Point(closest.X + 2, closest.Y - 1);
            Point outerrightSide = new Point(closest.X + 2, closest.Y);
            Point outerrightSideDown = new Point(closest.X + 2, closest.Y + 1);


            Point outerleftleftDown = new Point(closest.X - 2, closest.Y + 2);
            Point outerleftDown = new Point(closest.X - 1, closest.Y + 2);
            Point outerDown = new Point(closest.X, closest.Y + 2);
            Point outerrightrightDown = new Point(closest.X + 1, closest.Y + 2);
            Point outerrightDown = new Point(closest.X + 2, closest.Y + 2);

            AddGridInformationToCap(closest, 0.5f);
            float smaller = 0.2f;
            AddGridInformationToCap(leftUp, smaller);
            AddGridInformationToCap(leftMiddle, smaller);
            AddGridInformationToCap(leftDown, smaller);

            AddGridInformationToCap(middleUp, smaller);
            AddGridInformationToCap(middleDown, smaller);

            AddGridInformationToCap(rightUp, smaller);
            AddGridInformationToCap(rightMiddle, smaller);
            AddGridInformationToCap(rightDown, smaller);

            float smallest = 0.05f;
            AddGridInformationToCap(outerleftleftUp, smallest);
            AddGridInformationToCap(outerleftUp, smallest);
            AddGridInformationToCap(outerUp, smallest);
            AddGridInformationToCap(outerrightrightUp, smallest);
            AddGridInformationToCap(outerrightUp, smallest);

            AddGridInformationToCap(outerleftSideUp, smallest);
            AddGridInformationToCap(outerleftSide, smallest);
            AddGridInformationToCap(outerleftSideDown, smallest);

            AddGridInformationToCap(outerrightSideUp, smallest);
            AddGridInformationToCap(outerrightSide, smallest);
            AddGridInformationToCap(outerrightSideDown, smallest);

            AddGridInformationToCap(outerleftleftDown, smallest);
            AddGridInformationToCap(outerleftDown, smallest);
            AddGridInformationToCap(outerDown, smallest);
            AddGridInformationToCap(outerrightrightDown, smallest);
            AddGridInformationToCap(outerrightDown, smallest);


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
            return getDangerAngleFromPosition(currentBoid.position, currentBoid);
        }


        public Vector2 getDangerAngleFromPosition(Vector2 position, Boid currentBoid)
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

            if (dangerAngle.Length() > 1)
            {
                dangerAngle.Normalize();
                dangerAngle = Vector2.Multiply(dangerAngle, Globals.boidLBIMaxSpeed);
                dangerAngle = Vector2.Subtract(dangerAngle, currentBoid.velocity);
            }


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


        public void setGridInformation(Point gridPos, float value)
        {
            int modXplus = gridPos.X + gridXPrecision;
            int modYplus = gridPos.Y + gridYPrecision;
            int modX = modXplus % gridXPrecision;
            int modY = modYplus % gridYPrecision;

            informationGrid[modX + modY*gridXPrecision] = value;
        }

        public void AddGridInformationToCap(Point gridPos, float value)
        {
            float old = getGridInformation(gridPos);
            float newValue = old + value;
            if (newValue > 1.0f)
            {
                newValue = 1.0f;
            }
            setGridInformation(gridPos, newValue);
        }
    }
}
