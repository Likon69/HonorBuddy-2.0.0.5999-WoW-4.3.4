namespace UltimatePaladinHealerBT
{
    public partial class UltimatePalaHealerBT
    {
        #region PalaVariableInizializer

        public bool Variable_inizializer()
        {
            General_variable_inizializer();
            switch (usedBehaviour)
            {
                case "Solo":
                    Solo_Variable_inizializer();
                    break;
                case "Party or Raid":
                    RAF_Variable_inizializer();
                    break;
                case "Arena":
                    ARENA_Variable_inizializer();
                    break;
                case "World PVP":
                    WorldPVP_Variable_inizializer();
                    break;
                case "Battleground":
                    Battleground_Variable_inizializer();
                    break;
                case "Dungeon":
                    PVE_Variable_inizializer();
                    break;
                case "Raid":
                    Raid_Variable_inizializer();
                    break;
                case "Retribution":
                    Retribution_Variable_inizializer();
                    break;
                default:
                    slog("no valid Behaviour found, CC will stop NOW!");
                    return false;
                //break;
            }
            return true;
        }

        public bool General_variable_inizializer()
        {
            _Stop_Healing = UPaHBTSetting.Instance.General_Stop_Healing;
            _trinket1_name = UPaHBTSetting.Instance.Trinket1_name;
            _trinket1_ID = UPaHBTSetting.Instance.Trinket1_ID;
            _trinket1_CD = UPaHBTSetting.Instance.Trinket1_CD;
            _trinket1_use_when = UPaHBTSetting.Instance.Trinket1_use_when;
            _trinket1_passive = UPaHBTSetting.Instance.Trinket1_passive;
            _trinket2_name = UPaHBTSetting.Instance.Trinket2_name;
            _trinket2_ID = UPaHBTSetting.Instance.Trinket2_ID;
            _trinket2_CD = UPaHBTSetting.Instance.Trinket2_CD;
            _trinket2_use_when = UPaHBTSetting.Instance.Trinket2_use_when;
            _trinket2_passive = UPaHBTSetting.Instance.Trinket2_passive;
            _precasting = UPaHBTSetting.Instance.General_precasting*10;
            return true;
        }

        public bool Retribution_Variable_inizializer()
        {
            _bless_type = UPaHBTSetting.Instance.Retr_bless_type;
            _wanna_buff = UPaHBTSetting.Instance.Retr_wanna_buff;
            _wanna_crusader = UPaHBTSetting.Instance.Retr_wanna_crusader;
            _intellywait = UPaHBTSetting.Instance.Retr_intellywait;
            _max_healing_distance = UPaHBTSetting.Instance.Retr_max_healing_distance;
            return true;
        }

        public bool Solo_Variable_inizializer()
        {
            PVE_dispell_inizializer();
            _aura_type = UPaHBTSetting.Instance.Solo_aura_type;
            _do_not_heal_above = UPaHBTSetting.Instance.Solo_do_not_heal_above;
            _HR_how_far = UPaHBTSetting.Instance.Solo_HR_how_far;
            _HR_how_much_health = UPaHBTSetting.Instance.Solo_HR_how_much_health;
            _Inf_of_light_wanna_DL = UPaHBTSetting.Instance.Solo_Inf_of_light_wanna_DL;
            _mana_judge = UPaHBTSetting.Instance.Solo_mana_judge;
            _max_healing_distance = UPaHBTSetting.Instance.Solo_max_healing_distance;
            _min_Divine_Plea_mana = UPaHBTSetting.Instance.Solo_min_Divine_Plea_mana;
            _min_DL_hp = UPaHBTSetting.Instance.Solo_min_DL_hp;
            _min_DP_hp = UPaHBTSetting.Instance.Solo_min_DP_hp;
            _min_DS_hp = UPaHBTSetting.Instance.Solo_min_DS_hp;
            _min_FoL_hp = UPaHBTSetting.Instance.Solo_min_FoL_hp;
            _min_gift_hp = UPaHBTSetting.Instance.Solo_min_gift_hp;
            _min_HL_hp = UPaHBTSetting.Instance.Solo_min_HL_hp;
            _min_HoP_hp = UPaHBTSetting.Instance.Solo_min_HoP_hp;
            _min_HoS_hp = UPaHBTSetting.Instance.Solo_min_HoS_hp;
            _min_Inf_of_light_DL_hp = UPaHBTSetting.Instance.Solo_min_Inf_of_light_DL_hp;
            _min_LoH_hp = UPaHBTSetting.Instance.Solo_min_LoH_hp;
            _min_mana_potion = UPaHBTSetting.Instance.Solo_min_mana_potion;
            _min_mana_rec_trinket = UPaHBTSetting.Instance.Solo_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPaHBTSetting.Instance.Solo_min_ohshitbutton_activator;
            _min_player_inside_HR = UPaHBTSetting.Instance.Solo_min_player_inside_HR;
            _min_stoneform = UPaHBTSetting.Instance.Solo_min_stoneform;
            _min_torrent_mana_perc = UPaHBTSetting.Instance.Solo_min_torrent_mana_perc;
            _rest_if_mana_below = UPaHBTSetting.Instance.Solo_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPaHBTSetting.Instance.Solo_use_mana_rec_trinket_every;
            _wanna_AW = UPaHBTSetting.Instance.Solo_wanna_AW;
            _wanna_buff = UPaHBTSetting.Instance.Solo_wanna_buff;
            _wanna_cleanse = UPaHBTSetting.Instance.Solo_wanna_cleanse;
            _wanna_crusader = UPaHBTSetting.Instance.Solo_wanna_crusader;
            _wanna_CS = UPaHBTSetting.Instance.Solo_wanna_CS;
            _wanna_DF = UPaHBTSetting.Instance.Solo_wanna_DF;
            _wanna_DP = UPaHBTSetting.Instance.Solo_wanna_DP;
            _wanna_DS = UPaHBTSetting.Instance.Solo_wanna_DS;
            _wanna_everymanforhimself = UPaHBTSetting.Instance.Solo_wanna_everymanforhimself;
            _wanna_face = UPaHBTSetting.Instance.Solo_wanna_face;
            _wanna_gift = UPaHBTSetting.Instance.Solo_wanna_gift;
            _wanna_GotAK = UPaHBTSetting.Instance.Solo_wanna_GotAK;
            _wanna_HoJ = UPaHBTSetting.Instance.Solo_wanna_HoJ;
            _wanna_HoP = UPaHBTSetting.Instance.Solo_wanna_HoP;
            _wanna_HoS = UPaHBTSetting.Instance.Solo_wanna_HoS;
            _wanna_HoW = UPaHBTSetting.Instance.Solo_wanna_HoW;
            _wanna_HR = UPaHBTSetting.Instance.Solo_wanna_HR;
            _wanna_Judge = UPaHBTSetting.Instance.Solo_wanna_Judge;
            _wanna_LoH = UPaHBTSetting.Instance.Solo_wanna_LoH;
            _wanna_mana_potion = UPaHBTSetting.Instance.Solo_wanna_mana_potion;
            _wanna_mount = UPaHBTSetting.Instance.Solo_wanna_mount;
            _wanna_move_to_heal = UPaHBTSetting.Instance.Solo_wanna_move_to_heal;
            _wanna_move_to_HoJ = UPaHBTSetting.Instance.Solo_wanna_move_to_HoJ;
            _wanna_rebuke = UPaHBTSetting.Instance.Solo_wanna_rebuke;
            _wanna_stoneform = UPaHBTSetting.Instance.Solo_wanna_stoneform;
            _wanna_target = UPaHBTSetting.Instance.Solo_wanna_target;
            _wanna_torrent = UPaHBTSetting.Instance.Solo_wanna_torrent;
            _wanna_urgent_cleanse = UPaHBTSetting.Instance.Solo_wanna_urgent_cleanse;
            _wanna_move = UPaHBTSetting.Instance.Solo_wanna_move;
            _wanna_lifeblood = UPaHBTSetting.Instance.Solo_wanna_lifeblood;
            _do_not_dismount_EVER = UPaHBTSetting.Instance.Solo_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPaHBTSetting.Instance.Solo_do_not_dismount_ooc;
            _get_tank_from_focus = UPaHBTSetting.Instance.Solo_get_tank_from_focus;
            _get_tank_from_lua = UPaHBTSetting.Instance.Solo_get_tank_from_lua;
            _bless_type = UPaHBTSetting.Instance.Solo_bless_type;
            _decice_during_GCD = UPaHBTSetting.Instance.Solo_decice_during_GCD;
            _intellywait = UPaHBTSetting.Instance.Solo_intellywait;
            _answer_PVP_attacks = UPaHBTSetting.Instance.Solo_answer_PVP_attacks;
            _enable_pull = UPaHBTSetting.Instance.Solo_enable_pull;
            _wanna_HS_on_CD = UPaHBTSetting.Instance.Solo_wanna_HS_on_CD;
            return true;
        }

        private bool PVE_Variable_inizializer()
        {
            PVE_dispell_inizializer();
            _aura_type = UPaHBTSetting.Instance.PVE_aura_type;
            _do_not_heal_above = UPaHBTSetting.Instance.PVE_do_not_heal_above;
            _HR_how_far = UPaHBTSetting.Instance.PVE_HR_how_far;
            _HR_how_much_health = UPaHBTSetting.Instance.PVE_HR_how_much_health;
            _Inf_of_light_wanna_DL = UPaHBTSetting.Instance.PVE_Inf_of_light_wanna_DL;
            _mana_judge = UPaHBTSetting.Instance.PVE_mana_judge;
            _max_healing_distance = UPaHBTSetting.Instance.PVE_max_healing_distance;
            _min_Divine_Plea_mana = UPaHBTSetting.Instance.PVE_min_Divine_Plea_mana;
            _min_DL_hp = UPaHBTSetting.Instance.PVE_min_DL_hp;
            _min_DP_hp = UPaHBTSetting.Instance.PVE_min_DP_hp;
            _min_DS_hp = UPaHBTSetting.Instance.PVE_min_DS_hp;
            _min_FoL_hp = UPaHBTSetting.Instance.PVE_min_FoL_hp;
            _min_gift_hp = UPaHBTSetting.Instance.PVE_min_gift_hp;
            _min_HL_hp = UPaHBTSetting.Instance.PVE_min_HL_hp;
            _min_HoP_hp = UPaHBTSetting.Instance.PVE_min_HoP_hp;
            _min_HoS_hp = UPaHBTSetting.Instance.PVE_min_HoS_hp;
            _min_Inf_of_light_DL_hp = UPaHBTSetting.Instance.PVE_min_Inf_of_light_DL_hp;
            _min_LoH_hp = UPaHBTSetting.Instance.PVE_min_LoH_hp;
            _min_mana_potion = UPaHBTSetting.Instance.PVE_min_mana_potion;
            _min_mana_rec_trinket = UPaHBTSetting.Instance.PVE_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPaHBTSetting.Instance.PVE_min_ohshitbutton_activator;
            _min_player_inside_HR = UPaHBTSetting.Instance.PVE_min_player_inside_HR;
            _min_stoneform = UPaHBTSetting.Instance.PVE_min_stoneform;
            _min_torrent_mana_perc = UPaHBTSetting.Instance.PVE_min_torrent_mana_perc;
            _rest_if_mana_below = UPaHBTSetting.Instance.PVE_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPaHBTSetting.Instance.PVE_use_mana_rec_trinket_every;
            _wanna_AW = UPaHBTSetting.Instance.PVE_wanna_AW;
            _wanna_buff = UPaHBTSetting.Instance.PVE_wanna_buff;
            _wanna_cleanse = UPaHBTSetting.Instance.PVE_wanna_cleanse;
            _wanna_crusader = UPaHBTSetting.Instance.PVE_wanna_crusader;
            _wanna_CS = UPaHBTSetting.Instance.PVE_wanna_CS;
            _wanna_DF = UPaHBTSetting.Instance.PVE_wanna_DF;
            _wanna_DP = UPaHBTSetting.Instance.PVE_wanna_DP;
            _wanna_DS = UPaHBTSetting.Instance.PVE_wanna_DS;
            _wanna_everymanforhimself = UPaHBTSetting.Instance.PVE_wanna_everymanforhimself;
            _wanna_face = UPaHBTSetting.Instance.PVE_wanna_face;
            _wanna_gift = UPaHBTSetting.Instance.PVE_wanna_gift;
            _wanna_GotAK = UPaHBTSetting.Instance.PVE_wanna_GotAK;
            _wanna_HoJ = UPaHBTSetting.Instance.PVE_wanna_HoJ;
            _wanna_HoP = UPaHBTSetting.Instance.PVE_wanna_HoP;
            _wanna_HoS = UPaHBTSetting.Instance.PVE_wanna_HoS;
            _wanna_HoW = UPaHBTSetting.Instance.PVE_wanna_HoW;
            _wanna_HR = UPaHBTSetting.Instance.PVE_wanna_HR;
            _wanna_Judge = UPaHBTSetting.Instance.PVE_wanna_Judge;
            _wanna_LoH = UPaHBTSetting.Instance.PVE_wanna_LoH;
            _wanna_mana_potion = UPaHBTSetting.Instance.PVE_wanna_mana_potion;
            _wanna_mount = UPaHBTSetting.Instance.PVE_wanna_mount;
            _wanna_move_to_heal = UPaHBTSetting.Instance.PVE_wanna_move_to_heal;
            _wanna_move_to_HoJ = UPaHBTSetting.Instance.PVE_wanna_move_to_HoJ;
            _wanna_rebuke = UPaHBTSetting.Instance.PVE_wanna_rebuke;
            _wanna_stoneform = UPaHBTSetting.Instance.PVE_wanna_stoneform;
            _wanna_target = UPaHBTSetting.Instance.PVE_wanna_target;
            _wanna_torrent = UPaHBTSetting.Instance.PVE_wanna_torrent;
            _wanna_urgent_cleanse = UPaHBTSetting.Instance.PVE_wanna_urgent_cleanse;
            _wanna_lifeblood = UPaHBTSetting.Instance.PVE_wanna_lifeblood;
            _do_not_dismount_EVER = UPaHBTSetting.Instance.PVE_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPaHBTSetting.Instance.PVE_do_not_dismount_ooc;
            _get_tank_from_focus = UPaHBTSetting.Instance.PVE_get_tank_from_focus;
            _get_tank_from_lua = UPaHBTSetting.Instance.PVE_get_tank_from_lua;
            _stop_DL_if_above = UPaHBTSetting.Instance.PVE_stop_DL_if_above;
            _bless_type = UPaHBTSetting.Instance.PVE_bless_type;
            _healing_tank_priority = UPaHBTSetting.Instance.PVE_healing_tank_priority;
            _tank_healing_priority_multiplier = UPaHBTSetting.Instance.PVE_tank_healing_priority_multiplier;
            _decice_during_GCD = UPaHBTSetting.Instance.PVE_decice_during_GCD;
            _intellywait = UPaHBTSetting.Instance.PVE_intellywait;
            _wanna_HS_on_CD = UPaHBTSetting.Instance.PVE_wanna_HS_on_CD;
            _cleanse_only_self_and_tank = UPaHBTSetting.Instance.PVE_cleanse_only_self_and_tank;
            return true;
        }

        private bool Raid_Variable_inizializer()
        {
            PVE_dispell_inizializer();
            _aura_type = UPaHBTSetting.Instance.Raid_aura_type;
            _do_not_heal_above = UPaHBTSetting.Instance.Raid_do_not_heal_above;
            _HR_how_far = UPaHBTSetting.Instance.Raid_HR_how_far;
            _HR_how_much_health = UPaHBTSetting.Instance.Raid_HR_how_much_health;
            _Inf_of_light_wanna_DL = UPaHBTSetting.Instance.Raid_Inf_of_light_wanna_DL;
            _mana_judge = UPaHBTSetting.Instance.Raid_mana_judge;
            _max_healing_distance = UPaHBTSetting.Instance.Raid_max_healing_distance;
            _min_Divine_Plea_mana = UPaHBTSetting.Instance.Raid_min_Divine_Plea_mana;
            _min_DL_hp = UPaHBTSetting.Instance.Raid_min_DL_hp;
            _min_DP_hp = UPaHBTSetting.Instance.Raid_min_DP_hp;
            _min_DS_hp = UPaHBTSetting.Instance.Raid_min_DS_hp;
            _min_FoL_hp = UPaHBTSetting.Instance.Raid_min_FoL_hp;
            _min_gift_hp = UPaHBTSetting.Instance.Raid_min_gift_hp;
            _min_HL_hp = UPaHBTSetting.Instance.Raid_min_HL_hp;
            _min_HoP_hp = UPaHBTSetting.Instance.Raid_min_HoP_hp;
            _min_HoS_hp = UPaHBTSetting.Instance.Raid_min_HoS_hp;
            _min_Inf_of_light_DL_hp = UPaHBTSetting.Instance.Raid_min_Inf_of_light_DL_hp;
            _min_LoH_hp = UPaHBTSetting.Instance.Raid_min_LoH_hp;
            _min_mana_potion = UPaHBTSetting.Instance.Raid_min_mana_potion;
            _min_mana_rec_trinket = UPaHBTSetting.Instance.Raid_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPaHBTSetting.Instance.Raid_min_ohshitbutton_activator;
            _min_player_inside_HR = UPaHBTSetting.Instance.Raid_min_player_inside_HR;
            _min_stoneform = UPaHBTSetting.Instance.Raid_min_stoneform;
            _min_torrent_mana_perc = UPaHBTSetting.Instance.Raid_min_torrent_mana_perc;
            _rest_if_mana_below = UPaHBTSetting.Instance.Raid_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPaHBTSetting.Instance.Raid_use_mana_rec_trinket_every;
            _wanna_AW = UPaHBTSetting.Instance.Raid_wanna_AW;
            _wanna_buff = UPaHBTSetting.Instance.Raid_wanna_buff;
            _wanna_cleanse = UPaHBTSetting.Instance.Raid_wanna_cleanse;
            _wanna_crusader = UPaHBTSetting.Instance.Raid_wanna_crusader;
            _wanna_CS = UPaHBTSetting.Instance.Raid_wanna_CS;
            _wanna_DF = UPaHBTSetting.Instance.Raid_wanna_DF;
            _wanna_DP = UPaHBTSetting.Instance.Raid_wanna_DP;
            _wanna_DS = UPaHBTSetting.Instance.Raid_wanna_DS;
            _wanna_everymanforhimself = UPaHBTSetting.Instance.Raid_wanna_everymanforhimself;
            _wanna_face = UPaHBTSetting.Instance.Raid_wanna_face;
            _wanna_gift = UPaHBTSetting.Instance.Raid_wanna_gift;
            _wanna_GotAK = UPaHBTSetting.Instance.Raid_wanna_GotAK;
            _wanna_HoJ = UPaHBTSetting.Instance.Raid_wanna_HoJ;
            _wanna_HoP = UPaHBTSetting.Instance.Raid_wanna_HoP;
            _wanna_HoS = UPaHBTSetting.Instance.Raid_wanna_HoS;
            _wanna_HoW = UPaHBTSetting.Instance.Raid_wanna_HoW;
            _wanna_HR = UPaHBTSetting.Instance.Raid_wanna_HR;
            _wanna_Judge = UPaHBTSetting.Instance.Raid_wanna_Judge;
            _wanna_LoH = UPaHBTSetting.Instance.Raid_wanna_LoH;
            _wanna_mana_potion = UPaHBTSetting.Instance.Raid_wanna_mana_potion;
            _wanna_mount = UPaHBTSetting.Instance.Raid_wanna_mount;
            _wanna_move_to_heal = UPaHBTSetting.Instance.Raid_wanna_move_to_heal;
            _wanna_move_to_HoJ = UPaHBTSetting.Instance.Raid_wanna_move_to_HoJ;
            _wanna_rebuke = UPaHBTSetting.Instance.Raid_wanna_rebuke;
            _wanna_stoneform = UPaHBTSetting.Instance.Raid_wanna_stoneform;
            _wanna_target = UPaHBTSetting.Instance.Raid_wanna_target;
            _wanna_torrent = UPaHBTSetting.Instance.Raid_wanna_torrent;
            _wanna_urgent_cleanse = UPaHBTSetting.Instance.Raid_wanna_urgent_cleanse;
            _wanna_lifeblood = UPaHBTSetting.Instance.Raid_wanna_lifeblood;
            _do_not_dismount_EVER = UPaHBTSetting.Instance.Raid_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPaHBTSetting.Instance.Raid_do_not_dismount_ooc;
            _selective_healing = UPaHBTSetting.Instance.Selective_Healing;
            populate_heal_or_not();
            populate_heal_raid();
            _get_tank_from_focus = UPaHBTSetting.Instance.Raid_get_tank_from_focus;
            _ignore_beacon = UPaHBTSetting.Instance.Raid_ignore_beacon;
            _get_tank_from_lua = UPaHBTSetting.Instance.Raid_get_tank_from_lua;
            _stop_DL_if_above = UPaHBTSetting.Instance.Raid_stop_DL_if_above;
            _bless_type = UPaHBTSetting.Instance.Raid_bless_type;
            _healing_tank_priority = UPaHBTSetting.Instance.Raid_healing_tank_priority;
            _tank_healing_priority_multiplier = UPaHBTSetting.Instance.Raid_tank_healing_priority_multiplier;
            _decice_during_GCD = UPaHBTSetting.Instance.Raid_decice_during_GCD;
            _intellywait = UPaHBTSetting.Instance.Raid_intellywait;
            _wanna_HS_on_CD = UPaHBTSetting.Instance.Raid_wanna_HS_on_CD;
            _cleanse_only_self_and_tank = UPaHBTSetting.Instance.Raid_cleanse_only_self_and_tank;
            return true;
        }

        private bool Battleground_Variable_inizializer()
        {
            PVP_dispell_inizializer();
            _aura_type = UPaHBTSetting.Instance.Battleground_aura_type;
            _do_not_heal_above = UPaHBTSetting.Instance.Battleground_do_not_heal_above;
            _HR_how_far = UPaHBTSetting.Instance.Battleground_HR_how_far;
            _HR_how_much_health = UPaHBTSetting.Instance.Battleground_HR_how_much_health;
            _Inf_of_light_wanna_DL = UPaHBTSetting.Instance.Battleground_Inf_of_light_wanna_DL;
            _mana_judge = UPaHBTSetting.Instance.Battleground_mana_judge;
            _max_healing_distance = UPaHBTSetting.Instance.Battleground_max_healing_distance;
            _min_Divine_Plea_mana = UPaHBTSetting.Instance.Battleground_min_Divine_Plea_mana;
            _min_DL_hp = UPaHBTSetting.Instance.Battleground_min_DL_hp;
            _min_DP_hp = UPaHBTSetting.Instance.Battleground_min_DP_hp;
            _min_DS_hp = UPaHBTSetting.Instance.Battleground_min_DS_hp;
            _min_FoL_hp = UPaHBTSetting.Instance.Battleground_min_FoL_hp;
            _min_gift_hp = UPaHBTSetting.Instance.Battleground_min_gift_hp;
            _min_HL_hp = UPaHBTSetting.Instance.Battleground_min_HL_hp;
            _min_HoP_hp = UPaHBTSetting.Instance.Battleground_min_HoP_hp;
            _min_HoS_hp = UPaHBTSetting.Instance.Battleground_min_HoS_hp;
            _min_Inf_of_light_DL_hp = UPaHBTSetting.Instance.Battleground_min_Inf_of_light_DL_hp;
            _min_LoH_hp = UPaHBTSetting.Instance.Battleground_min_LoH_hp;
            _min_mana_potion = UPaHBTSetting.Instance.Battleground_min_mana_potion;
            _min_mana_rec_trinket = UPaHBTSetting.Instance.Battleground_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPaHBTSetting.Instance.Battleground_min_ohshitbutton_activator;
            _min_player_inside_HR = UPaHBTSetting.Instance.Battleground_min_player_inside_HR;
            _min_stoneform = UPaHBTSetting.Instance.Battleground_min_stoneform;
            _min_torrent_mana_perc = UPaHBTSetting.Instance.Battleground_min_torrent_mana_perc;
            _rest_if_mana_below = UPaHBTSetting.Instance.Battleground_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPaHBTSetting.Instance.Battleground_use_mana_rec_trinket_every;
            _wanna_AW = UPaHBTSetting.Instance.Battleground_wanna_AW;
            _wanna_buff = UPaHBTSetting.Instance.Battleground_wanna_buff;
            _wanna_cleanse = UPaHBTSetting.Instance.Battleground_wanna_cleanse;
            _wanna_crusader = UPaHBTSetting.Instance.Battleground_wanna_crusader;
            _wanna_CS = UPaHBTSetting.Instance.Battleground_wanna_CS;
            _wanna_DF = UPaHBTSetting.Instance.Battleground_wanna_DF;
            _wanna_DP = UPaHBTSetting.Instance.Battleground_wanna_DP;
            _wanna_DS = UPaHBTSetting.Instance.Battleground_wanna_DS;
            _wanna_everymanforhimself = UPaHBTSetting.Instance.Battleground_wanna_everymanforhimself;
            _wanna_face = UPaHBTSetting.Instance.Battleground_wanna_face;
            _wanna_gift = UPaHBTSetting.Instance.Battleground_wanna_gift;
            _wanna_GotAK = UPaHBTSetting.Instance.Battleground_wanna_GotAK;
            _wanna_HoJ = UPaHBTSetting.Instance.Battleground_wanna_HoJ;
            _wanna_HoP = UPaHBTSetting.Instance.Battleground_wanna_HoP;
            _wanna_HoS = UPaHBTSetting.Instance.Battleground_wanna_HoS;
            _wanna_HoW = UPaHBTSetting.Instance.Battleground_wanna_HoW;
            _wanna_HR = UPaHBTSetting.Instance.Battleground_wanna_HR;
            _wanna_Judge = UPaHBTSetting.Instance.Battleground_wanna_Judge;
            _wanna_LoH = UPaHBTSetting.Instance.Battleground_wanna_LoH;
            _wanna_mana_potion = UPaHBTSetting.Instance.Battleground_wanna_mana_potion;
            _wanna_mount = UPaHBTSetting.Instance.Battleground_wanna_mount;
            _wanna_move_to_heal = UPaHBTSetting.Instance.Battleground_wanna_move_to_heal;
            _wanna_move_to_HoJ = UPaHBTSetting.Instance.Battleground_wanna_move_to_HoJ;
            _wanna_rebuke = UPaHBTSetting.Instance.Battleground_wanna_rebuke;
            _wanna_stoneform = UPaHBTSetting.Instance.Battleground_wanna_stoneform;
            _wanna_target = UPaHBTSetting.Instance.Battleground_wanna_target;
            _wanna_torrent = UPaHBTSetting.Instance.Battleground_wanna_torrent;
            _wanna_urgent_cleanse = UPaHBTSetting.Instance.Battleground_wanna_urgent_cleanse;
            _wanna_lifeblood = UPaHBTSetting.Instance.Battleground_wanna_lifeblood;
            _do_not_dismount_EVER = UPaHBTSetting.Instance.Battleground_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPaHBTSetting.Instance.Battleground_do_not_dismount_ooc;
            _get_tank_from_focus = UPaHBTSetting.Instance.Battleground_get_tank_from_focus;
            _get_tank_from_lua = UPaHBTSetting.Instance.Battleground_get_tank_from_lua;
            _bless_type = UPaHBTSetting.Instance.Battleground_bless_type;
            _decice_during_GCD = UPaHBTSetting.Instance.Battleground_decice_during_GCD;
            _intellywait = UPaHBTSetting.Instance.Battleground_intellywait;
            _wanna_HS_on_CD = UPaHBTSetting.Instance.Battleground_wanna_HS_on_CD;
            _cleanse_only_self_and_tank = UPaHBTSetting.Instance.Battleground_cleanse_only_self_and_tank;
            return true;
        }

        private bool WorldPVP_Variable_inizializer()
        {
            PVP_dispell_inizializer();
            _aura_type = UPaHBTSetting.Instance.WorldPVP_aura_type;
            _do_not_heal_above = UPaHBTSetting.Instance.WorldPVP_do_not_heal_above;
            _HR_how_far = UPaHBTSetting.Instance.WorldPVP_HR_how_far;
            _HR_how_much_health = UPaHBTSetting.Instance.WorldPVP_HR_how_much_health;
            _Inf_of_light_wanna_DL = UPaHBTSetting.Instance.WorldPVP_Inf_of_light_wanna_DL;
            _mana_judge = UPaHBTSetting.Instance.WorldPVP_mana_judge;
            _max_healing_distance = UPaHBTSetting.Instance.WorldPVP_max_healing_distance;
            _min_Divine_Plea_mana = UPaHBTSetting.Instance.WorldPVP_min_Divine_Plea_mana;
            _min_DL_hp = UPaHBTSetting.Instance.WorldPVP_min_DL_hp;
            _min_DP_hp = UPaHBTSetting.Instance.WorldPVP_min_DP_hp;
            _min_DS_hp = UPaHBTSetting.Instance.WorldPVP_min_DS_hp;
            _min_FoL_hp = UPaHBTSetting.Instance.WorldPVP_min_FoL_hp;
            _min_gift_hp = UPaHBTSetting.Instance.WorldPVP_min_gift_hp;
            _min_HL_hp = UPaHBTSetting.Instance.WorldPVP_min_HL_hp;
            _min_HoP_hp = UPaHBTSetting.Instance.WorldPVP_min_HoP_hp;
            _min_HoS_hp = UPaHBTSetting.Instance.WorldPVP_min_HoS_hp;
            _min_Inf_of_light_DL_hp = UPaHBTSetting.Instance.WorldPVP_min_Inf_of_light_DL_hp;
            _min_LoH_hp = UPaHBTSetting.Instance.WorldPVP_min_LoH_hp;
            _min_mana_potion = UPaHBTSetting.Instance.WorldPVP_min_mana_potion;
            _min_mana_rec_trinket = UPaHBTSetting.Instance.WorldPVP_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPaHBTSetting.Instance.WorldPVP_min_ohshitbutton_activator;
            _min_player_inside_HR = UPaHBTSetting.Instance.WorldPVP_min_player_inside_HR;
            _min_stoneform = UPaHBTSetting.Instance.WorldPVP_min_stoneform;
            _min_torrent_mana_perc = UPaHBTSetting.Instance.WorldPVP_min_torrent_mana_perc;
            _rest_if_mana_below = UPaHBTSetting.Instance.WorldPVP_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPaHBTSetting.Instance.WorldPVP_use_mana_rec_trinket_every;
            _wanna_AW = UPaHBTSetting.Instance.WorldPVP_wanna_AW;
            _wanna_buff = UPaHBTSetting.Instance.WorldPVP_wanna_buff;
            _wanna_cleanse = UPaHBTSetting.Instance.WorldPVP_wanna_cleanse;
            _wanna_crusader = UPaHBTSetting.Instance.WorldPVP_wanna_crusader;
            _wanna_CS = UPaHBTSetting.Instance.WorldPVP_wanna_CS;
            _wanna_DF = UPaHBTSetting.Instance.WorldPVP_wanna_DF;
            _wanna_DP = UPaHBTSetting.Instance.WorldPVP_wanna_DP;
            _wanna_DS = UPaHBTSetting.Instance.WorldPVP_wanna_DS;
            _wanna_everymanforhimself = UPaHBTSetting.Instance.WorldPVP_wanna_everymanforhimself;
            _wanna_face = UPaHBTSetting.Instance.WorldPVP_wanna_face;
            _wanna_gift = UPaHBTSetting.Instance.WorldPVP_wanna_gift;
            _wanna_GotAK = UPaHBTSetting.Instance.WorldPVP_wanna_GotAK;
            _wanna_HoJ = UPaHBTSetting.Instance.WorldPVP_wanna_HoJ;
            _wanna_HoP = UPaHBTSetting.Instance.WorldPVP_wanna_HoP;
            _wanna_HoS = UPaHBTSetting.Instance.WorldPVP_wanna_HoS;
            _wanna_HoW = UPaHBTSetting.Instance.WorldPVP_wanna_HoW;
            _wanna_HR = UPaHBTSetting.Instance.WorldPVP_wanna_HR;
            _wanna_Judge = UPaHBTSetting.Instance.WorldPVP_wanna_Judge;
            _wanna_LoH = UPaHBTSetting.Instance.WorldPVP_wanna_LoH;
            _wanna_mana_potion = UPaHBTSetting.Instance.WorldPVP_wanna_mana_potion;
            _wanna_mount = UPaHBTSetting.Instance.WorldPVP_wanna_mount;
            _wanna_move_to_heal = UPaHBTSetting.Instance.WorldPVP_wanna_move_to_heal;
            _wanna_move_to_HoJ = UPaHBTSetting.Instance.WorldPVP_wanna_move_to_HoJ;
            _wanna_rebuke = UPaHBTSetting.Instance.WorldPVP_wanna_rebuke;
            _wanna_stoneform = UPaHBTSetting.Instance.WorldPVP_wanna_stoneform;
            _wanna_target = UPaHBTSetting.Instance.WorldPVP_wanna_target;
            _wanna_torrent = UPaHBTSetting.Instance.WorldPVP_wanna_torrent;
            _wanna_urgent_cleanse = UPaHBTSetting.Instance.WorldPVP_wanna_urgent_cleanse;
            _wanna_lifeblood = UPaHBTSetting.Instance.WorldPVP_wanna_lifeblood;
            _do_not_dismount_EVER = UPaHBTSetting.Instance.WorldPVP_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPaHBTSetting.Instance.WorldPVP_do_not_dismount_ooc;
            _get_tank_from_focus = UPaHBTSetting.Instance.WorldPVP_get_tank_from_focus;
            _get_tank_from_lua = UPaHBTSetting.Instance.WorldPVP_get_tank_from_lua;
            _bless_type = UPaHBTSetting.Instance.WorldPVP_bless_type;
            _decice_during_GCD = UPaHBTSetting.Instance.WorldPVP_decice_during_GCD;
            _intellywait = UPaHBTSetting.Instance.WorldPVP_intellywait;
            _wanna_HS_on_CD = UPaHBTSetting.Instance.WorldPVP_wanna_HS_on_CD;
            _cleanse_only_self_and_tank = UPaHBTSetting.Instance.WorldPVP_cleanse_only_self_and_tank;
            return true;
        }

        private bool ARENA_Variable_inizializer()
        {
            PVP_dispell_inizializer();
            _aura_type = UPaHBTSetting.Instance.ARENA_aura_type;
            _do_not_heal_above = UPaHBTSetting.Instance.ARENA_do_not_heal_above;
            _HR_how_far = UPaHBTSetting.Instance.ARENA_HR_how_far;
            _HR_how_much_health = UPaHBTSetting.Instance.ARENA_HR_how_much_health;
            _Inf_of_light_wanna_DL = UPaHBTSetting.Instance.ARENA_Inf_of_light_wanna_DL;
            _mana_judge = UPaHBTSetting.Instance.ARENA_mana_judge;
            _max_healing_distance = UPaHBTSetting.Instance.ARENA_max_healing_distance;
            _min_Divine_Plea_mana = UPaHBTSetting.Instance.ARENA_min_Divine_Plea_mana;
            _min_DL_hp = UPaHBTSetting.Instance.ARENA_min_DL_hp;
            _min_DP_hp = UPaHBTSetting.Instance.ARENA_min_DP_hp;
            _min_DS_hp = UPaHBTSetting.Instance.ARENA_min_DS_hp;
            _min_FoL_hp = UPaHBTSetting.Instance.ARENA_min_FoL_hp;
            _min_gift_hp = UPaHBTSetting.Instance.ARENA_min_gift_hp;
            _min_HL_hp = UPaHBTSetting.Instance.ARENA_min_HL_hp;
            _min_HoP_hp = UPaHBTSetting.Instance.ARENA_min_HoP_hp;
            _min_HoS_hp = UPaHBTSetting.Instance.ARENA_min_HoS_hp;
            _min_Inf_of_light_DL_hp = UPaHBTSetting.Instance.ARENA_min_Inf_of_light_DL_hp;
            _min_LoH_hp = UPaHBTSetting.Instance.ARENA_min_LoH_hp;
            _min_mana_potion = UPaHBTSetting.Instance.ARENA_min_mana_potion;
            _min_mana_rec_trinket = UPaHBTSetting.Instance.ARENA_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPaHBTSetting.Instance.ARENA_min_ohshitbutton_activator;
            _min_player_inside_HR = UPaHBTSetting.Instance.ARENA_min_player_inside_HR;
            _min_stoneform = UPaHBTSetting.Instance.ARENA_min_stoneform;
            _min_torrent_mana_perc = UPaHBTSetting.Instance.ARENA_min_torrent_mana_perc;
            _rest_if_mana_below = UPaHBTSetting.Instance.ARENA_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPaHBTSetting.Instance.ARENA_use_mana_rec_trinket_every;
            _wanna_AW = UPaHBTSetting.Instance.ARENA_wanna_AW;
            _wanna_buff = UPaHBTSetting.Instance.ARENA_wanna_buff;
            _wanna_cleanse = UPaHBTSetting.Instance.ARENA_wanna_cleanse;
            _wanna_crusader = UPaHBTSetting.Instance.ARENA_wanna_crusader;
            _wanna_CS = UPaHBTSetting.Instance.ARENA_wanna_CS;
            _wanna_DF = UPaHBTSetting.Instance.ARENA_wanna_DF;
            _wanna_DP = UPaHBTSetting.Instance.ARENA_wanna_DP;
            _wanna_DS = UPaHBTSetting.Instance.ARENA_wanna_DS;
            _wanna_everymanforhimself = UPaHBTSetting.Instance.ARENA_wanna_everymanforhimself;
            _wanna_face = UPaHBTSetting.Instance.ARENA_wanna_face;
            _wanna_gift = UPaHBTSetting.Instance.ARENA_wanna_gift;
            _wanna_GotAK = UPaHBTSetting.Instance.ARENA_wanna_GotAK;
            _wanna_HoJ = UPaHBTSetting.Instance.ARENA_wanna_HoJ;
            _wanna_HoP = UPaHBTSetting.Instance.ARENA_wanna_HoP;
            _wanna_HoS = UPaHBTSetting.Instance.ARENA_wanna_HoS;
            _wanna_HoW = UPaHBTSetting.Instance.ARENA_wanna_HoW;
            _wanna_HR = UPaHBTSetting.Instance.ARENA_wanna_HR;
            _wanna_Judge = UPaHBTSetting.Instance.ARENA_wanna_Judge;
            _wanna_LoH = UPaHBTSetting.Instance.ARENA_wanna_LoH;
            _wanna_mana_potion = UPaHBTSetting.Instance.ARENA_wanna_mana_potion;
            _wanna_mount = UPaHBTSetting.Instance.ARENA_wanna_mount;
            _wanna_move_to_heal = UPaHBTSetting.Instance.ARENA_wanna_move_to_heal;
            _wanna_move_to_HoJ = UPaHBTSetting.Instance.ARENA_wanna_move_to_HoJ;
            _wanna_rebuke = UPaHBTSetting.Instance.ARENA_wanna_rebuke;
            _wanna_stoneform = UPaHBTSetting.Instance.ARENA_wanna_stoneform;
            _wanna_target = UPaHBTSetting.Instance.ARENA_wanna_target;
            _wanna_torrent = UPaHBTSetting.Instance.ARENA_wanna_torrent;
            _wanna_urgent_cleanse = UPaHBTSetting.Instance.ARENA_wanna_urgent_cleanse;
            _wanna_taunt = UPaHBTSetting.Instance.ARENA_wanna_taunt;
            _wanna_HoF = UPaHBTSetting.Instance.ARENA_wanna_HoF;
            _wanna_lifeblood = UPaHBTSetting.Instance.ARENA_wanna_lifeblood;
            _do_not_dismount_EVER = UPaHBTSetting.Instance.ARENA_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPaHBTSetting.Instance.ARENA_do_not_dismount_ooc;
            _get_tank_from_focus = UPaHBTSetting.Instance.ARENA_get_tank_from_focus;
            _get_tank_from_lua = UPaHBTSetting.Instance.ARENA_get_tank_from_lua;
            _bless_type = UPaHBTSetting.Instance.ARENA_bless_type;
            _decice_during_GCD = UPaHBTSetting.Instance.ARENA_decice_during_GCD;
            _intellywait = UPaHBTSetting.Instance.ARENA_intellywait;
            _wanna_HS_on_CD = UPaHBTSetting.Instance.ARENA_wanna_HS_on_CD;
            _cleanse_only_self_and_tank = UPaHBTSetting.Instance.ARENA_cleanse_only_self_and_tank;
            return true;
        }

        private bool ARENA2v2_Variable_inizializer()
        {
            PVP_dispell_inizializer();
            _aura_type = UPaHBTSetting.Instance.ARENA2v2_aura_type;
            _do_not_heal_above = UPaHBTSetting.Instance.ARENA2v2_do_not_heal_above;
            _HR_how_far = UPaHBTSetting.Instance.ARENA2v2_HR_how_far;
            _HR_how_much_health = UPaHBTSetting.Instance.ARENA2v2_HR_how_much_health;
            _Inf_of_light_wanna_DL = UPaHBTSetting.Instance.ARENA2v2_Inf_of_light_wanna_DL;
            _mana_judge = UPaHBTSetting.Instance.ARENA2v2_mana_judge;
            _max_healing_distance = UPaHBTSetting.Instance.ARENA2v2_max_healing_distance;
            _min_Divine_Plea_mana = UPaHBTSetting.Instance.ARENA2v2_min_Divine_Plea_mana;
            _min_DL_hp = UPaHBTSetting.Instance.ARENA2v2_min_DL_hp;
            _min_DP_hp = UPaHBTSetting.Instance.ARENA2v2_min_DP_hp;
            _min_DS_hp = UPaHBTSetting.Instance.ARENA2v2_min_DS_hp;
            _min_FoL_hp = UPaHBTSetting.Instance.ARENA2v2_min_FoL_hp;
            _min_gift_hp = UPaHBTSetting.Instance.ARENA2v2_min_gift_hp;
            _min_HL_hp = UPaHBTSetting.Instance.ARENA2v2_min_HL_hp;
            _min_HoP_hp = UPaHBTSetting.Instance.ARENA2v2_min_HoP_hp;
            _min_HoS_hp = UPaHBTSetting.Instance.ARENA2v2_min_HoS_hp;
            _min_Inf_of_light_DL_hp = UPaHBTSetting.Instance.ARENA2v2_min_Inf_of_light_DL_hp;
            _min_LoH_hp = UPaHBTSetting.Instance.ARENA2v2_min_LoH_hp;
            _min_mana_potion = UPaHBTSetting.Instance.ARENA2v2_min_mana_potion;
            _min_mana_rec_trinket = UPaHBTSetting.Instance.ARENA2v2_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPaHBTSetting.Instance.ARENA2v2_min_ohshitbutton_activator;
            _min_player_inside_HR = UPaHBTSetting.Instance.ARENA2v2_min_player_inside_HR;
            _min_stoneform = UPaHBTSetting.Instance.ARENA2v2_min_stoneform;
            _min_torrent_mana_perc = UPaHBTSetting.Instance.ARENA2v2_min_torrent_mana_perc;
            _rest_if_mana_below = UPaHBTSetting.Instance.ARENA2v2_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPaHBTSetting.Instance.ARENA2v2_use_mana_rec_trinket_every;
            _wanna_AW = UPaHBTSetting.Instance.ARENA2v2_wanna_AW;
            _wanna_buff = UPaHBTSetting.Instance.ARENA2v2_wanna_buff;
            _wanna_cleanse = UPaHBTSetting.Instance.ARENA2v2_wanna_cleanse;
            _wanna_crusader = UPaHBTSetting.Instance.ARENA2v2_wanna_crusader;
            _wanna_CS = UPaHBTSetting.Instance.ARENA2v2_wanna_CS;
            _wanna_DF = UPaHBTSetting.Instance.ARENA2v2_wanna_DF;
            _wanna_DP = UPaHBTSetting.Instance.ARENA2v2_wanna_DP;
            _wanna_DS = UPaHBTSetting.Instance.ARENA2v2_wanna_DS;
            _wanna_everymanforhimself = UPaHBTSetting.Instance.ARENA2v2_wanna_everymanforhimself;
            _wanna_face = UPaHBTSetting.Instance.ARENA2v2_wanna_face;
            _wanna_gift = UPaHBTSetting.Instance.ARENA2v2_wanna_gift;
            _wanna_GotAK = UPaHBTSetting.Instance.ARENA2v2_wanna_GotAK;
            _wanna_HoJ = UPaHBTSetting.Instance.ARENA2v2_wanna_HoJ;
            _wanna_HoP = UPaHBTSetting.Instance.ARENA2v2_wanna_HoP;
            _wanna_HoS = UPaHBTSetting.Instance.ARENA2v2_wanna_HoS;
            _wanna_HoW = UPaHBTSetting.Instance.ARENA2v2_wanna_HoW;
            _wanna_HR = UPaHBTSetting.Instance.ARENA2v2_wanna_HR;
            _wanna_Judge = UPaHBTSetting.Instance.ARENA2v2_wanna_Judge;
            _wanna_LoH = UPaHBTSetting.Instance.ARENA2v2_wanna_LoH;
            _wanna_mana_potion = UPaHBTSetting.Instance.ARENA2v2_wanna_mana_potion;
            _wanna_mount = UPaHBTSetting.Instance.ARENA2v2_wanna_mount;
            _wanna_move_to_heal = UPaHBTSetting.Instance.ARENA2v2_wanna_move_to_heal;
            _wanna_move_to_HoJ = UPaHBTSetting.Instance.ARENA2v2_wanna_move_to_HoJ;
            _wanna_rebuke = UPaHBTSetting.Instance.ARENA2v2_wanna_rebuke;
            _wanna_stoneform = UPaHBTSetting.Instance.ARENA2v2_wanna_stoneform;
            _wanna_target = UPaHBTSetting.Instance.ARENA2v2_wanna_target;
            _wanna_torrent = UPaHBTSetting.Instance.ARENA2v2_wanna_torrent;
            _wanna_urgent_cleanse = UPaHBTSetting.Instance.ARENA2v2_wanna_urgent_cleanse;
            _wanna_lifeblood = UPaHBTSetting.Instance.ARENA2v2_wanna_lifeblood;
            _do_not_dismount_EVER = UPaHBTSetting.Instance.ARENA2v2_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPaHBTSetting.Instance.ARENA2v2_do_not_dismount_ooc;
            _get_tank_from_focus = UPaHBTSetting.Instance.ARENA2v2_get_tank_from_focus;
            _get_tank_from_lua = UPaHBTSetting.Instance.ARENA2v2_get_tank_from_lua;
            _bless_type = UPaHBTSetting.Instance.ARENA2v2_bless_type;
            _decice_during_GCD = UPaHBTSetting.Instance.ARENA2v2_decice_during_GCD;
            _intellywait = UPaHBTSetting.Instance.ARENA2v2_intellywait;
            _wanna_HS_on_CD = UPaHBTSetting.Instance.ARENA2v2_wanna_HS_on_CD;
            _cleanse_only_self_and_tank = UPaHBTSetting.Instance.ARENA2v2_cleanse_only_self_and_tank;
            return true;
        }

        private bool RAF_Variable_inizializer()
        {
            PVE_dispell_inizializer();
            _aura_type = UPaHBTSetting.Instance.RAF_aura_type;
            _do_not_heal_above = UPaHBTSetting.Instance.RAF_do_not_heal_above;
            _HR_how_far = UPaHBTSetting.Instance.RAF_HR_how_far;
            _HR_how_much_health = UPaHBTSetting.Instance.RAF_HR_how_much_health;
            _Inf_of_light_wanna_DL = UPaHBTSetting.Instance.RAF_Inf_of_light_wanna_DL;
            _mana_judge = UPaHBTSetting.Instance.RAF_mana_judge;
            _max_healing_distance = UPaHBTSetting.Instance.RAF_max_healing_distance;
            _min_Divine_Plea_mana = UPaHBTSetting.Instance.RAF_min_Divine_Plea_mana;
            _min_DL_hp = UPaHBTSetting.Instance.RAF_min_DL_hp;
            _min_DP_hp = UPaHBTSetting.Instance.RAF_min_DP_hp;
            _min_DS_hp = UPaHBTSetting.Instance.RAF_min_DS_hp;
            _min_FoL_hp = UPaHBTSetting.Instance.RAF_min_FoL_hp;
            _min_gift_hp = UPaHBTSetting.Instance.RAF_min_gift_hp;
            _min_HL_hp = UPaHBTSetting.Instance.RAF_min_HL_hp;
            _min_HoP_hp = UPaHBTSetting.Instance.RAF_min_HoP_hp;
            _min_HoS_hp = UPaHBTSetting.Instance.RAF_min_HoS_hp;
            _min_Inf_of_light_DL_hp = UPaHBTSetting.Instance.RAF_min_Inf_of_light_DL_hp;
            _min_LoH_hp = UPaHBTSetting.Instance.RAF_min_LoH_hp;
            _min_mana_potion = UPaHBTSetting.Instance.RAF_min_mana_potion;
            _min_mana_rec_trinket = UPaHBTSetting.Instance.RAF_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPaHBTSetting.Instance.RAF_min_ohshitbutton_activator;
            _min_player_inside_HR = UPaHBTSetting.Instance.RAF_min_player_inside_HR;
            _min_stoneform = UPaHBTSetting.Instance.RAF_min_stoneform;
            _min_torrent_mana_perc = UPaHBTSetting.Instance.RAF_min_torrent_mana_perc;
            _rest_if_mana_below = UPaHBTSetting.Instance.RAF_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPaHBTSetting.Instance.RAF_use_mana_rec_trinket_every;
            _wanna_AW = UPaHBTSetting.Instance.RAF_wanna_AW;
            _wanna_buff = UPaHBTSetting.Instance.RAF_wanna_buff;
            _wanna_cleanse = UPaHBTSetting.Instance.RAF_wanna_cleanse;
            _wanna_crusader = UPaHBTSetting.Instance.RAF_wanna_crusader;
            _wanna_CS = UPaHBTSetting.Instance.RAF_wanna_CS;
            _wanna_DF = UPaHBTSetting.Instance.RAF_wanna_DF;
            _wanna_DP = UPaHBTSetting.Instance.RAF_wanna_DP;
            _wanna_DS = UPaHBTSetting.Instance.RAF_wanna_DS;
            _wanna_everymanforhimself = UPaHBTSetting.Instance.RAF_wanna_everymanforhimself;
            _wanna_face = UPaHBTSetting.Instance.RAF_wanna_face;
            _wanna_gift = UPaHBTSetting.Instance.RAF_wanna_gift;
            _wanna_GotAK = UPaHBTSetting.Instance.RAF_wanna_GotAK;
            _wanna_HoJ = UPaHBTSetting.Instance.RAF_wanna_HoJ;
            _wanna_HoP = UPaHBTSetting.Instance.RAF_wanna_HoP;
            _wanna_HoS = UPaHBTSetting.Instance.RAF_wanna_HoS;
            _wanna_HoW = UPaHBTSetting.Instance.RAF_wanna_HoW;
            _wanna_HR = UPaHBTSetting.Instance.RAF_wanna_HR;
            _wanna_Judge = UPaHBTSetting.Instance.RAF_wanna_Judge;
            _wanna_LoH = UPaHBTSetting.Instance.RAF_wanna_LoH;
            _wanna_mana_potion = UPaHBTSetting.Instance.RAF_wanna_mana_potion;
            _wanna_mount = UPaHBTSetting.Instance.RAF_wanna_mount;
            _wanna_move_to_heal = UPaHBTSetting.Instance.RAF_wanna_move_to_heal;
            _wanna_move_to_HoJ = UPaHBTSetting.Instance.RAF_wanna_move_to_HoJ;
            _wanna_rebuke = UPaHBTSetting.Instance.RAF_wanna_rebuke;
            _wanna_stoneform = UPaHBTSetting.Instance.RAF_wanna_stoneform;
            _wanna_target = UPaHBTSetting.Instance.RAF_wanna_target;
            _wanna_torrent = UPaHBTSetting.Instance.RAF_wanna_torrent;
            _wanna_urgent_cleanse = UPaHBTSetting.Instance.RAF_wanna_urgent_cleanse;
            _wanna_lifeblood = UPaHBTSetting.Instance.RAF_wanna_lifeblood;
            _do_not_dismount_EVER = UPaHBTSetting.Instance.RAF_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPaHBTSetting.Instance.RAF_do_not_dismount_ooc;
            _get_tank_from_focus = UPaHBTSetting.Instance.RAF_get_tank_from_focus;
            _get_tank_from_lua = UPaHBTSetting.Instance.RAF_get_tank_from_lua;
            _bless_type = UPaHBTSetting.Instance.RAF_bless_type;
            _decice_during_GCD = UPaHBTSetting.Instance.RAF_decice_during_GCD;
            _intellywait = UPaHBTSetting.Instance.RAF_intellywait;
            _wanna_HS_on_CD = UPaHBTSetting.Instance.RAF_wanna_HS_on_CD;
            _cleanse_only_self_and_tank = UPaHBTSetting.Instance.RAF_cleanse_only_self_and_tank;
            return true;
        }

        public bool PVE_dispell_inizializer()
        {
            _do_not_touch[1] = UPaHBTSetting.Instance.PVE_do_not_touch_TB1;
            _do_not_touch[2] = UPaHBTSetting.Instance.PVE_do_not_touch_TB2;
            _do_not_touch[3] = UPaHBTSetting.Instance.PVE_do_not_touch_TB3;
            _do_not_touch[4] = UPaHBTSetting.Instance.PVE_do_not_touch_TB4;
            _do_not_touch[5] = UPaHBTSetting.Instance.PVE_do_not_touch_TB5;
            _do_not_touch[6] = UPaHBTSetting.Instance.PVE_do_not_touch_TB6;
            _do_not_touch[7] = UPaHBTSetting.Instance.PVE_do_not_touch_TB7;
            _do_not_touch[8] = UPaHBTSetting.Instance.PVE_do_not_touch_TB8;
            _do_not_touch[9] = UPaHBTSetting.Instance.PVE_do_not_touch_TB9;
            _do_not_touch[10] = UPaHBTSetting.Instance.PVE_do_not_touch_TB10;
            _do_not_touch[11] = UPaHBTSetting.Instance.PVE_do_not_touch_TB11;
            _do_not_touch[12] = UPaHBTSetting.Instance.PVE_do_not_touch_TB12;
            _do_not_touch[13] = UPaHBTSetting.Instance.PVE_do_not_touch_TB13;
            _do_not_touch[14] = UPaHBTSetting.Instance.PVE_do_not_touch_TB14;
            _do_not_touch[15] = UPaHBTSetting.Instance.PVE_do_not_touch_TB15;
            _do_not_touch[16] = UPaHBTSetting.Instance.PVE_do_not_touch_TB16;
            _do_not_touch[17] = UPaHBTSetting.Instance.PVE_do_not_touch_TB17;
            _do_not_touch[18] = UPaHBTSetting.Instance.PVE_do_not_touch_TB18;
            _do_not_touch[19] = UPaHBTSetting.Instance.PVE_do_not_touch_TB19;
            _do_not_touch[20] = UPaHBTSetting.Instance.PVE_do_not_touch_TB20;

            _dispell_ASAP[1] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB1;
            _dispell_ASAP[2] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB2;
            _dispell_ASAP[3] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB3;
            _dispell_ASAP[4] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB4;
            _dispell_ASAP[5] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB5;
            _dispell_ASAP[6] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB6;
            _dispell_ASAP[7] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB7;
            _dispell_ASAP[8] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB8;
            _dispell_ASAP[9] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB9;
            _dispell_ASAP[10] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB10;
            _dispell_ASAP[11] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB11;
            _dispell_ASAP[12] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB12;
            _dispell_ASAP[13] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB13;
            _dispell_ASAP[14] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB14;
            _dispell_ASAP[15] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB15;
            _dispell_ASAP[16] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB16;
            _dispell_ASAP[17] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB17;
            _dispell_ASAP[18] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB18;
            _dispell_ASAP[19] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB19;
            _dispell_ASAP[20] = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB20;
            return true;
        }

        public bool PVP_dispell_inizializer()
        {
            _do_not_touch[1] = UPaHBTSetting.Instance.PVP_do_not_touch_TB1;
            _do_not_touch[2] = UPaHBTSetting.Instance.PVP_do_not_touch_TB2;
            _do_not_touch[3] = UPaHBTSetting.Instance.PVP_do_not_touch_TB3;
            _do_not_touch[4] = UPaHBTSetting.Instance.PVP_do_not_touch_TB4;
            _do_not_touch[5] = UPaHBTSetting.Instance.PVP_do_not_touch_TB5;
            _do_not_touch[6] = UPaHBTSetting.Instance.PVP_do_not_touch_TB6;
            _do_not_touch[7] = UPaHBTSetting.Instance.PVP_do_not_touch_TB7;
            _do_not_touch[8] = UPaHBTSetting.Instance.PVP_do_not_touch_TB8;
            _do_not_touch[9] = UPaHBTSetting.Instance.PVP_do_not_touch_TB9;
            _do_not_touch[10] = UPaHBTSetting.Instance.PVP_do_not_touch_TB10;
            _do_not_touch[11] = UPaHBTSetting.Instance.PVP_do_not_touch_TB11;
            _do_not_touch[12] = UPaHBTSetting.Instance.PVP_do_not_touch_TB12;
            _do_not_touch[13] = UPaHBTSetting.Instance.PVP_do_not_touch_TB13;
            _do_not_touch[14] = UPaHBTSetting.Instance.PVP_do_not_touch_TB14;
            _do_not_touch[15] = UPaHBTSetting.Instance.PVP_do_not_touch_TB15;
            _do_not_touch[16] = UPaHBTSetting.Instance.PVP_do_not_touch_TB16;
            _do_not_touch[17] = UPaHBTSetting.Instance.PVP_do_not_touch_TB17;
            _do_not_touch[18] = UPaHBTSetting.Instance.PVP_do_not_touch_TB18;
            _do_not_touch[19] = UPaHBTSetting.Instance.PVP_do_not_touch_TB19;
            _do_not_touch[20] = UPaHBTSetting.Instance.PVP_do_not_touch_TB20;

            _dispell_ASAP[1] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB1;
            _dispell_ASAP[2] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB2;
            _dispell_ASAP[3] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB3;
            _dispell_ASAP[4] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB4;
            _dispell_ASAP[5] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB5;
            _dispell_ASAP[6] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB6;
            _dispell_ASAP[7] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB7;
            _dispell_ASAP[8] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB8;
            _dispell_ASAP[9] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB9;
            _dispell_ASAP[10] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB10;
            _dispell_ASAP[11] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB11;
            _dispell_ASAP[12] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB12;
            _dispell_ASAP[13] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB13;
            _dispell_ASAP[14] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB14;
            _dispell_ASAP[15] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB15;
            _dispell_ASAP[16] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB16;
            _dispell_ASAP[17] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB17;
            _dispell_ASAP[18] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB18;
            _dispell_ASAP[19] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB19;
            _dispell_ASAP[20] = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB20;
            return true;
        }
        public bool Variable_Printer()
        {
            slog("_Inf_of_light_wanna_DL {0}", _Inf_of_light_wanna_DL);
            slog("_wanna_AW {0}", _wanna_AW);
            slog("_wanna_buff {0}", _wanna_buff);
            slog("_wanna_cleanse {0}", _wanna_cleanse);
            slog("_wanna_crusader {0}", _wanna_crusader);
            slog("_wanna_CS {0}", _wanna_CS);
            slog("_wanna_DF {0}", _wanna_DF);
            slog("_wanna_DP {0}", _wanna_DP);
            slog("_wanna_DS {0}", _wanna_DS);
            slog("_wanna_everymanforhimself {0}", _wanna_everymanforhimself);
            slog("_wanna_face {0}", _wanna_face);
            slog("_wanna_gift {0}", _wanna_gift);
            slog("_wanna_GotAK {0}", _wanna_GotAK);
            slog("_wanna_HoJ {0}", _wanna_HoJ);
            slog("_wanna_HoP {0}", _wanna_HoP);
            slog("_wanna_HoS {0}", _wanna_HoS);
            slog("_wanna_HoW {0}", _wanna_HoW);
            slog("_wanna_HR {0}", _wanna_HR);
            slog("_wanna_Judge {0}", _wanna_Judge);
            slog("_wanna_LoH {0}", _wanna_LoH);
            slog("_wanna_mana_potion {0}", _wanna_mana_potion);
            slog("_wanna_mount {0}", _wanna_mount);
            slog("_wanna_move_to_heal {0}", _wanna_move_to_heal);
            slog("_wanna_move_to_HoJ {0}", _wanna_move_to_HoJ);
            slog("_wanna_rebuke {0}", _wanna_rebuke);
            slog("_wanna_stoneform {0}", _wanna_stoneform);
            slog("_wanna_target {0}", _wanna_target);
            slog("_wanna_torrent {0}", _wanna_torrent);
            slog("_wanna_urgent_cleanse {0}", _wanna_urgent_cleanse);
            slog("_aura_type {0}", _aura_type);
            slog("_do_not_heal_above {0}", _do_not_heal_above);
            slog("_HR_how_far {0}", _HR_how_far);
            slog("_HR_how_much_health {0}", _HR_how_much_health);
            slog("_mana_judge {0}", _mana_judge);
            slog("_max_healing_distance {0}", _max_healing_distance);
            slog("_min_Divine_Plea_mana {0}", _min_Divine_Plea_mana);
            slog("_min_DL_hp {0}", _min_DL_hp);
            slog("_min_DP_hp {0}", _min_DP_hp);
            slog("_min_DS_hp {0}", _min_DS_hp);
            slog("_min_FoL_hp {0}", _min_FoL_hp);
            slog("_min_gift_hp {0}", _min_gift_hp);
            slog("_min_HL_hp {0}", _min_HL_hp);
            slog("_min_HoP_hp {0}", _min_HoP_hp);
            slog("_min_HoS_hp {0}", _min_HoS_hp);
            slog("_min_Inf_of_light_DL_hp {0}", _min_Inf_of_light_DL_hp);
            slog("_min_LoH_hp {0}", _min_LoH_hp);
            slog("_min_mana_potion {0}", _min_mana_potion);
            slog("_min_mana_rec_trinket {0}", _min_mana_rec_trinket);
            slog("_min_ohshitbutton_activator {0}", _min_ohshitbutton_activator);
            slog("_min_player_inside_HR {0}", _min_player_inside_HR);
            slog("_min_stoneform {0}", _min_stoneform);
            slog("_min_torrent_mana_perc {0}", _min_torrent_mana_perc);
            slog("_rest_if_mana_below {0}", _rest_if_mana_below);
            slog("_use_mana_rec_trinket_every {0}", _use_mana_rec_trinket_every);
            slog("_wanna_move {0}", _wanna_move);
            slog("_wanna_taunt {0}", _wanna_taunt);
            slog("_wanna_HoF {0}", _wanna_HoF);
            slog("_wanna_lifeblood {0}", _wanna_lifeblood);
            slog("_do_not_dismount_ooc {0}", _do_not_dismount_ooc);
            slog("_do_not_dismount_EVER {0}", _do_not_dismount_EVER);
            slog("_selective_healing {0}", _selective_healing);
            if (_selective_healing)
            {
                for (i = 0; i < 25; i++)
                {
                    slog("Will heal raid member " + i + " {0} ", healornot[i]);
                }
            }
            slog("_get_tank_from_focus {0}", _get_tank_from_focus);
            slog("_ignore_beacon {0}", _ignore_beacon);
            slog("_get_tank_from_lua {0}", _get_tank_from_lua);
            slog("_stop_DL_if_above {0}", _stop_DL_if_above);
            slog("_bless_type {0}", _bless_type);
            slog("_healing_tank_priority {0}", _healing_tank_priority);
            slog("_tank_healing_priority_multiplier {0}", _tank_healing_priority_multiplier);
            slog("_decice_during_GCD {0}", _decice_during_GCD);
            slog("_intellywait {0}", _intellywait);
            slog("_how_much_wait {0}", _how_much_wait);
            if (_intellywait)
            {
                slog("you are using combat sistem 6, INTELLYWAIT!");
            }
            else if (_decice_during_GCD)
            {
                slog("you are using combat sistem 4, SPEED!");
            }
            else
            {
                slog("you are using combat sistem 5, ACCURANCY!");
            }
            slog("_answer_PVP_attacks {0}", _answer_PVP_attacks);
            for (i = 1; i < 21; i++)
            {
                if (_do_not_touch[i] != "")
                {
                    slog("{0} Will not touch people with {1}", i, _do_not_touch[i]);
                }
            }
            for (i = 1; i < 21; i++)
            {
                if (_dispell_ASAP[i] != "")
                {
                    slog("{0} Will heal ASAP people with {1}", i, _dispell_ASAP[i]);
                }
            }
            slog("_enable_pull {0}", _enable_pull);
            slog("_wanna_HS_on_CD {0}", _wanna_HS_on_CD);
            slog("_trinket1_name {0}",_trinket1_name);
            slog("_trinket1_ID {0}", _trinket1_ID);
            slog("_trinket1_CD {0}", _trinket1_CD);
            slog("_trinket1_use_when {0}", _trinket1_use_when);
            slog("_trinket1_passive {0}", _trinket1_passive);
            slog("_trinket2_name {0}", _trinket2_name);
            slog("_trinket2_ID {0}", _trinket2_ID);
            slog("_trinket2_CD {0}", _trinket2_CD);
            slog("_trinket2_use_when {0}", _trinket2_use_when);
            slog("_trinket2_passive {0}", _trinket2_passive);
            slog("_Stop_Healing {0}", _Stop_Healing);
            slog("_precasting {0}", _precasting);
            slog("_cleanse_only_self_and_tank {0} ", _cleanse_only_self_and_tank);
            return true;
        }

        #endregion
    }
}