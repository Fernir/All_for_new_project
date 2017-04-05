namespace nManager.Wow.Helpers.PathFinderClass
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class UraiceixaororSioSix
    {
        public UraiceixaororSioSix(Point loc, float rad)
        {
            try
            {
                this.set_Location(loc);
                this.set_Radius(rad);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Danger(Point loc, float rad): " + exception, true);
            }
        }

        public UraiceixaororSioSix(Point loc, int levelDifference, float factor = 1f)
        {
            try
            {
                this.set_Location(loc);
                this.set_Radius((levelDifference * 3.5f) + 10f);
                this.set_Radius(this.get_Radius() * factor);
                if (levelDifference < 0)
                {
                    this.set_Radius(-this.get_Radius());
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Danger(Point loc, int levelDifference, float factor = 1.0f): " + exception, true);
            }
        }

        public Point _ekuabajeati { get; private set; }

        public float _mefemipedaOpitifa { get; private set; }
    }
}

