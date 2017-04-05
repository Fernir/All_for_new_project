﻿namespace nManager
{
    using nManager.Helpful;
    using nManager.Helpful.Forms;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Translate
    {
        private static Language _translate = new Language();

        public static string Get(Id id)
        {
            if ((_translate != null) && (_translate.Translations.Count != 0))
            {
                if (_translate.Translations.Count == 0)
                {
                    Load("English.xml");
                }
                foreach (Translation translation in from v in _translate.Translations
                    where v.Id == id
                    select v)
                {
                    if (!string.IsNullOrWhiteSpace(translation.Text))
                    {
                        return translation.Text;
                    }
                }
                foreach (Translation translation2 in XmlSerializer.Deserialize<Language>(Application.StartupPath + @"\Data\Lang\English.xml").Translations)
                {
                    if ((id == translation2.Id) && !string.IsNullOrWhiteSpace(translation2.Text))
                    {
                        _translate.Translations.Add(translation2);
                        return translation2.Text;
                    }
                }
            }
            return "";
        }

        public static bool Load(string fileName = "English.xml")
        {
            if (!File.Exists(Application.StartupPath + @"\Data\Lang\" + fileName))
            {
                fileName = "English.xml";
            }
            if (!File.Exists(Application.StartupPath + @"\Data\Lang\" + fileName))
            {
                Logging.WriteError("File '" + Application.StartupPath + @"\Data\Lang\" + fileName + "' does not exist!!!", true);
                new ErrorPopup("File '" + Application.StartupPath + @"\Data\Lang\" + fileName + "' not found!!\n\nPlease install all the required file for TheNoobBot to work properly.").ShowDialog();
                Process.GetCurrentProcess().Kill();
                return false;
            }
            _translate = new Language();
            _translate = XmlSerializer.Deserialize<Language>(Application.StartupPath + @"\Data\Lang\" + fileName) ?? XmlSerializer.Deserialize<Language>(Application.StartupPath + @"\Data\Lang\English.xml");
            return true;
        }

        [Serializable]
        public enum Id
        {
            none,
            In_progress,
            Please_select_game_Process_and_connect_to_the_game,
            Stop,
            Please_connect_to_the_game,
            The_Game_is_currently_used_by_TheNoobBot_or_contains_traces,
            If_no_others_session_of_TheNoobBot_is_currently_active,
            Use_this_Game,
            Select_game_process,
            Please_enter_your_user_name_and_password,
            Error,
            Login___The_Noob_Bot_version,
            Server_connection,
            In_use,
            User_Name,
            Password,
            Save,
            Create,
            Launch_Tnb,
            Refresh,
            Login,
            User_name_or_Password_error,
            Incorrect_password__go_to_this_address_if_you_have_forget_your_password,
            Incorrect_user_name__go_here_if_you_want_create_an_account_and_buy_The_Noob_Bot,
            Login_error__try_to_disable_your_antivirus__go_to_the_website_if_you_need_help,
            Subscription_finished__renew_it_if_you_want_use_no_limited_version_of_the_tnb_again_here,
            You_starting_trial_version__the_tnb_will_automatically_stopped_after____min,
            LatestUpdateVersion,
            LatestUpdateDescription,
            ConfirmUpdate,
            LatestUpdateTitle,
            Suspect_Activity,
            TheNoobBotServerIsOffline,
            WoWIsReducedError,
            Information,
            Trial_version,
            UserName,
            Trial_version__time_left,
            Subscription_time_left,
            Tnb_online,
            Start,
            Level,
            XP_HR,
            Honor_HR,
            Loots,
            Kills,
            Deaths,
            Farms,
            hr,
            Game_Informations,
            Running_time,
            Target_Name,
            Health,
            Last_log,
            Maximize_window,
            WEBSITE,
            Dev_Tools,
            Target_Level,
            Target_Health,
            Account_Informations,
            Information_account,
            Remote,
            Home,
            Log,
            My_tnb_Account,
            Player_Name,
            Product,
            Anti_Afk,
            Minimise,
            Smelting_here,
            Smelting_zone_not_found_in,
            No_setting_for_this_product,
            Please_add_items_to_prospect_in__General_Settings_____Looting_____Prospecting_List,
            Please_add_items_to_mil_in__General_Settings_____Looting_____Milling_List,
            Convertion_finish,
            Convert_Profiles,
            to,
            profiles,
            Profiles_Converters,
            Record_Way,
            Stop_Record_Way,
            Name_Empty,
            NPCNotFound,
            Separation_distance_record,
            Del,
            Add_this_position_to_Black_list_Radius,
            Add_Target_to_Npc_list,
            Load,
            Name,
            Add_by_Name_to_Npc_list,
            List_Zones,
            Add_Zone,
            Del_Zone,
            Zone_Name,
            Player_Lvl,
            Min,
            Max,
            Target_Lvl,
            Add_Target,
            points,
            Clear,
            Target_Ids,
            Nodes,
            Load_Profile,
            Profile,
            Use_Lure,
            Fish_School,
            Lure_Name,
            Fishing_Pole_Name,
            Weapon_Name,
            If_special__If_empty__default_items_is_used,
            Profile_Creator,
            Precision_Mode__fish_school,
            Settings_Fisherbot,
            Please_select_an_profile_or_disable_School_Fish_option,
            Survey_spell_not_found__stopping_tnb,
            SolvingEveryXMinutes,
            MaxTryByDigsite,
            min,
            ArchaeologistSettingsFrameTitle,
            Normal,
            Debug,
            Navigator,
            Fight,
            Get_informations_of_all_ingame_objects,
            Get_Taget_informations,
            Get_My_Position,
            Npc_type_list,
            Get_infomations_by_name,
            Launch_Lua_script,
            Developer_Tools,
            AdvancedSettingsPanelName,
            Npc_Mailbox_Search_Radius,
            Use_Paths_Finder,
            SecuritySystemPanelName,
            Song_if_New_Whisper,
            Security,
            NO,
            YES,
            Close_game,
            Record_whisper_in_Log_file,
            Blockages,
            If_Player_Teleported,
            Pause_tnb_if_Nearby_Player,
            If_reached______Honor_Points,
            If_Whisper_bigger_or_equal_to,
            If_full_Bag,
            MailsManagementPanelName,
            Mail_Recipient,
            Force_Mail_List__one_item_by_line,
            Mail_Purple_items,
            Mail_Blue_items,
            Mail_Green_items,
            Mail_White_items,
            Mail_Gray_items,
            Subject,
            Use_Mail,
            Do_not_Mail_List__one_item_by_line,
            NPCsRepairSellBuyPanelName,
            Force_Sell_List__one_item_by_line,
            Sell_Purple_items,
            Sell_Blue_items,
            Sell_Green_items,
            Sell_White_items,
            Sell_Gray_items,
            Selling,
            Repair,
            Food_Amount,
            Drink_Amount,
            Do_not_Sell_List__one_item_by_line,
            Relogger,
            BattleNet_Account,
            Account_Password,
            Account_Email,
            LootingFarmingManagementPanelName,
            Prospecting_only_in_town,
            Prospecting_Every__in_minute,
            Prospecting,
            Prospecting_list__one_item_by_line,
            Milling_only_in_town,
            Milling_Every__in_minute,
            Milling,
            Milling_list__one_item_by_line,
            Smelting,
            Don_t_harvest,
            Harvest_During_Long_Move,
            Ninja,
            Search_Radius,
            Max_Units_Near,
            Harvest_Herbs,
            Harvest_Minerals,
            Skin_Mobs,
            Loot_Chests,
            Harvest_Avoid_Players_Radius,
            Loot_Mobs,
            ActivateLootStatistics,
            RegenerationManagementPanelName,
            on,
            Drink,
            Food,
            MountManagementPanelName,
            Aquatic,
            Ignore_Fight_if_in_Gound_Mount,
            Mount_Distance,
            Flying,
            Ground,
            Use_Ground_Mount,
            SpellManagementSystemPanelName,
            Use_Spirit_Healer,
            ActivateSkillsAutoTraining,
            OnlyTrainCurrentlyUsedSkills,
            TrainMountingCapacity,
            OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum,
            BecomeApprenticeIfNeededByProduct,
            BecomeApprenticeOfSecondarySkillsWhileQuesting,
            Don_t_start_fighting,
            Can_attack_units_already_in_fight,
            Assign_Talents,
            CombatClass,
            HealerClass,
            CloseWithoutSaving,
            Reset_Settings,
            Save_and_Close,
            General_Settings,
            Do_you_want_save_this_setting,
            Required,
            VisualStudioRedistribuablePackages,
            New_whisper,
            Stop_tnb_if,
            Please_configure_your_Fly_mount_in_General_settings,
            ThisAquaticMountDoesNotExist,
            ThisFlyingMountDoesNotExist,
            ThisGroundMountDoesNotExist,
            Please_select_exe_in_the_install_folder_of_the_game,
            This_Programme_is_for_The_Game,
            UpdateRequiredCasesTitle,
            UpdateRequiredCases,
            UpdateRequireOlderTheNoobBotTitle,
            UpdateRequireOlderTheNoobBot,
            UpdateRequireNewerTheNoobBotTitle,
            UpdateRequireNewerTheNoobBot,
            UpdateRequiredCase1,
            UpdateRequiredCase2,
            RunningWoWBuildDot,
            RunningWoWBuild,
            PleaseDownloadOlder,
            PleaseDownloadNewer,
            Suspect_activity_of_the_game_which_haven_t_verified_yet__Closing_game_and_tnb,
            Click_on__Yes__to_close_tnb,
            Product_Settings,
            Pathing_Reverse_Direction,
            Settings,
            ResetSettings,
            Please_select_an_profile,
            File_not_found,
            success,
            errors,
            Npc_faction_list,
            Translate_Tools,
            Whisper_Egal_at,
            tnb_started_since,
            Your_player_is_now_level,
            level_upper,
            Player_Teleported,
            Reached_4000_Honor_Points,
            Bag_is_full,
            After,
            Add,
            Launch_C__script,
            Auto_Make_Elemental,
            Nodes_List_Manager,
            Smelting_Product_Description,
            Milling_Product_Description,
            Prospecting_Product_Description,
            Profiles_Converters_Product_Description,
            Flying_To_Ground_Profiles_Converter_Product_Description,
            Grinder_Product_Description,
            Gatherer_Product_Description,
            Damage_Dealer_Product_Description,
            Heal_Bot_Product_Description,
            Fisherbot_Product_Description,
            Battlegrounder_Product_Description,
            Quester_Product_Description,
            Archaeologist_Product_Description,
            Mimesis_Product_Description,
            GarrisonFarming_Product_Description,
            By_npc_name,
            Object_type,
            Creature_type,
            Tracker,
            Tracker_Product_Description,
            With,
            Launch_Game,
            Settings_Battlegrounder,
            AlteracValley,
            WarsongGulch,
            ArathiBasin,
            EyeoftheStorm,
            StrandoftheAncients,
            IsleofConquest,
            TwinPeaks,
            BattleforGilneas,
            RandomBattleground,
            TempleofKotmogu,
            SilvershardMines,
            QueueAlteracValley,
            QueueWarsongGulch,
            QueueArathiBasin,
            QueueEyeoftheStorm,
            QueueStrandoftheAncients,
            QueueIsleofConquest,
            QueueTwinPeaks,
            QueueBattleforGilneas,
            QueueRandomBattleground,
            QueueTempleofKotmogu,
            QueueSilvershardMines,
            WoW_Client_64bit,
            Title_WoW_Client_64bit,
            Uncap_MaxFPS,
            AlwaysOnTop,
            No_mounts_in_settings,
            AutoConfirmOnBoPItems,
            JoinQueue,
            RequeueingInProcess,
            Battleground_Ended,
            NotInBg,
            SendMailWhenLessThanXSlotLeft,
            SellItemsWhenLessThanXSlotLeft,
            RepairWhenDurabilityIsUnderPercent,
            RequeueAfterXMinutes,
            ErrorMultipleQueue,
            ErrorRandomQueue,
            ErrorSingleRandomQueue,
            CantDuplicateZone,
            CantRecordInWrongZone,
            BattlegrounderTipOffTitle,
            BattlegrounderTipOffMessage,
            GrinderTipOffTitle,
            GrinderTipOffMessage,
            GathererTipOffTitle,
            GathererTipOffMessage,
            QuesterTipOffTitle,
            QuesterTipOffMessage,
            ArchaeologistTipOffTitle,
            ArchaeologistTipOffMessage,
            TipOffLootingOn,
            TipOffLootingOff,
            TipOffLootingOnArchaeologist,
            TipOffLootingOffArchaeologist,
            TipOffSellingOnQuester,
            TipOffSellingQualityQuester,
            TipOffRadiusLow,
            TipOffRadiusHigh,
            TipOffUseGroundMountOn,
            TipOffUseGroundMountOff,
            TipOffEmptyGroundMount,
            TipOffEmptyFlyingMount,
            TipOffMinimumDistanceToUseGroundMount,
            UseHearthstone,
            HearthstoneNotFound,
            UseMollE,
            UseRobot,
            ReloggerManagementPanelName,
            QuesterProfileManagementSystem,
            GroupedProfileManager,
            SimpleProfileManager,
            GroupedQuestProfileFile,
            NoGroupedProfileToEdit,
            RemoveGroupedProfile,
            Confirm,
            NoGroupedProfileToDelete,
            FeatureNotYetAvailable,
            AddGrouped,
            EditGrouped,
            GroupedDocumentation,
            RemoveGrouped,
            AddSimple,
            EditSimple,
            SimpleDocumentation,
            RemoveSimple,
            AvailableSimpleProfiles,
            CurrentlyGroupedProfiles,
            GroupSelectedProfile,
            UngroupSelectedProfile,
            UngroupAllProfiles,
            SaveAsAndClose,
            SaveAndClose,
            CancelAndClose,
            MoveUp,
            MoveDown,
            CantSaveEmptyGroupedNew,
            CantSaveEmptyGroupedExisting,
            QuesterProfileLoader,
            NoSimpleProfileToLoad,
            NoGroupedProfileToLoad,
            SimpleProfilesList,
            GroupedProfilesList,
            LoadSimpleProfile,
            LoadGroupedProfile,
            ProfileQuestList,
            AddNewQuest,
            EditSelectedQuest,
            DeleteSelectedQuest,
            SimpleQuestProfileFile,
            CantSaveEmptySimpleNew,
            CantSaveEmptySimpleExisting,
            NoSimpleProfileToEdit,
            AutoCloseChatFrame,
            Settings_Mimesis,
            MasterBotIPAddress,
            MasterBotIPPort,
            ActivatePartyMode,
            MimesisBroadcasterSettings,
            BroadcastingPortDefault,
            BroadcastingIPWan,
            BroadcastingIPLan,
            ActivateBroadcastingMimesis,
            BroadcastingIPLocal,
            BroadcastingPort,
            ProfileQuesterList,
            AddNewQuester,
            EditSelectedQuester,
            DeleteSelectedQuester,
            LoginFormTitle,
            LoginFormDefaultIdentifier,
            LoginFormUseKey,
            LoginFormKey,
            LoginFormRemember,
            LoginFormStart,
            LoginFormRefresh,
            LoginFormWebsite,
            LoginFormForum,
            LoginFormRegister,
            LoadTranslationFile,
            LuaExecButton,
            GpsButton,
            TargetInfoButton,
            TargetInfo2Button,
            TranslationManagerButton,
            CsharpExecButton,
            NpcTypeButton,
            NpcFactionButton,
            SearchObjectButton,
            AllObjectsButton,
            SearchObjectBox,
            Active,
            DigsiteName,
            Priority,
            UseKeystones,
            CrateRestoredArtifacts,
            LatestLogEntry,
            TargetLevel,
            TargetName,
            TargetHealth,
            HonorPerHour,
            UnitKills,
            XPPerHour,
            PlayerNameChanged,
            StopProduct,
            StartProduct,
            OnlineBots,
            SessionInfo,
            ItemsFounds,
            SwitchDxError1,
            SwitchDxError2,
            RemoteSession,
            MyAccount,
            DevTools,
            Website,
            WantToSubscribe,
            SelectOffer,
            PaymentPage,
            AccountName,
            TimeLeft,
            SubscribeMonth,
            SubscribeMonthPlatinium,
            SubscribeHalfYear,
            SubscribeLifeTime,
            FtGConverterHeaderText,
            FtGConverterLine1,
            FtGConverterLine2,
            FtGConverterLine3,
            FtGConverterButton,
            TrackerPopUp,
            HideSdkFiles,
            Whisper
        }

        [Serializable]
        public class Language
        {
            private List<Translate.Translation> _translations = new List<Translate.Translation>();

            public List<Translate.Translation> Translations
            {
                get
                {
                    return this._translations;
                }
                set
                {
                    this._translations = value;
                }
            }
        }

        [Serializable]
        public class Translation
        {
            private nManager.Translate.Id _id;
            private string _text = "";

            public nManager.Translate.Id Id
            {
                get
                {
                    return this._id;
                }
                set
                {
                    this._id = value;
                }
            }

            public string Text
            {
                get
                {
                    return this._text;
                }
                set
                {
                    this._text = value;
                }
            }
        }
    }
}

