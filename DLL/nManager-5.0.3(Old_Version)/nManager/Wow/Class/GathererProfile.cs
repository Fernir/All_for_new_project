namespace nManager.Wow.Class
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class GathererProfile
    {
        public List<GathererBlackListRadius> BlackListRadius = new List<GathererBlackListRadius>();
        public List<nManager.Wow.Class.Npc> Npc = new List<nManager.Wow.Class.Npc>();
        public List<Point> Points = new List<Point>();

        public bool ShouldSerializeBlackListRadius()
        {
            return ((this.BlackListRadius != null) && (this.BlackListRadius.Count > 0));
        }

        public bool ShouldSerializeNpc()
        {
            return ((this.Npc != null) && (this.Npc.Count > 0));
        }
    }
}

