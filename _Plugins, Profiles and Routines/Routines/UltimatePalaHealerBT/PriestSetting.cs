using System.IO;
using Styx;
using Styx.Helpers;

namespace UltimatePaladinHealerBT
{
    public class UPrHBTSetting : Settings
    {
        public static readonly UPrHBTSetting Instance = new UPrHBTSetting();
        public UPrHBTSetting()
            : base(Path.Combine(Logging.ApplicationPath, string.Format("CustomClasses/Config/UPrHCCBT-Settings-{0}.xml", StyxWoW.Me.Name)))
        { }


        [Setting, DefaultValue(false)]
        public bool Selective_Healing { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member0 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member1 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member2 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member3 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member4 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member5 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member6 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member7 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member8 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member9 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member10 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member11 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member12 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member13 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member14 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member15 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member16 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member17 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member18 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member19 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member20 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member21 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member22 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member23 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member24 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member25 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member26 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member27 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member28 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member29 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member30 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member31 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member32 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member33 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member34 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member35 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member36 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member37 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member38 { get; set; }
        [Setting, DefaultValue(true)]
        public bool Heal_raid_member39 { get; set; }

        [Setting, DefaultValue(5)]
        public int General_precasting { get; set; }
        [Setting, DefaultValue(200)]
        public int General_how_much_wait { get; set; }

        /*Solo*/
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(95)]
        public int Solo_do_not_heal_above { get; set; }
        [Setting, DefaultValue(40)]
        public int Solo_max_healing_distance { get; set; }
        [Setting, DefaultValue(40)]
        public int Solo_min_gift_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int Solo_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int Solo_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int Solo_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(80)]
        public int Solo_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int Solo_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int Solo_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int Solo_use_mana_rec_trinket_every { get; set; }
        [Setting, DefaultValue(0)]
        public int Solo_healing_tank_priority { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(70)]
        public int Solo_binding_heal_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int Solo_HWS_min_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int Solo_Serendipity_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Solo_Serendipity_GH_min_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Solo_Heal_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Solo_GH_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int Solo_SoL_min_hp { get; set; }
        [Setting, DefaultValue(3)]
        public int Solo_min_player_inside_CoH { get; set; }
        [Setting, DefaultValue(85)]
        public int Solo_CoH_how_much_health { get; set; }
        [Setting, DefaultValue(3)]
        public int Solo_min_player_inside_DY { get; set; }
        [Setting, DefaultValue(50)]
        public int Solo_DY_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_GS { get; set; }
        [Setting, DefaultValue(85)]
        public int Solo_stop_GH_if_above { get; set; }
        [Setting, DefaultValue(60)]
        public int Solo_min_renew_hp_on_non_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_DP { get; set; }
        [Setting, DefaultValue(40)]
        public int Solo_min_DP_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_fade { get; set; }
        [Setting, DefaultValue(85)]
        public int Solo_min_fade_hp { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_wanna_pom_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_pom_in_combat { get; set; }
        [Setting, DefaultValue(60)]
        public int Solo_min_Shadowfiend_mana_if_safe { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_Hymm_of_Hope_after_shadowfiend { get; set; }
        [Setting, DefaultValue(20)]
        public int Solo_min_Shadowfiend_mana_NOT_safe { get; set; }
        [Setting, DefaultValue(40)]
        public int Solo_interrupt_HoH_if_someone_below { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_check_level_for_shadowfind { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_intellywait { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_AOE_check_self { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_AOE_check_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_AOE_check_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_AOE_check_everyone { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_AOE_check_tar { get; set; }
        [Setting, DefaultValue(3)]
        public int Solo_min_player_inside_PoH { get; set; }
        [Setting, DefaultValue(85)]
        public int Solo_PoH_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_PoM { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_wanna_PoH { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_wanna_CoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_SWD { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_enable_pull { get; set; }

        /*PVE*/
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(95)]
        public int PVE_do_not_heal_above { get; set; }
        [Setting, DefaultValue(40)]
        public int PVE_max_healing_distance { get; set; }
        [Setting, DefaultValue(40)]
        public int PVE_min_gift_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int PVE_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int PVE_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int PVE_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(80)]
        public int PVE_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int PVE_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int PVE_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int PVE_use_mana_rec_trinket_every { get; set; }
        [Setting, DefaultValue(0)]
        public int PVE_healing_tank_priority { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(70)]
        public int PVE_binding_heal_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int PVE_HWS_min_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int PVE_Serendipity_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int PVE_Serendipity_GH_min_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int PVE_Heal_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int PVE_GH_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int PVE_SoL_min_hp { get; set; }
        [Setting, DefaultValue(3)]
        public int PVE_min_player_inside_CoH { get; set; }
        [Setting, DefaultValue(85)]
        public int PVE_CoH_how_much_health { get; set; }
        [Setting, DefaultValue(3)]
        public int PVE_min_player_inside_DY { get; set; }
        [Setting, DefaultValue(50)]
        public int PVE_DY_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_GS { get; set; }
        [Setting, DefaultValue(85)]
        public int PVE_stop_GH_if_above { get; set; }
        [Setting, DefaultValue(60)]
        public int PVE_min_renew_hp_on_non_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_DP { get; set; }
        [Setting, DefaultValue(40)]
        public int PVE_min_DP_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_fade { get; set; }
        [Setting, DefaultValue(85)]
        public int PVE_min_fade_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_pom_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_pom_in_combat { get; set; }
        [Setting, DefaultValue(60)]
        public int PVE_min_Shadowfiend_mana_if_safe { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_Hymm_of_Hope_after_shadowfiend { get; set; }
        [Setting, DefaultValue(20)]
        public int PVE_min_Shadowfiend_mana_NOT_safe { get; set; }
        [Setting, DefaultValue(40)]
        public int PVE_interrupt_HoH_if_someone_below { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_check_level_for_shadowfind { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_intellywait { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_AOE_check_self { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_AOE_check_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_AOE_check_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_AOE_check_everyone { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_AOE_check_tar { get; set; }				
        [Setting, DefaultValue(3)]
        public int PVE_min_player_inside_PoH { get; set; }
        [Setting, DefaultValue(85)]
        public int PVE_PoH_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_PoM { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_PoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_CoH { get; set; }

        /*Raid Tank Healer*/
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(95)]
        public int Raid_do_not_heal_above { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_max_healing_distance { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_min_gift_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int Raid_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(80)]
        public int Raid_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int Raid_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_use_mana_rec_trinket_every { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_healing_tank_priority { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_binding_heal_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int Raid_HWS_min_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int Raid_Serendipity_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_Serendipity_GH_min_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_Heal_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_GH_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int Raid_SoL_min_hp { get; set; }
        [Setting, DefaultValue(3)]
        public int Raid_min_player_inside_CoH { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_CoH_how_much_health { get; set; }
        [Setting, DefaultValue(3)]
        public int Raid_min_player_inside_DY { get; set; }
        [Setting, DefaultValue(50)]
        public int Raid_DY_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_GS { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_stop_GH_if_above { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_min_renew_hp_on_non_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_DP { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_min_DP_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_fade { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_min_fade_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_pom_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_pom_in_combat { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_min_Shadowfiend_mana_if_safe { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_Hymm_of_Hope_after_shadowfiend { get; set; }
        [Setting, DefaultValue(20)]
        public int Raid_min_Shadowfiend_mana_NOT_safe { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_interrupt_HoH_if_someone_below { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_check_level_for_shadowfind { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_intellywait { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_check_self { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_check_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_check_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_AOE_check_everyone { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_check_tar { get; set; }
        [Setting, DefaultValue(3)]
        public int Raid_min_player_inside_PoH { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_PoH_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_PoM { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_PoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_CoH { get; set; }

        /*General Raid Healer or Not*/
        [Setting, DefaultValue(0)]
        public int Raid_general_raid_healer { get; set; }
        [Setting, DefaultValue(0)]
        public int Battleground_general_raid_healer { get; set; }
        [Setting, DefaultValue(0)]
        public int WorldPVP_general_raid_healer { get; set; }
        [Setting, DefaultValue(0)]
        public int ARENA_general_raid_healer { get; set; }

        /*Raid AOE Healer*/
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_AOE_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(95)]
        public int Raid_AOE_do_not_heal_above { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_AOE_max_healing_distance { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_AOE_min_gift_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int Raid_AOE_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_AOE_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_AOE_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(80)]
        public int Raid_AOE_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int Raid_AOE_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_AOE_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_AOE_use_mana_rec_trinket_every { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_AOE_healing_tank_priority { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_AOE_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_AOE_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_AOE_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_AOE_binding_heal_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int Raid_AOE_HWS_min_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int Raid_AOE_Serendipity_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_AOE_Serendipity_GH_min_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_AOE_Heal_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_AOE_GH_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int Raid_AOE_SoL_min_hp { get; set; }
        [Setting, DefaultValue(3)]
        public int Raid_AOE_min_player_inside_CoH { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_AOE_CoH_how_much_health { get; set; }
        [Setting, DefaultValue(3)]
        public int Raid_AOE_min_player_inside_DY { get; set; }
        [Setting, DefaultValue(50)]
        public int Raid_AOE_DY_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_GS { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_AOE_stop_GH_if_above { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_AOE_min_renew_hp_on_non_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_DP { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_AOE_min_DP_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_fade { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_AOE_min_fade_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_pom_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_pom_in_combat { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_AOE_min_Shadowfiend_mana_if_safe { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_Hymm_of_Hope_after_shadowfiend { get; set; }
        [Setting, DefaultValue(20)]
        public int Raid_AOE_min_Shadowfiend_mana_NOT_safe { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_AOE_interrupt_HoH_if_someone_below { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_check_level_for_shadowfind { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_intellywait { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_AOE_AOE_check_everyone { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_AOE_check_focus { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_AOE_check_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_AOE_check_self { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_use_PoM_on_CD { get; set; }
        [Setting, DefaultValue(3)]
        public int Raid_AOE_min_player_inside_HWSa { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_AOE_HWSa_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_AOE_check_tar { get; set; }
        [Setting, DefaultValue(3)]
        public int Raid_AOE_min_player_inside_PoH { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_AOE_PoH_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_PoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_PoM { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_AOE_wanna_CoH { get; set; }

        /*WorldPVP*/
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(95)]
        public int WorldPVP_do_not_heal_above { get; set; }
        [Setting, DefaultValue(40)]
        public int WorldPVP_max_healing_distance { get; set; }
        [Setting, DefaultValue(40)]
        public int WorldPVP_min_gift_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int WorldPVP_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int WorldPVP_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int WorldPVP_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(80)]
        public int WorldPVP_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int WorldPVP_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int WorldPVP_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int WorldPVP_use_mana_rec_trinket_every { get; set; }
        [Setting, DefaultValue(60)]
        public int WorldPVP_healing_tank_priority { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(70)]
        public int WorldPVP_binding_heal_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int WorldPVP_HWS_min_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int WorldPVP_Serendipity_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int WorldPVP_Serendipity_GH_min_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_Heal_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int WorldPVP_GH_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int WorldPVP_SoL_min_hp { get; set; }
        [Setting, DefaultValue(3)]
        public int WorldPVP_min_player_inside_CoH { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_CoH_how_much_health { get; set; }
        [Setting, DefaultValue(3)]
        public int WorldPVP_min_player_inside_DY { get; set; }
        [Setting, DefaultValue(50)]
        public int WorldPVP_DY_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_GS { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_stop_GH_if_above { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_min_renew_hp_on_non_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_DP { get; set; }
        [Setting, DefaultValue(40)]
        public int WorldPVP_min_DP_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_fade { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_min_fade_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_pom_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_pom_in_combat { get; set; }
        [Setting, DefaultValue(60)]
        public int WorldPVP_min_Shadowfiend_mana_if_safe { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_Hymm_of_Hope_after_shadowfiend { get; set; }
        [Setting, DefaultValue(20)]
        public int WorldPVP_min_Shadowfiend_mana_NOT_safe { get; set; }
        [Setting, DefaultValue(40)]
        public int WorldPVP_interrupt_HoH_if_someone_below { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_check_level_for_shadowfind { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_intellywait { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_AOE_check_everyone { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_AOE_check_focus { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_AOE_check_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_AOE_check_self { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_use_PoM_on_CD { get; set; }
        [Setting, DefaultValue(3)]
        public int WorldPVP_min_player_inside_HWSa { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_HWSa_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_AOE_check_tar { get; set; }
        [Setting, DefaultValue(3)]
        public int WorldPVP_min_player_inside_PoH { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_PoH_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_PoM { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_PoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_CoH { get; set; }

        /*ARENA*/
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(95)]
        public int ARENA_do_not_heal_above { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA_max_healing_distance { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA_min_gift_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int ARENA_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(80)]
        public int ARENA_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int ARENA_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int ARENA_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int ARENA_use_mana_rec_trinket_every { get; set; }
        [Setting, DefaultValue(60)]
        public int ARENA_healing_tank_priority { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(70)]
        public int ARENA_binding_heal_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int ARENA_HWS_min_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int ARENA_Serendipity_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int ARENA_Serendipity_GH_min_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA_Heal_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int ARENA_GH_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int ARENA_SoL_min_hp { get; set; }
        [Setting, DefaultValue(3)]
        public int ARENA_min_player_inside_CoH { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA_CoH_how_much_health { get; set; }
        [Setting, DefaultValue(3)]
        public int ARENA_min_player_inside_DY { get; set; }
        [Setting, DefaultValue(50)]
        public int ARENA_DY_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_GS { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA_stop_GH_if_above { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA_min_renew_hp_on_non_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_DP { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA_min_DP_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_fade { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA_min_fade_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_pom_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_pom_in_combat { get; set; }
        [Setting, DefaultValue(60)]
        public int ARENA_min_Shadowfiend_mana_if_safe { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_Hymm_of_Hope_after_shadowfiend { get; set; }
        [Setting, DefaultValue(20)]
        public int ARENA_min_Shadowfiend_mana_NOT_safe { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA_interrupt_HoH_if_someone_below { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_check_level_for_shadowfind { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_intellywait { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA_AOE_check_everyone { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_AOE_check_focus { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_AOE_check_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_AOE_check_self { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_use_PoM_on_CD { get; set; }
        [Setting, DefaultValue(3)]
        public int ARENA_min_player_inside_HWSa { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA_HWSa_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_AOE_check_tar { get; set; }
        [Setting, DefaultValue(3)]
        public int ARENA_min_player_inside_PoH { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA_PoH_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_PoM { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_PoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_CoH { get; set; }

        /*Battleground*/
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(95)]
        public int Battleground_do_not_heal_above { get; set; }
        [Setting, DefaultValue(40)]
        public int Battleground_max_healing_distance { get; set; }
        [Setting, DefaultValue(40)]
        public int Battleground_min_gift_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int Battleground_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int Battleground_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int Battleground_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(80)]
        public int Battleground_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int Battleground_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int Battleground_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int Battleground_use_mana_rec_trinket_every { get; set; }
        [Setting, DefaultValue(60)]
        public int Battleground_healing_tank_priority { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(70)]
        public int Battleground_binding_heal_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int Battleground_HWS_min_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int Battleground_Serendipity_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Battleground_Serendipity_GH_min_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_Heal_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Battleground_GH_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int Battleground_SoL_min_hp { get; set; }
        [Setting, DefaultValue(3)]
        public int Battleground_min_player_inside_CoH { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_CoH_how_much_health { get; set; }
        [Setting, DefaultValue(3)]
        public int Battleground_min_player_inside_DY { get; set; }
        [Setting, DefaultValue(50)]
        public int Battleground_DY_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_GS { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_stop_GH_if_above { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_min_renew_hp_on_non_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_DP { get; set; }
        [Setting, DefaultValue(40)]
        public int Battleground_min_DP_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_fade { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_min_fade_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_pom_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_pom_in_combat { get; set; }
        [Setting, DefaultValue(60)]
        public int Battleground_min_Shadowfiend_mana_if_safe { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_Hymm_of_Hope_after_shadowfiend { get; set; }
        [Setting, DefaultValue(20)]
        public int Battleground_min_Shadowfiend_mana_NOT_safe { get; set; }
        [Setting, DefaultValue(40)]
        public int Battleground_interrupt_HoH_if_someone_below { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_check_level_for_shadowfind { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_intellywait { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_AOE_check_everyone { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_AOE_check_focus { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_AOE_check_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_AOE_check_self { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_use_PoM_on_CD { get; set; }
        [Setting, DefaultValue(3)]
        public int Battleground_min_player_inside_HWSa { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_HWSa_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_AOE_check_tar { get; set; }
        [Setting, DefaultValue(3)]
        public int Battleground_min_player_inside_PoH { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_PoH_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_PoM { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_PoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_CoH { get; set; }

        /*RAF*/
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(95)]
        public int RAF_do_not_heal_above { get; set; }
        [Setting, DefaultValue(40)]
        public int RAF_max_healing_distance { get; set; }
        [Setting, DefaultValue(40)]
        public int RAF_min_gift_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int RAF_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int RAF_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int RAF_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(80)]
        public int RAF_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int RAF_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int RAF_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int RAF_use_mana_rec_trinket_every { get; set; }
        [Setting, DefaultValue(0)]
        public int RAF_healing_tank_priority { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(70)]
        public int RAF_binding_heal_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int RAF_HWS_min_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int RAF_Serendipity_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int RAF_Serendipity_GH_min_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int RAF_Heal_min_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int RAF_GH_min_hp { get; set; }
        [Setting, DefaultValue(90)]
        public int RAF_SoL_min_hp { get; set; }
        [Setting, DefaultValue(3)]
        public int RAF_min_player_inside_CoH { get; set; }
        [Setting, DefaultValue(85)]
        public int RAF_CoH_how_much_health { get; set; }
        [Setting, DefaultValue(3)]
        public int RAF_min_player_inside_DY { get; set; }
        [Setting, DefaultValue(50)]
        public int RAF_DY_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_GS { get; set; }
        [Setting, DefaultValue(85)]
        public int RAF_stop_GH_if_above { get; set; }
        [Setting, DefaultValue(60)]
        public int RAF_min_renew_hp_on_non_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_DP { get; set; }
        [Setting, DefaultValue(40)]
        public int RAF_min_DP_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_fade { get; set; }
        [Setting, DefaultValue(85)]
        public int RAF_min_fade_hp { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_pom_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_pom_in_combat { get; set; }
        [Setting, DefaultValue(60)]
        public int RAF_min_Shadowfiend_mana_if_safe { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_Hymm_of_Hope_after_shadowfiend { get; set; }
        [Setting, DefaultValue(20)]
        public int RAF_min_Shadowfiend_mana_NOT_safe { get; set; }
        [Setting, DefaultValue(40)]
        public int RAF_interrupt_HoH_if_someone_below { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_check_level_for_shadowfind { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_intellywait { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_AOE_check_self { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_AOE_check_tank { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_AOE_check_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_AOE_check_everyone { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_AOE_check_tar { get; set; }
        [Setting, DefaultValue(3)]
        public int RAF_min_player_inside_PoH { get; set; }
        [Setting, DefaultValue(85)]
        public int RAF_PoH_how_much_health { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_PoM { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_PoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_CoH { get; set; }				





        [Setting, DefaultValue("Unstable Affliction")]
        public string PVP_do_not_touch_TB1 { get; set; }
        [Setting, DefaultValue("Vampiric Touch")]
        public string PVP_do_not_touch_TB2 { get; set; }
        [Setting, DefaultValue("Flame Shock")]
        public string PVP_do_not_touch_TB3 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB4 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB5 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB6 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB7 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB8 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB9 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB10 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB11 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB12 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB13 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB14 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB15 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB16 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB17 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB18 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB19 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_do_not_touch_TB20 { get; set; }

        [Setting, DefaultValue("Fear")]
        public string PVP_dispell_ASAP_TB1 { get; set; }
        [Setting, DefaultValue("Polymorph")]
        public string PVP_dispell_ASAP_TB2 { get; set; }
        [Setting, DefaultValue("Freezing Trap")]
        public string PVP_dispell_ASAP_TB3 { get; set; }
        [Setting, DefaultValue("Wyvern Sting")]
        public string PVP_dispell_ASAP_TB4 { get; set; }
        [Setting, DefaultValue("Seduction")]
        public string PVP_dispell_ASAP_TB5 { get; set; }
        [Setting, DefaultValue("Mind Control")]
        public string PVP_dispell_ASAP_TB6 { get; set; }
        [Setting, DefaultValue("Repetance")]
        public string PVP_dispell_ASAP_TB7 { get; set; }
        [Setting, DefaultValue("Psychic Scream")]
        public string PVP_dispell_ASAP_TB8 { get; set; }
        [Setting, DefaultValue("Hammer of Justice")]
        public string PVP_dispell_ASAP_TB9 { get; set; }
        [Setting, DefaultValue("Intimidating Shout")]
        public string PVP_dispell_ASAP_TB10 { get; set; }
        [Setting, DefaultValue("Howl of Terror")]
        public string PVP_dispell_ASAP_TB11 { get; set; }
        [Setting, DefaultValue("Deep Freeze")]
        public string PVP_dispell_ASAP_TB12 { get; set; }
        [Setting, DefaultValue("Ring of Frost")]
        public string PVP_dispell_ASAP_TB13 { get; set; }
        [Setting, DefaultValue("Hungering Cold")]
        public string PVP_dispell_ASAP_TB14 { get; set; }
        [Setting, DefaultValue("Repentance")]
        public string PVP_dispell_ASAP_TB15 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_dispell_ASAP_TB16 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_dispell_ASAP_TB17 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_dispell_ASAP_TB18 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_dispell_ASAP_TB19 { get; set; }
        [Setting, DefaultValue("")]
        public string PVP_dispell_ASAP_TB20 { get; set; }

        [Setting, DefaultValue("Blackout")]
        public string PVE_do_not_touch_TB1 { get; set; }
        [Setting, DefaultValue("Toxic Torment")]
        public string PVE_do_not_touch_TB2 { get; set; }
        [Setting, DefaultValue("Frostburn Formula")]
        public string PVE_do_not_touch_TB3 { get; set; }
        [Setting, DefaultValue("Burning Blood")]
        public string PVE_do_not_touch_TB4 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB5 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB6 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB7 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB8 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB9 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB10 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB11 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB12 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB13 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB14 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB15 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB16 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB17 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB18 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB19 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_do_not_touch_TB20 { get; set; }

        [Setting, DefaultValue("Fear")]
        public string PVE_dispell_ASAP_TB1 { get; set; }
        [Setting, DefaultValue("Static Cling")]
        public string PVE_dispell_ASAP_TB2 { get; set; }
        [Setting, DefaultValue("Flame Shock")]
        public string PVE_dispell_ASAP_TB3 { get; set; }
        [Setting, DefaultValue("Static Discharge")]
        public string PVE_dispell_ASAP_TB4 { get; set; }
        [Setting, DefaultValue("Consuming Darkness")]
        public string PVE_dispell_ASAP_TB5 { get; set; }
        [Setting, DefaultValue("Lash of Anguish")]
        public string PVE_dispell_ASAP_TB6 { get; set; }
        [Setting, DefaultValue("Static Disruption")]
        public string PVE_dispell_ASAP_TB7 { get; set; }
        [Setting, DefaultValue("Accelerated Corruption")]
        public string PVE_dispell_ASAP_TB8 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB9 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB10 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB11 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB12 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB13 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB14 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB15 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB16 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB17 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB18 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB19 { get; set; }
        [Setting, DefaultValue("")]
        public string PVE_dispell_ASAP_TB20 { get; set; }

        [Setting, DefaultValue(false)]
        public bool ARENA_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA2v2_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_AOE_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool WriteLog { get; set; }
    }
}
