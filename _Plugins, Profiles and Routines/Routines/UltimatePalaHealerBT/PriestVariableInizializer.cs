namespace UltimatePaladinHealerBT
{
    public partial class UltimatePalaHealerBT
    {
        #region PriestVariableInizializer

        public bool Priest_Variable_inizializer()
        {
            Priest_General_variable_inizializer();
            switch (usedBehaviour)
            {
                case "Solo":
                    Solo_Priest_Variable_inizializer();
                    break;
                case "Arena":
                    ARENA_Priest_Variable_inizializer();
                    break;
                case "World PVP":
                    WorldPVP_Priest_Variable_inizializer();
                    break;
                case "Party or Raid":
                    RAF_Priest_Variable_inizializer();
                    break;
                case "Battleground":
                    Battleground_Priest_Variable_inizializer();
                    break;
                /*case "Solo":
                    PVE_Priest_Variable_inizializer();
                    break;*/
                case "Dungeon":
                    PVE_Priest_Variable_inizializer();
                    break;
                case "Raid":
                    Raid_Priest_Variable_inizializer();
                    break;
                default:
                    slog("no valid Behaviour found, CC will stop NOW!");
                    return false;
                //break;
            }
            return true;
        }

        private bool Priest_General_variable_inizializer()
        {
            _precasting = UPrHBTSetting.Instance.General_precasting * 10;
            _how_much_wait = UPrHBTSetting.Instance.General_how_much_wait;
            return true;
        }

        private bool Raid_Priest_Variable_inizializer()
        {
            Priest_PVE_dispell_inizializer();
            _general_raid_healer = UPrHBTSetting.Instance.Raid_general_raid_healer;
            Decice_if_special_or_normal_raid();
            if (!_raid_healer)
            {
                Raid_Priest_Normal_Variable_inizializer();
            }
            else 
            {
                Raid_Priest_AOE_variable_inizializer();
            }
            return true;
        }

        private bool Raid_Priest_AOE_variable_inizializer()
        {
            _do_not_heal_above = UPrHBTSetting.Instance.Raid_AOE_do_not_heal_above;
            _max_healing_distance = UPrHBTSetting.Instance.Raid_AOE_max_healing_distance;
            _min_gift_hp = UPrHBTSetting.Instance.Raid_AOE_min_gift_hp;
            _min_mana_potion = UPrHBTSetting.Instance.Raid_AOE_min_mana_potion;
            _min_mana_rec_trinket = UPrHBTSetting.Instance.Raid_AOE_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPrHBTSetting.Instance.Raid_AOE_min_ohshitbutton_activator;
            _min_stoneform = UPrHBTSetting.Instance.Raid_AOE_min_stoneform;
            _min_torrent_mana_perc = UPrHBTSetting.Instance.Raid_AOE_min_torrent_mana_perc;
            _rest_if_mana_below = UPrHBTSetting.Instance.Raid_AOE_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPrHBTSetting.Instance.Raid_AOE_use_mana_rec_trinket_every;
            _wanna_buff = UPrHBTSetting.Instance.Raid_AOE_wanna_buff;
            _wanna_cleanse = UPrHBTSetting.Instance.Raid_AOE_wanna_cleanse;
            _wanna_everymanforhimself = UPrHBTSetting.Instance.Raid_AOE_wanna_everymanforhimself;
            _wanna_face = UPrHBTSetting.Instance.Raid_AOE_wanna_face;
            _wanna_gift = UPrHBTSetting.Instance.Raid_AOE_wanna_gift;
            _wanna_mana_potion = UPrHBTSetting.Instance.Raid_AOE_wanna_mana_potion;
            _wanna_mount = UPrHBTSetting.Instance.Raid_AOE_wanna_mount;
            _wanna_move_to_heal = UPrHBTSetting.Instance.Raid_AOE_wanna_move_to_heal;
            _wanna_stoneform = UPrHBTSetting.Instance.Raid_AOE_wanna_stoneform;
            _wanna_target = UPrHBTSetting.Instance.Raid_AOE_wanna_target;
            _wanna_torrent = UPrHBTSetting.Instance.Raid_AOE_wanna_torrent;
            _wanna_urgent_cleanse = UPrHBTSetting.Instance.Raid_AOE_wanna_urgent_cleanse;
            _wanna_lifeblood = UPrHBTSetting.Instance.Raid_AOE_wanna_lifeblood;
            _do_not_dismount_EVER = UPrHBTSetting.Instance.Raid_AOE_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPrHBTSetting.Instance.Raid_AOE_do_not_dismount_ooc;
            _get_tank_from_focus = UPrHBTSetting.Instance.Raid_AOE_get_tank_from_focus;
            _get_tank_from_lua = UPrHBTSetting.Instance.Raid_AOE_get_tank_from_lua;
            _healing_tank_priority = UPrHBTSetting.Instance.Raid_AOE_healing_tank_priority;
            _binding_heal_min_hp = UPrHBTSetting.Instance.Raid_AOE_binding_heal_min_hp;
            _HWS_min_hp = UPrHBTSetting.Instance.Raid_AOE_HWS_min_hp;
            _Serendipity_min_hp = UPrHBTSetting.Instance.Raid_AOE_Serendipity_min_hp;
            _Serendipity_GH_min_hp = UPrHBTSetting.Instance.Raid_AOE_Serendipity_GH_min_hp;
            _Heal_min_hp = UPrHBTSetting.Instance.Raid_AOE_Heal_min_hp;
            _GH_min_hp = UPrHBTSetting.Instance.Raid_AOE_GH_min_hp;
            _SoL_min_hp = UPrHBTSetting.Instance.Raid_AOE_SoL_min_hp;
            _min_player_inside_CoH = UPrHBTSetting.Instance.Raid_AOE_min_player_inside_CoH;
            _CoH_how_much_health = UPrHBTSetting.Instance.Raid_AOE_CoH_how_much_health;
            _DY_how_much_health = UPrHBTSetting.Instance.Raid_AOE_DY_how_much_health;
            _min_player_inside_DY = UPrHBTSetting.Instance.Raid_AOE_min_player_inside_DY;
            _wanna_GS = UPrHBTSetting.Instance.Raid_AOE_wanna_GS;
            _stop_GH_if_above = UPrHBTSetting.Instance.Raid_AOE_stop_GH_if_above;
            _min_renew_hp_on_non_tank = UPrHBTSetting.Instance.Raid_AOE_min_renew_hp_on_non_tank;
            _wanna_DP = UPrHBTSetting.Instance.Raid_AOE_wanna_DP;
            _min_DP_hp = UPrHBTSetting.Instance.Raid_AOE_min_DP_hp;
            _wanna_fade = UPrHBTSetting.Instance.Raid_AOE_wanna_fade;
            _min_fade_hp = UPrHBTSetting.Instance.Raid_AOE_min_fade_hp;
            _wanna_pom_ooc = UPrHBTSetting.Instance.Raid_AOE_wanna_pom_ooc;
            _wanna_pom_in_combat = UPrHBTSetting.Instance.Raid_AOE_wanna_pom_in_combat;
            _min_Shadowfiend_mana_if_safe = UPrHBTSetting.Instance.Raid_AOE_min_Shadowfiend_mana_if_safe;
            _wanna_Hymm_of_Hope_after_shadowfiend = UPrHBTSetting.Instance.Raid_AOE_wanna_Hymm_of_Hope_after_shadowfiend;
            _min_Shadowfiend_mana_NOT_safe = UPrHBTSetting.Instance.Raid_AOE_min_Shadowfiend_mana_NOT_safe;
            _interrupt_HoH_if_someone_below = UPrHBTSetting.Instance.Raid_AOE_interrupt_HoH_if_someone_below;
            _check_level_for_shadowfind = UPrHBTSetting.Instance.Raid_AOE_check_level_for_shadowfind;
            _decice_during_GCD = UPrHBTSetting.Instance.Raid_AOE_decice_during_GCD;
            _intellywait = UPrHBTSetting.Instance.Raid_AOE_intellywait;
            _AOE_check_self = UPrHBTSetting.Instance.Raid_AOE_AOE_check_self;
            _AOE_check_tank = UPrHBTSetting.Instance.Raid_AOE_AOE_check_tank;
            _AOE_check_focus = UPrHBTSetting.Instance.Raid_AOE_AOE_check_focus;
            _AOE_check_everyone = UPrHBTSetting.Instance.Raid_AOE_AOE_check_everyone;
            _use_PoM_on_CD = UPrHBTSetting.Instance.Raid_AOE_use_PoM_on_CD;
            _min_player_inside_HWSa = UPrHBTSetting.Instance.Raid_AOE_min_player_inside_HWSa;
            _HWSa_how_much_health = UPrHBTSetting.Instance.Raid_AOE_HWSa_how_much_health;
            _AOE_check_tar = UPrHBTSetting.Instance.Raid_AOE_AOE_check_tar;
            _min_player_inside_PoH=UPrHBTSetting.Instance.Raid_AOE_min_player_inside_PoH;
            _PoH_how_much_health = UPrHBTSetting.Instance.Raid_AOE_PoH_how_much_health;
            _wanna_PoH = UPrHBTSetting.Instance.Raid_AOE_wanna_PoH;
            _wanna_PoM = UPrHBTSetting.Instance.Raid_AOE_wanna_PoM;
            _wanna_CoH = UPrHBTSetting.Instance.Raid_AOE_wanna_CoH;
            _cleanse_only_self_and_tank = UPrHBTSetting.Instance.Raid_AOE_cleanse_only_self_and_tank;
            return true;
        }

        private bool Raid_Priest_Normal_Variable_inizializer()
        {
            _do_not_heal_above = UPrHBTSetting.Instance.Raid_do_not_heal_above;
            _max_healing_distance = UPrHBTSetting.Instance.Raid_max_healing_distance;
            _min_gift_hp = UPrHBTSetting.Instance.Raid_min_gift_hp;
            _min_mana_potion = UPrHBTSetting.Instance.Raid_min_mana_potion;
            _min_mana_rec_trinket = UPrHBTSetting.Instance.Raid_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPrHBTSetting.Instance.Raid_min_ohshitbutton_activator;
            _min_stoneform = UPrHBTSetting.Instance.Raid_min_stoneform;
            _min_torrent_mana_perc = UPrHBTSetting.Instance.Raid_min_torrent_mana_perc;
            _rest_if_mana_below = UPrHBTSetting.Instance.Raid_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPrHBTSetting.Instance.Raid_use_mana_rec_trinket_every;
            _wanna_buff = UPrHBTSetting.Instance.Raid_wanna_buff;
            _wanna_cleanse = UPrHBTSetting.Instance.Raid_wanna_cleanse;
            _wanna_everymanforhimself = UPrHBTSetting.Instance.Raid_wanna_everymanforhimself;
            _wanna_face = UPrHBTSetting.Instance.Raid_wanna_face;
            _wanna_gift = UPrHBTSetting.Instance.Raid_wanna_gift;
            _wanna_mana_potion = UPrHBTSetting.Instance.Raid_wanna_mana_potion;
            _wanna_mount = UPrHBTSetting.Instance.Raid_wanna_mount;
            _wanna_move_to_heal = UPrHBTSetting.Instance.Raid_wanna_move_to_heal;
            _wanna_stoneform = UPrHBTSetting.Instance.Raid_wanna_stoneform;
            _wanna_target = UPrHBTSetting.Instance.Raid_wanna_target;
            _wanna_torrent = UPrHBTSetting.Instance.Raid_wanna_torrent;
            _wanna_urgent_cleanse = UPrHBTSetting.Instance.Raid_wanna_urgent_cleanse;
            _wanna_lifeblood = UPrHBTSetting.Instance.Raid_wanna_lifeblood;
            _do_not_dismount_EVER = UPrHBTSetting.Instance.Raid_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPrHBTSetting.Instance.Raid_do_not_dismount_ooc;
            _get_tank_from_focus = UPrHBTSetting.Instance.Raid_get_tank_from_focus;
            _get_tank_from_lua = UPrHBTSetting.Instance.Raid_get_tank_from_lua;
            _healing_tank_priority = UPrHBTSetting.Instance.Raid_healing_tank_priority;
            _binding_heal_min_hp = UPrHBTSetting.Instance.Raid_binding_heal_min_hp;
            _HWS_min_hp = UPrHBTSetting.Instance.Raid_HWS_min_hp;
            _Serendipity_min_hp = UPrHBTSetting.Instance.Raid_Serendipity_min_hp;
            _Serendipity_GH_min_hp = UPrHBTSetting.Instance.Raid_Serendipity_GH_min_hp;
            _Heal_min_hp = UPrHBTSetting.Instance.Raid_Heal_min_hp;
            _GH_min_hp = UPrHBTSetting.Instance.Raid_GH_min_hp;
            _SoL_min_hp = UPrHBTSetting.Instance.Raid_SoL_min_hp;
            _min_player_inside_CoH = UPrHBTSetting.Instance.Raid_min_player_inside_CoH;
            _CoH_how_much_health = UPrHBTSetting.Instance.Raid_CoH_how_much_health;
            _DY_how_much_health = UPrHBTSetting.Instance.Raid_DY_how_much_health;
            _min_player_inside_DY = UPrHBTSetting.Instance.Raid_min_player_inside_DY;
            _wanna_GS = UPrHBTSetting.Instance.Raid_wanna_GS;
            _stop_GH_if_above = UPrHBTSetting.Instance.Raid_stop_GH_if_above;
            _min_renew_hp_on_non_tank = UPrHBTSetting.Instance.Raid_min_renew_hp_on_non_tank;
            _wanna_DP = UPrHBTSetting.Instance.Raid_wanna_DP;
            _min_DP_hp = UPrHBTSetting.Instance.Raid_min_DP_hp;
            _wanna_fade = UPrHBTSetting.Instance.Raid_wanna_fade;
            _min_fade_hp = UPrHBTSetting.Instance.Raid_min_fade_hp;
            _wanna_pom_ooc = UPrHBTSetting.Instance.Raid_wanna_pom_ooc;
            _wanna_pom_in_combat = UPrHBTSetting.Instance.Raid_wanna_pom_in_combat;
            _min_Shadowfiend_mana_if_safe = UPrHBTSetting.Instance.Raid_min_Shadowfiend_mana_if_safe;
            _wanna_Hymm_of_Hope_after_shadowfiend = UPrHBTSetting.Instance.Raid_wanna_Hymm_of_Hope_after_shadowfiend;
            _min_Shadowfiend_mana_NOT_safe = UPrHBTSetting.Instance.Raid_min_Shadowfiend_mana_NOT_safe;
            _interrupt_HoH_if_someone_below = UPrHBTSetting.Instance.Raid_interrupt_HoH_if_someone_below;
            _check_level_for_shadowfind = UPrHBTSetting.Instance.Raid_check_level_for_shadowfind;
            _decice_during_GCD = UPrHBTSetting.Instance.Raid_decice_during_GCD;
            _intellywait = UPrHBTSetting.Instance.Raid_intellywait;
            _AOE_check_self = UPrHBTSetting.Instance.Raid_AOE_check_self;
            _AOE_check_tank = UPrHBTSetting.Instance.Raid_AOE_check_tank;
            _AOE_check_focus = UPrHBTSetting.Instance.Raid_AOE_check_focus;
            _AOE_check_everyone = UPrHBTSetting.Instance.Raid_AOE_check_everyone;
            _AOE_check_tar = UPrHBTSetting.Instance.Raid_AOE_check_tar;
            _min_player_inside_PoH = UPrHBTSetting.Instance.Raid_min_player_inside_PoH;
            _PoH_how_much_health = UPrHBTSetting.Instance.Raid_PoH_how_much_health;
            _wanna_PoH = UPrHBTSetting.Instance.Raid_wanna_PoH;
            _wanna_PoM = UPrHBTSetting.Instance.Raid_wanna_PoM;
            _wanna_CoH = UPrHBTSetting.Instance.Raid_wanna_CoH;
            _cleanse_only_self_and_tank = UPrHBTSetting.Instance.Raid_cleanse_only_self_and_tank;
            return true;
        }
        private bool WorldPVP_Priest_Variable_inizializer()
        {
            Priest_PVP_dispell_inizializer();
            _general_raid_healer = UPrHBTSetting.Instance.WorldPVP_general_raid_healer;
            Decice_if_special_or_normal_raid();
            _do_not_heal_above = UPrHBTSetting.Instance.WorldPVP_do_not_heal_above;
            _max_healing_distance = UPrHBTSetting.Instance.WorldPVP_max_healing_distance;
            _min_gift_hp = UPrHBTSetting.Instance.WorldPVP_min_gift_hp;
            _min_mana_potion = UPrHBTSetting.Instance.WorldPVP_min_mana_potion;
            _min_mana_rec_trinket = UPrHBTSetting.Instance.WorldPVP_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPrHBTSetting.Instance.WorldPVP_min_ohshitbutton_activator;
            _min_stoneform = UPrHBTSetting.Instance.WorldPVP_min_stoneform;
            _min_torrent_mana_perc = UPrHBTSetting.Instance.WorldPVP_min_torrent_mana_perc;
            _rest_if_mana_below = UPrHBTSetting.Instance.WorldPVP_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPrHBTSetting.Instance.WorldPVP_use_mana_rec_trinket_every;
            _wanna_buff = UPrHBTSetting.Instance.WorldPVP_wanna_buff;
            _wanna_cleanse = UPrHBTSetting.Instance.WorldPVP_wanna_cleanse;
            _wanna_everymanforhimself = UPrHBTSetting.Instance.WorldPVP_wanna_everymanforhimself;
            _wanna_face = UPrHBTSetting.Instance.WorldPVP_wanna_face;
            _wanna_gift = UPrHBTSetting.Instance.WorldPVP_wanna_gift;
            _wanna_mana_potion = UPrHBTSetting.Instance.WorldPVP_wanna_mana_potion;
            _wanna_mount = UPrHBTSetting.Instance.WorldPVP_wanna_mount;
            _wanna_move_to_heal = UPrHBTSetting.Instance.WorldPVP_wanna_move_to_heal;
            _wanna_stoneform = UPrHBTSetting.Instance.WorldPVP_wanna_stoneform;
            _wanna_target = UPrHBTSetting.Instance.WorldPVP_wanna_target;
            _wanna_torrent = UPrHBTSetting.Instance.WorldPVP_wanna_torrent;
            _wanna_urgent_cleanse = UPrHBTSetting.Instance.WorldPVP_wanna_urgent_cleanse;
            _wanna_lifeblood = UPrHBTSetting.Instance.WorldPVP_wanna_lifeblood;
            _do_not_dismount_EVER = UPrHBTSetting.Instance.WorldPVP_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPrHBTSetting.Instance.WorldPVP_do_not_dismount_ooc;
            _get_tank_from_focus = UPrHBTSetting.Instance.WorldPVP_get_tank_from_focus;
            _get_tank_from_lua = UPrHBTSetting.Instance.WorldPVP_get_tank_from_lua;
            _healing_tank_priority = UPrHBTSetting.Instance.WorldPVP_healing_tank_priority;
            _binding_heal_min_hp = UPrHBTSetting.Instance.WorldPVP_binding_heal_min_hp;
            _HWS_min_hp = UPrHBTSetting.Instance.WorldPVP_HWS_min_hp;
            _Serendipity_min_hp = UPrHBTSetting.Instance.WorldPVP_Serendipity_min_hp;
            _Serendipity_GH_min_hp = UPrHBTSetting.Instance.WorldPVP_Serendipity_GH_min_hp;
            _Heal_min_hp = UPrHBTSetting.Instance.WorldPVP_Heal_min_hp;
            _GH_min_hp = UPrHBTSetting.Instance.WorldPVP_GH_min_hp;
            _SoL_min_hp = UPrHBTSetting.Instance.WorldPVP_SoL_min_hp;
            _min_player_inside_CoH = UPrHBTSetting.Instance.WorldPVP_min_player_inside_CoH;
            _CoH_how_much_health = UPrHBTSetting.Instance.WorldPVP_CoH_how_much_health;
            _DY_how_much_health = UPrHBTSetting.Instance.WorldPVP_DY_how_much_health;
            _min_player_inside_DY = UPrHBTSetting.Instance.WorldPVP_min_player_inside_DY;
            _wanna_GS = UPrHBTSetting.Instance.WorldPVP_wanna_GS;
            _stop_GH_if_above = UPrHBTSetting.Instance.WorldPVP_stop_GH_if_above;
            _min_renew_hp_on_non_tank = UPrHBTSetting.Instance.WorldPVP_min_renew_hp_on_non_tank;
            _wanna_DP = UPrHBTSetting.Instance.WorldPVP_wanna_DP;
            _min_DP_hp = UPrHBTSetting.Instance.WorldPVP_min_DP_hp;
            _wanna_fade = UPrHBTSetting.Instance.WorldPVP_wanna_fade;
            _min_fade_hp = UPrHBTSetting.Instance.WorldPVP_min_fade_hp;
            _wanna_pom_ooc = UPrHBTSetting.Instance.WorldPVP_wanna_pom_ooc;
            _wanna_pom_in_combat = UPrHBTSetting.Instance.WorldPVP_wanna_pom_in_combat;
            _min_Shadowfiend_mana_if_safe = UPrHBTSetting.Instance.WorldPVP_min_Shadowfiend_mana_if_safe;
            _wanna_Hymm_of_Hope_after_shadowfiend = UPrHBTSetting.Instance.WorldPVP_wanna_Hymm_of_Hope_after_shadowfiend;
            _min_Shadowfiend_mana_NOT_safe = UPrHBTSetting.Instance.WorldPVP_min_Shadowfiend_mana_NOT_safe;
            _interrupt_HoH_if_someone_below = UPrHBTSetting.Instance.WorldPVP_interrupt_HoH_if_someone_below;
            _check_level_for_shadowfind = UPrHBTSetting.Instance.WorldPVP_check_level_for_shadowfind;
            _decice_during_GCD = UPrHBTSetting.Instance.WorldPVP_decice_during_GCD;
            _intellywait = UPrHBTSetting.Instance.WorldPVP_intellywait;
            _AOE_check_self = UPrHBTSetting.Instance.WorldPVP_AOE_check_self;
            _AOE_check_tank = UPrHBTSetting.Instance.WorldPVP_AOE_check_tank;
            _AOE_check_focus = UPrHBTSetting.Instance.WorldPVP_AOE_check_focus;
            _AOE_check_everyone = UPrHBTSetting.Instance.WorldPVP_AOE_check_everyone;
            _use_PoM_on_CD = UPrHBTSetting.Instance.WorldPVP_use_PoM_on_CD;
            _min_player_inside_HWSa = UPrHBTSetting.Instance.WorldPVP_min_player_inside_HWSa;
            _HWSa_how_much_health = UPrHBTSetting.Instance.WorldPVP_HWSa_how_much_health;
            _AOE_check_tar = UPrHBTSetting.Instance.WorldPVP_AOE_check_tar;
            _min_player_inside_PoH = UPrHBTSetting.Instance.WorldPVP_min_player_inside_PoH;
            _PoH_how_much_health = UPrHBTSetting.Instance.WorldPVP_PoH_how_much_health;
            _wanna_PoH = UPrHBTSetting.Instance.WorldPVP_wanna_PoH;
            _wanna_PoM = UPrHBTSetting.Instance.WorldPVP_wanna_PoM;
            _wanna_CoH = UPrHBTSetting.Instance.WorldPVP_wanna_CoH;
            _cleanse_only_self_and_tank = UPrHBTSetting.Instance.WorldPVP_cleanse_only_self_and_tank;
            return true;
        }

        private bool RAF_Priest_Variable_inizializer()
        {
            Priest_PVE_dispell_inizializer();
            _do_not_heal_above = UPrHBTSetting.Instance.RAF_do_not_heal_above;
            _max_healing_distance = UPrHBTSetting.Instance.RAF_max_healing_distance;
            _min_gift_hp = UPrHBTSetting.Instance.RAF_min_gift_hp;
            _min_mana_potion = UPrHBTSetting.Instance.RAF_min_mana_potion;
            _min_mana_rec_trinket = UPrHBTSetting.Instance.RAF_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPrHBTSetting.Instance.RAF_min_ohshitbutton_activator;
            _min_stoneform = UPrHBTSetting.Instance.RAF_min_stoneform;
            _min_torrent_mana_perc = UPrHBTSetting.Instance.RAF_min_torrent_mana_perc;
            _rest_if_mana_below = UPrHBTSetting.Instance.RAF_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPrHBTSetting.Instance.RAF_use_mana_rec_trinket_every;
            _wanna_buff = UPrHBTSetting.Instance.RAF_wanna_buff;
            _wanna_cleanse = UPrHBTSetting.Instance.RAF_wanna_cleanse;
            _wanna_everymanforhimself = UPrHBTSetting.Instance.RAF_wanna_everymanforhimself;
            _wanna_face = UPrHBTSetting.Instance.RAF_wanna_face;
            _wanna_gift = UPrHBTSetting.Instance.RAF_wanna_gift;
            _wanna_mana_potion = UPrHBTSetting.Instance.RAF_wanna_mana_potion;
            _wanna_mount = UPrHBTSetting.Instance.RAF_wanna_mount;
            _wanna_move_to_heal = UPrHBTSetting.Instance.RAF_wanna_move_to_heal;
            _wanna_stoneform = UPrHBTSetting.Instance.RAF_wanna_stoneform;
            _wanna_target = UPrHBTSetting.Instance.RAF_wanna_target;
            _wanna_torrent = UPrHBTSetting.Instance.RAF_wanna_torrent;
            _wanna_urgent_cleanse = UPrHBTSetting.Instance.RAF_wanna_urgent_cleanse;
            _wanna_lifeblood = UPrHBTSetting.Instance.RAF_wanna_lifeblood;
            _do_not_dismount_EVER = UPrHBTSetting.Instance.RAF_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPrHBTSetting.Instance.RAF_do_not_dismount_ooc;
            _get_tank_from_focus = UPrHBTSetting.Instance.RAF_get_tank_from_focus;
            _get_tank_from_lua = UPrHBTSetting.Instance.RAF_get_tank_from_lua;
            _healing_tank_priority = UPrHBTSetting.Instance.RAF_healing_tank_priority;
            _binding_heal_min_hp = UPrHBTSetting.Instance.RAF_binding_heal_min_hp;
            _HWS_min_hp = UPrHBTSetting.Instance.RAF_HWS_min_hp;
            _Serendipity_min_hp = UPrHBTSetting.Instance.RAF_Serendipity_min_hp;
            _Serendipity_GH_min_hp = UPrHBTSetting.Instance.RAF_Serendipity_GH_min_hp;
            _Heal_min_hp = UPrHBTSetting.Instance.RAF_Heal_min_hp;
            _GH_min_hp = UPrHBTSetting.Instance.RAF_GH_min_hp;
            _SoL_min_hp = UPrHBTSetting.Instance.RAF_SoL_min_hp;
            _min_player_inside_CoH = UPrHBTSetting.Instance.RAF_min_player_inside_CoH;
            _CoH_how_much_health = UPrHBTSetting.Instance.RAF_CoH_how_much_health;
            _DY_how_much_health = UPrHBTSetting.Instance.RAF_DY_how_much_health;
            _min_player_inside_DY = UPrHBTSetting.Instance.RAF_min_player_inside_DY;
            _wanna_GS = UPrHBTSetting.Instance.RAF_wanna_GS;
            _stop_GH_if_above = UPrHBTSetting.Instance.RAF_stop_GH_if_above;
            _min_renew_hp_on_non_tank = UPrHBTSetting.Instance.RAF_min_renew_hp_on_non_tank;
            _wanna_DP = UPrHBTSetting.Instance.RAF_wanna_DP;
            _min_DP_hp = UPrHBTSetting.Instance.RAF_min_DP_hp;
            _wanna_fade = UPrHBTSetting.Instance.RAF_wanna_fade;
            _min_fade_hp = UPrHBTSetting.Instance.RAF_min_fade_hp;
            _wanna_pom_ooc = UPrHBTSetting.Instance.RAF_wanna_pom_ooc;
            _wanna_pom_in_combat = UPrHBTSetting.Instance.RAF_wanna_pom_in_combat;
            _min_Shadowfiend_mana_if_safe = UPrHBTSetting.Instance.RAF_min_Shadowfiend_mana_if_safe;
            _wanna_Hymm_of_Hope_after_shadowfiend = UPrHBTSetting.Instance.RAF_wanna_Hymm_of_Hope_after_shadowfiend;
            _min_Shadowfiend_mana_NOT_safe = UPrHBTSetting.Instance.RAF_min_Shadowfiend_mana_NOT_safe;
            _interrupt_HoH_if_someone_below = UPrHBTSetting.Instance.RAF_interrupt_HoH_if_someone_below;
            _check_level_for_shadowfind = UPrHBTSetting.Instance.RAF_check_level_for_shadowfind;
            _decice_during_GCD = UPrHBTSetting.Instance.RAF_decice_during_GCD;
            _intellywait = UPrHBTSetting.Instance.RAF_intellywait;
            _AOE_check_self = UPrHBTSetting.Instance.RAF_AOE_check_self;
            _AOE_check_tank = UPrHBTSetting.Instance.RAF_AOE_check_tank;
            _AOE_check_focus = UPrHBTSetting.Instance.RAF_AOE_check_focus;
            _AOE_check_everyone = UPrHBTSetting.Instance.RAF_AOE_check_everyone;
            _AOE_check_tar = UPrHBTSetting.Instance.RAF_AOE_check_tar;
            _min_player_inside_PoH = UPrHBTSetting.Instance.RAF_min_player_inside_PoH;
            _PoH_how_much_health = UPrHBTSetting.Instance.RAF_PoH_how_much_health;
            _wanna_PoH = UPrHBTSetting.Instance.RAF_wanna_PoH;
            _wanna_PoM = UPrHBTSetting.Instance.RAF_wanna_PoM;
            _wanna_CoH = UPrHBTSetting.Instance.RAF_wanna_CoH;
            _cleanse_only_self_and_tank = UPrHBTSetting.Instance.RAF_cleanse_only_self_and_tank;
            return true;
        }

        private bool PVE_Priest_Variable_inizializer()
        {
            Priest_PVE_dispell_inizializer();
            _do_not_heal_above = UPrHBTSetting.Instance.PVE_do_not_heal_above;
            _max_healing_distance = UPrHBTSetting.Instance.PVE_max_healing_distance;
            _min_gift_hp = UPrHBTSetting.Instance.PVE_min_gift_hp;
            _min_mana_potion = UPrHBTSetting.Instance.PVE_min_mana_potion;
            _min_mana_rec_trinket = UPrHBTSetting.Instance.PVE_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPrHBTSetting.Instance.PVE_min_ohshitbutton_activator;
            _min_stoneform = UPrHBTSetting.Instance.PVE_min_stoneform;
            _min_torrent_mana_perc = UPrHBTSetting.Instance.PVE_min_torrent_mana_perc;
            _rest_if_mana_below = UPrHBTSetting.Instance.PVE_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPrHBTSetting.Instance.PVE_use_mana_rec_trinket_every;
            _wanna_buff = UPrHBTSetting.Instance.PVE_wanna_buff;
            _wanna_cleanse = UPrHBTSetting.Instance.PVE_wanna_cleanse;
            _wanna_everymanforhimself = UPrHBTSetting.Instance.PVE_wanna_everymanforhimself;
            _wanna_face = UPrHBTSetting.Instance.PVE_wanna_face;
            _wanna_gift = UPrHBTSetting.Instance.PVE_wanna_gift;
            _wanna_mana_potion = UPrHBTSetting.Instance.PVE_wanna_mana_potion;
            _wanna_mount = UPrHBTSetting.Instance.PVE_wanna_mount;
            _wanna_move_to_heal = UPrHBTSetting.Instance.PVE_wanna_move_to_heal;
            _wanna_stoneform = UPrHBTSetting.Instance.PVE_wanna_stoneform;
            _wanna_target = UPrHBTSetting.Instance.PVE_wanna_target;
            _wanna_torrent = UPrHBTSetting.Instance.PVE_wanna_torrent;
            _wanna_urgent_cleanse = UPrHBTSetting.Instance.PVE_wanna_urgent_cleanse;
            _wanna_lifeblood = UPrHBTSetting.Instance.PVE_wanna_lifeblood;
            _do_not_dismount_EVER = UPrHBTSetting.Instance.PVE_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPrHBTSetting.Instance.PVE_do_not_dismount_ooc;
            _get_tank_from_focus = UPrHBTSetting.Instance.PVE_get_tank_from_focus;
            _get_tank_from_lua = UPrHBTSetting.Instance.PVE_get_tank_from_lua;
            _healing_tank_priority = UPrHBTSetting.Instance.PVE_healing_tank_priority;
            _binding_heal_min_hp = UPrHBTSetting.Instance.PVE_binding_heal_min_hp;
            _HWS_min_hp = UPrHBTSetting.Instance.PVE_HWS_min_hp;
            _Serendipity_min_hp = UPrHBTSetting.Instance.PVE_Serendipity_min_hp;
            _Serendipity_GH_min_hp = UPrHBTSetting.Instance.PVE_Serendipity_GH_min_hp;
            _Heal_min_hp = UPrHBTSetting.Instance.PVE_Heal_min_hp;
            _GH_min_hp = UPrHBTSetting.Instance.PVE_GH_min_hp;
            _SoL_min_hp = UPrHBTSetting.Instance.PVE_SoL_min_hp;
            _min_player_inside_CoH = UPrHBTSetting.Instance.PVE_min_player_inside_CoH;
            _CoH_how_much_health = UPrHBTSetting.Instance.PVE_CoH_how_much_health;
            _DY_how_much_health = UPrHBTSetting.Instance.PVE_DY_how_much_health;
            _min_player_inside_DY = UPrHBTSetting.Instance.PVE_min_player_inside_DY;
            _wanna_GS = UPrHBTSetting.Instance.PVE_wanna_GS;
            _stop_GH_if_above = UPrHBTSetting.Instance.PVE_stop_GH_if_above;
            _min_renew_hp_on_non_tank = UPrHBTSetting.Instance.PVE_min_renew_hp_on_non_tank;
            _wanna_DP = UPrHBTSetting.Instance.PVE_wanna_DP;
            _min_DP_hp = UPrHBTSetting.Instance.PVE_min_DP_hp;
            _wanna_fade = UPrHBTSetting.Instance.PVE_wanna_fade;
            _min_fade_hp = UPrHBTSetting.Instance.PVE_min_fade_hp;
            _wanna_pom_ooc = UPrHBTSetting.Instance.PVE_wanna_pom_ooc;
            _wanna_pom_in_combat = UPrHBTSetting.Instance.PVE_wanna_pom_in_combat;
            _min_Shadowfiend_mana_if_safe = UPrHBTSetting.Instance.PVE_min_Shadowfiend_mana_if_safe;
            _wanna_Hymm_of_Hope_after_shadowfiend = UPrHBTSetting.Instance.PVE_wanna_Hymm_of_Hope_after_shadowfiend;
            _min_Shadowfiend_mana_NOT_safe = UPrHBTSetting.Instance.PVE_min_Shadowfiend_mana_NOT_safe;
            _interrupt_HoH_if_someone_below = UPrHBTSetting.Instance.PVE_interrupt_HoH_if_someone_below;
            _check_level_for_shadowfind = UPrHBTSetting.Instance.PVE_check_level_for_shadowfind;
            _decice_during_GCD = UPrHBTSetting.Instance.PVE_decice_during_GCD;
            _intellywait = UPrHBTSetting.Instance.PVE_intellywait;
            _AOE_check_self = UPrHBTSetting.Instance.PVE_AOE_check_self;
            _AOE_check_tank = UPrHBTSetting.Instance.PVE_AOE_check_tank;
            _AOE_check_focus = UPrHBTSetting.Instance.PVE_AOE_check_focus;
            _AOE_check_everyone = UPrHBTSetting.Instance.PVE_AOE_check_everyone;
            _AOE_check_tar = UPrHBTSetting.Instance.PVE_AOE_check_tar;
            _min_player_inside_PoH = UPrHBTSetting.Instance.PVE_min_player_inside_PoH;
            _PoH_how_much_health = UPrHBTSetting.Instance.PVE_PoH_how_much_health;
            _wanna_PoH = UPrHBTSetting.Instance.PVE_wanna_PoH;
            _wanna_PoM = UPrHBTSetting.Instance.PVE_wanna_PoM;
            _wanna_CoH = UPrHBTSetting.Instance.PVE_wanna_CoH;
            _cleanse_only_self_and_tank = UPrHBTSetting.Instance.PVE_cleanse_only_self_and_tank;
            return true;
        }

        private bool Solo_Priest_Variable_inizializer()
        {
            Priest_PVE_dispell_inizializer();
            _do_not_heal_above = UPrHBTSetting.Instance.Solo_do_not_heal_above;
            _max_healing_distance = UPrHBTSetting.Instance.Solo_max_healing_distance;
            _min_gift_hp = UPrHBTSetting.Instance.Solo_min_gift_hp;
            _min_mana_potion = UPrHBTSetting.Instance.Solo_min_mana_potion;
            _min_mana_rec_trinket = UPrHBTSetting.Instance.Solo_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPrHBTSetting.Instance.Solo_min_ohshitbutton_activator;
            _min_stoneform = UPrHBTSetting.Instance.Solo_min_stoneform;
            _min_torrent_mana_perc = UPrHBTSetting.Instance.Solo_min_torrent_mana_perc;
            _rest_if_mana_below = UPrHBTSetting.Instance.Solo_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPrHBTSetting.Instance.Solo_use_mana_rec_trinket_every;
            _wanna_buff = UPrHBTSetting.Instance.Solo_wanna_buff;
            _wanna_cleanse = UPrHBTSetting.Instance.Solo_wanna_cleanse;
            _wanna_everymanforhimself = UPrHBTSetting.Instance.Solo_wanna_everymanforhimself;
            _wanna_face = UPrHBTSetting.Instance.Solo_wanna_face;
            _wanna_gift = UPrHBTSetting.Instance.Solo_wanna_gift;
            _wanna_mana_potion = UPrHBTSetting.Instance.Solo_wanna_mana_potion;
            _wanna_mount = UPrHBTSetting.Instance.Solo_wanna_mount;
            _wanna_move_to_heal = UPrHBTSetting.Instance.Solo_wanna_move_to_heal;
            _wanna_stoneform = UPrHBTSetting.Instance.Solo_wanna_stoneform;
            _wanna_target = UPrHBTSetting.Instance.Solo_wanna_target;
            _wanna_torrent = UPrHBTSetting.Instance.Solo_wanna_torrent;
            _wanna_urgent_cleanse = UPrHBTSetting.Instance.Solo_wanna_urgent_cleanse;
            _wanna_lifeblood = UPrHBTSetting.Instance.Solo_wanna_lifeblood;
            _do_not_dismount_EVER = UPrHBTSetting.Instance.Solo_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPrHBTSetting.Instance.Solo_do_not_dismount_ooc;
            _get_tank_from_focus = UPrHBTSetting.Instance.Solo_get_tank_from_focus;
            _get_tank_from_lua = UPrHBTSetting.Instance.Solo_get_tank_from_lua;
            _healing_tank_priority = UPrHBTSetting.Instance.Solo_healing_tank_priority;
            _binding_heal_min_hp = UPrHBTSetting.Instance.Solo_binding_heal_min_hp;
            _HWS_min_hp = UPrHBTSetting.Instance.Solo_HWS_min_hp;
            _Serendipity_min_hp = UPrHBTSetting.Instance.Solo_Serendipity_min_hp;
            _Serendipity_GH_min_hp = UPrHBTSetting.Instance.Solo_Serendipity_GH_min_hp;
            _Heal_min_hp = UPrHBTSetting.Instance.Solo_Heal_min_hp;
            _GH_min_hp = UPrHBTSetting.Instance.Solo_GH_min_hp;
            _SoL_min_hp = UPrHBTSetting.Instance.Solo_SoL_min_hp;
            _min_player_inside_CoH = UPrHBTSetting.Instance.Solo_min_player_inside_CoH;
            _CoH_how_much_health = UPrHBTSetting.Instance.Solo_CoH_how_much_health;
            _DY_how_much_health = UPrHBTSetting.Instance.Solo_DY_how_much_health;
            _min_player_inside_DY = UPrHBTSetting.Instance.Solo_min_player_inside_DY;
            _wanna_GS = UPrHBTSetting.Instance.Solo_wanna_GS;
            _stop_GH_if_above = UPrHBTSetting.Instance.Solo_stop_GH_if_above;
            _min_renew_hp_on_non_tank = UPrHBTSetting.Instance.Solo_min_renew_hp_on_non_tank;
            _wanna_DP = UPrHBTSetting.Instance.Solo_wanna_DP;
            _min_DP_hp = UPrHBTSetting.Instance.Solo_min_DP_hp;
            _wanna_fade = UPrHBTSetting.Instance.Solo_wanna_fade;
            _min_fade_hp = UPrHBTSetting.Instance.Solo_min_fade_hp;
            _wanna_pom_ooc = UPrHBTSetting.Instance.Solo_wanna_pom_ooc;
            _wanna_pom_in_combat = UPrHBTSetting.Instance.Solo_wanna_pom_in_combat;
            _min_Shadowfiend_mana_if_safe = UPrHBTSetting.Instance.Solo_min_Shadowfiend_mana_if_safe;
            _wanna_Hymm_of_Hope_after_shadowfiend = UPrHBTSetting.Instance.Solo_wanna_Hymm_of_Hope_after_shadowfiend;
            _min_Shadowfiend_mana_NOT_safe = UPrHBTSetting.Instance.Solo_min_Shadowfiend_mana_NOT_safe;
            _interrupt_HoH_if_someone_below = UPrHBTSetting.Instance.Solo_interrupt_HoH_if_someone_below;
            _check_level_for_shadowfind = UPrHBTSetting.Instance.Solo_check_level_for_shadowfind;
            _decice_during_GCD = UPrHBTSetting.Instance.Solo_decice_during_GCD;
            _intellywait = UPrHBTSetting.Instance.Solo_intellywait;
            _AOE_check_self = UPrHBTSetting.Instance.Solo_AOE_check_self;
            _AOE_check_tank = UPrHBTSetting.Instance.Solo_AOE_check_tank;
            _AOE_check_focus = UPrHBTSetting.Instance.Solo_AOE_check_focus;
            _AOE_check_everyone = UPrHBTSetting.Instance.Solo_AOE_check_everyone;
            _AOE_check_tar = UPrHBTSetting.Instance.Solo_AOE_check_tar;
            _min_player_inside_PoH = UPrHBTSetting.Instance.Solo_min_player_inside_PoH;
            _PoH_how_much_health = UPrHBTSetting.Instance.Solo_PoH_how_much_health;
            _wanna_PoH = UPrHBTSetting.Instance.Solo_wanna_PoH;
            _wanna_PoM = UPrHBTSetting.Instance.Solo_wanna_PoM;
            _wanna_CoH = UPrHBTSetting.Instance.Solo_wanna_CoH;
            _wanna_SWD = UPrHBTSetting.Instance.Solo_wanna_SWD;
            _enable_pull = UPrHBTSetting.Instance.Solo_enable_pull;
            return true;
        }

        private bool Battleground_Priest_Variable_inizializer()
        {
            Priest_PVP_dispell_inizializer();
            _general_raid_healer = UPrHBTSetting.Instance.Battleground_general_raid_healer;
            Decice_if_special_or_normal_raid();
            _do_not_heal_above = UPrHBTSetting.Instance.Battleground_do_not_heal_above;
            _max_healing_distance = UPrHBTSetting.Instance.Battleground_max_healing_distance;
            _min_gift_hp = UPrHBTSetting.Instance.Battleground_min_gift_hp;
            _min_mana_potion = UPrHBTSetting.Instance.Battleground_min_mana_potion;
            _min_mana_rec_trinket = UPrHBTSetting.Instance.Battleground_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPrHBTSetting.Instance.Battleground_min_ohshitbutton_activator;
            _min_stoneform = UPrHBTSetting.Instance.Battleground_min_stoneform;
            _min_torrent_mana_perc = UPrHBTSetting.Instance.Battleground_min_torrent_mana_perc;
            _rest_if_mana_below = UPrHBTSetting.Instance.Battleground_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPrHBTSetting.Instance.Battleground_use_mana_rec_trinket_every;
            _wanna_buff = UPrHBTSetting.Instance.Battleground_wanna_buff;
            _wanna_cleanse = UPrHBTSetting.Instance.Battleground_wanna_cleanse;
            _wanna_everymanforhimself = UPrHBTSetting.Instance.Battleground_wanna_everymanforhimself;
            _wanna_face = UPrHBTSetting.Instance.Battleground_wanna_face;
            _wanna_gift = UPrHBTSetting.Instance.Battleground_wanna_gift;
            _wanna_mana_potion = UPrHBTSetting.Instance.Battleground_wanna_mana_potion;
            _wanna_mount = UPrHBTSetting.Instance.Battleground_wanna_mount;
            _wanna_move_to_heal = UPrHBTSetting.Instance.Battleground_wanna_move_to_heal;
            _wanna_stoneform = UPrHBTSetting.Instance.Battleground_wanna_stoneform;
            _wanna_target = UPrHBTSetting.Instance.Battleground_wanna_target;
            _wanna_torrent = UPrHBTSetting.Instance.Battleground_wanna_torrent;
            _wanna_urgent_cleanse = UPrHBTSetting.Instance.Battleground_wanna_urgent_cleanse;
            _wanna_lifeblood = UPrHBTSetting.Instance.Battleground_wanna_lifeblood;
            _do_not_dismount_EVER = UPrHBTSetting.Instance.Battleground_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPrHBTSetting.Instance.Battleground_do_not_dismount_ooc;
            _get_tank_from_focus = UPrHBTSetting.Instance.Battleground_get_tank_from_focus;
            _get_tank_from_lua = UPrHBTSetting.Instance.Battleground_get_tank_from_lua;
            _healing_tank_priority = UPrHBTSetting.Instance.Battleground_healing_tank_priority;
            _binding_heal_min_hp = UPrHBTSetting.Instance.Battleground_binding_heal_min_hp;
            _HWS_min_hp = UPrHBTSetting.Instance.Battleground_HWS_min_hp;
            _Serendipity_min_hp = UPrHBTSetting.Instance.Battleground_Serendipity_min_hp;
            _Serendipity_GH_min_hp = UPrHBTSetting.Instance.Battleground_Serendipity_GH_min_hp;
            _Heal_min_hp = UPrHBTSetting.Instance.Battleground_Heal_min_hp;
            _GH_min_hp = UPrHBTSetting.Instance.Battleground_GH_min_hp;
            _SoL_min_hp = UPrHBTSetting.Instance.Battleground_SoL_min_hp;
            _min_player_inside_CoH = UPrHBTSetting.Instance.Battleground_min_player_inside_CoH;
            _CoH_how_much_health = UPrHBTSetting.Instance.Battleground_CoH_how_much_health;
            _DY_how_much_health = UPrHBTSetting.Instance.Battleground_DY_how_much_health;
            _min_player_inside_DY = UPrHBTSetting.Instance.Battleground_min_player_inside_DY;
            _wanna_GS = UPrHBTSetting.Instance.Battleground_wanna_GS;
            _stop_GH_if_above = UPrHBTSetting.Instance.Battleground_stop_GH_if_above;
            _min_renew_hp_on_non_tank = UPrHBTSetting.Instance.Battleground_min_renew_hp_on_non_tank;
            _wanna_DP = UPrHBTSetting.Instance.Battleground_wanna_DP;
            _min_DP_hp = UPrHBTSetting.Instance.Battleground_min_DP_hp;
            _wanna_fade = UPrHBTSetting.Instance.Battleground_wanna_fade;
            _min_fade_hp = UPrHBTSetting.Instance.Battleground_min_fade_hp;
            _wanna_pom_ooc = UPrHBTSetting.Instance.Battleground_wanna_pom_ooc;
            _wanna_pom_in_combat = UPrHBTSetting.Instance.Battleground_wanna_pom_in_combat;
            _min_Shadowfiend_mana_if_safe = UPrHBTSetting.Instance.Battleground_min_Shadowfiend_mana_if_safe;
            _wanna_Hymm_of_Hope_after_shadowfiend = UPrHBTSetting.Instance.Battleground_wanna_Hymm_of_Hope_after_shadowfiend;
            _min_Shadowfiend_mana_NOT_safe = UPrHBTSetting.Instance.Battleground_min_Shadowfiend_mana_NOT_safe;
            _interrupt_HoH_if_someone_below = UPrHBTSetting.Instance.Battleground_interrupt_HoH_if_someone_below;
            _check_level_for_shadowfind = UPrHBTSetting.Instance.Battleground_check_level_for_shadowfind;
            _decice_during_GCD = UPrHBTSetting.Instance.Battleground_decice_during_GCD;
            _intellywait = UPrHBTSetting.Instance.Battleground_intellywait;
            _AOE_check_self = UPrHBTSetting.Instance.Battleground_AOE_check_self;
            _AOE_check_tank = UPrHBTSetting.Instance.Battleground_AOE_check_tank;
            _AOE_check_focus = UPrHBTSetting.Instance.Battleground_AOE_check_focus;
            _AOE_check_everyone = UPrHBTSetting.Instance.Battleground_AOE_check_everyone;
            _use_PoM_on_CD = UPrHBTSetting.Instance.Battleground_use_PoM_on_CD;
            _min_player_inside_HWSa = UPrHBTSetting.Instance.Battleground_min_player_inside_HWSa;
            _HWSa_how_much_health = UPrHBTSetting.Instance.Battleground_HWSa_how_much_health;
            _AOE_check_tar = UPrHBTSetting.Instance.Battleground_AOE_check_tar;
            _min_player_inside_PoH = UPrHBTSetting.Instance.Battleground_min_player_inside_PoH;
            _PoH_how_much_health = UPrHBTSetting.Instance.Battleground_PoH_how_much_health;
            _wanna_PoH = UPrHBTSetting.Instance.Battleground_wanna_PoH;
            _wanna_PoM = UPrHBTSetting.Instance.Battleground_wanna_PoM;
            _wanna_CoH = UPrHBTSetting.Instance.Battleground_wanna_CoH;
            _cleanse_only_self_and_tank = UPrHBTSetting.Instance.Battleground_cleanse_only_self_and_tank;
            return true;
        }

        private bool ARENA_Priest_Variable_inizializer()
        {
            Priest_PVP_dispell_inizializer();
            _general_raid_healer = UPrHBTSetting.Instance.ARENA_general_raid_healer;
            Decice_if_special_or_normal_raid();
            _do_not_heal_above = UPrHBTSetting.Instance.ARENA_do_not_heal_above;
            _max_healing_distance = UPrHBTSetting.Instance.ARENA_max_healing_distance;
            _min_gift_hp = UPrHBTSetting.Instance.ARENA_min_gift_hp;
            _min_mana_potion = UPrHBTSetting.Instance.ARENA_min_mana_potion;
            _min_mana_rec_trinket = UPrHBTSetting.Instance.ARENA_min_mana_rec_trinket;
            _min_ohshitbutton_activator = UPrHBTSetting.Instance.ARENA_min_ohshitbutton_activator;
            _min_stoneform = UPrHBTSetting.Instance.ARENA_min_stoneform;
            _min_torrent_mana_perc = UPrHBTSetting.Instance.ARENA_min_torrent_mana_perc;
            _rest_if_mana_below = UPrHBTSetting.Instance.ARENA_rest_if_mana_below;
            _use_mana_rec_trinket_every = UPrHBTSetting.Instance.ARENA_use_mana_rec_trinket_every;
            _wanna_buff = UPrHBTSetting.Instance.ARENA_wanna_buff;
            _wanna_cleanse = UPrHBTSetting.Instance.ARENA_wanna_cleanse;
            _wanna_everymanforhimself = UPrHBTSetting.Instance.ARENA_wanna_everymanforhimself;
            _wanna_face = UPrHBTSetting.Instance.ARENA_wanna_face;
            _wanna_gift = UPrHBTSetting.Instance.ARENA_wanna_gift;
            _wanna_mana_potion = UPrHBTSetting.Instance.ARENA_wanna_mana_potion;
            _wanna_mount = UPrHBTSetting.Instance.ARENA_wanna_mount;
            _wanna_move_to_heal = UPrHBTSetting.Instance.ARENA_wanna_move_to_heal;
            _wanna_stoneform = UPrHBTSetting.Instance.ARENA_wanna_stoneform;
            _wanna_target = UPrHBTSetting.Instance.ARENA_wanna_target;
            _wanna_torrent = UPrHBTSetting.Instance.ARENA_wanna_torrent;
            _wanna_urgent_cleanse = UPrHBTSetting.Instance.ARENA_wanna_urgent_cleanse;
            _wanna_lifeblood = UPrHBTSetting.Instance.ARENA_wanna_lifeblood;
            _do_not_dismount_EVER = UPrHBTSetting.Instance.ARENA_do_not_dismount_EVER;
            _do_not_dismount_ooc = UPrHBTSetting.Instance.ARENA_do_not_dismount_ooc;
            _get_tank_from_focus = UPrHBTSetting.Instance.ARENA_get_tank_from_focus;
            _get_tank_from_lua = UPrHBTSetting.Instance.ARENA_get_tank_from_lua;
            _healing_tank_priority = UPrHBTSetting.Instance.ARENA_healing_tank_priority;
            _binding_heal_min_hp = UPrHBTSetting.Instance.ARENA_binding_heal_min_hp;
            _HWS_min_hp = UPrHBTSetting.Instance.ARENA_HWS_min_hp;
            _Serendipity_min_hp = UPrHBTSetting.Instance.ARENA_Serendipity_min_hp;
            _Serendipity_GH_min_hp = UPrHBTSetting.Instance.ARENA_Serendipity_GH_min_hp;
            _Heal_min_hp = UPrHBTSetting.Instance.ARENA_Heal_min_hp;
            _GH_min_hp = UPrHBTSetting.Instance.ARENA_GH_min_hp;
            _SoL_min_hp = UPrHBTSetting.Instance.ARENA_SoL_min_hp;
            _min_player_inside_CoH = UPrHBTSetting.Instance.ARENA_min_player_inside_CoH;
            _CoH_how_much_health = UPrHBTSetting.Instance.ARENA_CoH_how_much_health;
            _DY_how_much_health = UPrHBTSetting.Instance.ARENA_DY_how_much_health;
            _min_player_inside_DY = UPrHBTSetting.Instance.ARENA_min_player_inside_DY;
            _wanna_GS = UPrHBTSetting.Instance.ARENA_wanna_GS;
            _stop_GH_if_above = UPrHBTSetting.Instance.ARENA_stop_GH_if_above;
            _min_renew_hp_on_non_tank = UPrHBTSetting.Instance.ARENA_min_renew_hp_on_non_tank;
            _wanna_DP = UPrHBTSetting.Instance.ARENA_wanna_DP;
            _min_DP_hp = UPrHBTSetting.Instance.ARENA_min_DP_hp;
            _wanna_fade = UPrHBTSetting.Instance.ARENA_wanna_fade;
            _min_fade_hp = UPrHBTSetting.Instance.ARENA_min_fade_hp;
            _wanna_pom_ooc = UPrHBTSetting.Instance.ARENA_wanna_pom_ooc;
            _wanna_pom_in_combat = UPrHBTSetting.Instance.ARENA_wanna_pom_in_combat;
            _min_Shadowfiend_mana_if_safe = UPrHBTSetting.Instance.ARENA_min_Shadowfiend_mana_if_safe;
            _wanna_Hymm_of_Hope_after_shadowfiend = UPrHBTSetting.Instance.ARENA_wanna_Hymm_of_Hope_after_shadowfiend;
            _min_Shadowfiend_mana_NOT_safe = UPrHBTSetting.Instance.ARENA_min_Shadowfiend_mana_NOT_safe;
            _interrupt_HoH_if_someone_below = UPrHBTSetting.Instance.ARENA_interrupt_HoH_if_someone_below;
            _check_level_for_shadowfind = UPrHBTSetting.Instance.ARENA_check_level_for_shadowfind;
            _decice_during_GCD = UPrHBTSetting.Instance.ARENA_decice_during_GCD;
            _intellywait = UPrHBTSetting.Instance.ARENA_intellywait;
            _AOE_check_self = UPrHBTSetting.Instance.ARENA_AOE_check_self;
            _AOE_check_tank = UPrHBTSetting.Instance.ARENA_AOE_check_tank;
            _AOE_check_focus = UPrHBTSetting.Instance.ARENA_AOE_check_focus;
            _AOE_check_everyone = UPrHBTSetting.Instance.ARENA_AOE_check_everyone;
            _use_PoM_on_CD = UPrHBTSetting.Instance.ARENA_use_PoM_on_CD;
            _min_player_inside_HWSa = UPrHBTSetting.Instance.ARENA_min_player_inside_HWSa;
            _HWSa_how_much_health = UPrHBTSetting.Instance.ARENA_HWSa_how_much_health;
            _AOE_check_tar = UPrHBTSetting.Instance.ARENA_AOE_check_tar;
            _min_player_inside_PoH = UPrHBTSetting.Instance.ARENA_min_player_inside_PoH;
            _PoH_how_much_health = UPrHBTSetting.Instance.ARENA_PoH_how_much_health;
            _wanna_PoH = UPrHBTSetting.Instance.ARENA_wanna_PoH;
            _wanna_PoM = UPrHBTSetting.Instance.ARENA_wanna_PoM;
            _wanna_CoH = UPrHBTSetting.Instance.ARENA_wanna_CoH;
            _cleanse_only_self_and_tank = UPrHBTSetting.Instance.ARENA_cleanse_only_self_and_tank;
            return true;
        }

        public bool Priest_PVE_dispell_inizializer()
        {
            _do_not_touch[1] = UPrHBTSetting.Instance.PVE_do_not_touch_TB1;
            _do_not_touch[2] = UPrHBTSetting.Instance.PVE_do_not_touch_TB2;
            _do_not_touch[3] = UPrHBTSetting.Instance.PVE_do_not_touch_TB3;
            _do_not_touch[4] = UPrHBTSetting.Instance.PVE_do_not_touch_TB4;
            _do_not_touch[5] = UPrHBTSetting.Instance.PVE_do_not_touch_TB5;
            _do_not_touch[6] = UPrHBTSetting.Instance.PVE_do_not_touch_TB6;
            _do_not_touch[7] = UPrHBTSetting.Instance.PVE_do_not_touch_TB7;
            _do_not_touch[8] = UPrHBTSetting.Instance.PVE_do_not_touch_TB8;
            _do_not_touch[9] = UPrHBTSetting.Instance.PVE_do_not_touch_TB9;
            _do_not_touch[10] = UPrHBTSetting.Instance.PVE_do_not_touch_TB10;
            _do_not_touch[11] = UPrHBTSetting.Instance.PVE_do_not_touch_TB11;
            _do_not_touch[12] = UPrHBTSetting.Instance.PVE_do_not_touch_TB12;
            _do_not_touch[13] = UPrHBTSetting.Instance.PVE_do_not_touch_TB13;
            _do_not_touch[14] = UPrHBTSetting.Instance.PVE_do_not_touch_TB14;
            _do_not_touch[15] = UPrHBTSetting.Instance.PVE_do_not_touch_TB15;
            _do_not_touch[16] = UPrHBTSetting.Instance.PVE_do_not_touch_TB16;
            _do_not_touch[17] = UPrHBTSetting.Instance.PVE_do_not_touch_TB17;
            _do_not_touch[18] = UPrHBTSetting.Instance.PVE_do_not_touch_TB18;
            _do_not_touch[19] = UPrHBTSetting.Instance.PVE_do_not_touch_TB19;
            _do_not_touch[20] = UPrHBTSetting.Instance.PVE_do_not_touch_TB20;

            _dispell_ASAP[1] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB1;
            _dispell_ASAP[2] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB2;
            _dispell_ASAP[3] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB3;
            _dispell_ASAP[4] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB4;
            _dispell_ASAP[5] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB5;
            _dispell_ASAP[6] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB6;
            _dispell_ASAP[7] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB7;
            _dispell_ASAP[8] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB8;
            _dispell_ASAP[9] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB9;
            _dispell_ASAP[10] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB10;
            _dispell_ASAP[11] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB11;
            _dispell_ASAP[12] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB12;
            _dispell_ASAP[13] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB13;
            _dispell_ASAP[14] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB14;
            _dispell_ASAP[15] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB15;
            _dispell_ASAP[16] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB16;
            _dispell_ASAP[17] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB17;
            _dispell_ASAP[18] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB18;
            _dispell_ASAP[19] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB19;
            _dispell_ASAP[20] = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB20;
            return true;
        }

        public bool Priest_PVP_dispell_inizializer()
        {
            _do_not_touch[1] = UPrHBTSetting.Instance.PVP_do_not_touch_TB1;
            _do_not_touch[2] = UPrHBTSetting.Instance.PVP_do_not_touch_TB2;
            _do_not_touch[3] = UPrHBTSetting.Instance.PVP_do_not_touch_TB3;
            _do_not_touch[4] = UPrHBTSetting.Instance.PVP_do_not_touch_TB4;
            _do_not_touch[5] = UPrHBTSetting.Instance.PVP_do_not_touch_TB5;
            _do_not_touch[6] = UPrHBTSetting.Instance.PVP_do_not_touch_TB6;
            _do_not_touch[7] = UPrHBTSetting.Instance.PVP_do_not_touch_TB7;
            _do_not_touch[8] = UPrHBTSetting.Instance.PVP_do_not_touch_TB8;
            _do_not_touch[9] = UPrHBTSetting.Instance.PVP_do_not_touch_TB9;
            _do_not_touch[10] = UPrHBTSetting.Instance.PVP_do_not_touch_TB10;
            _do_not_touch[11] = UPrHBTSetting.Instance.PVP_do_not_touch_TB11;
            _do_not_touch[12] = UPrHBTSetting.Instance.PVP_do_not_touch_TB12;
            _do_not_touch[13] = UPrHBTSetting.Instance.PVP_do_not_touch_TB13;
            _do_not_touch[14] = UPrHBTSetting.Instance.PVP_do_not_touch_TB14;
            _do_not_touch[15] = UPrHBTSetting.Instance.PVP_do_not_touch_TB15;
            _do_not_touch[16] = UPrHBTSetting.Instance.PVP_do_not_touch_TB16;
            _do_not_touch[17] = UPrHBTSetting.Instance.PVP_do_not_touch_TB17;
            _do_not_touch[18] = UPrHBTSetting.Instance.PVP_do_not_touch_TB18;
            _do_not_touch[19] = UPrHBTSetting.Instance.PVP_do_not_touch_TB19;
            _do_not_touch[20] = UPrHBTSetting.Instance.PVP_do_not_touch_TB20;

            _dispell_ASAP[1] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB1;
            _dispell_ASAP[2] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB2;
            _dispell_ASAP[3] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB3;
            _dispell_ASAP[4] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB4;
            _dispell_ASAP[5] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB5;
            _dispell_ASAP[6] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB6;
            _dispell_ASAP[7] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB7;
            _dispell_ASAP[8] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB8;
            _dispell_ASAP[9] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB9;
            _dispell_ASAP[10] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB10;
            _dispell_ASAP[11] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB11;
            _dispell_ASAP[12] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB12;
            _dispell_ASAP[13] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB13;
            _dispell_ASAP[14] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB14;
            _dispell_ASAP[15] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB15;
            _dispell_ASAP[16] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB16;
            _dispell_ASAP[17] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB17;
            _dispell_ASAP[18] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB18;
            _dispell_ASAP[19] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB19;
            _dispell_ASAP[20] = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB20;
            return true;
        }

        public bool Priest_Variable_Printer()
        {
            slog("_wanna_buff {0}", _wanna_buff);
            slog("_wanna_cleanse {0}", _wanna_cleanse);
            slog("_wanna_everymanforhimself {0}", _wanna_everymanforhimself);
            slog("_wanna_face {0}", _wanna_face);
            slog("_wanna_gift {0}", _wanna_gift);
            slog("_wanna_mana_potion {0}", _wanna_mana_potion);
            slog("_wanna_mount {0}", _wanna_mount);
            slog("_wanna_move_to_heal {0}", _wanna_move_to_heal);
            slog("_wanna_stoneform {0}", _wanna_stoneform);
            slog("_wanna_target {0}", _wanna_target);
            slog("_wanna_torrent {0}", _wanna_torrent);
            slog("_wanna_urgent_cleanse {0}", _wanna_urgent_cleanse);
            slog("_do_not_heal_above {0}", _do_not_heal_above);
            slog("_max_healing_distance {0}", _max_healing_distance);
            slog("_min_gift_hp {0}", _min_gift_hp);
            slog("_min_mana_potion {0}", _min_mana_potion);
            slog("_min_mana_rec_trinket {0}", _min_mana_rec_trinket);
            slog("_min_ohshitbutton_activator {0}", _min_ohshitbutton_activator);
            slog("_min_stoneform {0}", _min_stoneform);
            slog("_min_torrent_mana_perc {0}", _min_torrent_mana_perc);
            slog("_rest_if_mana_below {0}", _rest_if_mana_below);
            slog("_use_mana_rec_trinket_every {0}", _use_mana_rec_trinket_every);
            slog("_wanna_move {0}", _wanna_move);
            slog("_wanna_taunt {0}", _wanna_taunt);
            slog("_wanna_lifeblood {0}", _wanna_lifeblood);
            slog("_do_not_dismount_ooc {0}", _do_not_dismount_ooc);
            slog("_do_not_dismount_EVER {0}", _do_not_dismount_EVER);
            slog("_selective_healing {0}", _selective_healing);
            if (_selective_healing)
            {
                for (i = 0; i < 25; i++)
                {
                    slog("Will heal raid member " + i + " {0} ", _heal_raid_member[i]);
                }
            }
            slog("_get_tank_from_focus {0}", _get_tank_from_focus);
            slog("_ignore_beacon {0}", _ignore_beacon);
            slog("_get_tank_from_lua {0}", _get_tank_from_lua);
            slog("_healing_tank_priority {0}", _healing_tank_priority);
            slog("_binding_heal_min_hp {0}", _binding_heal_min_hp);
            slog("_HWS_min_hp {0}", _HWS_min_hp);
            slog("_Serendipity_min_hp {0}", _Serendipity_min_hp);
            slog("_Serendipity_GH_min_hp {0}", _Serendipity_GH_min_hp);
            slog("_Heal_min_hp {0}", _Heal_min_hp);
            slog("_GH_min_hp {0}", _GH_min_hp);
            slog("_SoL_min_hp {0}", _SoL_min_hp);
            slog("_wanna_CoH {0}", _wanna_CoH);
            slog("_min_player_inside_CoH {0}", _min_player_inside_CoH);
            slog("_CoH_how_much_health {0}", _CoH_how_much_health);
            slog("_DY_how_much_health {0}", _DY_how_much_health);
            slog("_min_player_inside_DY {0}", _min_player_inside_DY);
            slog("_stop_GH_if_above {0}", _stop_GH_if_above);
            slog("_min_renew_hp_on_non_tank {0}", _min_renew_hp_on_non_tank);
            slog("_wanna_DP {0}", _wanna_DP);
            slog("_min_DP_hp {0}", _min_DP_hp);
            slog("_wanna_fade {0}", _wanna_fade);
            slog("_wanna_pom_ooc {0}", _wanna_pom_ooc);
            slog("_wanna_pom_in_combat {0}", _wanna_pom_in_combat);
            slog("_min_Shadowfiend_mana_if_safe {0}", _min_Shadowfiend_mana_if_safe);
            slog("_wanna_Hymm_of_Hope_after_shadowfiend {0}", _wanna_Hymm_of_Hope_after_shadowfiend);
            slog("_min_Shadowfiend_mana_NOT_safe {0}", _min_Shadowfiend_mana_NOT_safe);
            slog("_interrupt_HoH_if_someone_below {0}", _interrupt_HoH_if_someone_below);
            slog("_check_level_for_shadowfind {0}", _check_level_for_shadowfind);
            slog("_raid_healer {0}", _raid_healer);
            slog("_decice_during_GCD {0}", _decice_during_GCD);
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
            slog("_AOE_check_self {0}", _AOE_check_self);
            slog("_AOE_check_tank {0}", _AOE_check_tank);
            slog("_AOE_check_focus {0}", _AOE_check_focus);
            slog("_AOE_check_tar {0}", _AOE_check_tar);
            slog("_AOE_check_everyone {0}", _AOE_check_everyone);
            slog("_general_raid_healer {0}", _general_raid_healer);
            slog("_wanna_PoM {0}", _wanna_PoM);
            slog("_use_PoM_on_CD {0}", _use_PoM_on_CD);
            slog("_min_player_inside_HWSa {0}", _min_player_inside_HWSa);
            slog("_HWSa_how_much_health {0}", _HWSa_how_much_health);
            slog("_wanna_PoH {0}", _wanna_PoH);
            slog("_min_player_inside_PoH {0}", _min_player_inside_PoH);
            slog("_PoH_how_much_health {0}", _PoH_how_much_health);
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
            slog("_wanna_SWD {0}", _wanna_SWD);
            slog("_precasting {0}", _precasting);
            slog("_how_much_wait {0}", _how_much_wait);
            slog("_cleanse_only_self_and_tank {0}", _cleanse_only_self_and_tank);
            return true;
        }

        #endregion
    }
}