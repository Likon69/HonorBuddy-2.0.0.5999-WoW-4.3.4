
namespace UltimatePaladinHealerBT
{
    partial class UltimatePalaHealerBT
    {
        private bool _Inf_of_light_wanna_DL;
        private bool _wanna_AW;
        private bool _wanna_buff;
        private bool _wanna_cleanse;
        private bool _wanna_crusader;
        private bool _wanna_CS;
        private bool _wanna_DF;
        private bool _wanna_DP;
        private bool _wanna_DS;
        private bool _wanna_everymanforhimself;
        private bool _wanna_face;
        private bool _wanna_gift;
        private bool _wanna_GotAK;
        private bool _wanna_HoJ;
        private bool _wanna_HoP;
        private bool _wanna_HoS;
        private bool _wanna_HoW;
        private bool _wanna_HR;
        private bool _wanna_Judge;
        private bool _wanna_LoH;
        private bool _wanna_mana_potion;
        private bool _wanna_mount;
        private bool _wanna_move_to_heal;
        private bool _wanna_move_to_HoJ;
        private bool _wanna_rebuke;
        private bool _wanna_stoneform;
        private bool _wanna_target;
        private bool _wanna_torrent;
        private bool _wanna_urgent_cleanse;
        private int _aura_type;
        private int _do_not_heal_above;
        private int _HR_how_far;
        private int _HR_how_much_health;
        private int _mana_judge;
        private int _max_healing_distance;
        private int _min_Divine_Plea_mana;
        private int _min_DL_hp;
        private int _min_DP_hp;
        private int _min_DS_hp;
        private int _min_FoL_hp;
        private int _min_gift_hp;
        private int _min_HL_hp;
        private int _min_HoP_hp;
        private int _min_HoS_hp;
        private int _min_Inf_of_light_DL_hp;
        private int _min_LoH_hp;
        private int _min_mana_potion;
        private int _min_mana_rec_trinket;
        private int _min_ohshitbutton_activator;
        private int _min_player_inside_HR;
        private int _min_stoneform;
        private int _min_torrent_mana_perc;
        private int _rest_if_mana_below;
        private int _use_mana_rec_trinket_every;

        private bool _wanna_move;       //Solo
        private bool _wanna_taunt;      //Arena
        private bool _wanna_HoF;        //Arena
        public bool _wanna_lifeblood;
        public bool _do_not_dismount_EVER;
        public bool _do_not_dismount_ooc;
        public bool _selective_healing;
        public bool[] _heal_raid_member =new bool[41];
        private bool _get_tank_from_focus;
        private bool _ignore_beacon=false;    //Raid
        private bool _get_tank_from_lua;
        private int _stop_DL_if_above=0;       //Raid and PVE
        private int _bless_type;
        private int _healing_tank_priority=0;   //Raid and PVE for now

        private int _binding_heal_min_hp;
        private int _HWS_min_hp;
        private int _Serendipity_min_hp;
        private int _Serendipity_GH_min_hp;
        private int _Heal_min_hp;
        private int _GH_min_hp;
        private int _SoL_min_hp;
        private int _min_player_inside_CoH;
        private int _CoH_how_much_health;
        private int _min_player_inside_DY;
        private int _DY_how_much_health;
        private bool _wanna_GS;
        private int _stop_GH_if_above;
        private bool _decice_during_GCD=false;
        private bool _can_dispel_disease;
        private bool _can_dispel_magic;
        private bool _can_dispel_poison;
        private int _min_renew_hp_on_non_tank;
        private bool _wanna_fade;
        private int _min_fade_hp;
        private bool _wanna_pom_ooc;
        private bool _wanna_pom_in_combat;
        private int _min_Shadowfiend_mana_if_safe;
        private bool _wanna_Hymm_of_Hope_after_shadowfiend;
        private int _min_Shadowfiend_mana_NOT_safe;
        private int _interrupt_HoH_if_someone_below;
        private bool _check_level_for_shadowfind;
        private bool _raid_healer;
        private bool _intellywait;
        private int _how_much_wait=200;
        private bool _AOE_check_self;
        private bool _AOE_check_tank;
        private bool _AOE_check_focus;
        private bool _AOE_check_everyone;
        private bool _AOE_check_tar;
        private bool _answer_PVP_attacks;
        private int _general_raid_healer;
        private bool _use_PoM_on_CD;
        private int _min_player_inside_HWSa;
        private int _HWSa_how_much_health;
        private bool General_casted_HWSa;
        private bool General_we_are_in_combat = false;
        private bool General_we_were_in_combat = false;
        private int index;
        private string supportname;
        private int indexj;
        //private bool namepassed; // never used
        private int indext;
        private int tryes2;
        private int counti;
        private int countj;
        private int _min_player_inside_PoH;
        private int _PoH_how_much_health;
        public string[] _do_not_touch = new string[21];
        public string[] _dispell_ASAP = new string[21];
        private bool _account_for_lag=true;
        private bool _enable_pull;
        private bool _wanna_PoH;
        private bool _wanna_CoH;
        private bool _wanna_PoM;
        private bool _wanna_HS_on_CD;
        private bool _wanna_SWD;
        private string _trinket1_name;
        private uint _trinket1_ID;
        private float _trinket1_CD;
        private bool _trinket1_usable;
        private int _tank_healing_priority_multiplier = 1;
        private string _trinket2_name;
        private uint _trinket2_ID;
        private float _trinket2_CD;
        private bool _trinket2_usable;
        private int _trinket1_use_when;
        private int _trinket2_use_when;
        private bool _trinket1_passive;
        private bool _trinket2_passive;
        private int trinketcount;
        private bool _Stop_Healing;
        private int _precasting;
        private bool _cleanse_only_self_and_tank;
    }
}
