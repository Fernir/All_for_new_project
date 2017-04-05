namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class MovementLoop : nManager.FiniteStateMachine.State
    {
        private int _battleground;
        private float _loopPathId = -1f;
        private readonly nManager.Helpful.Timer _outOfBattlegroundAntiAfk = new nManager.Helpful.Timer(60000.0);
        public bool IsProfileCSharp;

        public override void Run()
        {
            int pointId;
            float num2 = nManager.Helpful.Math.DistanceListPoint(this.PathLoop);
            if (this._loopPathId != num2)
            {
                pointId = nManager.Helpful.Math.NearestPointOfListPoints(this.PathLoop, nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                MovementManager.PointId = pointId;
            }
            else if (this.PathLoop[MovementManager.PointId].DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= 2f)
            {
                pointId = MovementManager.PointId;
            }
            else
            {
                pointId = MovementManager.PointId + 1;
                if (pointId > (this.PathLoop.Count - 1))
                {
                    pointId = 0;
                }
                int num3 = pointId;
                int num4 = 0;
                Vector3 vector = new Vector3(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                do
                {
                    int num5 = ((num3 + 1) <= (this.PathLoop.Count - 1)) ? (num3 + 1) : 0;
                    Vector3 vector2 = new Vector3(this.PathLoop[num3]);
                    Vector3 vector3 = new Vector3(this.PathLoop[num5]);
                    float num6 = (vector3 - vector2).Angle2D(vector2 - vector);
                    if (System.Math.Abs(num6) <= 1.0471975511965976)
                    {
                        Logging.WriteNavigator(string.Concat(new object[] { "Next Point is ", num3 - pointId, " ahead, his angle is ", System.Math.Round((double) (((double) (num6 * 180f)) / 3.1415926535897931), 2), "\x00b0" }));
                        pointId = num3;
                        break;
                    }
                    num3 = num5;
                    num4++;
                }
                while (num4 <= 10);
                MovementManager.PointId = pointId;
            }
            if (this._loopPathId == -1f)
            {
                this._loopPathId = num2;
            }
            if (((this.PathLoop[pointId].Type.ToLower() != "flying") && (this.PathLoop[pointId].Type.ToLower() != "swimming")) && (this.PathLoop[pointId].DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 7f))
            {
                bool flag;
                List<Point> points = PathFinder.FindPath(this.PathLoop[pointId], out flag, true, false);
                if (!flag)
                {
                    points.Add(new Point(this.PathLoop[pointId]));
                }
                MovementManager.Go(points);
            }
            else
            {
                MovementManager.GoLoop(this.PathLoop);
            }
        }

        public override List<nManager.FiniteStateMachine.State> BeforeStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Movement Loop";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if ((((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))) || !nManager.Products.Products.IsStarted)
                {
                    return false;
                }
                if (MovementManager.InMovement)
                {
                    return false;
                }
                if (nManager.Products.Products.ProductName == "Battlegrounder")
                {
                    if ((Usefuls.IsInBattleground && !Battleground.IsFinishBattleground()) && Battleground.BattlegroundIsStarted())
                    {
                        this._battleground = 0;
                        if (((this.PathLoop != null) && (this.PathLoop.Count > 0)) && !this.IsProfileCSharp)
                        {
                            return true;
                        }
                    }
                    if ((Usefuls.IsInBattleground && !Battleground.IsFinishBattleground()) && (!Battleground.BattlegroundIsStarted() && (this._battleground == 0)))
                    {
                        MovementsAction.MoveForward(true, false);
                        Thread.Sleep(0x3e8);
                        MovementsAction.MoveForward(false, false);
                        this._battleground++;
                    }
                    else if (!Usefuls.IsInBattleground && this._outOfBattlegroundAntiAfk.IsReady)
                    {
                        this._battleground = 0;
                        this._outOfBattlegroundAntiAfk.Reset();
                        MovementsAction.Ascend(true, false, false);
                        Thread.Sleep(300);
                        MovementsAction.Ascend(false, false, false);
                    }
                    return false;
                }
                return ((this.PathLoop != null) && (this.PathLoop.Count > 0));
            }
        }

        public override List<nManager.FiniteStateMachine.State> NextStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public List<Point> PathLoop { get; set; }

        public override int Priority { get; set; }
    }
}

