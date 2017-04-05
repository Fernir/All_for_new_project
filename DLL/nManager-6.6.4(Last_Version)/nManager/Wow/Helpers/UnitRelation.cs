namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;

    public class UnitRelation
    {
        public static Npc.FactionType GetObjectRacialFaction(uint objectFaction)
        {
            if ((GetReaction(1, objectFaction) >= Reaction.Neutral) && (GetReaction(2, objectFaction) < Reaction.Neutral))
            {
                return Npc.FactionType.Alliance;
            }
            if ((GetReaction(1, objectFaction) >= Reaction.Neutral) && (GetReaction(2, objectFaction) >= Reaction.Neutral))
            {
                return Npc.FactionType.Neutral;
            }
            return Npc.FactionType.Horde;
        }

        public static Reaction GetReaction(uint mobFaction)
        {
            try
            {
                return GetReaction(nManager.Wow.ObjectManager.ObjectManager.Me.Faction, mobFaction);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetReaction(uint mobFaction): " + exception, true);
            }
            return Reaction.Unknown;
        }

        public static Reaction GetReaction(WoWObject localObj, WoWObject mobObj)
        {
            try
            {
                return GetReaction(new WoWUnit(localObj.GetBaseAddress).Faction, new WoWUnit(mobObj.GetBaseAddress).Faction);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetReaction(WoWObject localObj, WoWObject mobObj): " + exception, true);
                return Reaction.Unknown;
            }
        }

        public static Reaction GetReaction(uint localFaction, uint mobFaction)
        {
            try
            {
                if ((mobFaction == 0) && ((localFaction == 1) || (localFaction == 2)))
                {
                    return Reaction.Neutral;
                }
                WoWFactionTemplate template = WoWFactionTemplate.FromId(mobFaction);
                WoWFactionTemplate otherFaction = WoWFactionTemplate.FromId(localFaction);
                if ((template == null) || (otherFaction == null))
                {
                    return Reaction.Unknown;
                }
                return template.GetReactionTowards(otherFaction);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetReaction(uint localFaction, uint mobFaction): " + exception, true);
            }
            return Reaction.Unknown;
        }
    }
}

