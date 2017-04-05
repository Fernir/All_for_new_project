namespace nManager.Helpful
{
    using nManager.Wow.Class;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;

    public static class Math
    {
        public static float CalculateNeededFacing(Vector3 start, Vector3 faceTarget)
        {
            return NormalizeRadian((float) System.Math.Atan2((double) (faceTarget.Y - start.Y), (double) (faceTarget.X - start.X)));
        }

        public static float DegreeToRadian(float degrees)
        {
            try
            {
                return (degrees * 0.01745329f);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DegreeToRadian(float degrees): " + exception, true);
            }
            return 0f;
        }

        public static float DistanceListPoint(List<Point> listPoints)
        {
            try
            {
                if (listPoints.Count == 1)
                {
                    return listPoints[0].DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                }
                float num = 0f;
                for (int i = 0; i <= (listPoints.Count - 1); i++)
                {
                    int num3 = i + 1;
                    if (num3 <= (listPoints.Count - 1))
                    {
                        num += listPoints[i].DistanceTo(listPoints[num3]);
                    }
                }
                return num;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DistanceListPoint(List<Point> listPoints): " + exception, true);
            }
            return 0f;
        }

        public static float FixAngle(float angle)
        {
            if (angle > 6.283185f)
            {
                return (angle - 6.283185f);
            }
            if (angle < 0f)
            {
                return (angle + 6.283185f);
            }
            return angle;
        }

        public static float GetAngle(Point pointA, Point pointB)
        {
            try
            {
                double num = System.Math.Abs((float) (pointB.X - pointA.X));
                double num2 = System.Math.Abs((float) (pointB.Y - pointA.Y));
                if ((pointB.X < pointA.X) && (pointB.Y < pointA.Y))
                {
                    return (float) (1.5707963267948966 + System.Math.Atan(num / num2));
                }
                if (pointB.X < pointA.X)
                {
                    return (float) (3.1415926535897931 + System.Math.Atan(num2 / num));
                }
                if (pointB.Y < pointA.Y)
                {
                    return (float) System.Math.Atan(num2 / num);
                }
                return (float) (4.71238898038469 + System.Math.Atan(num / num2));
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetAngle(float x, float y): " + exception, true);
            }
            return 0f;
        }

        public static float GetAngle(float x, float y)
        {
            try
            {
                double num;
                if (System.Math.Sqrt(System.Math.Pow((double) x, 2.0) + System.Math.Pow((double) y, 2.0)) == 0.0)
                {
                    return 0f;
                }
                if (x == 0f)
                {
                    num = (System.Math.Sign(y) * 3.1415926535897931) / 2.0;
                }
                else
                {
                    num = System.Math.Atan((double) (y / x));
                    if (x < 0f)
                    {
                        num = 3.1415926535897931 + num;
                    }
                }
                if (num < 0.0)
                {
                    num += 6.2831853071795862;
                }
                num = (180.0 * num) / 3.1415926535897931;
                return (float) num;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetAngle(float x, float y): " + exception, true);
            }
            return 0f;
        }

        public static Point GetPosition2DOfAngleAndDistance(Point a, float angle, float distance)
        {
            try
            {
                float num = ((float) System.Math.Sin((double) angle)) * distance;
                float num2 = ((float) System.Math.Cos((double) angle)) * distance;
                return new Point(a.X + num2, a.Y + num, a.Z, "None");
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetPosition2DOfAngleAndDistance(Point a, float angle, float distance): " + exception, true);
            }
            return new Point(0f, 0f, 0f, "None");
        }

        public static Point GetPosition2DOfLineByDistance(Point a, Point b, float distance)
        {
            try
            {
                if ((a.X == b.X) && (a.Y == b.Y))
                {
                    return a;
                }
                float num = b.X - a.X;
                float num2 = b.Y - a.Y;
                float num3 = (float) System.Math.Sqrt((num * num) + (num2 * num2));
                float x = a.X + ((num * distance) / num3);
                return new Point(x, a.Y + ((num2 * distance) / num3), System.Math.Max(a.Z, b.Z), "None");
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetPosition2dOfLineByDistance(Point a, Point b, float distance): " + exception, true);
            }
            return new Point(0f, 0f, 0f, "None");
        }

        public static Point GetPositionOffsetBy3DDistance(Point a, Point b, float distance)
        {
            if ((a.X == b.X) && (a.Y == b.Y))
            {
                return a;
            }
            float num = b.X - a.X;
            float num2 = b.Y - a.Y;
            float num3 = b.Z - a.Z;
            float num4 = (float) System.Math.Sqrt(((num * num) + (num2 * num2)) + (num3 * num3));
            float x = b.X + ((num / num4) * distance);
            float y = b.Y + ((num2 / num4) * distance);
            return new Point(x, y, b.Z + ((num3 / num4) * distance), "None");
        }

        public static int NearestPointOfListPoints(List<Point> listPoint, Point point)
        {
            try
            {
                if (listPoint.Count <= 0)
                {
                    return 0;
                }
                float num = 1E+08f;
                int num2 = 0;
                for (int i = 0; i <= (listPoint.Count - 1); i++)
                {
                    float num4 = listPoint[i].DistanceTo(point);
                    if (num4 < num)
                    {
                        num = num4;
                        num2 = i;
                    }
                }
                return num2;
            }
            catch (Exception exception)
            {
                Logging.WriteError("NearestPointOfListPoints(List<Point> listPoint, Point point)" + exception, true);
                return 0;
            }
        }

        public static float NormalizeRadian(float radian)
        {
            if (radian < 0f)
            {
                return (-((float) (-((double) radian) % 6.2831853071795862)) + ((float) 6.2831853071795862));
            }
            return (float) (((double) radian) % 6.2831853071795862);
        }

        public static float RadianToDegree(float radianValue)
        {
            try
            {
                return (float) System.Math.Round(Convert.ToDouble((double) (radianValue * 57.295779513082323)));
            }
            catch (Exception exception)
            {
                Logging.WriteError("RadianToDegree(float radianValue): " + exception, true);
            }
            return 0f;
        }
    }
}

