﻿namespace nManager.Wow.Enums
{
    using System;

    public enum WoWEventsType
    {
        UNIT_PET,
        UNIT_TARGET,
        UNIT_HEALTH,
        UNIT_MAXHEALTH,
        UNIT_DISPLAYPOWER,
        UNIT_FACTION,
        UNIT_LEVEL,
        UNIT_DAMAGE,
        UNIT_ATTACK_SPEED,
        UNIT_RANGEDDAMAGE,
        UNIT_SPELL_HASTE,
        UNIT_FLAGS,
        UNIT_RESISTANCES,
        UNIT_ATTACK_POWER,
        UNIT_PET_EXPERIENCE,
        UNIT_RANGED_ATTACK_POWER,
        UNIT_MANA,
        UNIT_STATS,
        UNIT_AURA,
        UNIT_COMBAT,
        UNIT_NAME_UPDATE,
        UNIT_PORTRAIT_UPDATE,
        UNIT_MODEL_CHANGED,
        UNIT_INVENTORY_CHANGED,
        UNIT_CLASSIFICATION_CHANGED,
        UNIT_COMBO_POINTS,
        UNIT_TARGETABLE_CHANGED,
        ITEM_LOCK_CHANGED,
        PLAYER_XP_UPDATE,
        PLAYER_REGEN_DISABLED,
        PLAYER_REGEN_ENABLED,
        PLAYER_AURAS_CHANGED,
        PLAYER_ENTER_COMBAT,
        PLAYER_LEAVE_COMBAT,
        PLAYER_TARGET_CHANGED,
        PLAYER_FOCUS_CHANGED,
        PLAYER_CONTROL_LOST,
        PLAYER_CONTROL_GAINED,
        PLAYER_FARSIGHT_FOCUS_CHANGED,
        PLAYER_LEVEL_UP,
        PLAYER_MONEY,
        PLAYER_DAMAGE_DONE_MODS,
        PLAYER_TOTEM_UPDATE,
        PLAYER_AVG_ITEM_LEVEL_UPDATE,
        ZONE_CHANGED,
        ZONE_CHANGED_INDOORS,
        ZONE_CHANGED_NEW_AREA,
        MINIMAP_UPDATE_ZOOM,
        MINIMAP_UPDATE_TRACKING,
        SCREENSHOT_STARTED,
        SCREENSHOT_SUCCEEDED,
        SCREENSHOT_FAILED,
        ACTIONBAR_SHOWGRID,
        ACTIONBAR_HIDEGRID,
        ACTIONBAR_PAGE_CHANGED,
        ACTIONBAR_SLOT_CHANGED,
        ACTIONBAR_UPDATE_STATE,
        ACTIONBAR_UPDATE_USABLE,
        ACTIONBAR_UPDATE_COOLDOWN,
        ACTIONBAR_SHOW_BOTTOMLEFT,
        UPDATE_SUMMONPETS_ACTION,
        UPDATE_BONUS_ACTIONBAR,
        UPDATE_OVERRIDE_ACTIONBAR,
        UPDATE_EXTRA_ACTIONBAR,
        UPDATE_VEHICLE_ACTIONBAR,
        UPDATE_POSSESS_BAR,
        PARTY_MEMBERS_CHANGED,
        PARTY_LEADER_CHANGED,
        PARTY_MEMBER_ENABLE,
        PARTY_MEMBER_DISABLE,
        PARTY_LOOT_METHOD_CHANGED,
        PARTY_REFER_A_FRIEND_UPDATED,
        SYSMSG,
        UI_ERROR_MESSAGE,
        UI_INFO_MESSAGE,
        UPDATE_CHAT_COLOR,
        CHAT_MSG_ADDON,
        CHAT_MSG_SYSTEM,
        CHAT_MSG_SAY,
        CHAT_MSG_PARTY,
        CHAT_MSG_RAID,
        CHAT_MSG_GUILD,
        CHAT_MSG_OFFICER,
        CHAT_MSG_YELL,
        CHAT_MSG_WHISPER,
        CHAT_MSG_WHISPER_INFORM,
        CHAT_MSG_EMOTE,
        CHAT_MSG_TEXT_EMOTE,
        CHAT_MSG_MONSTER_SAY,
        CHAT_MSG_MONSTER_PARTY,
        CHAT_MSG_MONSTER_YELL,
        CHAT_MSG_MONSTER_WHISPER,
        CHAT_MSG_MONSTER_EMOTE,
        CHAT_MSG_CHANNEL,
        CHAT_MSG_CHANNEL_JOIN,
        CHAT_MSG_CHANNEL_LEAVE,
        CHAT_MSG_CHANNEL_LIST,
        CHAT_MSG_CHANNEL_NOTICE,
        CHAT_MSG_CHANNEL_NOTICE_USER,
        CHAT_MSG_AFK,
        CHAT_MSG_DND,
        CHAT_MSG_IGNORED,
        CHAT_MSG_SKILL,
        CHAT_MSG_LOOT,
        CHAT_MSG_CURRENCY,
        CHAT_MSG_MONEY,
        CHAT_MSG_OPENING,
        CHAT_MSG_TRADESKILLS,
        CHAT_MSG_PET_INFO,
        CHAT_MSG_COMBAT_MISC_INFO,
        CHAT_MSG_COMBAT_XP_GAIN,
        CHAT_MSG_COMBAT_HONOR_GAIN,
        CHAT_MSG_COMBAT_FACTION_CHANGE,
        CHAT_MSG_BG_SYSTEM_NEUTRAL,
        CHAT_MSG_BG_SYSTEM_ALLIANCE,
        CHAT_MSG_BG_SYSTEM_HORDE,
        CHAT_MSG_RAID_LEADER,
        CHAT_MSG_RAID_WARNING,
        CHAT_MSG_RAID_BOSS_WHISPER,
        CHAT_MSG_RAID_BOSS_EMOTE,
        CHAT_MSG_FILTERED,
        CHAT_MSG_RESTRICTED,
        CHAT_MSG_ACHIEVEMENT,
        CHAT_MSG_GUILD_ACHIEVEMENT,
        CHAT_MSG_PET_BATTLE_COMBAT_LOG,
        CHAT_MSG_PET_BATTLE_INFO,
        CHAT_MSG_INSTANCE_CHAT,
        CHAT_MSG_INSTANCE_CHAT_LEADER,
        RAID_BOSS_WHISPER,
        RAID_BOSS_EMOTE,
        QUEST_BOSS_EMOTE,
        LANGUAGE_LIST_CHANGED,
        ALTERNATIVE_DEFAULT_LANGUAGE_CHANGED,
        TIME_PLAYED_MSG,
        SPELLS_CHANGED,
        CURRENT_SPELL_CAST_CHANGED,
        SPELL_UPDATE_COOLDOWN,
        SPELL_UPDATE_USABLE,
        SPELL_UPDATE_CHARGES,
        CHARACTER_POINTS_CHANGED,
        SKILL_LINES_CHANGED,
        ITEM_PUSH,
        LOOT_OPENED,
        LOOT_READY,
        LOOT_SLOT_CLEARED,
        LOOT_SLOT_CHANGED,
        LOOT_CLOSED,
        PLAYER_LOGIN,
        PLAYER_LOGOUT,
        PLAYER_ENTERING_WORLD,
        PLAYER_LEAVING_WORLD,
        PLAYER_ALIVE,
        PLAYER_DEAD,
        PLAYER_CAMPING,
        PLAYER_QUITING,
        LOGOUT_CANCEL,
        RESURRECT_REQUEST,
        PARTY_INVITE_REQUEST,
        PARTY_INVITE_CANCEL,
        GUILD_INVITE_REQUEST,
        GUILD_INVITE_CANCEL,
        GUILD_MOTD,
        TRADE_REQUEST,
        TRADE_REQUEST_CANCEL,
        LOOT_BIND_CONFIRM,
        EQUIP_BIND_CONFIRM,
        AUTOEQUIP_BIND_CONFIRM,
        USE_BIND_CONFIRM,
        DELETE_ITEM_CONFIRM,
        CURSOR_UPDATE,
        ITEM_TEXT_BEGIN,
        ITEM_TEXT_TRANSLATION,
        ITEM_TEXT_READY,
        ITEM_TEXT_CLOSED,
        GOSSIP_SHOW,
        GOSSIP_CONFIRM,
        GOSSIP_CONFIRM_CANCEL,
        GOSSIP_ENTER_CODE,
        GOSSIP_CLOSED,
        QUEST_GREETING,
        QUEST_DETAIL,
        QUEST_PROGRESS,
        QUEST_COMPLETE,
        QUEST_FINISHED,
        QUEST_ITEM_UPDATE,
        QUEST_AUTOCOMPLETE,
        TAXIMAP_OPENED,
        TAXIMAP_CLOSED,
        QUEST_LOG_UPDATE,
        TRAINER_SHOW,
        TRAINER_UPDATE,
        TRAINER_DESCRIPTION_UPDATE,
        TRAINER_CLOSED,
        CVAR_UPDATE,
        TRADE_SKILL_SHOW,
        TRADE_SKILL_UPDATE,
        TRADE_SKILL_NAME_UPDATE,
        TRADE_SKILL_CLOSE,
        MERCHANT_SHOW,
        MERCHANT_UPDATE,
        MERCHANT_FILTER_ITEM_UPDATE,
        MERCHANT_CLOSED,
        TRADE_SHOW,
        TRADE_CLOSED,
        TRADE_UPDATE,
        TRADE_ACCEPT_UPDATE,
        TRADE_TARGET_ITEM_CHANGED,
        TRADE_PLAYER_ITEM_CHANGED,
        TRADE_MONEY_CHANGED,
        PLAYER_TRADE_MONEY,
        BAG_OPEN,
        BAG_UPDATE,
        BAG_UPDATE_DELAYED,
        BAG_CLOSED,
        BAG_UPDATE_COOLDOWN,
        BAG_NEW_ITEMS_UPDATED,
        BAG_SLOT_FLAGS_UPDATED,
        BANK_BAG_SLOT_FLAGS_UPDATED,
        LOCALPLAYER_PET_RENAMED,
        UNIT_ATTACK,
        UNIT_DEFENSE,
        PET_ATTACK_START,
        PET_ATTACK_STOP,
        UPDATE_MOUSEOVER_UNIT,
        UNIT_SPELLCAST_SENT,
        UNIT_SPELLCAST_START,
        UNIT_SPELLCAST_STOP,
        UNIT_SPELLCAST_FAILED,
        UNIT_SPELLCAST_FAILED_QUIET,
        UNIT_SPELLCAST_INTERRUPTED,
        UNIT_SPELLCAST_DELAYED,
        UNIT_SPELLCAST_SUCCEEDED,
        UNIT_SPELLCAST_CHANNEL_START,
        UNIT_SPELLCAST_CHANNEL_UPDATE,
        UNIT_SPELLCAST_CHANNEL_STOP,
        UNIT_SPELLCAST_INTERRUPTIBLE,
        UNIT_SPELLCAST_NOT_INTERRUPTIBLE,
        PLAYER_GUILD_UPDATE,
        QUEST_ACCEPT_CONFIRM,
        PLAYERBANKSLOTS_CHANGED,
        PLAYERREAGENTBANKSLOTS_CHANGED,
        BANKFRAME_OPENED,
        BANKFRAME_CLOSED,
        PLAYERBANKBAGSLOTS_CHANGED,
        REAGENTBANK_PURCHASED,
        REAGENTBANK_UPDATE,
        FRIENDLIST_UPDATE,
        IGNORELIST_UPDATE,
        MUTELIST_UPDATE,
        PET_BAR_UPDATE,
        PET_BAR_UPDATE_COOLDOWN,
        PET_BAR_SHOWGRID,
        PET_BAR_HIDEGRID,
        PET_BAR_HIDE,
        PET_BAR_UPDATE_USABLE,
        MINIMAP_PING,
        MIRROR_TIMER_START,
        MIRROR_TIMER_PAUSE,
        MIRROR_TIMER_STOP,
        WORLD_MAP_UPDATE,
        AUTOFOLLOW_BEGIN,
        AUTOFOLLOW_END,
        CINEMATIC_START,
        CINEMATIC_STOP,
        UPDATE_FACTION,
        CLOSE_WORLD_MAP,
        OPEN_TABARD_FRAME,
        CLOSE_TABARD_FRAME,
        TABARD_CANSAVE_CHANGED,
        GUILD_REGISTRAR_SHOW,
        GUILD_REGISTRAR_CLOSED,
        DUEL_REQUESTED,
        DUEL_OUTOFBOUNDS,
        DUEL_INBOUNDS,
        DUEL_FINISHED,
        TUTORIAL_TRIGGER,
        PET_DISMISS_START,
        UPDATE_BINDINGS,
        UPDATE_SHAPESHIFT_FORMS,
        UPDATE_SHAPESHIFT_FORM,
        UPDATE_SHAPESHIFT_USABLE,
        UPDATE_SHAPESHIFT_COOLDOWN,
        WHO_LIST_UPDATE,
        PETITION_SHOW,
        PETITION_CLOSED,
        EXECUTE_CHAT_LINE,
        UPDATE_MACROS,
        UPDATE_TICKET,
        UPDATE_WEB_TICKET,
        UPDATE_CHAT_WINDOWS,
        CONFIRM_XP_LOSS,
        CORPSE_IN_RANGE,
        CORPSE_IN_INSTANCE,
        CORPSE_OUT_OF_RANGE,
        UPDATE_GM_STATUS,
        PLAYER_UNGHOST,
        BIND_ENCHANT,
        REPLACE_ENCHANT,
        TRADE_REPLACE_ENCHANT,
        TRADE_POTENTIAL_BIND_ENCHANT,
        PLAYER_UPDATE_RESTING,
        UPDATE_EXHAUSTION,
        PLAYER_FLAGS_CHANGED,
        GUILD_ROSTER_UPDATE,
        GM_PLAYER_INFO,
        MAIL_SHOW,
        MAIL_CLOSED,
        SEND_MAIL_MONEY_CHANGED,
        SEND_MAIL_COD_CHANGED,
        MAIL_SEND_INFO_UPDATE,
        MAIL_SEND_SUCCESS,
        MAIL_INBOX_UPDATE,
        MAIL_LOCK_SEND_ITEMS,
        MAIL_UNLOCK_SEND_ITEMS,
        BATTLEFIELDS_SHOW,
        BATTLEFIELDS_CLOSED,
        UPDATE_BATTLEFIELD_STATUS,
        UPDATE_BATTLEFIELD_SCORE,
        BATTLEFIELD_QUEUE_TIMEOUT,
        AUCTION_HOUSE_SHOW,
        AUCTION_HOUSE_CLOSED,
        NEW_AUCTION_UPDATE,
        AUCTION_ITEM_LIST_UPDATE,
        AUCTION_OWNED_LIST_UPDATE,
        AUCTION_BIDDER_LIST_UPDATE,
        PET_UI_UPDATE,
        PET_UI_CLOSE,
        SAVED_VARIABLES_TOO_LARGE,
        VARIABLES_LOADED,
        MACRO_ACTION_FORBIDDEN,
        MACRO_ACTION_BLOCKED,
        START_AUTOREPEAT_SPELL,
        STOP_AUTOREPEAT_SPELL,
        PET_STABLE_SHOW,
        PET_STABLE_UPDATE,
        PET_STABLE_UPDATE_PAPERDOLL,
        PET_STABLE_CLOSED,
        RAID_ROSTER_UPDATE,
        UPDATE_PENDING_MAIL,
        UPDATE_INVENTORY_ALERTS,
        UPDATE_INVENTORY_DURABILITY,
        UPDATE_TRADESKILL_RECAST,
        OPEN_MASTER_LOOT_LIST,
        UPDATE_MASTER_LOOT_LIST,
        START_LOOT_ROLL,
        CANCEL_LOOT_ROLL,
        CONFIRM_LOOT_ROLL,
        CONFIRM_DISENCHANT_ROLL,
        INSTANCE_BOOT_START,
        INSTANCE_BOOT_STOP,
        LEARNED_SPELL_IN_TAB,
        CONFIRM_TALENT_WIPE,
        CONFIRM_BINDER,
        MAIL_FAILED,
        CLOSE_INBOX_ITEM,
        CONFIRM_SUMMON,
        CANCEL_SUMMON,
        BILLING_NAG_DIALOG,
        IGR_BILLING_NAG_DIALOG,
        PLAYER_SKINNED,
        TABARD_SAVE_PENDING,
        UNIT_QUEST_LOG_CHANGED,
        PLAYER_PVP_KILLS_CHANGED,
        PLAYER_PVP_RANK_CHANGED,
        INSPECT_HONOR_UPDATE,
        UPDATE_WORLD_STATES,
        AREA_SPIRIT_HEALER_IN_RANGE,
        AREA_SPIRIT_HEALER_OUT_OF_RANGE,
        PLAYTIME_CHANGED,
        UPDATE_LFG_TYPES,
        UPDATE_LFG_LIST,
        UPDATE_LFG_LIST_INCREMENTAL,
        LFG_LIST_AVAILABILITY_UPDATE,
        LFG_LIST_ACTIVE_ENTRY_UPDATE,
        LFG_LIST_ENTRY_CREATION_FAILED,
        LFG_LIST_SEARCH_RESULTS_RECEIVED,
        LFG_LIST_SEARCH_RESULT_UPDATED,
        LFG_LIST_SEARCH_FAILED,
        LFG_LIST_APPLICANT_UPDATED,
        LFG_LIST_APPLICANT_LIST_UPDATED,
        LFG_LIST_JOINED_GROUP,
        LFG_LIST_ENTRY_EXPIRED_TOO_MANY_PLAYERS,
        LFG_LIST_ENTRY_EXPIRED_TIMEOUT,
        LFG_LIST_APPLICATION_STATUS_UPDATED,
        READY_CHECK,
        READY_CHECK_CONFIRM,
        READY_CHECK_FINISHED,
        RAID_TARGET_UPDATE,
        GMSURVEY_DISPLAY,
        UPDATE_INSTANCE_INFO,
        SOCKET_INFO_UPDATE,
        SOCKET_INFO_ACCEPT,
        SOCKET_INFO_SUCCESS,
        SOCKET_INFO_CLOSE,
        PETITION_VENDOR_SHOW,
        PETITION_VENDOR_CLOSED,
        PETITION_VENDOR_UPDATE,
        COMBAT_TEXT_UPDATE,
        QUEST_WATCH_UPDATE,
        QUEST_WATCH_LIST_CHANGED,
        QUEST_WATCH_OBJECTIVES_CHANGED,
        KNOWLEDGE_BASE_SETUP_LOAD_SUCCESS,
        KNOWLEDGE_BASE_SETUP_LOAD_FAILURE,
        KNOWLEDGE_BASE_QUERY_LOAD_SUCCESS,
        KNOWLEDGE_BASE_QUERY_LOAD_FAILURE,
        KNOWLEDGE_BASE_ARTICLE_LOAD_SUCCESS,
        KNOWLEDGE_BASE_ARTICLE_LOAD_FAILURE,
        KNOWLEDGE_BASE_SYSTEM_MOTD_UPDATED,
        KNOWLEDGE_BASE_SERVER_MESSAGE,
        KNOWN_TITLES_UPDATE,
        NEW_TITLE_EARNED,
        OLD_TITLE_LOST,
        LFG_UPDATE,
        LFG_PROPOSAL_UPDATE,
        LFG_PROPOSAL_SHOW,
        LFG_PROPOSAL_FAILED,
        LFG_PROPOSAL_SUCCEEDED,
        LFG_ROLE_UPDATE,
        LFG_ROLE_CHECK_UPDATE,
        LFG_ROLE_CHECK_SHOW,
        LFG_ROLE_CHECK_HIDE,
        LFG_ROLE_CHECK_ROLE_CHOSEN,
        LFG_QUEUE_STATUS_UPDATE,
        LFG_BOOT_PROPOSAL_UPDATE,
        LFG_LOCK_INFO_RECEIVED,
        LFG_UPDATE_RANDOM_INFO,
        LFG_OFFER_CONTINUE,
        LFG_OPEN_FROM_GOSSIP,
        LFG_COMPLETION_REWARD,
        LFG_INVALID_ERROR_MESSAGE,
        LFG_READY_CHECK_UPDATE,
        LFG_READY_CHECK_SHOW,
        LFG_READY_CHECK_HIDE,
        LFG_READY_CHECK_DECLINED,
        LFG_READY_CHECK_PLAYER_IS_READY,
        LFG_ROLE_CHECK_DECLINED,
        PARTY_LFG_RESTRICTED,
        PLAYER_ROLES_ASSIGNED,
        COMBAT_RATING_UPDATE,
        MODIFIER_STATE_CHANGED,
        UPDATE_STEALTH,
        ENABLE_TAXI_BENCHMARK,
        DISABLE_TAXI_BENCHMARK,
        VOICE_START,
        VOICE_STOP,
        VOICE_STATUS_UPDATE,
        VOICE_CHANNEL_STATUS_UPDATE,
        UPDATE_FLOATING_CHAT_WINDOWS,
        RAID_INSTANCE_WELCOME,
        MOVIE_RECORDING_PROGRESS,
        MOVIE_COMPRESSING_PROGRESS,
        MOVIE_UNCOMPRESSED_MOVIE,
        VOICE_PUSH_TO_TALK_START,
        VOICE_PUSH_TO_TALK_STOP,
        GUILDBANKFRAME_OPENED,
        GUILDBANKFRAME_CLOSED,
        GUILDBANKBAGSLOTS_CHANGED,
        GUILDBANK_ITEM_LOCK_CHANGED,
        GUILDBANK_UPDATE_TABS,
        GUILDBANK_UPDATE_MONEY,
        GUILDBANKLOG_UPDATE,
        GUILDBANK_UPDATE_WITHDRAWMONEY,
        GUILDBANK_UPDATE_TEXT,
        GUILDBANK_TEXT_CHANGED,
        CHANNEL_UI_UPDATE,
        CHANNEL_COUNT_UPDATE,
        CHANNEL_ROSTER_UPDATE,
        CHANNEL_VOICE_UPDATE,
        CHANNEL_INVITE_REQUEST,
        CHANNEL_PASSWORD_REQUEST,
        CHANNEL_FLAGS_UPDATED,
        VOICE_SESSIONS_UPDATE,
        VOICE_CHAT_ENABLED_UPDATE,
        VOICE_LEFT_SESSION,
        INSPECT_READY,
        VOICE_SELF_MUTE,
        VOICE_PLATE_START,
        VOICE_PLATE_STOP,
        ARENA_SEASON_WORLD_STATE,
        GUILD_EVENT_LOG_UPDATE,
        GUILDTABARD_UPDATE,
        SOUND_DEVICE_UPDATE,
        COMMENTATOR_MAP_UPDATE,
        COMMENTATOR_ENTER_WORLD,
        COMBAT_LOG_EVENT,
        COMBAT_LOG_EVENT_UNFILTERED,
        COMMENTATOR_PLAYER_UPDATE,
        COMMENTATOR_PLAYER_NAME_OVERRIDE_UPDATE,
        PLAYER_ENTERING_BATTLEGROUND,
        BARBER_SHOP_OPEN,
        BARBER_SHOP_CLOSE,
        BARBER_SHOP_SUCCESS,
        BARBER_SHOP_APPEARANCE_APPLIED,
        CALENDAR_UPDATE_INVITE_LIST,
        CALENDAR_UPDATE_EVENT_LIST,
        CALENDAR_NEW_EVENT,
        CALENDAR_OPEN_EVENT,
        CALENDAR_CLOSE_EVENT,
        CALENDAR_UPDATE_EVENT,
        CALENDAR_UPDATE_PENDING_INVITES,
        CALENDAR_EVENT_ALARM,
        CALENDAR_UPDATE_ERROR,
        CALENDAR_ACTION_PENDING,
        CALENDAR_UPDATE_GUILD_EVENTS,
        VEHICLE_ANGLE_SHOW,
        VEHICLE_ANGLE_UPDATE,
        VEHICLE_POWER_SHOW,
        UNIT_ENTERING_VEHICLE,
        UNIT_ENTERED_VEHICLE,
        UNIT_EXITING_VEHICLE,
        UNIT_EXITED_VEHICLE,
        VEHICLE_PASSENGERS_CHANGED,
        PLAYER_GAINS_VEHICLE_DATA,
        PLAYER_LOSES_VEHICLE_DATA,
        PET_FORCE_NAME_DECLENSION,
        LEVEL_GRANT_PROPOSED,
        SYNCHRONIZE_SETTINGS,
        PLAY_MOVIE,
        RUNE_POWER_UPDATE,
        RUNE_TYPE_UPDATE,
        ACHIEVEMENT_EARNED,
        CRITERIA_EARNED,
        CRITERIA_COMPLETE,
        CRITERIA_UPDATE,
        RECEIVED_ACHIEVEMENT_LIST,
        PET_RENAMEABLE,
        CURRENCY_DISPLAY_UPDATE,
        COMPANION_LEARNED,
        COMPANION_UNLEARNED,
        COMPANION_UPDATE,
        UNIT_THREAT_LIST_UPDATE,
        UNIT_THREAT_SITUATION_UPDATE,
        GLYPH_ADDED,
        GLYPH_REMOVED,
        GLYPH_UPDATED,
        GLYPH_ENABLED,
        GLYPH_DISABLED,
        USE_GLYPH,
        TRACKED_ACHIEVEMENT_UPDATE,
        TRACKED_ACHIEVEMENT_LIST_CHANGED,
        ARENA_OPPONENT_UPDATE,
        INSPECT_ACHIEVEMENT_READY,
        RAISED_AS_GHOUL,
        PARTY_CONVERTED_TO_RAID,
        PVPQUEUE_ANYWHERE_SHOW,
        PVPQUEUE_ANYWHERE_UPDATE_AVAILABLE,
        QUEST_ACCEPTED,
        QUEST_TURNED_IN,
        PLAYER_SPECIALIZATION_CHANGED,
        PLAYER_TALENT_UPDATE,
        ACTIVE_TALENT_GROUP_CHANGED,
        PLAYER_CHARACTER_UPGRADE_TALENT_COUNT_CHANGED,
        PET_SPECIALIZATION_CHANGED,
        PREVIEW_TALENT_POINTS_CHANGED,
        PREVIEW_TALENT_PRIMARY_TREE_CHANGED,
        WEAR_EQUIPMENT_SET,
        EQUIPMENT_SETS_CHANGED,
        INSTANCE_LOCK_START,
        INSTANCE_LOCK_STOP,
        INSTANCE_LOCK_WARNING,
        PLAYER_EQUIPMENT_CHANGED,
        ITEM_LOCKED,
        ITEM_UNLOCKED,
        TRADE_SKILL_FILTER_UPDATE,
        EQUIPMENT_SWAP_PENDING,
        EQUIPMENT_SWAP_FINISHED,
        NPC_PVPQUEUE_ANYWHERE,
        UPDATE_MULTI_CAST_ACTIONBAR,
        ENABLE_XP_GAIN,
        DISABLE_XP_GAIN,
        UPDATE_EXPANSION_LEVEL,
        BATTLEFIELD_MGR_ENTRY_INVITE,
        BATTLEFIELD_MGR_ENTERED,
        BATTLEFIELD_MGR_QUEUE_REQUEST_RESPONSE,
        BATTLEFIELD_MGR_QUEUE_STATUS_UPDATE,
        BATTLEFIELD_MGR_EJECT_PENDING,
        BATTLEFIELD_MGR_EJECTED,
        BATTLEFIELD_MGR_DROP_TIMER_STARTED,
        BATTLEFIELD_MGR_DROP_TIMER_CANCELED,
        BATTLEFIELD_MGR_QUEUE_INVITE,
        BATTLEFIELD_MGR_STATE_CHANGE,
        PVP_TYPES_ENABLED,
        WORLD_STATE_UI_TIMER_UPDATE,
        WORLD_STATE_TIMER_START,
        WORLD_STATE_TIMER_STOP,
        END_BOUND_TRADEABLE,
        UPDATE_CHAT_COLOR_NAME_BY_CLASS,
        GMRESPONSE_RECEIVED,
        VEHICLE_UPDATE,
        WOW_MOUSE_NOT_FOUND,
        CHAT_COMBAT_MSG_ARENA_POINTS_GAIN,
        MAIL_SUCCESS,
        TALENTS_INVOLUNTARILY_RESET,
        INSTANCE_ENCOUNTER_ENGAGE_UNIT,
        QUEST_POI_UPDATE,
        PLAYER_DIFFICULTY_CHANGED,
        CHAT_MSG_PARTY_LEADER,
        VOTE_KICK_REASON_NEEDED,
        ENABLE_LOW_LEVEL_RAID,
        DISABLE_LOW_LEVEL_RAID,
        CHAT_MSG_TARGETICONS,
        AUCTION_HOUSE_DISABLED,
        AUCTION_MULTISELL_START,
        AUCTION_MULTISELL_UPDATE,
        AUCTION_MULTISELL_FAILURE,
        PET_SPELL_POWER_UPDATE,
        BN_CONNECTED,
        BN_DISCONNECTED,
        BN_SELF_ONLINE,
        BN_SELF_OFFLINE,
        BN_INFO_CHANGED,
        BN_FRIEND_LIST_SIZE_CHANGED,
        BN_FRIEND_INVITE_LIST_INITIALIZED,
        BN_FRIEND_INVITE_SEND_RESULT,
        BN_FRIEND_INVITE_ADDED,
        BN_FRIEND_INVITE_REMOVED,
        BN_FRIEND_INFO_CHANGED,
        BN_CUSTOM_MESSAGE_CHANGED,
        BN_CUSTOM_MESSAGE_LOADED,
        CHAT_MSG_BN_WHISPER,
        CHAT_MSG_BN_WHISPER_INFORM,
        BN_CHAT_WHISPER_UNDELIVERABLE,
        BN_CHAT_CHANNEL_JOINED,
        BN_CHAT_CHANNEL_LEFT,
        BN_CHAT_CHANNEL_CLOSED,
        CHAT_MSG_BN_CONVERSATION,
        CHAT_MSG_BN_CONVERSATION_NOTICE,
        CHAT_MSG_BN_CONVERSATION_LIST,
        BN_CHAT_CHANNEL_MESSAGE_UNDELIVERABLE,
        BN_CHAT_CHANNEL_MESSAGE_BLOCKED,
        BN_CHAT_CHANNEL_MEMBER_JOINED,
        BN_CHAT_CHANNEL_MEMBER_LEFT,
        BN_CHAT_CHANNEL_MEMBER_UPDATED,
        BN_CHAT_CHANNEL_CREATE_SUCCEEDED,
        BN_CHAT_CHANNEL_CREATE_FAILED,
        BN_CHAT_CHANNEL_INVITE_SUCCEEDED,
        BN_CHAT_CHANNEL_INVITE_FAILED,
        BN_BLOCK_LIST_UPDATED,
        BN_SYSTEM_MESSAGE,
        BN_REQUEST_FOF_SUCCEEDED,
        BN_REQUEST_FOF_FAILED,
        BN_NEW_PRESENCE,
        BN_TOON_NAME_UPDATED,
        BN_FRIEND_ACCOUNT_ONLINE,
        BN_FRIEND_ACCOUNT_OFFLINE,
        BN_FRIEND_TOON_ONLINE,
        BN_FRIEND_TOON_OFFLINE,
        BN_MATURE_LANGUAGE_FILTER,
        BATTLETAG_INVITE_SHOW,
        MASTERY_UPDATE,
        AMPLIFY_UPDATE,
        MULTISTRIKE_UPDATE,
        READINESS_UPDATE,
        SPEED_UPDATE,
        LIFESTEAL_UPDATE,
        AVOIDANCE_UPDATE,
        STURDINESS_UPDATE,
        CLEAVE_UPDATE,
        COMMENTATOR_PARTY_INFO_REQUEST,
        CHAT_MSG_BN_INLINE_TOAST_ALERT,
        CHAT_MSG_BN_INLINE_TOAST_BROADCAST,
        CHAT_MSG_BN_INLINE_TOAST_BROADCAST_INFORM,
        CHAT_MSG_BN_INLINE_TOAST_CONVERSATION,
        CHAT_MSG_BN_WHISPER_PLAYER_OFFLINE,
        PLAYER_TRADE_CURRENCY,
        TRADE_CURRENCY_CHANGED,
        WEIGHTED_SPELL_UPDATED,
        GUILD_XP_UPDATE,
        GUILD_PERK_UPDATE,
        GUILD_TRADESKILL_UPDATE,
        UNIT_POWER,
        UNIT_POWER_FREQUENT,
        UNIT_MAXPOWER,
        ENABLE_DECLINE_GUILD_INVITE,
        DISABLE_DECLINE_GUILD_INVITE,
        GUILD_RECIPE_KNOWN_BY_MEMBERS,
        ARTIFACT_UPDATE,
        ARTIFACT_HISTORY_READY,
        ARTIFACT_COMPLETE,
        ARTIFACT_DIG_SITE_UPDATED,
        ARCHAEOLOGY_TOGGLE,
        ARCHAEOLOGY_CLOSED,
        ARTIFACT_DIGSITE_COMPLETE,
        ARCHAEOLOGY_FIND_COMPLETE,
        ARCHAEOLOGY_SURVEY_CAST,
        SPELL_FLYOUT_UPDATE,
        UNIT_CONNECTION,
        UNIT_HEAL_PREDICTION,
        ENTERED_DIFFERENT_INSTANCE_FROM_PARTY,
        ROLE_CHANGED_INFORM,
        GUILD_REWARDS_LIST,
        ROLE_POLL_BEGIN,
        REQUEST_CEMETERY_LIST_RESPONSE,
        WARGAME_REQUESTED,
        GUILD_NEWS_UPDATE,
        CHAT_SERVER_DISCONNECTED,
        CHAT_SERVER_RECONNECTED,
        STREAMING_ICON,
        RECEIVED_ACHIEVEMENT_MEMBER_LIST,
        SPELL_ACTIVATION_OVERLAY_SHOW,
        SPELL_ACTIVATION_OVERLAY_HIDE,
        SPELL_ACTIVATION_OVERLAY_GLOW_SHOW,
        SPELL_ACTIVATION_OVERLAY_GLOW_HIDE,
        UNIT_PHASE,
        UNIT_POWER_BAR_SHOW,
        UNIT_POWER_BAR_HIDE,
        UNIT_POWER_BAR_TIMER_UPDATE,
        GUILD_RANKS_UPDATE,
        PVP_RATED_STATS_UPDATE,
        PVP_REWARDS_UPDATE,
        CHAT_MSG_COMBAT_GUILD_XP_GAIN,
        UNIT_GUILD_LEVEL,
        GUILD_PARTY_STATE_UPDATED,
        GET_ITEM_INFO_RECEIVED,
        MAX_SPELL_START_RECOVERY_OFFSET_CHANGED,
        UNIT_HEALTH_FREQUENT,
        GUILD_REP_UPDATED,
        BN_BLOCK_FAILED_TOO_MANY,
        SPELL_PUSHED_TO_ACTIONBAR,
        START_TIMER,
        LF_GUILD_POST_UPDATED,
        LF_GUILD_BROWSE_UPDATED,
        LF_GUILD_RECRUITS_UPDATED,
        LF_GUILD_MEMBERSHIP_LIST_UPDATED,
        LF_GUILD_RECRUIT_LIST_CHANGED,
        LF_GUILD_MEMBERSHIP_LIST_CHANGED,
        GUILD_CHALLENGE_UPDATED,
        GUILD_CHALLENGE_COMPLETED,
        RESTRICTED_ACCOUNT_WARNING,
        EJ_LOOT_DATA_RECIEVED,
        EJ_DIFFICULTY_UPDATE,
        AJ_REWARD_DATA_RECIEVED,
        COMPACT_UNIT_FRAME_PROFILES_LOADED,
        CONFIRM_BEFORE_USE,
        CLEAR_BOSS_EMOTES,
        INCOMING_RESURRECT_CHANGED,
        TRIAL_CAP_REACHED_MONEY,
        TRIAL_CAP_REACHED_LEVEL,
        REQUIRED_GUILD_RENAME_RESULT,
        GUILD_RENAME_REQUIRED,
        ECLIPSE_DIRECTION_CHANGE,
        TRANSMOGRIFY_OPEN,
        TRANSMOGRIFY_CLOSE,
        TRANSMOGRIFY_UPDATE,
        TRANSMOGRIFY_SUCCESS,
        TRANSMOGRIFY_BIND_CONFIRM,
        VOID_STORAGE_OPEN,
        VOID_STORAGE_CLOSE,
        VOID_STORAGE_UPDATE,
        VOID_STORAGE_CONTENTS_UPDATE,
        VOID_STORAGE_DEPOSIT_UPDATE,
        VOID_TRANSFER_DONE,
        VOID_DEPOSIT_WARNING,
        INVENTORY_SEARCH_UPDATE,
        PLAYER_REPORT_SUBMITTED,
        SOR_BY_TEXT_UPDATED,
        MISSING_OUT_ON_LOOT,
        INELIGIBLE_FOR_LOOT,
        SHOW_FACTION_SELECT_UI,
        NEUTRAL_FACTION_SELECT_RESULT,
        SOR_START_EXPERIENCE_INCOMPLETE,
        SOR_COUNTS_UPDATED,
        SELF_RES_SPELL_CHANGED,
        SESSION_TIME_ALERT,
        PET_JOURNAL_LIST_UPDATE,
        BATTLE_PET_CURSOR_CLEAR,
        MOUNT_CURSOR_CLEAR,
        GROUP_ROSTER_UPDATE,
        GROUP_JOINED,
        PVP_POWER_UPDATE,
        PET_BATTLE_OPENING_START,
        PET_BATTLE_OPENING_DONE,
        PET_BATTLE_HEALTH_CHANGED,
        PET_BATTLE_MAX_HEALTH_CHANGED,
        PET_BATTLE_TURN_STARTED,
        PET_BATTLE_PET_CHANGED,
        PET_BATTLE_ABILITY_CHANGED,
        PET_BATTLE_CAPTURED,
        PET_BATTLE_XP_CHANGED,
        PET_BATTLE_LEVEL_CHANGED,
        PET_BATTLE_FINAL_ROUND,
        PET_BATTLE_OVER,
        PET_BATTLE_CLOSE,
        PET_BATTLE_PET_ROUND_RESULTS,
        PET_BATTLE_PET_ROUND_PLAYBACK_COMPLETE,
        PET_BATTLE_ACTION_SELECTED,
        PET_BATTLE_AURA_APPLIED,
        PET_BATTLE_AURA_CHANGED,
        PET_BATTLE_AURA_CANCELED,
        PET_BATTLE_PVP_DUEL_REQUESTED,
        PET_BATTLE_PVP_DUEL_REQUEST_CANCEL,
        PET_BATTLE_QUEUE_PROPOSE_MATCH,
        PET_BATTLE_QUEUE_PROPOSAL_DECLINED,
        PET_BATTLE_QUEUE_PROPOSAL_ACCEPTED,
        PET_BATTLE_PET_TYPE_CHANGED,
        CHALLENGE_MODE_MAPS_UPDATE,
        CHALLENGE_MODE_START,
        CHALLENGE_MODE_RESET,
        CHALLENGE_MODE_COMPLETED,
        CHALLENGE_MODE_NEW_RECORD,
        CHALLENGE_MODE_LEADERS_UPDATE,
        SPELL_CONFIRMATION_PROMPT,
        SPELL_CONFIRMATION_TIMEOUT,
        BONUS_ROLL_ACTIVATE,
        BONUS_ROLL_DEACTIVATE,
        BONUS_ROLL_STARTED,
        BONUS_ROLL_FAILED,
        BONUS_ROLL_RESULT,
        SHOW_LOOT_TOAST,
        SHOW_LOOT_TOAST_UPGRADE,
        SHOW_PVP_FACTION_LOOT_TOAST,
        BLACK_MARKET_OPEN,
        BLACK_MARKET_CLOSE,
        BLACK_MARKET_UNAVAILABLE,
        BLACK_MARKET_ITEM_UPDATE,
        BLACK_MARKET_BID_RESULT,
        BLACK_MARKET_OUTBID,
        BLACK_MARKET_WON,
        QUICK_TICKET_SYSTEM_STATUS,
        ITEM_RESTORATION_BUTTON_STATUS,
        QUICK_TICKET_THROTTLE_CHANGED,
        LOOT_ITEM_AVAILABLE,
        LOOT_ROLLS_COMPLETE,
        LOOT_ITEM_ROLL_WON,
        SCENARIO_UPDATE,
        SCENARIO_CRITERIA_UPDATE,
        SCENARIO_POI_UPDATE,
        SCENARIO_COMPLETED,
        PET_JOURNAL_PET_DELETED,
        PET_JOURNAL_PET_REVOKED,
        PET_JOURNAL_PET_RESTORED,
        PET_JOURNAL_CAGE_FAILED,
        LOOT_HISTORY_FULL_UPDATE,
        LOOT_HISTORY_ROLL_COMPLETE,
        LOOT_HISTORY_ROLL_CHANGED,
        LOOT_HISTORY_AUTO_SHOW,
        ITEM_UPGRADE_MASTER_OPENED,
        ITEM_UPGRADE_MASTER_CLOSED,
        ITEM_UPGRADE_MASTER_SET_ITEM,
        ITEM_UPGRADE_MASTER_UPDATE,
        PET_JOURNAL_PETS_HEALED,
        PET_JOURNAL_NEW_BATTLE_SLOT,
        PET_JOURNAL_TRAP_LEVEL_SET,
        UNIT_OTHER_PARTY_CHANGED,
        ARENA_PREP_OPPONENT_SPECIALIZATIONS,
        PET_JOURNAL_AUTO_SLOTTED_PET,
        PET_BATTLE_QUEUE_STATUS,
        SPELL_POWER_CHANGED,
        SCRIPT_ACHIEVEMENT_PLAYER_NAME,
        NEW_WMO_CHUNK,
        PET_BATTLE_LOOT_RECEIVED,
        LOSS_OF_CONTROL_ADDED,
        LOSS_OF_CONTROL_UPDATE,
        QUEST_CHOICE_UPDATE,
        QUEST_CHOICE_CLOSE,
        BATTLEPET_FORCE_NAME_DECLENSION,
        UNIT_ABSORB_AMOUNT_CHANGED,
        UNIT_HEAL_ABSORB_AMOUNT_CHANGED,
        LFG_BONUS_FACTION_ID_UPDATED,
        MAP_BAR_UPDATE,
        LOADING_SCREEN_ENABLED,
        LOADING_SCREEN_DISABLED,
        BATTLEGROUND_POINTS_UPDATE,
        BATTLEGROUND_OBJECTIVES_UPDATE,
        PLAYER_LOOT_SPEC_UPDATED,
        PVP_ROLE_UPDATE,
        SIMPLE_BROWSER_WEB_PROXY_FAILED,
        SIMPLE_BROWSER_WEB_ERROR,
        VIGNETTE_ADDED,
        VIGNETTE_REMOVED,
        ENCOUNTER_START,
        ENCOUNTER_END,
        BOSS_KILL,
        INSTANCE_GROUP_SIZE_CHANGED,
        SUPER_TRACKED_QUEST_CHANGED,
        PROVING_GROUNDS_SCORE_UPDATE,
        PRODUCT_CHOICE_UPDATE,
        RECRUIT_A_FRIEND_SYSTEM_STATUS,
        PLAYER_STARTED_MOVING,
        PLAYER_STOPPED_MOVING,
        RECRUIT_A_FRIEND_INVITATION_FAILED,
        RECRUIT_A_FRIEND_INVITER_FRIEND_ADDED,
        RECRUIT_A_FRIEND_CAN_EMAIL,
        LUA_WARNING,
        BN_CHAT_MSG_ADDON,
        MOUNT_JOURNAL_USABILITY_CHANGED,
        QUEST_REMOVED,
        TASK_PROGRESS_UPDATE,
        GARRISON_UPDATE,
        GARRISON_BUILDING_UPDATE,
        GARRISON_BUILDING_PLACED,
        GARRISON_BUILDING_REMOVED,
        GARRISON_BUILDING_LIST_UPDATE,
        GARRISON_BUILDING_ERROR,
        GARRISON_ARCHITECT_OPENED,
        GARRISON_ARCHITECT_CLOSED,
        GARRISON_MISSION_NPC_OPENED,
        GARRISON_MISSION_NPC_CLOSED,
        GARRISON_SHIPYARD_NPC_OPENED,
        GARRISON_SHIPYARD_NPC_CLOSED,
        GARRISON_BUILDING_ACTIVATED,
        GARRISON_BUILDING_ACTIVATABLE,
        GARRISON_MISSION_LIST_UPDATE,
        GARRISON_MISSION_STARTED,
        GARRISON_MISSION_COMPLETE_RESPONSE,
        GARRISON_MISSION_FINISHED,
        GARRISON_MISSION_BONUS_ROLL_COMPLETE,
        GARRISON_MISSION_BONUS_ROLL_LOOT,
        GARRISON_RANDOM_MISSION_ADDED,
        GARRISON_FOLLOWER_LIST_UPDATE,
        GARRISON_FOLLOWER_ADDED,
        GARRISON_FOLLOWER_REMOVED,
        GARRISON_FOLLOWER_XP_CHANGED,
        GARRISON_SHOW_LANDING_PAGE,
        GARRISON_HIDE_LANDING_PAGE,
        GARRISON_LANDINGPAGE_SHIPMENTS,
        SHIPMENT_CRAFTER_OPENED,
        SHIPMENT_CRAFTER_CLOSED,
        SHIPMENT_CRAFTER_INFO,
        SHIPMENT_CRAFTER_REAGENT_UPDATE,
        SHIPMENT_UPDATE,
        GARRISON_SHIPMENT_RECEIVED,
        GARRISON_RECRUITMENT_NPC_OPENED,
        GARRISON_RECRUITMENT_NPC_CLOSED,
        GARRISON_RECRUITMENT_FOLLOWERS_GENERATED,
        GARRISON_RECRUITMENT_READY,
        GARRISON_RECRUIT_FOLLOWER_RESULT,
        GARRISON_FOLLOWER_UPGRADED,
        GARRISON_MISSION_AREA_BONUS_ADDED,
        GARRISON_MONUMENT_SHOW_UI,
        GARRISON_MONUMENT_CLOSE_UI,
        GARRISON_MONUMENT_LIST_LOADED,
        GARRISON_MONUMENT_REPLACED,
        QUESTLINE_UPDATE,
        QUESTTASK_UPDATE,
        GARRISON_TRADESKILL_NPC_CLOSED,
        TOYS_UPDATED,
        HEIRLOOMS_UPDATED,
        HEIRLOOM_UPGRADE_TARGETING_CHANGED,
        GARRISON_MONUMENT_SELECTED_TROPHY_ID_LOADED,
        CHARACTER_UPGRADE_SPELL_TIER_SET,
        GARRISON_USE_PARTY_GARRISON_CHANGED,
        GARRISON_BUILDINGS_SWAPPED,
        GARRISON_RECALL_PORTAL_LAST_USED_TIME,
        GARRISON_RECALL_PORTAL_USED,
        GARRISON_INVASION_AVAILABLE,
        GARRISON_INVASION_UNAVAILABLE,
        GARRISON_UPGRADEABLE_RESULT,
        HEARTHSTONE_BOUND,
        NPE_TUTORIAL_UPDATE,
        TWITTER_STATUS_UPDATE,
        TWITTER_LINK_RESULT,
        TWITTER_POST_RESULT,
        SOCIAL_ITEM_RECEIVED,
        AJ_DUNGEON_ACTION,
        AJ_RAID_ACTION,
        AJ_PVP_ACTION,
        AJ_PVP_SKIRMISH_ACTION,
        AJ_PVE_LFG_ACTION,
        AJ_PVP_LFG_ACTION,
        AJ_PVP_RBG_ACTION,
        AJ_QUEST_LOG_OPEN,
        AJ_REFRESH_DISPLAY,
        AJ_OPEN,
        ENCOUNTER_LOOT_RECEIVED,
        SET_GLUE_SCREEN,
        START_GLUE_MUSIC,
        DISCONNECTED_FROM_SERVER,
        OPEN_STATUS_DIALOG,
        UPDATE_STATUS_DIALOG,
        CLOSE_STATUS_DIALOG,
        ADDON_LIST_UPDATE,
        CHARACTER_LIST_UPDATE,
        UPDATE_SELECTED_CHARACTER,
        OPEN_REALM_LIST,
        GET_PREFERRED_REALM_INFO,
        UPDATE_SELECTED_RACE,
        SELECT_LAST_CHARACTER,
        SELECT_FIRST_CHARACTER,
        GLUE_SCREENSHOT_SUCCEEDED,
        GLUE_SCREENSHOT_FAILED,
        PATCH_UPDATE_PROGRESS,
        PATCH_DOWNLOADED,
        SUGGEST_REALM,
        SUGGEST_REALM_WRONG_PVP,
        SUGGEST_REALM_WRONG_CATEGORY,
        SHOW_SERVER_ALERT,
        FRAMES_LOADED,
        FORCE_RENAME_CHARACTER,
        FORCE_DECLINE_CHARACTER,
        SHOW_SURVEY_NOTIFICATION,
        PLAYER_ENTER_PIN,
        CLIENT_ACCOUNT_MISMATCH,
        PLAYER_ENTER_MATRIX,
        SCANDLL_ERROR,
        SCANDLL_DOWNLOADING,
        SCANDLL_FINISHED,
        SERVER_SPLIT_NOTICE,
        TIMER_ALERT,
        ACCOUNT_MESSAGES_AVAILABLE,
        ACCOUNT_MESSAGES_HEADERS_LOADED,
        ACCOUNT_MESSAGES_BODY_LOADED,
        CLIENT_TRIAL,
        PLAYER_ENTER_TOKEN,
        GAME_ACCOUNTS_UPDATED,
        CLIENT_CONVERTED,
        RANDOM_CHARACTER_NAME_RESULT,
        ACCOUNT_DATA_INITIALIZED,
        GLUE_UPDATE_EXPANSION_LEVEL,
        DISPLAY_PROMOTION,
        LAUNCHER_LOGIN_STATUS_CHANGED,
        LOGIN_STARTED,
        LOGIN_STOPPED,
        SCREEN_FIRST_DISPLAYED,
        CHARACTER_UPGRADE_STARTED,
        CHARACTER_UPGRADE_ABORTED,
        VAS_CHARACTER_STATE_CHANGED,
        CHAR_RESTORE_COMPLETE,
        ACCOUNT_CHARACTER_LIST_RECIEVED,
        ACCOUNT_DATA_RESTORED,
        CHARACTER_UNDELETE_STATUS_CHANGED,
        CLIENT_FEATURE_STATUS_CHANGED,
        CHARACTER_UNDELETE_FINISHED,
        TOKEN_CAN_VETERAN_BUY_UPDATE,
        STORE_PRODUCTS_UPDATED,
        STORE_CONFIRM_PURCHASE,
        STORE_STATUS_CHANGED,
        STORE_PRODUCT_DELIVERED,
        STORE_PURCHASE_LIST_UPDATED,
        STORE_PURCHASE_ERROR,
        STORE_ORDER_INITIATION_FAILED,
        STORE_CHARACTER_LIST_RECEIVED,
        STORE_VAS_PURCHASE_ERROR,
        STORE_VAS_PURCHASE_COMPLETE,
        STORE_BOOST_AUTO_CONSUMED,
        AUTH_CHALLENGE_UI_INVALID,
        AUTH_CHALLENGE_FINISHED,
        ADDON_LOADED,
        ADDON_ACTION_FORBIDDEN,
        ADDON_ACTION_BLOCKED,
        UI_SCALE_CHANGED,
        DISPLAY_SIZE_CHANGED,
        PRODUCT_ASSIGN_TO_TARGET_FAILED,
        PRODUCT_DISTRIBUTIONS_UPDATED,
        TOKEN_DISTRIBUTIONS_UPDATED,
        TOKEN_BUY_RESULT,
        TOKEN_SELL_RESULT,
        TOKEN_AUCTION_SOLD,
        TOKEN_MARKET_PRICE_UPDATED,
        TOKEN_SELL_CONFIRM_REQUIRED,
        TOKEN_BUY_CONFIRM_REQUIRED,
        TOKEN_REDEEM_CONFIRM_REQUIRED,
        TOKEN_REDEEM_FRAME_SHOW,
        TOKEN_REDEEM_GAME_TIME_UPDATED,
        TOKEN_REDEEM_RESULT,
        TOKEN_STATUS_CHANGED
    }
}
