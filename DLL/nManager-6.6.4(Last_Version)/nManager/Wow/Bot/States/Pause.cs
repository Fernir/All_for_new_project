﻿namespace nManager.Wow.Bot.States
{
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class Pause : nManager.FiniteStateMachine.State
    {
        private bool _awanejaudiv;
        private uint _eweudoabueg;
        private static nManager.Helpful.Timer _fucioquameuwiUneo = new nManager.Helpful.Timer(0.0);
        private static nManager.Helpful.Timer _ipiofuowedeqe = new nManager.Helpful.Timer(0.0);
        private bool _nerawulEl;
        public bool FakeSettingsActivateBreakSystem;
        public int FakeSettingsMaximumBreakTime = 3;
        public int FakeSettingsMaximumTimeBetweenBreaks = 120;
        public int FakeSettingsMinimumBreakTime = 3;
        public int FakeSettingsMinimumTimeBetweenBreaks = 60;

        public override void Run()
        {
            Logging.Write("Pause started");
            if (!this._nerawulEl)
            {
                MovementManager.StopMove();
                while (nManager.Products.Products.IsStarted && nManager.Products.Products.InAutoPause)
                {
                    Thread.Sleep(300);
                }
            }
            else
            {
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                switch (Others.Random(1, 4))
                {
                    case 1:
                        position.X += Others.Random(100, 300);
                        break;

                    case 2:
                        position.X += Others.Random(-300, -100);
                        break;

                    case 3:
                        position.Y += Others.Random(100, 300);
                        break;

                    case 4:
                        position.Y += Others.Random(-300, -100);
                        break;
                }
                if (MountTask.GetMountCapacity() == MountCapacity.Fly)
                {
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                    {
                        MountTask.Mount(false, true);
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                    {
                        if (Usefuls.IsFlying)
                        {
                            position.Z += Others.Random(20, 50);
                        }
                        else
                        {
                            MountTask.Takeoff();
                            position.Z += Others.Random(50, 100);
                        }
                        MovementManager.MoveTo(position, false);
                    }
                }
                else
                {
                    Npc target = new Npc();
                    target = NpcDB.GetNpcNearby(Npc.NpcType.Mailbox, false);
                    if (target.Entry <= 0)
                    {
                        target = NpcDB.GetNpcNearby(Npc.NpcType.Repair, false);
                        if (target.Entry <= 0)
                        {
                            target = NpcDB.GetNpcNearby(Npc.NpcType.Vendor, false);
                            if (target.Entry <= 0)
                            {
                                target = NpcDB.GetNpcNearby(Npc.NpcType.FlightMaster, false);
                            }
                        }
                    }
                    if (target.Entry > 0)
                    {
                        uint num = MovementManager.FindTarget(ref target, 0f, true, false, 0f, false);
                        if (MovementManager.InMovement)
                        {
                            return;
                        }
                        if ((num == 0) && (target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 10f))
                        {
                            NpcDB.DelNpc(target);
                        }
                        else if (num > 0)
                        {
                        }
                    }
                }
                while (!_ipiofuowedeqe.IsReady)
                {
                    Thread.Sleep(300);
                }
                this._awanejaudiv = true;
            }
            Logging.Write("Pause stoped");
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
                return "Pause";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (nManager.Products.Products.IsStarted && nManager.Products.Products.InAutoPause)
                {
                    this._nerawulEl = false;
                    return true;
                }
                if (this.FakeSettingsActivateBreakSystem)
                {
                    if ((this._eweudoabueg == 0) || this._awanejaudiv)
                    {
                        _fucioquameuwiUneo = new nManager.Helpful.Timer((double) Others.Random((this.FakeSettingsMinimumTimeBetweenBreaks * 60) * 0x3e8, (this.FakeSettingsMaximumTimeBetweenBreaks * 60) * 0x3e8));
                        this._eweudoabueg++;
                    }
                    if (_fucioquameuwiUneo.IsReady)
                    {
                        _ipiofuowedeqe = new nManager.Helpful.Timer((double) Others.Random((this.FakeSettingsMinimumBreakTime * 60) * 0x3e8, (this.FakeSettingsMaximumBreakTime * 60) * 0x3e8));
                        this._nerawulEl = true;
                        return true;
                    }
                }
                return false;
            }
        }

        public override List<nManager.FiniteStateMachine.State> NextStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override int Priority { get; set; }
    }
}

