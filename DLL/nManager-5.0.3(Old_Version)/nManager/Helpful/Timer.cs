namespace nManager.Helpful
{
    using System;
    using System.Runtime.CompilerServices;

    public class Timer
    {
        private readonly double _countDowntime;
        private bool _varforceReady;

        public Timer()
        {
            this._countDowntime = 0.0;
            this._varforceReady = false;
            this.Reset();
        }

        public Timer(double countDowntime)
        {
            try
            {
                this._countDowntime = countDowntime;
                this._varforceReady = false;
                this.Reset();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Timer(double countDowntime): " + exception, true);
            }
        }

        public void ForceReady()
        {
            this._varforceReady = true;
        }

        private long GetValue()
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
                return (this.GetValue() - this.StartTime);
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
                this.StartTime = this.GetValue();
                this._varforceReady = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Timer > Reset(): " + exception, true);
            }
        }

        public bool IsReady
        {
            get
            {
                try
                {
                    if (this._varforceReady)
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

        private long StartTime { get; set; }
    }
}

