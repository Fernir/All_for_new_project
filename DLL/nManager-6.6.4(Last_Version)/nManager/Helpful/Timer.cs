namespace nManager.Helpful
{
    using System;
    using System.Runtime.CompilerServices;

    public class Timer
    {
        private readonly double _countDowntime;
        private bool _uciuriedib;

        public Timer()
        {
            this._countDowntime = 0.0;
            this._uciuriedib = false;
            this.Reset();
        }

        public Timer(double countDowntime)
        {
            try
            {
                this._countDowntime = countDowntime;
                this._uciuriedib = false;
                this.Reset();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Timer(double countDowntime): " + exception, true);
            }
        }

        public void ForceReady()
        {
            this._uciuriedib = true;
        }

        private long Kuikoapoase()
        {
            try
            {
                return (long) Environment.TickCount;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Timer > GetValue(): " + exception, true);
            }
            return 0L;
        }

        public long Peek()
        {
            try
            {
                return (this.Kuikoapoase() - this.get_StartTime());
            }
            catch (Exception exception)
            {
                Logging.WriteError("Timer > Peek(): " + exception, true);
            }
            return 0L;
        }

        public void Reset()
        {
            try
            {
                this.set_StartTime(this.Kuikoapoase());
                this._uciuriedib = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Timer > Reset(): " + exception, true);
            }
        }

        private long _leiraepair { get; set; }

        public bool IsReady
        {
            get
            {
                try
                {
                    if (this._uciuriedib)
                    {
                        return true;
                    }
                    if (this.Peek() > this._countDowntime)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Timer > IsReady: " + exception, true);
                }
                return false;
            }
        }
    }
}

