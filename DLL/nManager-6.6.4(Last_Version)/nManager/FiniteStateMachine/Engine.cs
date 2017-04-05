namespace nManager.FiniteStateMachine
{
    using nManager.Helpful;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Engine
    {
        private Stopwatch _fialihuexIpa;
        private Thread _leowa;
        private readonly bool _showStateInStatus;

        public Engine(bool showStateInStatus = true)
        {
            try
            {
                this._showStateInStatus = showStateInStatus;
                this.States = new List<nManager.FiniteStateMachine.State>();
                this.States.Sort();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Engine > Engine(): " + exception, true);
            }
        }

        public void AddState(nManager.FiniteStateMachine.State tempState)
        {
            try
            {
                if (!this.States.Contains(tempState))
                {
                    this.States.Add(tempState);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Engine > AddState(State tempState): " + exception, true);
            }
        }

        private bool Eviesiaxoam(nManager.FiniteStateMachine.State oxatein)
        {
            try
            {
                if (oxatein.NeedToRun)
                {
                    this.CurrentState = oxatein.DisplayName;
                    if (this._showStateInStatus)
                    {
                        Logging.Status = "Bot > " + oxatein.DisplayName;
                    }
                    try
                    {
                        foreach (nManager.FiniteStateMachine.State state in oxatein.BeforeStates)
                        {
                            this.Eviesiaxoam(state);
                        }
                        oxatein.Run();
                        foreach (nManager.FiniteStateMachine.State state2 in oxatein.NextStates)
                        {
                            this.Eviesiaxoam(state2);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError(string.Concat(new object[] { "RunState(State state): > State ", oxatein.DisplayName, " - ", exception }), true);
                    }
                    return true;
                }
            }
            catch (Exception exception2)
            {
                try
                {
                    Logging.WriteError(string.Concat(new object[] { "Engine > RunState(State state) > State: ", oxatein.DisplayName, " - ", exception2 }), true);
                }
                catch (Exception exception3)
                {
                    Logging.WriteError("Engine > RunState(State state): " + exception3, true);
                }
            }
            return false;
        }

        private void PiojiokuVeusifoa(object ejoemuisuad)
        {
            try
            {
                while (this.Running)
                {
                    this._fialihuexIpa.Start();
                    this.Pulse();
                    if (this._fialihuexIpa.ElapsedMilliseconds < ((int) ejoemuisuad))
                    {
                        int millisecondsTimeout = ((int) ejoemuisuad) - ((int) this._fialihuexIpa.ElapsedMilliseconds);
                        if (millisecondsTimeout > 0)
                        {
                            Thread.Sleep(millisecondsTimeout);
                        }
                    }
                    this._fialihuexIpa.Reset();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Engine > Run(object sleepTime): " + exception, true);
            }
            finally
            {
                this.CurrentState = "Stopped";
                this.Running = false;
            }
        }

        public virtual void Pulse()
        {
            try
            {
                foreach (nManager.FiniteStateMachine.State state in this.States)
                {
                    if (this.Eviesiaxoam(state))
                    {
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Engine > Pulse(): " + exception, true);
            }
        }

        public bool RemoveStateByName(string displayName)
        {
            try
            {
                for (int i = 0; i < this.States.Count; i++)
                {
                    if (this.States[i].DisplayName == displayName)
                    {
                        this.States.RemoveAt(i);
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Engine > RemoveStateByName(String displayName): " + exception, true);
            }
            return false;
        }

        public void StartEngine(byte framesPerSecond, string fsmCustomName = "FSM nManager")
        {
            try
            {
                int parameter = 0x3e8 / framesPerSecond;
                this._fialihuexIpa = new Stopwatch();
                this.Running = true;
                Thread thread = new Thread(new ParameterizedThreadStart(this.PiojiokuVeusifoa)) {
                    IsBackground = true,
                    Name = fsmCustomName
                };
                this._leowa = thread;
                this._leowa.Start(parameter);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Engine > StartEngine(byte framesPerSecond): " + exception, true);
            }
        }

        public void StopEngine()
        {
            try
            {
                if (((this._leowa != null) && this.Running) && this._leowa.IsAlive)
                {
                    this._leowa.Abort();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Engine > StopEngine(): " + exception, true);
            }
            finally
            {
                this._leowa = null;
                this.Running = false;
                this.CurrentState = "Stopped";
            }
        }

        public string CurrentState { get; private set; }

        public bool Running { get; private set; }

        public List<nManager.FiniteStateMachine.State> States { get; private set; }
    }
}

