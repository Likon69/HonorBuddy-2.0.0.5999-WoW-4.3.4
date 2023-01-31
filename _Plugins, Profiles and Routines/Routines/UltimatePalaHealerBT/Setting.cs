using System.IO;
using Styx;
using Styx.Helpers;
namespace UltimatePaladinHealerBT
{
    public class UPaHBTSetting : Settings
    {
        public static readonly UPaHBTSetting Instance = new UPaHBTSetting();
        public UPaHBTSetting()
            : base(Path.Combine(Logging.ApplicationPath, string.Format("CustomClasses/Config/UPaHCCBT-Settings-{0}.xml", StyxWoW.Me.Name)))
        { }
        /*SOLO*/
        [Setting, DefaultValue(true)]
        public bool Solo_Inf_of_light_wanna_DL { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_AW { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_crusader { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_CS { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_DF { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_DP { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_DS { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_GotAK { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_HoP { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_HoS { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_HoW { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_HR { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_Judge { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_LoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_move_to_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_rebuke { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(0)]
        public int Solo_aura_type { get; set; }
        [Setting, DefaultValue(95)]
        public int Solo_do_not_heal_above { get; set; }
        [Setting, DefaultValue(10)]
        public int Solo_HR_how_far { get; set; }
        [Setting, DefaultValue(70)]
        public int Solo_HR_how_much_health { get; set; }
        [Setting, DefaultValue(100)]
        public int Solo_mana_judge { get; set; }
        [Setting, DefaultValue(40)]
        public int Solo_max_healing_distance { get; set; }
        [Setting, DefaultValue(70)]
        public int Solo_min_Divine_Plea_mana { get; set; }
        [Setting, DefaultValue(70)]
        public int Solo_min_DL_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Solo_min_DP_hp { get; set; }
        [Setting, DefaultValue(35)]
        public int Solo_min_DS_hp { get; set; }
        [Setting, DefaultValue(35)]
        public int Solo_min_FoL_hp { get; set; }
        [Setting, DefaultValue(40)]
        public int Solo_min_gift_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Solo_min_HL_hp { get; set; }
        [Setting, DefaultValue(25)]
        public int Solo_min_HoP_hp { get; set; }
        [Setting, DefaultValue(65)]
        public int Solo_min_HoS_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Solo_min_Inf_of_light_DL_hp { get; set; }
        [Setting, DefaultValue(15)]
        public int Solo_min_LoH_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int Solo_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int Solo_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int Solo_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(3)]
        public int Solo_min_player_inside_HR { get; set; }
        [Setting, DefaultValue(80)]
        public int Solo_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int Solo_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int Solo_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int Solo_use_mana_rec_trinket_every { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_move { get; set; }
        /*PVE*/
        [Setting, DefaultValue(true)]
        public bool PVE_Inf_of_light_wanna_DL { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_AW { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_cleanse { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_wanna_crusader { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_CS { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_DF { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_DP { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_DS { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_GotAK { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_wanna_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_HoP { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_HoS { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_HoW { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_HR { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_Judge { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_LoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_wanna_move_to_HoJ { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_wanna_rebuke { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(0)]
        public int PVE_aura_type { get; set; }
        [Setting, DefaultValue(95)]
        public int PVE_do_not_heal_above { get; set; }
        [Setting, DefaultValue(10)]
        public int PVE_HR_how_far { get; set; }
        [Setting, DefaultValue(70)]
        public int PVE_HR_how_much_health { get; set; }
        [Setting, DefaultValue(70)]
        public int PVE_mana_judge { get; set; }
        [Setting, DefaultValue(40)]
        public int PVE_max_healing_distance { get; set; }
        [Setting, DefaultValue(70)]
        public int PVE_min_Divine_Plea_mana { get; set; }
        [Setting, DefaultValue(70)]
        public int PVE_min_DL_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int PVE_min_DP_hp { get; set; }
        [Setting, DefaultValue(35)]
        public int PVE_min_DS_hp { get; set; }
        [Setting, DefaultValue(35)]
        public int PVE_min_FoL_hp { get; set; }
        [Setting, DefaultValue(40)]
        public int PVE_min_gift_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int PVE_min_HL_hp { get; set; }
        [Setting, DefaultValue(25)]
        public int PVE_min_HoP_hp { get; set; }
        [Setting, DefaultValue(65)]
        public int PVE_min_HoS_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int PVE_min_Inf_of_light_DL_hp { get; set; }
        [Setting, DefaultValue(15)]
        public int PVE_min_LoH_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int PVE_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int PVE_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int PVE_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(3)]
        public int PVE_min_player_inside_HR { get; set; }
        [Setting, DefaultValue(80)]
        public int PVE_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int PVE_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int PVE_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int PVE_use_mana_rec_trinket_every { get; set; }
        /*Raid*/
        [Setting, DefaultValue(true)]
        public bool Raid_Inf_of_light_wanna_DL { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_AW { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_cleanse { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_wanna_crusader { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_CS { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_DF { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_DP { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_DS { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_GotAK { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_wanna_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_HoP { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_HoS { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_HoW { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_HR { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_Judge { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_LoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_wanna_move_to_HoJ { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_wanna_rebuke { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(0)]
        public int Raid_aura_type { get; set; }
        [Setting, DefaultValue(95)]
        public int Raid_do_not_heal_above { get; set; }
        [Setting, DefaultValue(10)]
        public int Raid_HR_how_far { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_HR_how_much_health { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_mana_judge { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_max_healing_distance { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_min_Divine_Plea_mana { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_min_DL_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_min_DP_hp { get; set; }
        [Setting, DefaultValue(35)]
        public int Raid_min_DS_hp { get; set; }
        [Setting, DefaultValue(35)]
        public int Raid_min_FoL_hp { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_min_gift_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Raid_min_HL_hp { get; set; }
        [Setting, DefaultValue(25)]
        public int Raid_min_HoP_hp { get; set; }
        [Setting, DefaultValue(65)]
        public int Raid_min_HoS_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Raid_min_Inf_of_light_DL_hp { get; set; }
        [Setting, DefaultValue(15)]
        public int Raid_min_LoH_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int Raid_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int Raid_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(3)]
        public int Raid_min_player_inside_HR { get; set; }
        [Setting, DefaultValue(80)]
        public int Raid_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int Raid_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int Raid_use_mana_rec_trinket_every { get; set; }
        /*Battleground*/
        [Setting, DefaultValue(true)]
        public bool Battleground_Inf_of_light_wanna_DL { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_AW { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_crusader { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_CS { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_DF { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_DP { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_DS { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_GotAK { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_HoP { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_HoS { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_HoW { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_HR { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_Judge { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_LoH { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_wanna_move_to_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_rebuke { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(0)]
        public int Battleground_aura_type { get; set; }
        [Setting, DefaultValue(95)]
        public int Battleground_do_not_heal_above { get; set; }
        [Setting, DefaultValue(10)]
        public int Battleground_HR_how_far { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_HR_how_much_health { get; set; }
        [Setting, DefaultValue(50)]
        public int Battleground_mana_judge { get; set; }
        [Setting, DefaultValue(40)]
        public int Battleground_max_healing_distance { get; set; }
        [Setting, DefaultValue(70)]
        public int Battleground_min_Divine_Plea_mana { get; set; }
        [Setting, DefaultValue(0)]
        public int Battleground_min_DL_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_min_DP_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int Battleground_min_DS_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int Battleground_min_FoL_hp { get; set; }
        [Setting, DefaultValue(40)]
        public int Battleground_min_gift_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_min_HL_hp { get; set; }
        [Setting, DefaultValue(25)]
        public int Battleground_min_HoP_hp { get; set; }
        [Setting, DefaultValue(65)]
        public int Battleground_min_HoS_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int Battleground_min_Inf_of_light_DL_hp { get; set; }
        [Setting, DefaultValue(15)]
        public int Battleground_min_LoH_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int Battleground_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int Battleground_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int Battleground_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(2)]
        public int Battleground_min_player_inside_HR { get; set; }
        [Setting, DefaultValue(80)]
        public int Battleground_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int Battleground_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int Battleground_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int Battleground_use_mana_rec_trinket_every { get; set; }
        /*WorldPVP*/
        [Setting, DefaultValue(true)]
        public bool WorldPVP_Inf_of_light_wanna_DL { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_AW { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_crusader { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_CS { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_DF { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_DP { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_DS { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_GotAK { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_HoP { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_HoS { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_HoW { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_HR { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_Judge { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_LoH { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_wanna_move_to_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_rebuke { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(0)]
        public int WorldPVP_aura_type { get; set; }
        [Setting, DefaultValue(95)]
        public int WorldPVP_do_not_heal_above { get; set; }
        [Setting, DefaultValue(10)]
        public int WorldPVP_HR_how_far { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_HR_how_much_health { get; set; }
        [Setting, DefaultValue(50)]
        public int WorldPVP_mana_judge { get; set; }
        [Setting, DefaultValue(40)]
        public int WorldPVP_max_healing_distance { get; set; }
        [Setting, DefaultValue(70)]
        public int WorldPVP_min_Divine_Plea_mana { get; set; }
        [Setting, DefaultValue(0)]
        public int WorldPVP_min_DL_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_min_DP_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int WorldPVP_min_DS_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int WorldPVP_min_FoL_hp { get; set; }
        [Setting, DefaultValue(40)]
        public int WorldPVP_min_gift_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_min_HL_hp { get; set; }
        [Setting, DefaultValue(25)]
        public int WorldPVP_min_HoP_hp { get; set; }
        [Setting, DefaultValue(65)]
        public int WorldPVP_min_HoS_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int WorldPVP_min_Inf_of_light_DL_hp { get; set; }
        [Setting, DefaultValue(15)]
        public int WorldPVP_min_LoH_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int WorldPVP_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int WorldPVP_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int WorldPVP_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(2)]
        public int WorldPVP_min_player_inside_HR { get; set; }
        [Setting, DefaultValue(80)]
        public int WorldPVP_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int WorldPVP_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int WorldPVP_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int WorldPVP_use_mana_rec_trinket_every { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_Inf_of_light_wanna_DL { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_AW { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_cleanse { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA_wanna_crusader { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_CS { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_DF { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_DP { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_DS { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_GotAK { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_HoP { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_HoS { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_HoW { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_HR { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_Judge { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_LoH { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_mount { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_move_to_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_rebuke { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(0)]
        public int ARENA_aura_type { get; set; }
        [Setting, DefaultValue(95)]
        public int ARENA_do_not_heal_above { get; set; }
        [Setting, DefaultValue(10)]
        public int ARENA_HR_how_far { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA_HR_how_much_health { get; set; }
        [Setting, DefaultValue(50)]
        public int ARENA_mana_judge { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA_max_healing_distance { get; set; }
        [Setting, DefaultValue(70)]
        public int ARENA_min_Divine_Plea_mana { get; set; }
        [Setting, DefaultValue(0)]
        public int ARENA_min_DL_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA_min_DP_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int ARENA_min_DS_hp { get; set; }
        [Setting, DefaultValue(95)]
        public int ARENA_min_FoL_hp { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA_min_gift_hp { get; set; }
        [Setting, DefaultValue(95)]
        public int ARENA_min_HL_hp { get; set; }
        [Setting, DefaultValue(25)]
        public int ARENA_min_HoP_hp { get; set; }
        [Setting, DefaultValue(65)]
        public int ARENA_min_HoS_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA_min_Inf_of_light_DL_hp { get; set; }
        [Setting, DefaultValue(15)]
        public int ARENA_min_LoH_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int ARENA_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(2)]
        public int ARENA_min_player_inside_HR { get; set; }
        [Setting, DefaultValue(80)]
        public int ARENA_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int ARENA_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int ARENA_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int ARENA_use_mana_rec_trinket_every { get; set; }
        /*ARENA2v2*/
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_Inf_of_light_wanna_DL { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_AW { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_cleanse { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_crusader { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_CS { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_DF { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_DP { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_DS { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_GotAK { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_HoP { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_HoS { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_HoW { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_HR { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_Judge { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_LoH { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA2v2_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_mount { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_move_to_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_rebuke { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(0)]
        public int ARENA2v2_aura_type { get; set; }
        [Setting, DefaultValue(95)]
        public int ARENA2v2_do_not_heal_above { get; set; }
        [Setting, DefaultValue(10)]
        public int ARENA2v2_HR_how_far { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA2v2_HR_how_much_health { get; set; }
        [Setting, DefaultValue(50)]
        public int ARENA2v2_mana_judge { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA2v2_max_healing_distance { get; set; }
        [Setting, DefaultValue(70)]
        public int ARENA2v2_min_Divine_Plea_mana { get; set; }
        [Setting, DefaultValue(0)]
        public int ARENA2v2_min_DL_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA2v2_min_DP_hp { get; set; }
        [Setting, DefaultValue(50)]
        public int ARENA2v2_min_DS_hp { get; set; }
        [Setting, DefaultValue(95)]
        public int ARENA2v2_min_FoL_hp { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA2v2_min_gift_hp { get; set; }
        [Setting, DefaultValue(95)]
        public int ARENA2v2_min_HL_hp { get; set; }
        [Setting, DefaultValue(25)]
        public int ARENA2v2_min_HoP_hp { get; set; }
        [Setting, DefaultValue(65)]
        public int ARENA2v2_min_HoS_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int ARENA2v2_min_Inf_of_light_DL_hp { get; set; }
        [Setting, DefaultValue(15)]
        public int ARENA2v2_min_LoH_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int ARENA2v2_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA2v2_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int ARENA2v2_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(2)]
        public int ARENA2v2_min_player_inside_HR { get; set; }
        [Setting, DefaultValue(80)]
        public int ARENA2v2_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int ARENA2v2_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int ARENA2v2_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int ARENA2v2_use_mana_rec_trinket_every { get; set; }
        /*RAF*/
        [Setting, DefaultValue(true)]
        public bool RAF_Inf_of_light_wanna_DL { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_AW { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_cleanse { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_wanna_crusader { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_CS { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_DF { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_DP { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_DS { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_everymanforhimself { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_face { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_gift { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_GotAK { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_wanna_HoJ { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_HoP { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_HoS { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_HoW { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_HR { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_Judge { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_LoH { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_mana_potion { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_mount { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_wanna_move_to_heal { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_wanna_move_to_HoJ { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_wanna_rebuke { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_stoneform { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_target { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_torrent { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_urgent_cleanse { get; set; }
        [Setting, DefaultValue(0)]
        public int RAF_aura_type { get; set; }
        [Setting, DefaultValue(95)]
        public int RAF_do_not_heal_above { get; set; }
        [Setting, DefaultValue(10)]
        public int RAF_HR_how_far { get; set; }
        [Setting, DefaultValue(70)]
        public int RAF_HR_how_much_health { get; set; }
        [Setting, DefaultValue(70)]
        public int RAF_mana_judge { get; set; }
        [Setting, DefaultValue(40)]
        public int RAF_max_healing_distance { get; set; }
        [Setting, DefaultValue(70)]
        public int RAF_min_Divine_Plea_mana { get; set; }
        [Setting, DefaultValue(70)]
        public int RAF_min_DL_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int RAF_min_DP_hp { get; set; }
        [Setting, DefaultValue(35)]
        public int RAF_min_DS_hp { get; set; }
        [Setting, DefaultValue(35)]
        public int RAF_min_FoL_hp { get; set; }
        [Setting, DefaultValue(40)]
        public int RAF_min_gift_hp { get; set; }
        [Setting, DefaultValue(85)]
        public int RAF_min_HL_hp { get; set; }
        [Setting, DefaultValue(25)]
        public int RAF_min_HoP_hp { get; set; }
        [Setting, DefaultValue(65)]
        public int RAF_min_HoS_hp { get; set; }
        [Setting, DefaultValue(70)]
        public int RAF_min_Inf_of_light_DL_hp { get; set; }
        [Setting, DefaultValue(15)]
        public int RAF_min_LoH_hp { get; set; }
        [Setting, DefaultValue(20)]
        public int RAF_min_mana_potion { get; set; }
        [Setting, DefaultValue(40)]
        public int RAF_min_mana_rec_trinket { get; set; }
        [Setting, DefaultValue(40)]
        public int RAF_min_ohshitbutton_activator { get; set; }
        [Setting, DefaultValue(3)]
        public int RAF_min_player_inside_HR { get; set; }
        [Setting, DefaultValue(80)]
        public int RAF_min_stoneform { get; set; }
        [Setting, DefaultValue(80)]
        public int RAF_min_torrent_mana_perc { get; set; }
        [Setting, DefaultValue(60)]
        public int RAF_rest_if_mana_below { get; set; }
        [Setting, DefaultValue(60)]
        public int RAF_use_mana_rec_trinket_every { get; set; }

        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_taunt { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_HoF { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_wanna_lifeblood { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_wanna_lifeblood { get; set; }

        [Setting, DefaultValue(true)]
        public bool ARENA_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_do_not_dismount_ooc { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_do_not_dismount_ooc { get; set; }

        [Setting, DefaultValue(false)]
        public bool ARENA_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA2v2_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_do_not_dismount_EVER { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_do_not_dismount_EVER { get; set; }
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

        
        [Setting, DefaultValue(false)]
        public bool ARENA_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA2v2_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_get_tank_from_focus { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_get_tank_from_focus { get; set; }

        [Setting, DefaultValue(false)]
        public bool Raid_ignore_beacon { get; set; }

        [Setting, DefaultValue(false)]
        public bool ARENA_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA2v2_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_advanced_option { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_advanced_option { get; set; }

        [Setting, DefaultValue(false)]
        public bool ARENA_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA2v2_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_get_tank_from_lua { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_get_tank_from_lua { get; set; }

        [Setting, DefaultValue(85)]
        public int Raid_stop_DL_if_above { get; set; }
        [Setting, DefaultValue(85)]
        public int PVE_stop_DL_if_above { get; set; }

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

        [Setting, DefaultValue(0)]
        public int ARENA_bless_type { get; set; }
        [Setting, DefaultValue(0)]
        public int ARENA2v2_bless_type { get; set; }
        [Setting, DefaultValue(0)]
        public int PVE_bless_type { get; set; }
        [Setting, DefaultValue(0)]
        public int Raid_bless_type { get; set; }
        [Setting, DefaultValue(0)]
        public int Battleground_bless_type { get; set; }
        [Setting, DefaultValue(0)]
        public int RAF_bless_type { get; set; }
        [Setting, DefaultValue(0)]
        public int Solo_bless_type { get; set; }
        [Setting, DefaultValue(0)]
        public int WorldPVP_bless_type { get; set; }

        [Setting, DefaultValue(0)]
        public int PVE_healing_tank_priority { get; set; }
        [Setting, DefaultValue(0)]
        public int Raid_healing_tank_priority { get; set; }
        [Setting, DefaultValue(0)]
        public int PVE_tank_healing_priority_multiplier { get; set; }
        [Setting, DefaultValue(0)]
        public int Raid_tank_healing_priority_multiplier { get; set; }
        

        [Setting, DefaultValue(false)]
        public bool ARENA_decice_during_GCD { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA2v2_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_decice_during_GCD { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_decice_during_GCD { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_decice_during_GCD { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_decice_during_GCD { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_decice_during_GCD { get; set; }

        [Setting, DefaultValue(true)]
        public bool ARENA_intellywait { get; set; }
        [Setting, DefaultValue(true)]
        public bool ARENA2v2_intellywait { get; set; }
        [Setting, DefaultValue(true)]
        public bool PVE_intellywait { get; set; }
        [Setting, DefaultValue(true)]
        public bool Raid_intellywait { get; set; }
        [Setting, DefaultValue(true)]
        public bool Battleground_intellywait { get; set; }
        [Setting, DefaultValue(true)]
        public bool RAF_intellywait { get; set; }
        [Setting, DefaultValue(true)]
        public bool Solo_intellywait { get; set; }
        [Setting, DefaultValue(true)]
        public bool WorldPVP_intellywait { get; set; }

        [Setting, DefaultValue(true)]
        public bool Solo_answer_PVP_attacks { get; set; }

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
        public bool Solo_enable_pull { get; set; }

        [Setting, DefaultValue(false)]
        public bool ARENA_wanna_HS_on_CD { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA2v2_wanna_HS_on_CD { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_wanna_HS_on_CD { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_wanna_HS_on_CD { get; set; }
        [Setting, DefaultValue(false)]
        public bool Battleground_wanna_HS_on_CD { get; set; }
        [Setting, DefaultValue(false)]
        public bool RAF_wanna_HS_on_CD { get; set; }
        [Setting, DefaultValue(false)]
        public bool Solo_wanna_HS_on_CD { get; set; }
        [Setting, DefaultValue(false)]
        public bool WorldPVP_wanna_HS_on_CD { get; set; }

        [Setting, DefaultValue("")]
        public string Trinket1_name { get; set; }
        [Setting, DefaultValue(0u)]
        public uint Trinket1_ID { get; set; }
        [Setting, DefaultValue(0f)]
        public float Trinket1_CD { get; set; }
        [Setting, DefaultValue(1)]
        public int Trinket1_use_when { get; set; }
        [Setting, DefaultValue(false)]
        public bool Trinket1_passive { get; set; }

        [Setting, DefaultValue("")]
        public string Trinket2_name { get; set; }
        [Setting, DefaultValue(0u)]
        public uint Trinket2_ID { get; set; }
        [Setting, DefaultValue(0f)]
        public float Trinket2_CD { get; set; }
        [Setting, DefaultValue(1)]
        public int Trinket2_use_when { get; set; }
        [Setting, DefaultValue(false)]
        public bool Trinket2_passive { get; set; }

        [Setting, DefaultValue(false)]
        public bool General_Stop_Healing { get; set; }

        [Setting, DefaultValue(5)]
        public int General_precasting { get; set; }

        [Setting, DefaultValue(0)]
        public int Retr_bless_type { get; set; }
        [Setting, DefaultValue(true)]
        public bool Retr_wanna_buff { get; set; }
        [Setting, DefaultValue(true)]
        public bool Retr_wanna_crusader { get; set; }
        [Setting, DefaultValue(true)]
        public bool Retr_intellywait { get; set; }
        [Setting, DefaultValue(40)]
        public int Retr_max_healing_distance { get; set; }

        [Setting, DefaultValue(false)]
        public bool ARENA_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool ARENA2v2_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool PVE_cleanse_only_self_and_tank { get; set; }
        [Setting, DefaultValue(false)]
        public bool Raid_cleanse_only_self_and_tank { get; set; }
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