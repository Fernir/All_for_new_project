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
        private readonly bool _showStateInStatus;
        private Stopwatch _stopWatch;
        private Thread _workerThread;

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

        public virtual void Pulse()
        {
            try
            {
                foreach (nManager.FiniteStateMachine.State state in this.States)
                {
                    if (this.RunState(state))
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

        private void Run(object sleepTime)
        {
            try
            {
                while (this.Running)
                {
                    this._stopWatch.Start();
                    this.Pulse();
                    if (this._stopWatch.ElapsedMilliseconds < ((int) sleepTime))
                    {
                        int millisecondsTimeout = ((int) sleepTime) - ((int) this._stopWatch.ElapsedMilliseconds);
                        if (millisecondsTimeout > 0)
                        {
                            Thread.Sleep(millisecondsTimeout);
                        }
                    }
                    this._stopWatch.Reset();
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

        private bool RunState(nManager.FiniteStateMachine.State state)
        {
            try
            {
                if (state.NeedToRun)
                {
                    this.CurrentState = state.DisplayName;
                    if (this._showStateInStatus)
                    {
                        Logging.Status = "Bot > " + state.DisplayName;
                    }
                    try
                    {
                        foreach (nManager.FiniteStateMachine.State state2 in state.BeforeStates)
                        {
                            this.RunState(state2);
                        }
                        state.Run();
                        foreach (nManager.FiniteStateMachine.State state3 in state.NextStates)
                        {
                            this.RunState(state3);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError(string.Concat(new object[] { "RunState(State state): > State ", state.DisplayName, " - ", exception }), true);
                    }
                    return true;
                }
            }
            catch (Exception exception2)
            {
                try
                {
                    Logging.WriteError(string.Concat(new object[] { "Engine > RunState(State state) > State: ", state.DisplayName, " - ", exception2 }), true);
                }
                catch (Exception exception3)
                {
                    Logging.WriteError("Engine > RunState(State state): " + exception3, true);
                }
            }
            return false;
        }

        public void StartEngine(byte framesPerSecond, string fsmCustomName = "FSM nManager")
        {
            try
            {
                int parameter = 0x3e8 / framesPerSecond;
                this._stopWatch = new Stopwatch();
                this.Running = true;
                Thread thread = new Thread(new ParameterizedThreadStart(this.Run)) {
                    IsBackground = true,
                    Name = fsmCustomName
                };
                this._workerThread = thread;
                this._workerThread.Start(parameter);
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
                if (((this._workerThread != null) && this.Running) && this._workerThread.IsAlive)
                {
                    this._workerThread.Abort();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Engine > StopEngine(): " + exception, true);
            }
            finally
            {
                this._workerThread = null;
                this.Running = false;
                this.CurrentState = "Stopped";
            }
        }

        public string CurrentState { get; private set; }

        public bool Running { get; private set; }

        public List<nManager.FiniteStateMachine.State> States { get; private set; }
    }
}

