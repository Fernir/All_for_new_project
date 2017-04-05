namespace nManager.Wow.Helpers.PathFinderClass
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class Danger
    {
        public Danger(Point loc, float rad)
        {
            try
            {
                this.Location = loc;
                this.Radius = rad;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Danger(Point loc, float rad): " + exception, true);
            }
        }

        public Danger(Point loc, int levelDifference, float factor = 1f)
        {
            try
            {
                this.Location = loc;
                this.Radius = (levelDifference * 3.5f) + 10f;
                this.Radius *= factor;
                if (levelDifference < 0)
                {
                    this.Radius = -this.Radius;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Danger(Point loc, int levelDifference, float factor = 1.0f): " + exception, true);
            }
        }

        public Point Location { get; private set; }

        public float Radius { get; private set; }
    }
}

