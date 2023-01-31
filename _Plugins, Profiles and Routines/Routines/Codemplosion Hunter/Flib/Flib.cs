using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using System;
using System.Drawing;

namespace Hera
{
    public static class Utils
    {
        public enum CastingBreak
        {
            None = 0,
            HealthIsAbove,
            HealthIsBelow,
            PowerIsAbove,
            PowerIsBelow
        }

        private static string _logSpam;
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return Me.CurrentTarget; } }

        public static void Log(string msg, Color colour) { if (msg == _logSpam) return; Logging.Write(colour, msg); _logSpam = msg; }
        public static void Log(string msg) { if (msg == _logSpam) return; Logging.Write(msg); _logSpam = msg; }
        public static Color Colour(string nameOfColour) { return Color.FromName(nameOfColour); }

        public static string LuaGetReturnValueString(string lua) { return Lua.GetReturnValues(lua, "stuff.lua")[0]; }

        /// <summary>
        /// Sleep for the duration of your lag
        /// </summary>
        public static void LagSleep() { StyxWoW.SleepForLagDuration(); }

        public static bool CombatCheckOk() { return CombatCheckOk(null); }
        public static bool CombatCheckOk(string spellName) { return CombatCheckOk(spellName, false); }


        public static List<string> CrowdControlSpellsList = new List<string>
                                                                {
                                                                    "Shackle Undead",
                                                                    "Hex",
                                                                    "Fear",
                                                                    "Polymorph",
                                                                    "Blind",
                                                                    "Sap",
                                                                    "Hibernate",
                                                                    "Freezing Trap",
                                                                    "Banish",
                                                                    "Seduction",
                                                                    "Mind Control",
                                                                    "Repentance",
                                                                    "Bind Elemental",
                                                                    "Cyclone",
                                                                    "Sooth Animal",
                                                                    "Turn Evil",
                                                                    "Wyvern Sting",
                                                                    "Cyclone"
                                                                };

        /// <summary>
        /// A list (EntryID) of mobs who are capable of ranged attacks. Useful to determine the type of opening spell/attack
        /// </summary>
        public static List<uint> RangedCapableMobs = new List<uint>();


        /// <summary>
        /// A list (EntryID) of mobs that should be killed ASAP for whatever reason; casters, stun, knockback effects etc
        /// </summary>
        public static List<uint> PriorityMobs = new List<uint>();

        /// <summary>
        /// A list (SpellID) of all healing spells for all classes capable of healing. 
        /// </summary>
        public static List<uint> HealingSpells = new List<uint>();

        /// <summary>
        /// A list (SpellID) of important spells that must be interrupted 
        /// </summary>
        public static List<uint> ImportantInterruptSpells = new List<uint>();

        // A list (EntryID) of instance bosses
        #region BossIDs
        public static List<uint> BossIDs = new List<uint>{
                        //Ragefire Chasm
                        11517, //Oggleflint
                        11520, //Taragaman the Hungerer
                        11518, //Jergosh the Invoker
                        11519, //Bazzalan
                        17830, //Zelemar the Wrathful

                        //The Deadmines
                        644, //Rhahk'Zor
                        3586, //Miner Johnson
                        643, //Sneed
                        642, //Sneed's Shredder
                        1763, //Gilnid
                        646, //Mr. Smite
                        645, //Cookie
                        647, //Captain Greenskin
                        639, //Edwin VanCleef
                        596, //Brainwashed Noble, outside
                        626, //Foreman Thistlenettle, outside
                        599, //Marisa du'Paige, outside
                        47162, //Glubtok
                        47296, //Helix Gearbreaker
                        43778, //Foe Reaper 5000
                        47626, //Admiral Ripsnarl
                        47739, //"Captain" Cookie
                        49541, //Vanessa VanCleef

                        //Wailing Caverns
                        5775, //Verdan the Everliving
                        3670, //Lord Pythas
                        3673, //Lord Serpentis
                        3669, //Lord Cobrahn
                        3654, //Mutanus the Devourer
                        3674, //Skum
                        3653, //Kresh
                        3671, //Lady Anacondra
                        5912, //Deviate Faerie Dragon
                        3672, //Boahn, outside
                        3655, //Mad Magglish, outside
                        3652, //Trigore the Lasher, outside

                        //Shadowfang Keep
                        3914, //Rethilgore
                        3886, //Razorclaw the Butcher
                        4279, //Odo the Blindwatcher
                        3887, //Baron Silverlaine
                        4278, //Commander Springvale
                        4274, //Fenrus the Devourer
                        3927, //Wolf Master Nandos
                        14682, //Sever (Scourge invasion only)
                        4275, //Archmage Arugal
                        3872, //Deathsworn Captain
                        46962, //Baron Ashbury
                        46963, //Lord Walden
                        46964, //Lord Godfrey

                        //Blackfathom Deeps
                        4887, //Ghamoo-ra
                        4831, //Lady Sarevess
                        12902, //Lorgus Jett
                        6243, //Gelihast
                        12876, //Baron Aquanis
                        4830, //Old Serra'kis
                        4832, //Twilight Lord Kelris
                        4829, //Aku'mai

                        //Stormwind Stockade
                        1716, //Bazil Thredd
                        1663, //Dextren Ward
                        1717, //Hamhock
                        1666, //Kam Deepfury
                        1696, //Targorr the Dread
                        1720, //Bruegal Ironknuckle

                        //Razorfen Kraul
                        4421, //Charlga Razorflank
                        4420, //Overlord Ramtusk
                        4422, //Agathelos the Raging
                        4428, //Death Speaker Jargba
                        4424, //Aggem Thorncurse
                        6168, //Roogug
                        4425, //Blind Hunter
                        4842, //Earthcaller Halmgar

                        //Gnomeregan
                        7800, //Mekgineer Thermaplugg
                        7079, //Viscous Fallout
                        7361, //Grubbis
                        6235, //Electrocutioner 6000
                        6229, //Crowd Pummeler 9-60
                        6228, //Dark Iron Ambassador
                        6231, //Techbot, outside

                        //Scarlet Monastery: The Graveyard
                        3983, //Interrogator Vishas
                        6488, //Fallen Champion
                        6490, //Azshir the Sleepless
                        6489, //Ironspine
                        14693, //Scorn
                        4543, //Bloodmage Thalnos
                        23682, //Headless Horseman
                        23800, //Headless Horseman

                        //Scarley Monastery: Library
                        3974, //Houndmaster Loksey
                        6487, //Arcanist Doan

                        //Scarley Monastery: Armory
                        3975, //Herod

                        //Scarley Monastery: Cathedral
                        4542, //High Inquisitor Fairbanks
                        3976, //Scarlet Commander Mograine
                        3977, //High Inquisitor Whitemane

                        //Razorfen Downs
                        7355, //Tuten'kash
                        14686, //Lady Falther'ess
                        7356, //Plaguemaw the Rotting
                        7357, //Mordresh Fire Eye
                        8567, //Glutton
                        7354, //Ragglesnout
                        7358, //Amnennar the Coldbringer

                        //Uldaman
                        7057, //Digmaster Shovelphlange
                        6910, //Revelosh
                        7228, //Ironaya
                        7023, //Obsidian Sentinel
                        7206, //Ancient Stone Keeper
                        7291, //Galgann Firehammer
                        4854, //Grimlok
                        2748, //Archaedas
                        6906, //Baelog

                        //Zul'Farrak
                        10082, //Zerillis
                        10080, //Sandarr Dunereaver
                        7272, //Theka the Martyr
                        8127, //Antu'sul
                        7271, //Witch Doctor Zum'rah
                        7274, //Sandfury Executioner
                        7275, //Shadowpriest Sezz'ziz
                        7796, //Nekrum Gutchewer
                        7797, //Ruuzlu
                        7267, //Chief Ukorz Sandscalp
                        10081, //Dustwraith
                        7795, //Hydromancer Velratha
                        7273, //Gahz'rilla
                        7608, //Murta Grimgut
                        7606, //Oro Eyegouge
                        7604, //Sergeant Bly

                        //Maraudon
                        //13718, //The Nameless Prophet (Pre-instance)
                        13742, //Kolk <The First Khan>
                        13741, //Gelk <The Second Khan>
                        13740, //Magra <The Third Khan>
                        13739, //Maraudos <The Fourth Khan>
                        12236, //Lord Vyletongue
                        13738, //Veng <The Fifth Khan>
                        13282, //Noxxion
                        12258, //Razorlash
                        12237, //Meshlok the Harvester
                        12225, //Celebras the Cursed
                        12203, //Landslide
                        13601, //Tinkerer Gizlock
                        13596, //Rotgrip
                        12201, //Princess Theradras

                        //Temple of Atal'Hakkar
                        1063, //Jade
                        5400, //Zekkis
                        5713, //Gasher
                        5715, //Hukku
                        5714, //Loro
                        5717, //Mijan
                        5712, //Zolo
                        5716, //Zul'Lor
                        5399, //Veyzhak the Cannibal
                        5401, //Kazkaz the Unholy
                        8580, //Atal'alarion
                        8443, //Avatar of Hakkar
                        5711, //Ogom the Wretched
                        5710, //Jammal'an the Prophet
                        5721, //Dreamscythe
                        5720, //Weaver
                        5719, //Morphaz
                        5722, //Hazzas
                        5709, //Shade of Eranikus

                        //The Blackrock Depths: Detention Block
                        9018, //High Interrogator Gerstahn

                        //The Blackrock Depths: Halls of the Law
                        9025, //Lord Roccor
                        9319, //Houndmaster Grebmar

                        //The Blackrock Depths: Ring of Law (Arena)
                        9031, //Anub'shiah
                        9029, //Eviscerator
                        9027, //Gorosh the Dervish
                        9028, //Grizzle
                        9032, //Hedrum the Creeper
                        9030, //Ok'thor the Breaker
                        16059, //Theldren

                        //The Blackrock Depths: Outer Blackrock Depths
                        9024, //Pyromancer Loregrain
                        9041, //Warder Stilgiss
                        9042, //Verek
                        9476, //Watchman Doomgrip
                        9056, //Fineous Darkvire
                        9017, //Lord Incendius
                        9016, //Bael'Gar
                        9033, //General Angerforge
                        8983, //Golem Lord Argelmach

                        //The Blackrock Depths: Grim Guzzler
                        9543, //Ribbly Screwspigot
                        9537, //Hurley Blackbreath
                        9502, //Phalanx
                        9499, //Plugger Spazzring
                        23872, //Coren Direbrew

                        //The Blackrock Depths: Inner Blackrock Depths
                        9156, //Ambassador Flamelash
                        8923, //Panzor the Invincible
                        17808, //Anger'rel
                        9039, //Doom'rel
                        9040, //Dope'rel
                        9037, //Gloom'rel
                        9034, //Hate'rel
                        9038, //Seeth'rel
                        9036, //Vile'rel
                        9938, //Magmus
                        10076, //High Priestess of Thaurissan
                        8929, //Princess Moira Bronzebeard
                        9019, //Emperor Dagran Thaurissan

                        //Dire Maul: Arena
                        11447, //Mushgog
                        11498, //Skarr the Unbreakable
                        11497, //The Razza

                        //Dire Maul: East
                        14354, //Pusillin
                        14327, //Lethtendris
                        14349, //Pimgib
                        13280, //Hydrospawn
                        11490, //Zevrim Thornhoof
                        11492, //Alzzin the Wildshaper
                        16097, //Isalien

                        //Dire Maul: North
                        14326, //Guard Mol'dar
                        14322, //Stomper Kreeg
                        14321, //Guard Fengus
                        14323, //Guard Slip'kik
                        14325, //Captain Kromcrush
                        14324, //Cho'Rush the Observer
                        11501, //King Gordok

                        //Dire Maul: West
                        11489, //Tendris Warpwood
                        11487, //Magister Kalendris
                        11467, //Tsu'zee
                        11488, //Illyanna Ravenoak
                        14690, //Revanchion (Scourge Invasion)
                        11496, //Immol'thar
                        14506, //Lord Hel'nurath
                        11486, //Prince Tortheldrin

                        //Lower Blackrock Spire
                        10263, //Burning Felguard
                        9218, //Spirestone Battle Lord
                        9219, //Spirestone Butcher
                        9217, //Spirestone Lord Magus
                        9196, //Highlord Omokk
                        9236, //Shadow Hunter Vosh'gajin
                        9237, //War Master Voone
                        16080, //Mor Grayhoof
                        9596, //Bannok Grimaxe
                        10596, //Mother Smolderweb
                        10376, //Crystal Fang
                        10584, //Urok Doomhowl
                        9736, //Quartermaster Zigris
                        10220, //Halycon
                        10268, //Gizrul the Slavener
                        9718, //Ghok Bashguud
                        9568, //Overlord Wyrmthalak

                        //Stratholme: Scarlet Stratholme
                        10393, //Skul
                        14684, //Balzaphon (Scourge Invasion)
                        //11082, //Stratholme Courier
                        11058, //Fras Siabi
                        10558, //Hearthsinger Forresten
                        10516, //The Unforgiven
                        16387, //Atiesh
                        11143, //Postmaster Malown
                        10808, //Timmy the Cruel
                        11032, //Malor the Zealous
                        11120, //Crimson Hammersmith
                        10997, //Cannon Master Willey
                        10811, //Archivist Galford
                        10813, //Balnazzar
                        16101, //Jarien
                        16102, //Sothos

                        //Stratholme: Undead Stratholme
                        10809, //Stonespine
                        10437, //Nerub'enkan
                        10436, //Baroness Anastari
                        11121, //Black Guard Swordsmith
                        10438, //Maleki the Pallid
                        10435, //Magistrate Barthilas
                        10439, //Ramstein the Gorger
                        10440, //Baron Rivendare (Stratholme)

                        //Stratholme: Defenders of the Chapel
                        17913, //Aelmar the Vanquisher
                        17911, //Cathela the Seeker
                        17910, //Gregor the Justiciar
                        17914, //Vicar Hieronymus
                        17912, //Nemas the Arbiter

                        //Scholomance
                        14861, //Blood Steward of Kirtonos
                        10506, //Kirtonos the Herald
                        14695, //Lord Blackwood (Scourge Invasion)
                        10503, //Jandice Barov
                        11622, //Rattlegore
                        14516, //Death Knight Darkreaver
                        10433, //Marduk Blackpool
                        10432, //Vectus
                        16118, //Kormok
                        10508, //Ras Frostwhisper
                        10505, //Instructor Malicia
                        11261, //Doctor Theolen Krastinov
                        10901, //Lorekeeper Polkelt
                        10507, //The Ravenian
                        10504, //Lord Alexei Barov
                        10502, //Lady Illucia Barov
                        1853, //Darkmaster Gandling

                        //Upper Blackrock Spire
                        9816, //Pyroguard Emberseer
                        10264, //Solakar Flamewreath
                        10509, //Jed Runewatcher
                        10899, //Goraluk Anvilcrack
                        10339, //Gyth
                        10429, //Warchief Rend Blackhand
                        10430, //The Beast
                        16042, //Lord Valthalak
                        10363, //General Drakkisath

                        //Zul'Gurub
                        14517, //High Priestess Jeklik
                        14507, //High Priest Venoxis
                        14510, //High Priestess Mar'li
                        11382, //Bloodlord Mandokir
                        15114, //Gahz'ranka
                        14509, //High Priest Thekal
                        14515, //High Priestess Arlokk
                        11380, //Jin'do the Hexxer
                        14834, //Hakkar
                        15082, //Gri'lek
                        15083, //Hazza'rah
                        15084, //Renataki
                        15085, //Wushoolay

                        //Onyxia's Lair
                        10184, //Onyxia

                        //Molten Core
                        12118, //Lucifron
                        11982, //Magmadar
                        12259, //Gehennas
                        12057, //Garr
                        12056, //Baron Geddon
                        12264, //Shazzrah
                        12098, //Sulfuron Harbinger
                        11988, //Golemagg the Incinerator
                        12018, //Majordomo Executus
                        11502, //Ragnaros

                        //Blackwing Lair
                        12435, //Razorgore the Untamed
                        13020, //Vaelastrasz the Corrupt
                        12017, //Broodlord Lashlayer
                        11983, //Firemaw
                        14601, //Ebonroc
                        11981, //Flamegor
                        14020, //Chromaggus
                        11583, //Nefarian
                        12557, //Grethok the Controller
                        10162, //Lord Victor Nefarius <Lord of Blackrock> (Also found in Blackrock Spire)

                        //Ruins of Ahn'Qiraj
                        15348, //Kurinnaxx
                        15341, //General Rajaxx
                        15340, //Moam
                        15370, //Buru the Gorger
                        15369, //Ayamiss the Hunter
                        15339, //Ossirian the Unscarred

                        //Temple of Ahn'Qiraj
                        15263, //The Prophet Skeram
                        15511, //Lord Kri
                        15543, //Princess Yauj
                        15544, //Vem
                        15516, //Battleguard Sartura
                        15510, //Fankriss the Unyielding
                        15299, //Viscidus
                        15509, //Princess Huhuran
                        15276, //Emperor Vek'lor
                        15275, //Emperor Vek'nilash
                        15517, //Ouro
                        15727, //C'Thun
                        15589, //Eye of C'Thun

                        //Naxxramas
                        30549, //Baron Rivendare (Naxxramas)
                        16803, //Death Knight Understudy
                        15930, //Feugen
                        15929, //Stalagg

                        //Naxxramas: Spider Wing
                        15956, //Anub'Rekhan
                        15953, //Grand Widow Faerlina
                        15952, //Maexxna

                        //Naxxramas: Abomination Wing
                        16028, //Patchwerk
                        15931, //Grobbulus
                        15932, //Gluth
                        15928, //Thaddius

                        //Naxxramas: Plague Wing
                        15954, //Noth the Plaguebringer
                        15936, //Heigan the Unclean
                        16011, //Loatheb

                        //Naxxramas: Deathknight Wing
                        16061, //Instructor Razuvious
                        16060, //Gothik the Harvester

                        //Naxxramas: The Four Horsemen
                        16065, //Lady Blaumeux
                        16064, //Thane Korth'azz
                        16062, //Highlord Mograine
                        16063, //Sir Zeliek

                        //Naxxramas: Frostwyrm Lair
                        15989, //Sapphiron
                        15990, //Kel'Thuzad
                        25465, //Kel'Thuzad


                        //Hellfire Citadel: Hellfire Ramparts
                        17306, //Watchkeeper Gargolmar
                        17308, //Omor the Unscarred
                        17537, //Vazruden
                        17307, //Vazruden the Herald
                        17536, //Nazan

                        //Hellfire Citadel: The Blood Furnace
                        17381, //The Maker
                        17380, //Broggok
                        17377, //Keli'dan the Breaker

                        //Coilfang Reservoir: Slave Pens
                        25740, //Ahune
                        17941, //Mennu the Betrayer
                        17991, //Rokmar the Crackler
                        17942, //Quagmirran

                        //Coilfang Reservoir: The Underbog
                        17770, //Hungarfen
                        18105, //Ghaz'an
                        17826, //Swamplord Musel'ek
                        17827, //Claw <Swamplord Musel'ek's Pet>
                        17882, //The Black Stalker

                        //Auchindoun: Mana-Tombs
                        18341, //Pandemonius
                        18343, //Tavarok
                        22930, //Yor (Heroic)
                        18344, //Nexus-Prince Shaffar

                        //Auchindoun: Auchenai Crypts
                        18371, //Shirrak the Dead Watcher
                        18373, //Exarch Maladaar

                        //Caverns of Time: Escape from Durnholde Keep
                        17848, //Lieutenant Drake
                        17862, //Captain Skarloc
                        18096, //Epoch Hunter
                        28132, //Don Carlos

                        //Auchindoun: Sethekk Halls
                        18472, //Darkweaver Syth
                        23035, //Anzu (Heroic)
                        18473, //Talon King Ikiss

                        //Coilfang Reservoir: The Steamvault
                        17797, //Hydromancer Thespia
                        17796, //Mekgineer Steamrigger
                        17798, //Warlord Kalithresh

                        //Auchindoun: Shadow Labyrinth
                        18731, //Ambassador Hellmaw
                        18667, //Blackheart the Inciter
                        18732, //Grandmaster Vorpil
                        18708, //Murmur

                        //Hellfire Citadel: Shattered Halls
                        16807, //Grand Warlock Nethekurse
                        20923, //Blood Guard Porung (Heroic)
                        16809, //Warbringer O'mrogg
                        16808, //Warchief Kargath Bladefist

                        //Caverns of Time: Opening the Dark Portal
                        17879, //Chrono Lord Deja
                        17880, //Temporus
                        17881, //Aeonus

                        //Tempest Keep: The Mechanar
                        19218, //Gatewatcher Gyro-Kill
                        19710, //Gatewatcher Iron-Hand
                        19219, //Mechano-Lord Capacitus
                        19221, //Nethermancer Sepethrea
                        19220, //Pathaleon the Calculator

                        //Tempest Keep: The Botanica
                        17976, //Commander Sarannis
                        17975, //High Botanist Freywinn
                        17978, //Thorngrin the Tender
                        17980, //Laj
                        17977, //Warp Splinter

                        //Tempest Keep: The Arcatraz
                        20870, //Zereketh the Unbound
                        20886, //Wrath-Scryer Soccothrates
                        20885, //Dalliah the Doomsayer
                        20912, //Harbinger Skyriss
                        20904, //Warden Mellichar

                        //Magisters' Terrace
                        24723, //Selin Fireheart
                        24744, //Vexallus
                        24560, //Priestess Delrissa
                        24664, //Kael'thas Sunstrider

                        //Karazhan
                        15550, //Attumen the Huntsman
                        16151, //Midnight
                        28194, //Tenris Mirkblood (Scourge invasion)
                        15687, //Moroes
                        16457, //Maiden of Virtue
                        15691, //The Curator
                        15688, //Terestian Illhoof
                        16524, //Shade of Aran
                        15689, //Netherspite
                        15690, //Prince Malchezaar
                        17225, //Nightbane
                        17229, //Kil'rek
                        //Chess event

                        //Karazhan: Servants' Quarters Beasts
                        16179, //Hyakiss the Lurker
                        16181, //Rokad the Ravager
                        16180, //Shadikith the Glider

                        //Karazhan: Opera Event
                        17535, //Dorothee
                        17546, //Roar
                        17543, //Strawman
                        17547, //Tinhead
                        17548, //Tito
                        18168, //The Crone
                        17521, //The Big Bad Wolf
                        17533, //Romulo
                        17534, //Julianne

                        //Gruul's Lair
                        18831, //High King Maulgar
                        19044, //Gruul the Dragonkiller

                        //Gruul's Lair: Maulgar's Ogre Council
                        18835, //Kiggler the Crazed
                        18836, //Blindeye the Seer
                        18834, //Olm the Summoner
                        18832, //Krosh Firehand

                        //Hellfire Citadel: Magtheridon's Lair
                        17257, //Magtheridon

                        //Zul'Aman: Animal Bosses
                        29024, //Nalorakk
                        28514, //Nalorakk
                        23576, //Nalorakk
                        23574, //Akil'zon
                        23578, //Jan'alai
                        28515, //Jan'alai
                        29023, //Jan'alai
                        23577, //Halazzi
                        28517, //Halazzi
                        29022, //Halazzi
                        24239, //Malacrass

                        //Zul'Aman: Final Bosses
                        24239, //Hex Lord Malacrass
                        23863, //Zul'jin

                        //Coilfang Reservoir: Serpentshrine Cavern
                        21216, //Hydross the Unstable
                        21217, //The Lurker Below
                        21215, //Leotheras the Blind
                        21214, //Fathom-Lord Karathress
                        21213, //Morogrim Tidewalker
                        21212, //Lady Vashj
                        21875, //Shadow of Leotheras

                        //Tempest Keep: The Eye
                        19514, //Al'ar
                        19516, //Void Reaver
                        18805, //High Astromancer Solarian
                        19622, //Kael'thas Sunstrider
                        20064, //Thaladred the Darkener
                        20060, //Lord Sanguinar
                        20062, //Grand Astromancer Capernian
                        20063, //Master Engineer Telonicus
                        21270, //Cosmic Infuser
                        21269, //Devastation
                        21271, //Infinity Blades
                        21268, //Netherstrand Longbow
                        21273, //Phaseshift Bulwark
                        21274, //Staff of Disintegration
                        21272, //Warp Slicer

                        //Caverns of Time: Battle for Mount Hyjal
                        17767, //Rage Winterchill
                        17808, //Anetheron
                        17888, //Kaz'rogal
                        17842, //Azgalor
                        17968, //Archimonde

                        //Black Temple
                        22887, //High Warlord Naj'entus
                        22898, //Supremus
                        22841, //Shade of Akama
                        22871, //Teron Gorefiend
                        22948, //Gurtogg Bloodboil
                        23420, //Essence of Anger
                        23419, //Essence of Desire
                        23418, //Essence of Suffering
                        22947, //Mother Shahraz
                        23426, //Illidari Council
                        22917, //Illidan Stormrage -- Not adding solo quest IDs for now
                        22949, //Gathios the Shatterer
                        22950, //High Nethermancer Zerevor
                        22951, //Lady Malande
                        22952, //Veras Darkshadow

                        //Sunwell Plateau
                        24891, //Kalecgos
                        25319, //Kalecgos
                        24850, //Kalecgos
                        24882, //Brutallus
                        25038, //Felmyst
                        25165, //Lady Sacrolash
                        25166, //Grand Warlock Alythess
                        25741, //M'uru
                        25315, //Kil'jaeden
                        25840, //Entropius
                        24892, //Sathrovarr the Corruptor


                        //Utgarde Keep: Main Bosses
                        23953, //Prince Keleseth (Utgarde Keep)
                        27390, //Skarvald the Constructor
                        24200, //Skarvald the Constructor
                        23954, //Ingvar the Plunderer
                        23980, //Ingvar the Plunderer

                        //Utgarde Keep: Secondary Bosses
                        27389, //Dalronn the Controller
                        24201, //Dalronn the Controller

                        //The Nexus
                        26798, //Commander Kolurg (Heroic)
                        26796, //Commander Stoutbeard (Heroic)
                        26731, //Grand Magus Telestra
                        26832, //Grand Magus Telestra
                        26928, //Grand Magus Telestra
                        26929, //Grand Magus Telestra
                        26930, //Grand Magus Telestra
                        26763, //Anomalus
                        26794, //Ormorok the Tree-Shaper
                        26723, //Keristrasza

                        //Azjol-Nerub
                        28684, //Krik'thir the Gatewatcher
                        28921, //Hadronox
                        29120, //Anub'arak

                        //Ahn'kahet: The Old Kingdom
                        29309, //Elder Nadox
                        29308, //Prince Taldaram (Ahn'kahet: The Old Kingdom)
                        29310, //Jedoga Shadowseeker
                        29311, //Herald Volazj
                        30258, //Amanitar (Heroic)

                        //Drak'Tharon Keep
                        26630, //Trollgore
                        26631, //Novos the Summoner
                        27483, //King Dred
                        26632, //The Prophet Tharon'ja
                        27696, //The Prophet Tharon'ja

                        //The Violet Hold
                        29315, //Erekem
                        29313, //Ichoron
                        29312, //Lavanthor
                        29316, //Moragg
                        29266, //Xevozz
                        29314, //Zuramat the Obliterator
                        31134, //Cyanigosa

                        //Gundrak
                        29304, //Slad'ran
                        29305, //Moorabi
                        29307, //Drakkari Colossus
                        29306, //Gal'darah
                        29932, //Eck the Ferocious (Heroic)

                        //Halls of Stone
                        27977, //Krystallus
                        27975, //Maiden of Grief
                        28234, //The Tribunal of Ages
                        27978, //Sjonnir The Ironshaper

                        //Halls of Lightning
                        28586, //General Bjarngrim
                        28587, //Volkhan
                        28546, //Ionar
                        28923, //Loken

                        //The Oculus
                        27654, //Drakos the Interrogator
                        27447, //Varos Cloudstrider
                        27655, //Mage-Lord Urom
                        27656, //Ley-Guardian Eregos

                        //Caverns of Time: Culling of Stratholme
                        26529, //Meathook
                        26530, //Salramm the Fleshcrafter
                        26532, //Chrono-Lord Epoch
                        32273, //Infinite Corruptor
                        26533, //Mal'Ganis
                        29620, //Mal'Ganis

                        //Utgarde Pinnacle
                        26668, //Svala Sorrowgrave
                        26687, //Gortok Palehoof
                        26693, //Skadi the Ruthless
                        26861, //King Ymiron

                        //Trial of the Champion: Alliance
                        35617, //Deathstalker Visceri <Grand Champion of Undercity>
                        35569, //Eressea Dawnsinger <Grand Champion of Silvermoon>
                        35572, //Mokra the Skullcrusher <Grand Champion of Orgrimmar>
                        35571, //Runok Wildmane <Grand Champion of the Thunder Bluff>
                        35570, //Zul'tore <Grand Champion of Sen'jin>

                        //Trial of the Champion: Horde
                        34702, //Ambrose Boltspark <Grand Champion of Gnomeregan>
                        34701, //Colosos <Grand Champion of the Exodar>
                        34705, //Marshal Jacob Alerius <Grand Champion of Stormwind>
                        34657, //Jaelyne Evensong <Grand Champion of Darnassus>
                        34703, //Lana Stouthammer <Grand Champion of Ironforge>

                        //Trial of the Champion: Neutral
                        34928, //Argent Confessor Paletress
                        35119, //Eadric the Pure
                        35451, //The Black Knight

                        //Forge of Souls
                        36497, //Bronjahm
                        36502, //Devourer of Souls

                        //Pit of Saron
                        36494, //Forgemaster Garfrost
                        36477, //Krick
                        36476, //Ick <Krick's Minion>
                        36658, //Scourgelord Tyrannus

                        //Halls of Reflection
                        38112, //Falric
                        38113, //Marwyn
                        37226, //The Lich King
                        38113, //Marvyn

                        //Obsidian Sanctum
                        30451, //Shadron
                        30452, //Tenebron
                        30449, //Vesperon
                        28860, //Sartharion

                        //Vault of Archavon
                        31125, //Archavon the Stone Watcher
                        33993, //Emalon the Storm Watcher
                        35013, //Koralon the Flamewatcher
                        38433, //Toravon the Ice Watcher

                        //The Eye of Eternity
                        28859, //Malygos

                        //Ulduar: The Siege of Ulduar
                        33113, //Flame Leviathan
                        33118, //Ignis the Furnace Master
                        33186, //Razorscale
                        33293, //XT-002 Deconstructor
                        33670, //Aerial Command Unit
                        33329, //Heart of the Deconstructor
                        33651, //VX-001

                        //Ulduar: The Antechamber of Ulduar
                        32867, //Steelbreaker
                        32927, //Runemaster Molgeim
                        32857, //Stormcaller Brundir
                        32930, //Kologarn
                        33515, //Auriaya
                        34035, //Feral Defender
                        32933, //Left Arm
                        32934, //Right Arm
                        33524, //Saronite Animus

                        //Ulduar: The Keepers of Ulduar
                        33350, //Mimiron
                        32906, //Freya
                        32865, //Thorim
                        32845, //Hodir

                        //Ulduar: The Descent into Madness
                        33271, //General Vezax
                        33890, //Brain of Yogg-Saron
                        33136, //Guardian of Yogg-Saron
                        33288, //Yogg-Saron
                        32915, //Elder Brightleaf
                        32913, //Elder Ironbranch
                        32914, //Elder Stonebark
                        32882, //Jormungar Behemoth
                        33432, //Leviathan Mk II
                        34014, //Sanctum Sentry

                        //Ulduar: The Celestial Planetarium
                        32871, //Algalon the Observer

                        //Trial of the Crusader
                        34796, //Gormok
                        35144, //Acidmaw
                        34799, //Dreadscale
                        34797, //Icehowl

                        34780, //Jaraxxus

                        34461, //Tyrius Duskblade <Death Knight>
                        34460, //Kavina Grovesong <Druid>
                        34469, //Melador Valestrider <Druid>
                        34467, //Alyssia Moonstalker <Hunter>
                        34468, //Noozle Whizzlestick <Mage>
                        34465, //Velanaa <Paladin>
                        34471, //Baelnor Lightbearer <Paladin>
                        34466, //Anthar Forgemender <Priest>
                        34473, //Brienna Nightfell <Priest>
                        34472, //Irieth Shadowstep <Rogue>
                        34470, //Saamul <Shaman>
                        34463, //Shaabad <Shaman>
                        34474, //Serissa Grimdabbler <Warlock>
                        34475, //Shocuul <Warrior>

                        34458, //Gorgrim Shadowcleave <Death Knight>
                        34451, //Birana Stormhoof <Druid>
                        34459, //Erin Misthoof <Druid>
                        34448, //Ruj'kah <Hunter>
                        34449, //Ginselle Blightslinger <Mage>
                        34445, //Liandra Suncaller <Paladin>
                        34456, //Malithas Brightblade <Paladin>
                        34447, //Caiphus the Stern <Priest>
                        34441, //Vivienne Blackwhisper <Priest>
                        34454, //Maz'dinah <Rogue>
                        34444, //Thrakgar	<Shaman>
                        34455, //Broln Stouthorn <Shaman>
                        34450, //Harkzog <Warlock>
                        34453, //Narrhok Steelbreaker <Warrior>

                        35610, //Cat <Ruj'kah's Pet / Alyssia Moonstalker's Pet>
                        35465, //Zhaagrym <Harkzog's Minion / Serissa Grimdabbler's Minion>

                        34497, //Fjola Lightbane
                        34496, //Eydis Darkbane
                        34564, //Anub'arak (Trial of the Crusader)

                        //Icecrown Citadel
                        36612, //Lord Marrowgar
                        36855, //Lady Deathwhisper

                        //Gunship Battle
                        37813, //Deathbringer Saurfang
                        36626, //Festergut
                        36627, //Rotface
                        36678, //Professor Putricide
                        37972, //Prince Keleseth (Icecrown Citadel)
                        37970, //Prince Valanar
                        37973, //Prince Taldaram (Icecrown Citadel)
                        37955, //Queen Lana'thel
                        36789, //Valithria Dreamwalker
                        37950, //Valithria Dreamwalker (Phased)
                        37868, //Risen Archmage, Valitrhia Add
                        36791, //Blazing Skeleton, Valithria Add
                        37934, //Blistering Zombie, Valithria Add
                        37886, //Gluttonous Abomination, Valithria Add
                        37985, //Dream Cloud , Valithria "Add" 
                        36853, //Sindragosa
                        36597, //The Lich King (Icecrown Citadel)
                        37217, //Precious
                        37025, //Stinki
                        36661, //Rimefang <Drake of Tyrannus>

                        //Ruby Sanctum (PTR 3.3.5)
                        39746, //Zarithrian
                        39747, //Saviana
                        39751, //Baltharus
                        39863, //Halion
                        39899, //Baltharus (Copy has an own id apparently)
                        40142, //Halion (twilight realm)

                        //Blackrock Mountain: Blackrock Caverns
                        39665, //Rom'ogg Bonecrusher
                        39679, //Corla, Herald of Twilight
                        39698, //Karsh Steelbender
                        39700, //Beauty
                        39705, //Ascendant Lord Obsidius

                        //Abyssal Maw: Throne of the Tides
                        40586, //Lady Naz'jar
                        40765, //Commander Ulthok
                        40825, //Erunak Stonespeaker
                        40788, //Mindbender Ghur'sha
                        42172, //Ozumat

                        //The Stonecore
                        43438, //Corborus
                        43214, //Slabhide
                        42188, //Ozruk
                        42333, //High Priestess Azil

                        //The Vortex Pinnacle
                        43878, //Grand Vizier Ertan
                        43873, //Altairus
                        43875, //Asaad

                        //Grim Batol
                        39625, //General Umbriss
                        40177, //Forgemaster Throngus
                        40319, //Drahga Shadowburner
                        40484, //Erudax

                        //Halls of Origination
                        39425, //Temple Guardian Anhuur
                        39428, //Earthrager Ptah
                        39788, //Anraphet
                        39587, //Isiset
                        39731, //Ammunae
                        39732, //Setesh
                        39378, //Rajh

                        //Lost City of the Tolvir
                        44577, //General Husam
                        43612, //High Prophet Barim
                        43614, //Lockmaw
                        49045, //Augh
                        44819, //Siamat

                        //Baradin Hold
                        47120, //Argaloth

                        //Blackrock Mountain: Blackwing Descent
                        41570, //Magmaw
                        42180, //Toxitron
                        41378, //Maloriak
                        41442, //Atramedes
                        43296, //Chimaeron
                        41376, //Nefarian

                        //Throne of the Four Winds
                        45871, //Nezir
                        46753, //Al'Akir

                        //The Bastion of Twilight
                        45992, //Valiona
                        45993, //Theralion
                        44600, //Halfus Wyrmbreaker
                        43735, //Elementium Monstrosity
                        43324, //Cho'gall
                        45213, //Sinestra (heroic)

                        //World Dragons
                        14889, //Emeriss
                        14888, //Lethon
                        14890, //Taerar
                        14887, //Ysondre

                        //Azshara
                        14464, //Avalanchion
                        6109, //Azuregos

                        //Un'Goro Crater
                        14461, //Baron Charr

                        //Silithus
                        15205, //Baron Kazum <Abyssal High Council>
                        15204, //High Marshal Whirlaxis <Abyssal High Council>
                        15305, //Lord Skwol <Abyssal High Council>
                        15203, //Prince Skaldrenox <Abyssal High Council>
                        14454, //The Windreaver

                        //Searing Gorge
                        9026, //Overmaster Pyron

                        //Winterspring
                        14457, //Princess Tempestria

                        //Hellfire Peninsula
                        18728, //Doom Lord Kazzak
                        12397, //Lord Kazzak

                        //Shadowmoon Valley
                        17711, //Doomwalker

                        //Nagrand
                        18398, //Brokentoe
                        18069, //Mogor <Hero of the Warmaul>, friendly
                        18399, //Murkblood Twin
                        18400, //Rokdar the Sundered Lord
                        18401, //Skra'gath
                        18402 //Warmaul Champion
                    };
        #endregion

        public static int HealingWeight(WoWPlayer unit)
        {
            WoWPlayer tank = RAF.PartyTankRole.ToPlayer();
            bool isCaster = (unit.Class == WoWClass.Mage || unit.Class == WoWClass.Priest || unit.Class == WoWClass.Warlock || unit.Class == WoWClass.Shaman || unit.Class == WoWClass.Druid && unit.Shapeshift == ShapeshiftForm.Normal);
            bool isHealer = (unit.Class == WoWClass.Priest && !unit.Auras.ContainsKey("Shadow Form") || unit.Class == WoWClass.Shaman || unit.Class == WoWClass.Paladin || unit.Class == WoWClass.Druid && (unit.Shapeshift != ShapeshiftForm.Cat || unit.Shapeshift != ShapeshiftForm.Bear));
            int weight = 0;                                   
            const int healthMultiplier = 11;
            const int debuffMultiplier = -5;
            const int tankMultiplier = -50;
            const int addMultiplier = -75;
            const int casterMultiplier = -155;
            const int healerMultiplier = -200;
            const int rejuvenationMultiplier = 44;
            const int regrowthMultiplier = 10;

            weight += (tank != null && tank.Guid == unit.Guid) ? tankMultiplier : 0;
            weight += (int)unit.HealthPercent * healthMultiplier;

            int countOfAdds = 0;
            foreach (WoWUnit u in ObjectManager.GetObjectsOfType<WoWUnit>())
            {
                if (u.IsPlayer) continue;
                if (!u.Combat) continue;
                if (u.Distance > 80) continue;
                if (u.CurrentTargetGuid != unit.Guid) continue;
                countOfAdds += 1;
            }
            weight += isCaster ? (countOfAdds * casterMultiplier) : countOfAdds * addMultiplier;
            weight += (Utils.IsBattleground && isHealer) ? healerMultiplier : 0;
            //weight += countOfAdds * addMultiplier;

            int countOfDoTs = 0;
            foreach (KeyValuePair<string, WoWAura> aura in unit.Auras)
            {
                if (aura.Key == "Rejuvenation") weight += rejuvenationMultiplier;
                if (aura.Key == "Regrowth") weight += regrowthMultiplier;

                if (!aura.Value.IsHarmful) continue;
                if (aura.Value.Spell.DispelType == WoWDispelType.None) continue;
                //countOfDoTs += 1;
                weight += debuffMultiplier;
            }
            weight += countOfDoTs * debuffMultiplier;


            return weight;
        }

        public static WoWUnit BestHealTarget(out int healingWeight, int minHealth)
        {
            Dictionary<WoWPlayer, int> targets = new Dictionary<WoWPlayer, int>();
            List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers; players.Add(Me);

            foreach (WoWPlayer player in players)
            {
                if (player.HealthPercent > minHealth) continue;
                if (player.Dead) continue;
                if (player.IsGhost) continue;

                targets.Add(player, HealingWeight(player));
            }

            if (targets.Count <= 0) { healingWeight = 0; return null; }

            // Order by values. Use LINQ to specify sorting by value.
            var items = from k in targets.Keys orderby targets[k] ascending select k;

            // Display results.
            //Utils.Log("------------------------------");
            //foreach (WoWPlayer k in items) { Utils.Log(string.Format("*** {0}, scored {1}", k.Name, targets[k])); }
            //Utils.Log("------------------------------");

            WoWPlayer bestTarget = items.FirstOrDefault();

            healingWeight = targets[bestTarget];
            return bestTarget;
        }


        public static int TargetWeight(WoWUnit unit)
        {
            int weight = 0;                                                 // Base target weight (0)
            const int healthMultiplier = 11;                                // The lower their health the better
            const int levelMultiplier = 50;                                 // Prioritise lower level mobs
            const int playerMultiplier = -900;                               // Players are dealt with FIRST!
            const int petMultiplier = 1000;                                 // Pets are much lower priority
            const int fleeingMultiplier = 700;                              // Ignore the fleeing target and deal with something else
            const int ccMultiplier = 500;                                   // Ignore CC'd targets and deal with something else
            const int priorityMultiplier = -200;                             // Priority targets are dealt with quickly
            const int totemMultiplier = -500;                               // Totem killer, but if in PVE
            const int distanceMultiplier = 3;                               // Different weight for mobs further away - should iron out some bugs with mobs having the same weight
            const int npcCasterMultiplier = -15;                            // Prefer casters over melee targets. Typically they do more damage but can be killed faster

            bool ccTarget = false;
            foreach (string crowdControlSpell in CrowdControlSpellsList)
            {
                string spell = crowdControlSpell;
                ccTarget = unit.Auras.Any(aura => aura.Key == spell);
            }

            if (ccTarget) weight += ccMultiplier;

            if (PriorityMobs.Any(mobEntryID => unit.Entry == mobEntryID)) { weight += priorityMultiplier; }


            weight += unit.IsPlayer ? playerMultiplier : (unit.Class == WoWClass.Warrior ? 0 : npcCasterMultiplier);     // If the target is a player we REALLY want to kill them first!
            weight += unit.IsPet ? (unit.OwnedByUnit.IsPlayer ? petMultiplier : 0) : 0;                                 // If the target is a pet then find something better
            weight += (int)unit.HealthPercent * healthMultiplier;                                                       // The lower their health percent the higher the priority - quick kills = less mobs attacking us
            weight += (unit.Level - Me.Level) * levelMultiplier;                                                        // The lower their level the higher the priority - quick kills = less mobs attacking us
            weight += (unit.IsTotem && (Utils.IsBattleground || Me.IsInInstance || Me.IsInRaid)) ? 0 : totemMultiplier; // Non-player totems should be killed
            weight += (int) unit.Distance*distanceMultiplier;
            weight += unit.Fleeing ? fleeingMultiplier : 0;                                                                 // fleeing targets should be left to flee??

            return weight;
        }
        public static WoWUnit BestTarget(out int targetWeight)
        {
            Dictionary<WoWUnit, int> targets = new Dictionary<WoWUnit, int>();

            foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>(false, false))
            {
                if (!unit.Combat) continue;
                if (!unit.IsTargetingMeOrPet && !unit.IsTargetingMyPartyMember && !unit.IsTargetingMyRaidMember)
                    continue;

                targets.Add(unit, TargetWeight(unit));
            }


            // Order by values. Use LINQ to specify sorting by value.
            var items = from k in targets.Keys orderby targets[k] ascending select k;

            // Display results.
            /*
                Utils.Log("------------------------------");
                foreach (WoWUnit k in items) { Utils.Log(string.Format("*** {0}, scored {1}", k.Name, targets[k])); }
                Utils.Log("------------------------------");
                 */

            WoWUnit bestTarget = items.FirstOrDefault();

            targetWeight = targets[bestTarget];
            return bestTarget;
        }

        /// <summary>
        /// Check if any of the buff/debuff IDs are on the unit
        /// </summary>
        /// <param name="unit">wowunit to be checked for buff/debuff ID</param>
        /// <param name="auraIDs">list of buff/debuff IDs to check</param>
        /// <returns>TRUE if any of the buff/debuff IDs are present</returns>
        public static bool IsBuffOnUnit(WoWUnit unit,  List<int> auraIDs)
        {
            if (unit == null) return false;

            return unit.Auras.Any(aura => auraIDs.Any(auraID => aura.Value.SpellId == auraID));
        }

        public static bool IsBuffOnUnit(WoWUnit unit, int auraID)
        {
            if (unit == null) return false;

            return unit.Auras.Any(aura => aura.Value.SpellId == auraID);
        }

        public static bool IsBuffOnUnit(WoWUnit unit, List<string> auras)
        {
            if (unit == null) return false;

            return (from unitAura in unit.Auras from aura in auras where unitAura.Key.ToUpper() == aura.ToUpper() select unitAura).Any();
        }


        /// <summary>
        /// Performs common combat checking: is on GCD, Me.Casting, Me.Dead, Me.IsGhost
        /// </summary>
        /// <param name="spellName"></param>
        /// <param name="skipTargetCheck"></param>
        /// <returns>TRUE if its safe to continue, FALSE is something failed</returns>
        public static bool CombatCheckOk(string spellName, bool skipTargetCheck)
        {
            if (Spell.IsGCD || Me.IsGhost || Me.IsCasting || Me.Dead) return false;

            // You want to skip the target check if you are healing or casting a spell that does not require a target
            if (!skipTargetCheck && (!Me.GotTarget || CT.Dead)) return false;

            // if we're not checking a spell then all is good
            if (string.IsNullOrEmpty(spellName)) return true;

            // check if we know the spell and its not on cooldown
            if (!Spell.IsKnown(spellName) || Spell.IsOnCooldown(spellName)) return false;

            int powerCost = Spell.PowerCost(spellName);
            if (SpellManager.Spells[spellName].PowerType == WoWPowerType.Mana && Me.CurrentMana < powerCost) return false;
            if (SpellManager.Spells[spellName].PowerType == WoWPowerType.Rage && Me.CurrentRage < powerCost) return false;
            if (SpellManager.Spells[spellName].PowerType == WoWPowerType.Energy && Me.CurrentEnergy < powerCost) return false;
            if (SpellManager.Spells[spellName].PowerType == WoWPowerType.Focus && Me.CurrentFocus < powerCost) return false;

            // all is good lets continue)
            return true;
        }

        /// <summary>
        /// Pass multiple parameters to perform logic checks
        /// </summary>
        /// <param name="spellName">Name of the spell / buff / debuff to be cast</param>
        /// <param name="args">CLC:setting, MIN_POWERPERCENT:value, MAX_POWERPERCENT:value, MIN_HEALTHPERCENT:value, "MAX_HEALTHPERCENT:value, TARGET_ISCASTING, TARGET_ISTARGETINGME, DEBUFF_NAME:debuffname, DEBUFF_ID:spellid, IGNORE_CANCAST, IGNORE_POWERCOST, BUFF_ID:value, BUFF_NAME:buffname, LUACHECK_PRESENT:buffname, LUACHECK_NOTPRESENT:buffname, DEBUG:message|messages|'debugon', GOT_ALIVEPET, IGNORE_DISTANCE</param>
        /// <returns></returns>
        public static bool CCheck(string spellName, params object[] args)
        {
            bool ignoreCanCast = false;
            bool ignorePowerCost = false;
            bool ignoreDistance = false;
            bool debug = false;
            string debugMessageStart = "";
            string debugMessageEnd = "";
            WoWUnit currentTarget = null;
            List<string> keywords = new List<string>
                                        {
                                            "MAX_POWERPERCENT:",
                                            "MIN_POWERPERCENT:",
                                            "MIN_HEALTHPERCENT",
                                            "MAX_HEALTHPERCENT",
                                            "SELF_IS:",
                                            "TARGET_IS:",           // Caster, Casting, Fleeing, Mounted, Flying, Player, LowLevel, HighLevel, WithinInteractRange, Pet, LOS, Silenced, Stunned, Elite
                                            "TARGET_ISNOT:",        // ... same as above
                                            "TARGET_ISCASTING",
                                            "TARGET_ISTARGETINGME",
                                            "DEBUFF_NAME:",         // On the target
                                            "DEBUFF_ID:",           // On the target
                                            "IGNORE_CANCAST",
                                            "IGNORE_POWERCOST",
                                            "IGNORE_DISTANCE",
                                            "BUFF_ID:",             // On me
                                            "BUFF_NAME:",           // On me
                                            "LUACHECK_PRESENT",
                                            "LUACHECK_NOTPRESENT",
                                            "GOT_ALIVEPET",
                                            "CLC:",
                                            "DEBUG:",
                                            "TIMEREXPIRED:"

                                        };


            if (string.IsNullOrEmpty(spellName)) return false;
            if (Self.Immobilised) return false;
            if (Spell.IsGCD || Me.IsGhost || Me.IsCasting || Me.Dead) return false;

            // Enumerate all our parameters. Hopefully our first one if our current target (WoWUnit).
            foreach (var obj in args)
            {
                if (obj is WoWUnit) currentTarget = (WoWUnit)obj;
                if (obj is string)
                {
                    string workingString = (string)obj;
                    string debugMessage = "";
                    string clcSetting = "";
                    string buffName = "";
                    string debuffName = "";
                    int debuffID = 0;
                    int minPowerPercent = 0;
                    int maxPowerPercent = 0;
                    bool keyWordFound = keywords.Any(keyword => workingString.ToUpper().StartsWith(keyword));

                    if (!keyWordFound)
                    {
                        Utils.Log("***********************************************", Utils.Colour("Red"));
                        Utils.Log("*              Keyword not found               *", Utils.Colour("Red"));
                        Utils.Log("***********************************************", Utils.Colour("Red"));
                        Utils.Log(" '" + workingString + "' does not contain any keywords", Utils.Colour("Red"));
                        return false;
                    }


                    // Make sure we have a valid unit in currentTarget. This causes problems further down if we don't.
                    // If a target has not been passed, then take our current target.
                    if (currentTarget == null && Me.GotTarget) currentTarget = Me.CurrentTarget;

                    // Debug messaging setup. Turn on 'debug' if needed
                    if (workingString.ToUpper().StartsWith("DEBUG:"))
                    {
                        debugMessage = workingString.Substring(6);
                        string[] debugmessages = debugMessage.Split('|');
                        if (debugmessages.Length > 0) { debugMessageStart = debugmessages[0]; debugMessageEnd = debugmessages[1]; Utils.Log(debugMessageStart.ToUpper().Trim() + " [" + spellName + "]", Utils.Colour("Red")); }
                        else
                        {
                            Utils.Log(debugMessage.ToUpper().Trim() + " [" + spellName + "]", Utils.Colour("Red"));
                        }
                        foreach (string message in debugmessages.Where(message => message.ToUpper().Trim() == "DEBUGON"))
                        {
                            debug = true;
                        }
                    }

                    // Check if we know the spell and its on cooldown. No point continuing if it is on cooldown
                    if (debug) Utils.Log("** DEBUG: --> " + "IsKnown and Cooldown", Color.SlateGray);
                    if (Spell.IsKnown(spellName) && Spell.IsOnCooldown(spellName)) return false;
                    if (!Spell.IsKnown(spellName)) return false;

                    // CLC settings. Must have a valid CLC setting string passed
                    if (debug) Utils.Log("** DEBUG: --> " + "CLC", Color.SlateGray);
                    if (workingString.ToUpper().StartsWith("CLC:")) { clcSetting = workingString.Substring(workingString.IndexOf(":") + 1); if (!CLC.ResultOK(clcSetting)) return false; }


                    // Power percent check. Check for both MIN and MAX values - BETA
                    if (debug) Utils.Log("** DEBUG: --> " + "POWERPERCENT", Color.SlateGray);
                    {
                        if (workingString.ToUpper().Contains("POWERPERCENT:"))
                        {
                            int powerPercent = Convert.ToInt32(workingString.Substring(workingString.IndexOf(":") + 1));
                            if (workingString.ToUpper().StartsWith("MIN_"))
                            {
                                if (Me.PowerPercent < powerPercent) return false;
                            }
                            else if (workingString.ToUpper().StartsWith("MAX_"))
                            {
                                if (Me.PowerPercent > powerPercent) return false;
                            }
                        }
                    }

                    // Health percent check. Check for both MIN and MAX values - BETA
                    if (debug) Utils.Log("** DEBUG: --> " + "HEALTHPERCENT", Color.SlateGray);
                    {
                        if (workingString.ToUpper().Contains("HEALTHPERCENT:"))
                        {
                            int healthPercent = Convert.ToInt32(workingString.Substring(workingString.IndexOf(":") + 1));
                            if (workingString.ToUpper().StartsWith("MIN_"))
                            {
                                if (Me.HealthPercent < healthPercent) return false;
                            }
                            else if (workingString.ToUpper().StartsWith("MAX_"))
                            {
                                if (Me.HealthPercent > healthPercent) return false;
                            }
                        }
                    }

                    // Timer expired
                    if (debug) Utils.Log("** DEBUG: --> " + "TIMEREXPIRED", Color.SlateGray);
                    {
                        if (workingString.ToUpper().Contains("TIMEREXPIRED:"))
                        {
                            string[] values = (workingString.Substring(workingString.IndexOf(":") + 1)).Split('|');
                            string timerName = values[0];
                            int timerNumber = Convert.ToInt32(values[1]);

                            if (!Timers.Expired(timerName, timerNumber)) return false;
                        }
                    }


                    // Self is...
                    if (debug) Utils.Log("** DEBUG: --> " + "SELF_IS:...", Color.SlateGray);
                    if (workingString.ToUpper().StartsWith("SELF_IS:") || workingString.ToUpper().StartsWith("SELF_ISNOT:"))
                    {
                        bool isNot = workingString.ToUpper().Contains("_ISNOT:");
                        string action = workingString.Substring(workingString.IndexOf(":") + 1).ToUpper();

                        switch (action)
                        {
                            case "STUNNED":
                                if (isNot && Me.Stunned) return false;
                                if (!Me.Stunned) return false;
                                break;

                            case "MOVING":
                                if (isNot && Me.IsMoving) return false;
                                if (!Me.IsMoving) return false;
                                break;

                            case "MOUNTED":
                                if (isNot && Me.Mounted) return false;
                                if (!Me.Mounted) return false;
                                break;

                            case "FLYING":
                                if (isNot && Me.IsFlying) return false;
                                if (!Me.IsFlying) return false;
                                break;

                            case "CASTING":
                                if (isNot && Me.IsCasting) return false;
                                if (!Me.IsCasting) return false;
                                break;

                            case "ININSTANCE":
                                if (isNot && Me.IsInInstance) return false;
                                if (!Me.IsInInstance) return false;
                                break;

                            case "INRAID":
                                if (isNot && Me.IsInRaid) return false;
                                if (!Me.IsInRaid) return false;
                                break;

                            case "INPARTY":
                                if (isNot && Me.IsInParty) return false;
                                if (!Me.IsInParty) return false;
                                break;

                                

                            case "INBATTLEGROUND":
                            case "INBATTLEGROUNDS":
                            case "BATTLEGROUND":
                            case "BATTLEGROUNDS":
                                if (isNot && Utils.IsBattleground) return false;
                                if (!Utils.IsBattleground) return false;
                                break;

                            default:
                                // Health percent is above
                                if (action.ToUpper().StartsWith("HEALTHPERCENTABOVE|"))
                                {
                                    string[] values = workingString.Substring(workingString.IndexOf(":") + 1).Split('|');
                                    bool result = false;
                                    try
                                    {
                                        int healthPercent = Convert.ToInt32(values[1]);
                                        if (isNot && Me.HealthPercent < healthPercent) { result = true; break; }
                                        if (Me.HealthPercent > healthPercent) { result = true; break; }
                                    }
                                    catch (Exception) { Utils.Log("****************************** OH CRAP. SOMETHING WENT BOOM");  }

                                    if (!result) return false;
                                }

                                // Health percent is below
                                if (action.ToUpper().StartsWith("HEALTHPERCENTBELOW|"))
                                {
                                    string[] values = workingString.Substring(workingString.IndexOf(":") + 1).Split('|');
                                    bool result = false;
                                    try
                                    {
                                        int healthPercent = Convert.ToInt32(values[1]);
                                        if (isNot && Me.HealthPercent > healthPercent) { result = true; break; }
                                        if (Me.HealthPercent < healthPercent) { result = true; break; }
                                    }
                                    catch (Exception) { Utils.Log("****************************** OH CRAP. SOMETHING WENT BOOM"); }

                                    if (!result) return false;
                                }

                                // Spell known
                                if (action.ToUpper().StartsWith("SPELLKNOWN|"))
                                {
                                    string[] values = workingString.Substring(workingString.IndexOf(":") + 1).Split('|');
                                    bool result = false;

                                    foreach (string value in values.Where(value => value.ToUpper() != "SPELLKNOWN"))
                                    {
                                        if (isNot && !Spell.IsKnown(value)) {result = true; break; }
                                        if (!Spell.IsKnown(value)) continue;
                                        result = true; break;
                                    }

                                    if (!result) return false;
                                }

                                // Spell on cooldown
                                if (action.ToUpper().StartsWith("SPELLCOOLDOWN|"))
                                {
                                    string[] values = workingString.Substring(workingString.IndexOf(":") + 1).Split('|');
                                    bool result = false;

                                    foreach (string value in values.Where(value => value.ToUpper() != "SPELLCOOLDOWN"))
                                    {
                                        if (!Spell.IsKnown(value)) return false;
                                        if (isNot && !Spell.IsOnCooldown(value)) { result = true; break; }
                                        if (!Spell.IsOnCooldown(value)) continue;
                                        result = true; break;
                                    }

                                    if (!result) return false;
                                }

                                // Buff name (string)
                                if (action.ToUpper().StartsWith("BUFFED|"))
                                {
                                    string[] values = workingString.Substring(workingString.IndexOf(":") + 1).Split('|');
                                    bool result = false;

                                    foreach (string value in values.Where(value => value.ToUpper() != "BUFFED"))
                                    {
                                        //if (isNot && Me.Auras.ContainsKey(value)) { return false; } else result = true;
                                        if (isNot && !Me.Auras.ContainsKey(value)) { result = true; break; }
                                        if (!Me.Auras.ContainsKey(value)) continue;
                                        result = true; break;
                                    }

                                    if (!result) return false;
                                }

                                // Buff ID (int)
                                if (action.ToUpper().StartsWith("BUFFEDID|"))
                                {
                                    string[] values = workingString.Substring(workingString.IndexOf(":") + 1).Split('|');
                                    bool result = false;

                                    foreach (string value in values.Where(value => value.ToUpper() != "BUFFEDID"))
                                    {
                                        try
                                        {
                                            int spellID = Convert.ToInt32(value);
                                            foreach (KeyValuePair<string, WoWAura> aura in Me.Auras)
                                            {
                                                if (isNot && aura.Value.SpellId == spellID) { return false; }
                                                if (aura.Value.SpellId != spellID) continue;
                                                result = true; break;
                                            }
                                        }
                                        catch (Exception) { Utils.Log("****************************** OH CRAP. SOMETHING WENT BOOM"); }
                                    }

                                    if (!result) return false;
                                }
                                break;
                        }
                    }



                    // Target is....
                    if (debug) Utils.Log("** DEBUG: --> " + "TARGET_IS:...", Color.SlateGray);
                    if (workingString.ToUpper().StartsWith("TARGET_IS:") || workingString.ToUpper().StartsWith("TARGET_ISNOT:"))
                    {
                        if (!Me.GotTarget) return false;
                        bool isNot = workingString.ToUpper().Contains("_ISNOT:");
                        string action = workingString.Substring(workingString.IndexOf(":") + 1).ToUpper();

                        switch (action)
                        {
                            case "FLEEING":
                                if (isNot && Target.IsFleeing) return false;
                                if (!Target.IsFleeing) return false;
                                break;

                            case "CASTING":
                                if (isNot && Target.IsCasting) return false;
                                if (!Target.IsCasting) return false;
                                break;

                            case "CASTER":
                                if (isNot && Target.IsCaster) return false;
                                if (!Target.IsCaster) return false;
                                break;

                            case "PLAYER":
                                if (isNot && Target.IsPlayer) return false;
                                if (!Target.IsPlayer) return false;
                                break;

                            case "ELITE":
                                if (isNot && Target.IsElite) return false;
                                if (!Target.IsElite) return false;
                                break;

                            case "LOWLEVEL":
                                if (isNot && Target.IsLowLevel) return false;
                                if (Target.IsLowLevel) return false;
                                break;

                            case "WITHININTERACTRANGE":
                                if (isNot && Target.IsWithinInteractRange) return false;
                                if (!Target.IsWithinInteractRange) return false;
                                break;

                            case "HIGHLEVEL":
                                if (isNot && Target.IsHighLevel) return false;
                                if (!Target.IsHighLevel) return false;
                                break;

                            case "PET":
                                if (isNot && Me.CurrentTarget.IsPet) return false;
                                if (!Me.CurrentTarget.IsPet) return false;
                                break;

                            case "LOS":
                                if (isNot && Me.CurrentTarget.InLineOfSight) return false;
                                if (!Me.CurrentTarget.InLineOfSight) return false;
                                break;

                            case "SILENCED":
                                if (isNot && Me.CurrentTarget.Silenced) return false;
                                if (!Me.CurrentTarget.Silenced) return false;
                                break;

                            case "STUNNED":
                                if (isNot && Me.CurrentTarget.Stunned) return false;
                                if (!Me.CurrentTarget.Stunned) return false;
                                break;

                            case "MOVING":
                                if (isNot && Me.CurrentTarget.IsMoving) return false;
                                if (!Me.CurrentTarget.IsMoving) return false;
                                break;

                            case "MOUNTED":
                                if (isNot && Me.CurrentTarget.Mounted) return false;
                                if (!Me.CurrentTarget.Mounted) return false;
                                break;

                            case "FLYING":
                                if (isNot && Me.CurrentTarget.IsFlying) return false;
                                if (!Me.CurrentTarget.IsFlying) return false;
                                break;

                            case "TARGETINGME":
                                if (isNot && Me.CurrentTarget.CurrentTargetGuid == Me.Guid) return false;
                                if (Me.CurrentTarget.CurrentTargetGuid != Me.Guid) return false;
                                break;

                            case "TARGETINGMYPET":
                                if (!Me.GotAlivePet) return false;
                                if (isNot && Me.CurrentTarget.CurrentTargetGuid == Me.Pet.Guid) return false;
                                if (Me.CurrentTarget.CurrentTargetGuid != Me.Pet.Guid) return false;
                                break;

                            default:
                                if (action.ToUpper().StartsWith("DEBUFFED"))
                                {
                                    if (!Me.GotTarget) return false;
                                    string[] values = workingString.Substring(workingString.IndexOf(":") + 1).Split('|');
                                    bool result = false;

                                    foreach (string value in values.Where(value => value.ToUpper() != "DEBUFFED"))
                                    {
                                        if (isNot && Me.CurrentTarget.Auras.ContainsKey(value)) { return false; } else result = true;
                                        if (Me.CurrentTarget.Auras.ContainsKey(value) && Me.CurrentTarget.Auras[value].CreatorGuid == Me.Guid)
                                        {
                                            result = true;
                                            break;
                                        }
                                    }

                                    if (!result) return false;
                                }
                                break;
                        }
                    }


                    // Target is casting. Only continue if the target IS casting a spell.
                    if (debug) Utils.Log("** DEBUG: --> " + "Target_ISCASTING", Color.SlateGray);
                    if (workingString.ToUpper().StartsWith("TARGET_ISCASTING"))
                    {
                        if (currentTarget != null && !currentTarget.IsCasting) return false;
                    }

                    // Target is Targeting me. Only if they are targeting me - not our pet, not a party member but ME.
                    if (debug) Utils.Log("** DEBUG: --> " + "Target_ISTARGETINGME", Color.SlateGray);
                    if (workingString.ToUpper().StartsWith("TARGET_ISTARGETINGME"))
                    {
                        if (currentTarget != null && currentTarget.CurrentTargetGuid != Me.Guid) return false;
                    }


                    // Check the target for a debuff
                    if (debug) Utils.Log("** DEBUG: --> " + "DEBUFF_", Color.SlateGray);
                    if ((workingString.ToUpper().StartsWith("DEBUFF_")) && currentTarget != null)
                    {
                        if (workingString.ToUpper().Contains("_ID:"))
                        {
                            debuffID = Convert.ToInt32(workingString.Substring(workingString.IndexOf(":") + 1));
                            foreach (KeyValuePair<string, WoWAura> aura in currentTarget.Auras)
                            {
                                if (aura.Value.SpellId != debuffID) continue;
                                if (aura.Value.SpellId == debuffID && aura.Value.CreatorGuid == Me.Guid)
                                {
                                    if (aura.Value.StackCount >= SpellManager.Spells[spellName].MaxStackCount)
                                        return false;
                                }
                            }

                        }
                        else if (workingString.ToUpper().Contains("_NAME:"))
                        {
                            debuffName = workingString.Substring(workingString.IndexOf(":") + 1);
                            foreach (KeyValuePair<string, WoWAura> aura in currentTarget.Auras)
                            {
                                if (aura.Key.ToUpper() != debuffName.ToUpper()) continue;
                                if (aura.Key.ToUpper() == debuffName.ToUpper() && aura.Value.CreatorGuid == Me.Guid)
                                {
                                    if (aura.Value.StackCount >= SpellManager.Spells[debuffName].MaxStackCount)
                                        return false;
                                }
                            }

                        }
                    }


                    // Check yourself for a buff. Either by ID or by NAME
                    if (debug) Utils.Log("** DEBUG: --> " + "BUFF_", Color.SlateGray);
                    if ((workingString.ToUpper().StartsWith("BUFF_")) && currentTarget != null)
                    {
                        if (workingString.ToUpper().Contains("_ID:"))
                        {
                            int buffID = Convert.ToInt32(workingString.Substring(workingString.IndexOf(":") + 1));
                            bool buffIDFound = Me.Auras.Any(aura => aura.Value.SpellId == buffID);

                            if (!buffIDFound) return false;
                        }
                        else if (workingString.ToUpper().Contains("_NAME:"))
                        {
                            string buff = workingString.Substring(workingString.IndexOf(":") + 1);
                            bool buffFound = Me.Auras.Where(aura => aura.Key.ToUpper() == buff.ToUpper()).Any(aura => aura.Key.ToUpper() == buff.ToUpper() && aura.Value.CreatorGuid == Me.Guid);

                            if (!buffFound) return false;
                        }

                    }


                    // Pet Check. Do we have a valid pet, do we even need a pet?
                    if (debug) Utils.Log("** DEBUG: --> " + "GOT_ALIVEPET", Color.SlateGray);
                    if (workingString.ToUpper().Contains("GOT_ALIVEPET")) { if (!Me.GotAlivePet) return false; }

                    // 'CanCast'. Can we skip this check? Some spells always seem to return false even though we can cast them
                    if (debug) Utils.Log("** DEBUG: --> " + "IGNORE_CANCAST", Color.SlateGray);
                    if (workingString.ToUpper().Contains("IGNORE_CANCAST")) ignoreCanCast = true;

                    // Power Cost. Should be skip checking for the spell power cost?
                    if (debug) Utils.Log("** DEBUG: --> " + "IGNORE_POWERCOST", Color.SlateGray);
                    if (workingString.ToUpper().Contains("IGNORE_POWERCOST")) ignorePowerCost = true;

                    // Target distance. Should we ignore the max spell distance?
                    if (debug) Utils.Log("** DEBUG: --> " + "IGNORE_DISTANCE", Color.SlateGray);
                    if (workingString.ToUpper().Contains("IGNORE_DISTANCE")) ignoreDistance = true;

                    // LUA Self buff check. Returns FALSE if the buff is NOT present. We need to have this buff in order to continue.
                    if (debug) Utils.Log("** DEBUG: --> " + "LUACHECK_PRESENT", Color.SlateGray);
                    if (workingString.ToUpper().StartsWith("LUACHECK_PRESENT:")) { buffName = workingString.Substring(workingString.IndexOf(":") + 1); if (!Self.IsBuffOnMeLUA(buffName)) return false; }

                    // LUA Self buff check. Returns FALSE if the buff IS present. We can't have this buff on us, if we do then drop out now.
                    if (debug) Utils.Log("** DEBUG: --> " + "LUACHECK_NOTPRESENT", Color.SlateGray);
                    if (workingString.ToUpper().StartsWith("LUACHECK_NOTPRESENT:")) { buffName = workingString.Substring(workingString.IndexOf(":") + 1); if (Self.IsBuffOnMeLUA(buffName)) return false; }
                }
            }

            // Target distance. Should we ignore the max spell distance?
            if (debug) Utils.Log("** DEBUG: --> " + "ignoreDistance", Color.SlateGray);
            if (currentTarget != null && !ignoreDistance)
            {
                if (SpellManager.Spells[spellName].HasRange && SpellManager.Spells[spellName].MaxRange > 0)
                {
                    if (currentTarget.Distance > SpellManager.Spells[spellName].MaxRange)
                    {
                        if (debug) Utils.Log("** DEBUG: --> " + "distanceCheck FAILED", Color.Red);
                        if (debug) Utils.Log(string.Format("** {0} max distance is {1}, target distance is {2}", spellName, SpellManager.Spells[spellName].MaxRange,currentTarget.Distance));
                        return false;
                    }
                }
                else
                {
                    if (debug) Utils.Log(string.Format("** DEBUG: {0} has no max range. Target distance is {1}",spellName, currentTarget.Distance));
                    //if (debug) Utils.Log(string.Format("** {0} max distance is {1}, target distance is {2}", spellName, SpellManager.Spells[spellName].MaxRange, currentTarget.Distance));
                }
            }

            // Power Cost. Should be skip checking for the spell power cost?
            if (debug) Utils.Log("** DEBUG: --> " + "ignorePowerCost", Color.SlateGray);
            if (!ignorePowerCost) if (SpellManager.Spells[spellName].PowerCost > Me.CurrentPower) return false;

            // Either check for (spell is known and not on cooldown) or check for (spell.cancast). 
            if (debug) Utils.Log("** DEBUG: --> " + "ignoreCanCast", Color.SlateGray);
            if (ignoreCanCast) { if (!Spell.IsKnown(spellName) || Spell.IsOnCooldown(spellName)) return false; } else { if (!Spell.CanCast(spellName)) return false; }
            if (!string.IsNullOrEmpty(debugMessageEnd)) Utils.Log(debugMessageEnd.ToUpper().Trim() + "[" + spellName + "]",Utils.Colour("Red"));

            return true;
        }

        // Credit to CNG for this snipit of code
        public static bool IsNotWanding
        {
            get
            {
                if (Lua.GetReturnVal<int>("return IsAutoRepeatSpell(\"Shoot\")", 0) == 1) { return false; }
                if (Lua.GetReturnVal<int>("return HasWandEquipped()", 0) == 0) { return false; }
                return true;
            }
        }

        public static bool AddsInstance
        {
            get
            {
                List<WoWUnit> hlist =
                    (from o in ObjectManager.ObjectList
                     where o is WoWUnit
                     let p = o.ToUnit()
                     where p.Distance2D < 60
                           && !p.Dead
                           && p.Combat
                           && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet)
                           && p.IsHostile
                           && p.Attackable
                     select p).ToList();

                return hlist.Count > 1;
            }
        }

        public static int CountOfMobsAttackingPlayer(ulong playerGUID)
        {
            List<WoWUnit> hlist =
                (from o in ObjectManager.ObjectList
                 where o is WoWUnit
                 let p = o.ToUnit()
                 where p.Distance2D < 60
                       && !p.Dead
                       && p.Combat
                       && p.CurrentTargetGuid == playerGUID
                       && p.IsHostile
                       && p.Attackable
                 select p).ToList();

            return hlist.Count;
        }

        public static bool CanAoEInstance
        {
            get
            {
                if (RaFHelper.Leader == null) return false;

                List<WoWUnit> hlist =
                    (from o in ObjectManager.ObjectList
                     where o is WoWUnit
                     let p = o.ToUnit()
                     where p.Distance2D < 60
                           && !p.Dead
                           && p.Combat
                           && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet)
                           && p.IsHostile
                           && p.Attackable
                     select p).ToList();

                int countNearTank = hlist.Where(u => u.HealthPercent >= 25).Count(u => RaFHelper.Leader.Location.Distance(u.Location) < 15);

                // If you have 3+ adds then AoE
                return countNearTank > 2;
            }
        }

        public static int CountOfAddsInRange(double distance, WoWPoint location)
        {
            List<WoWUnit> hlist =
                (from o in ObjectManager.ObjectList
                 where o is WoWUnit
                 let p = o.ToUnit()
                 where p.Distance2D < 50
                       && !p.Dead
                       && p.Combat
                       && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet)
                       && p.IsHostile
                       && p.Attackable
                 select p).ToList();

            return hlist.Count(u => location.Distance(u.Location) <= distance);
        }

        public static int AddsCount
        {
            get
            {
                List<WoWUnit> hlist =
               (from o in ObjectManager.ObjectList
                where o is WoWUnit
                let p = o.ToUnit()
                where p.Distance2D < 45
                      && !p.Dead
                    //&& p.IsTargetingMeOrPet
                      && (p.Aggro || p.PetAggro)
                      && p.Attackable
                      && p.CreatureType != WoWCreatureType.Critter
                select p).ToList();


                return hlist.Count;
                //return Targeting.Instance.TargetList.Count;
            }
        }

        /// <summary>
        /// TRUE if you have adds
        /// </summary>
        public static bool Adds
        {
            get
            {
                // I'm No longer using HB's TargetList count to do the add check as this is not producing the desired result
                // Instead using my own add check. Basically get all alive mobs attacking me or my pet
                if (!Me.IsInParty)
                {
                    List<WoWUnit> hlist =
                        (from o in ObjectManager.ObjectList
                         where o is WoWUnit
                         let p = o.ToUnit()
                         where p.Distance2D < 50
                               && !p.Dead
                               && p.IsTargetingMeOrPet // || Me.IsInInstance && p.IsTargetingMyPartyMember)
                               && p.Attackable
                         select p).ToList();


                    return hlist.Count > 1;
                }


                List<WoWUnit> hplist =
                       (from o in ObjectManager.ObjectList
                        where o is WoWUnit
                        let p = o.ToUnit()
                        where p.Distance2D < 50
                              && !p.Dead
                              && p.Combat
                              && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet)
                              && p.Attackable
                        select p).ToList();


                return hplist.Count > 1;
            }
        }


        public static void AutoAttack(bool autoAttackOn)
        {
            if (autoAttackOn) { if (Me.IsAutoAttacking) return; Lua.DoString("StartAttack()"); return; }
            Lua.DoString("StopAttack()");
        }

        // Do a simple loop while casting a spell. 
        // Required so you don't double heal 
        public static void WaitWhileCasting()
        {
            Thread.Sleep(150);
            while (Me.IsCasting) { Thread.Sleep(100); }
        }

        public static void WaitWhileCasting(CastingBreak statCheck, double breakValue, WoWUnit targetCheck)
        {
            Thread.Sleep(150);
            while (Me.IsCasting)
            {
                Thread.Sleep(100);
                switch (statCheck)
                {
                    case CastingBreak.None:
                        Thread.Sleep(1);
                        break;

                    case CastingBreak.HealthIsAbove:
                        if (targetCheck.HealthPercent > breakValue)
                        {
                            Spell.StopCasting();
                            return;
                        }
                        break;

                    case CastingBreak.HealthIsBelow:
                        if (targetCheck.HealthPercent < breakValue)
                        {
                            Spell.StopCasting();
                            return;
                        }
                        break;

                    case CastingBreak.PowerIsAbove:
                        if (targetCheck.PowerPercent > breakValue)
                        {
                            Spell.StopCasting();
                            return;
                        }
                        break;

                    case CastingBreak.PowerIsBelow:
                        if (targetCheck.PowerPercent < breakValue)
                        {
                            Spell.StopCasting();
                            return;
                        }
                        break;
                }
            }
        }

        // Are you in a battleground
        public static bool IsBattleground { get { return Battlegrounds.IsInsideBattleground; } }

        /// <summary>
        /// Return a WoWUnit type of a player in your party/raid in need of healing. Null if noone is in need of healing
        /// </summary>
        /// <param name="minimumHealth">The health a player must be to be considered for healing</param>
        /// <returns>WoWUnit the player most in need of healing</returns>
        public static WoWUnit PlayerNeedsHealing(double minimumHealth)
        {
            // If you're not in a party then just leave
            if (!Me.IsInParty) return null;

            // MyGroup is populated with raid members or party members, whichever you are in
            List<WoWPlayer> myGroup = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;

            // Enumerate all players in myGroup and find the person with the lowest health %
            List<WoWPlayer> playersToHeal = (from o in myGroup
                                             let p = o.ToPlayer()
                                             where p.Distance < 40
                                                   && !p.Dead
                                                   && !p.IsGhost
                                                   && p.InLineOfSight
                                                   && p.HealthPercent < minimumHealth
                                             orderby p.HealthPercent ascending
                                             select p).ToList();

            // If playersToHeal is more than 0 then we have someone to heal
            // So return the first person in the list, they will be the most in need
            return playersToHeal.Count > 0 ? playersToHeal[0] : null;
        }

       
        public static bool HostileMobsInRange(double searchRange)
        {
            List<WoWUnit> hlist =
               (from o in ObjectManager.ObjectList
                where o is WoWUnit
                let p = o.ToUnit()
                where p.Distance2D < searchRange
                      && !p.Dead
                      && !p.TaggedByOther
                      && p.IsHostile
                select p).ToList();


            return hlist.Count > 0;
        }

        public static bool AllMobsAttackingPetOrOther
        {
            get { return !Targeting.Instance.TargetList.Any(u => u.CurrentTargetGuid == Me.Guid); }
        }

        public static bool AttackableMobsInRange(double searchRange)
        {
            List<WoWUnit> hlist =
               (from o in ObjectManager.ObjectList
                where o is WoWUnit
                let p = o.ToUnit()
                where p.Distance2D < searchRange
                      && !p.Dead
                      && !p.TaggedByOther
                      && p.Attackable
                select p).ToList();


            return hlist.Count > 0;
        }

        public static WoWUnit AttackableMobInRange(double searchRange)
        {
            List<WoWUnit> hlist =
                (from o in ObjectManager.ObjectList
                 where o is WoWUnit
                 let p = o.ToUnit()
                 where p.Distance2D < searchRange
                       && !p.Dead
                       && !p.TaggedByOther
                       && p.Attackable
                       && !p.IsPlayer
                       && !p.IsPet
                       && p.CreatureType != WoWCreatureType.Critter // Appears to be bugged as it selects critters
                       && p.Level > 1                               // Added a level check. All critters should be level 1. 
                       && !p.IsFlying
                 orderby p.Distance2D ascending
                 select p).ToList();


            return hlist.Where(p => Navigator.CanNavigateFully(Me.Location, p.Location, 60)).FirstOrDefault();
        }

        public static bool IsInLineOfSight(WoWPoint location)
        {
            bool result = GameWorld.IsInLineOfSight(Me.Location, location);

            return result;
        }

        public static bool IsInLineOfSight()
        {
            if (Me.IsInParty)
            {
                if (RaFHelper.Leader != null && !IsInLineOfSight(RaFHelper.Leader.Location)) return false;
            }
            else
            {
                if (Me.GotTarget && !IsInLineOfSight(CT.Location)) return false;
            }

            return true;

        }

        public static void MoveToLineOfSight()
        {
            WoWPoint location = Me.Location;
            if (Me.IsInParty && RaFHelper.Leader != null) location = RaFHelper.Leader.Location;
            if (!Me.IsInParty && Me.GotTarget) location = CT.Location;

            Movement.MoveTo(location);
            while (!GameWorld.IsInLineOfSight(Me.Location, location))
            {
                Movement.MoveTo(location);
                Thread.Sleep(250);
            }

            if (Me.IsMoving) Movement.StopMoving();
        }

        public static void MoveToLineOfSight(WoWPoint location)
        {
            //Utils.Log(string.Format("We don't have LOS on {0} moving closer...", CT.Name),System.Drawing.Color.FromName("DarkRed"));
            Movement.MoveTo(location);
            while (!GameWorld.IsInLineOfSight(Me.Location, location))
            {
                Movement.MoveTo(location);
                Thread.Sleep(250);
            }

            if (Me.IsMoving) Movement.StopMoving();
        }


    }

    public static class Timers
    {
        private static Dictionary<string, Stopwatch> _timerCollection = new Dictionary<string, Stopwatch>();

        public static void Add(string timerName)
        {
            Stopwatch stw = new Stopwatch();
            stw.Start();

            _timerCollection.Add(timerName, stw);
        }

        public static void Remove(string timerName)
        {
            _timerCollection.Remove(timerName);
        }

        public static Stopwatch Timer(string timerName)
        {
            if (!TimerExists(timerName)) return null;
            return _timerCollection[timerName];
        }

        public static bool Expired(string timerName, long maximumMilliseconds)
        {
            if (!TimerExists(timerName)) return false;
            return _timerCollection[timerName].ElapsedMilliseconds > maximumMilliseconds;
        }

        public static bool IsRunning(string timerName)
        {
            if (!TimerExists(timerName)) return false;
            return _timerCollection[timerName].IsRunning;
        }

        public static void Start(string timerName)
        {
            if (!TimerExists(timerName)) return;
            _timerCollection[timerName].Start();
        }

        public static void Stop(string timerName)
        {
            if (!TimerExists(timerName)) return;
            _timerCollection[timerName].Stop();
        }

        public static long ElapsedMilliseconds(string timerName)
        {
            if (!TimerExists(timerName)) return 0;
            return _timerCollection[timerName].ElapsedMilliseconds;
        }

        public static long ElapsedSeconds(string timerName)
        {
            if (!TimerExists(timerName)) return 0;
            return _timerCollection[timerName].ElapsedMilliseconds / 1000;
        }

        public static void Reset(string timerName)
        {
            if (!TimerExists(timerName)) return;
            _timerCollection[timerName].Reset();
            _timerCollection[timerName].Start();
        }

        public static void Recycle(string timerName, long elapsedMilliseconds)
        {
            if (_timerCollection[timerName].ElapsedMilliseconds < elapsedMilliseconds) return;

            _timerCollection[timerName].Reset();
            _timerCollection[timerName].Start();
        }

        public static bool Exists(string timerName)
        {
            return _timerCollection.ContainsKey(timerName);
        }

        public static bool SpellTimerOk(string spellname, long castTimePadding)
        {
            if (!Spell.IsKnown(spellname)) return false;

            uint spellCastTime = SpellManager.Spells[spellname].CastTime;
            long totalTime = spellCastTime + castTimePadding;

            if (Exists(spellname))
            {
                if (Expired(spellname, totalTime))
                {
                    Timers.Remove(spellname);
                    //Reset(spellname);
                    return true;
                }
                return false;
            }

            Add(spellname);
            return true;
        }

        private static bool TimerExists(string timerName)
        {
            if (!_timerCollection.ContainsKey(timerName))
            {
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(String.Format(" Timer '{0}' does not exist", timerName));
                Logging.WriteDebug(" ");
                Logging.WriteDebug("**********************************************************************");
                return false;
            }

            return true;
        }
    }
    
    public static class Spell
    {

        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return ObjectManager.Me.CurrentTarget; } }

        public static string BestSpell(string spells)
        {
            string[] spellList = spells.Split('+');

            foreach (string spell in spellList.Where(spell => CanCast(spell.Trim()) && !IsOnCooldown(spell.Trim())))
            {
                return spell.Trim();
            }

            return "";

        }

        public static string BestDebuff(string debuffs)
        {
            string[] debuffList = debuffs.Split('+');

            foreach (string debuff in debuffList.Where(debuff => CanCast(debuff) && !Target.IsDebuffOnTarget(debuff)))
            {
                return debuff;
            }
           
            return "";
        }

        /// <summary>
        /// TRUE if a spell is known by HB
        /// </summary>
        /// <param name="spellName">Name of the spell to check</param>
        /// <returns>TRUE if the spell is know</returns>
        public static bool IsKnown(string spellName) { return SpellManager.HasSpell(spellName); }


        /// <summary>
        /// Stop casting, goes without saying really
        /// </summary>
        public static void StopCasting() { SpellManager.StopCasting(); }

        /// <summary>
        /// TRUE if on global cooldown
        /// </summary>
        public static bool IsGCD { get { return LegacySpellManager.GlobalCooldown; } }

        
        /// <summary>
        /// TRUE if HB can cast the spell. This performs a LUA check on the spell
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns></returns>
        public static bool CanCastLUA(string spellName)
        {
            var isUsable = Lua.GetReturnValues("return IsUsableSpell('" + spellName + "')", "stuffnthings.lua");

            return isUsable != null && isUsable[0] == "1";
        }

        /// <summary>
        /// TRUE if HB can cast the spell. But first check our mana levels. 
        /// This way we can leave some mana for healing spells
        /// </summary>
        /// <param name="spellName">Spell name to check</param>
        /// <returns>TRUE if the spell can be cast</returns>
        public static bool CanCast(string spellName)
        {
            return CanCast(spellName, 0d);
        }


        public static bool CanCast(int spellID)
        {
            return SpellManager.CanCast(spellID,true);
        }

        
        /// <summary>
        /// TRUE if HB can cast the spell. But first check our mana levels. 
        /// This way we can leave some mana for healing spells
        /// </summary>
        /// <param name="spellName">Spell name to check</param>
        /// <param name="minimumPower">If power is below minimumPower percent then return FALSE</param>
        /// <returns>TRUE if the spell can be cast</returns>
        public static bool CanCast(string spellName, double minimumPower)
        {
            if (string.IsNullOrEmpty(spellName)) return false;
            return Me.CurrentPower > minimumPower && SpellManager.CanCast(spellName);
        }

        /// <summary>
        /// Cast a specific spell
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns>TRUE if the spell was cast successfully</returns>
        public static bool Cast(string spellName)
        {
            bool result = SpellManager.Cast(spellName);

            if (result) Utils.Log("-" + spellName, Utils.Colour("Green"));
            return result;
        }

        /// <summary>
        /// Cast a specific spell using LUA. This allows you to cast spells not in the HB spell dictionary.
        /// </summary>
        /// <param name="spellName"></param>
        public static void CastByNameLUA(string spellName)
        {
            Lua.DoString(String.Format("CastSpellByName(\"{0}\")", spellName));
            Utils.Log("-" + spellName, Utils.Colour("Green"));
        }

        /// <summary>
        /// Cast a spell on a specific target. This does not deselect your current target if it differs from targetUnit
        /// </summary>
        /// <param name="spellName">Name of the spell to cast</param>
        /// <param name="targetUnit">WoWUnit to cast the spell on</param>
        /// <returns>TRUE if the spell was cast successfully</returns>
        public static bool Cast(string spellName, WoWUnit targetUnit)
        {
            bool result = SpellManager.Cast(spellName, targetUnit);
            string targetName;

            if (RaFHelper.Leader != null && targetUnit.Guid == RaFHelper.Leader.Guid)
                targetName = "Leader / Tank";
            else
                targetName = targetUnit.Guid == Me.Guid ? "Me" : "Target";

            if (targetName == "Target" && targetUnit.IsPlayer) targetName = targetUnit.Class.ToString();
            if (result) Utils.Log(String.Format("-{0} on {1}", spellName, targetName), Utils.Colour("Green"));
            return result;
        }

        public static bool Cast(string spellName, WoWUnit targetUnit, bool waitForCastAndGCD)
        {
            bool result = SpellManager.Cast(spellName, targetUnit);

            

            string targetName;

            if (RaFHelper.Leader != null && targetUnit.Guid == RaFHelper.Leader.Guid)
                targetName = "Leader / Tank";
            else
                targetName = targetUnit.Guid == Me.Guid ? "Me" : "Target";

            if (targetName == "Target" && targetUnit.IsPet) targetName = "Pet";
            if (targetName == "Target" && targetUnit.IsPlayer) targetName = targetUnit.Class.ToString();
            

            if (waitForCastAndGCD)
            {
                Utils.LagSleep();
                while (Me.IsCasting) Thread.Sleep(100);
                while (IsGCD) Thread.Sleep(100);
            }

            if (result) Utils.Log(String.Format("-{0} on {1}", spellName, targetName), Utils.Colour("Green"));
            return result;
        }



        

        /// <summary>
        /// Cast a given spell using click-to-cast spells
        /// </summary>
        /// <param name="spellName">Spell name to cast</param>
        /// <param name="clickCastLocation">WoWPoint to cast the spell</param>
        /// <returns></returns>
        public static bool Cast(string spellName, WoWPoint clickCastLocation)
        {

            bool result = SpellManager.Cast(spellName);
            LegacySpellManager.ClickRemoteLocation(clickCastLocation);

            Utils.Log("-" + spellName, Utils.Colour("Green"));
            return result;

        }

        /// <summary>
        /// TRUE if the spell is on cooldown and can not be cast
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns>Name of the spell to check</returns>
        public static bool IsOnCooldown(string spellName)
        {
            if (!IsKnown(spellName)) return true;

            bool result = SpellManager.Spells[spellName].Cooldown;

            return result;
        }

        /// <summary>
        /// Conditionally cast a debuff on your current target
        ///   * Check if the debuff is on the target
        ///   * Check if HB can cast the spell
        /// </summary>
        /// <param name="spellName">Name of the debuff to cast</param>
        /// <returns>TRUE if the debuff was cast successfully</returns>
        public static bool CastDebuff(string spellName)
        {
            if (Target.IsDebuffOnTarget(spellName) || !CanCast(spellName)) return false;

            bool result = SpellManager.Cast(spellName);
            if (!result) return false;

            Utils.Log(String.Format("{0} on {1}", spellName, Me.CurrentTarget.Name), Utils.Colour("Green"));
            return true;
        }

        /// <summary>
        /// Cast a debuff on a specific target
        /// </summary>
        /// <param name="spellName">Name of the debuff to cast</param>
        /// <param name="targetUnit">WoWUnit to cast the debuff on</param>
        /// <returns>TRUE if the debuff was cast successfully</returns>
        public static bool CastDebuff(string spellName, WoWUnit targetUnit)
        {
            if (targetUnit.HasAura(spellName) || !CanCast(spellName)) return false;

            bool result = SpellManager.Cast(spellName, targetUnit);
            if (!result) return false;

            Utils.Log(String.Format("{0} on {1}", spellName, Me.CurrentTarget.Name), Utils.Colour("Green"));
            return true;
        }

        /// <summary>
        /// Cast a buff on a specific target
        /// </summary>
        /// <param name="spellName">Name of the buff to cast</param>
        /// <param name="targetUnit">WoWUnit to cast the buff on</param>
        /// <returns>TRUE if the buff was cast successfully</returns>
        public static bool CastBuff(string spellName, WoWUnit targetUnit)
        {
            if (targetUnit.HasAura(spellName) || !CanCast(spellName)) return false;

            bool result = SpellManager.Cast(spellName, targetUnit);
            if (!result) return false;

            Utils.Log(spellName);
            return true;
        }


        /// <summary>
        /// TRUE if you have enough mana to cast the spell
        /// </summary>
        /// <param name="spellName">Spell name to check</param>
        /// <returns></returns>
        public static bool IsEnoughPower(string spellName)
        {
            if (!IsKnown(spellName)) return false;
            
            return (Me.CurrentPower > SpellManager.Spells[spellName].PowerCost);
        }


        public static int PowerCost(string spellName)
        {
            if (!IsKnown(spellName)) return 9999999;
            
            return SpellManager.Spells[spellName].PowerCost;
        }


        /// <summary>
        /// Returns the maximum distance of the spell
        /// </summary>
        /// <param name="spellName">Spell name to check</param>
        /// <returns>The maximum distance the spell can be cast</returns>
        public static double MaxDistance(string spellName)
        {
            if (!IsKnown(spellName)) return 0.0;

            return SpellManager.Spells[spellName].MaxRange;
        }

        public static double MinDistance(string spellName)
        {
            if (!IsKnown(spellName)) return 0.0;

            return SpellManager.Spells[spellName].MinRange;
        }


    }

    public static class Target
    {

        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return Me.CurrentTarget; } }
        private static Stopwatch pullTimer = new Stopwatch();
        private static ulong _pullGuid;
        private static Stopwatch combatTimer = new Stopwatch();
        private static ulong _combatGuid;

        public static int CombatTimeout { get; set; }
        public static string LazyRaider { get; set; }


        /// <summary>
        /// The current health percent of the target
        /// </summary>
        public static double HealthPercent { get { return CT.HealthPercent; } }

        /// <summary>
        /// TRUE if the current target is more than 4 levels lower than you
        /// </summary>
        public static bool IsLowLevel { get { if (!Me.GotTarget) return false;  if (CT.Level < 6) return false; return CT.Level <= Me.Level - 3; } }

        /// <summary>
        /// TRUE if the current target is more than 3 levels higher than you
        /// </summary>
        public static bool IsHighLevel { get { return Me.GotTarget && (CT.Elite || CT.Level >= Me.Level + 3); } }

        public static bool IsFleeing { get { return Me.GotTarget && CT.Fleeing; } }

        public static bool IsDistanceLessThan(double distanceCheck) { return Me.GotTarget && CT.Distance < distanceCheck; }

        public static bool IsDistanceMoreThan(double distanceCheck) { return Me.GotTarget && CT.Distance > distanceCheck; }

        public static bool IsTargetingMe { get { return Me.GotTarget && CT.CurrentTargetGuid == Me.Guid; } }

        public static bool IsHealthAbove(double targetHealth) { return Me.GotTarget && CT.CurrentHealth > targetHealth; }

        public static bool IsHealthPercentAbove(double targetHealthPercent) { return Me.GotTarget && CT.HealthPercent > targetHealthPercent; }

        public static double Distance { get { return !Me.GotTarget ? 0 : CT.Distance; } }

        public static bool IsCasting { get { return Me.GotTarget && CT.IsCasting; } }

        public static bool CombatTimerExpired
        {
            get
            {
                if (!Me.GotTarget || Me.CurrentTarget.Dead) return false;

                if (_combatGuid != Me.CurrentTarget.Guid)
                {
                    Utils.Log(string.Format("New combat target {0}, resetting combat timer.", Me.CurrentTarget.Name), Color.FromName("Green"));
                    _combatGuid = Me.CurrentTarget.Guid;
                    combatTimer.Reset();
                    combatTimer.Start();
                }

                return combatTimer.Elapsed.Seconds >=  CombatTimeout;
            }
        }

        /// <summary>
        /// This is not actually true, this checks if the target's health is > 10X your own health
        /// </summary>
        public static bool IsInstanceBoss
        {
            get
            {
                if (!Me.GotTarget) return false;

                uint myHp = Me.MaxHealth;
                uint ctHp = CT.MaxHealth;
                bool result = (ctHp > myHp * 11);

                return result;
            }
        }

        public static bool IsWithinInteractRange { get { return Me.GotTarget && CT.WithinInteractRange; } }

        public static float InteractRange { get { return !Me.GotTarget ? 0 : CT.InteractRange; }
        }

        /// <summary>
        /// TRUE is the debuff is on the target.
        /// Similar to CanDebuffTarget except this does not check if you can cast the spell
        /// </summary>
        /// <param name="debuffName"></param>
        /// <returns></returns>
        public static bool IsDebuffOnTarget(string debuffName) { return Me.GotTarget && CT.HasAura(debuffName) && CT.Auras[debuffName].CreatorGuid == Me.Guid; }  //.HasAura(DebuffName)); }

        public static bool IsDebuffOnTarget(int debuffID)
        {
            if (!Me.GotTarget) return false;
            return CT.Auras.Any(aura => aura.Value.SpellId == debuffID && aura.Value.CreatorGuid == Me.Guid);
        }

        public static WoWAura AuraOnTarget(int debuffID)
        {
            if (!Me.GotTarget) return null;
            if (Me.GotTarget && CT.Dead) return null;

            foreach (KeyValuePair<string, WoWAura> aura in CT.Auras)
            {
                if (aura.Value.SpellId != debuffID) continue;
                if (aura.Value.CreatorGuid != Me.Guid) continue;

                return aura.Value;
            }

            return null;
        }

        public static WoWAura AuraOnTarget(string debuffName)
        {
            if (!Me.GotTarget) return null;
            if (Me.GotTarget && CT.Dead) return null;

            foreach (KeyValuePair<string, WoWAura> aura in CT.Auras)
            {
                if (aura.Key.ToUpper() != debuffName.ToUpper()) continue;
                if (aura.Value.CreatorGuid != Me.Guid) continue;

                return aura.Value;
            }

            return null;
        }

        /// <summary>
        /// TRUE if the target has been tagged and you are not in Party/Raid
        /// </summary>
        public static bool IsTaggedByOther { get { return (Me.GotTarget && !(Me.IsInParty || Me.IsInRaid) && CT.TaggedByOther); } }

        /// <summary>
        /// TRUE if the target is considered a caster
        /// A caster is a NPC that has MANA
        /// </summary>
        public static bool IsCaster { get { return Me.GotTarget && CT.ManaPercent > 1; } }

        public static bool IsPlayerCaster
        {
            get
            {
                if (!Me.GotTarget) return false;
                if (CT.Class == WoWClass.DeathKnight) return false;
                if (CT.Class == WoWClass.Hunter) return false;
                if (CT.Class == WoWClass.Rogue) return false;
                if (CT.Class == WoWClass.Warrior) return false;
                if (CT.Class == WoWClass.Paladin) return false;
                if (CT.Class == WoWClass.Druid && CT.Shapeshift == ShapeshiftForm.Cat) return false;
                if (CT.Class == WoWClass.Druid && CT.Shapeshift == ShapeshiftForm.Bear) return false;


                return true;
            }
        }

        /// <summary>
        /// TRUE if HB can generate a path to the target. 
        /// </summary>
        //public static bool CanGenerateNavPath { get { return Me.GotTarget && Navigator.GeneratePath(Me.Location, CT.Location).Length > 0; } }
        public static bool CanGenerateNavPath
        {
            get { return Me.GotTarget && Navigator.CanNavigateFully(Me.Location, CT.Location, 20); }
        }

        public static bool CanGenerateNavPathWithHops(int maxHops)
        {
            {
                return Me.GotTarget && Navigator.CanNavigateFully(Me.Location, CT.Location, maxHops);
            }
        }

        /// <summary>
        /// Blacklist the target for X seconds
        /// </summary>
        /// <param name="seconds">Seconds to blacklist the target</param>
        public static void BlackList(int seconds) { if (!Me.GotTarget) return; Blacklist.Add(CT, new TimeSpan(0, 0, seconds)); Me.ClearTarget(); }

        public static bool IsPlayer { get { return Me.GotTarget && CT.IsPlayer; } }

        public static void Face()
        {
            if (LazyRaider.Contains("always")) return;
            if (!Me.GotTarget) return; CT.Face();
        }

        public static bool IsFacing { get { return Me.GotTarget && WoWMathHelper.IsFacing(Me.Location, Me.Rotation, Me.CurrentTarget.Location, WoWMathHelper.DegreesToRadians(120)); } }

        public static bool IsElite { get { return Me.GotTarget && CT.Elite; } }

        /// <summary>
        /// TRUE if you can cast the debuff on the target
        ///   * Do you have a target
        ///   * Can you cast the spell
        ///   * Is the debuff already on the target
        /// </summary>
        /// <param name="spellName">Debuff spell to cast on the target</param>
        /// <returns></returns>
        public static bool CanDebuffTarget(string spellName)
        {
            return Me.GotTarget && Spell.CanCast(spellName) && (!CT.HasAura(spellName) || CT.HasAura(spellName) && CT.Auras[spellName].CreatorGuid != Me.Guid) && CT.Distance <= Spell.MaxDistance(spellName);
        }

        /// <summary>
        /// Return the stack count of a given debuff on the current target
        /// </summary>
        /// <param name="debuffName">Debuff name to check</param>
        /// <returns>int of debuff stacks</returns>
        public static int DebuffStackCount(string debuffName)
        {
            if (!Me.GotTarget || !IsDebuffOnTarget(debuffName)) return 0;
            
            return (int)CT.Auras[debuffName].StackCount;
        }

        public static int StackCountLUA(string buffName)
        {
            Lua.DoString(string.Format(@"buffName,_,_,stackCount,_,_,timeLeft,_,_=UnitBuff(""target"",""{0}"")", buffName));
            int stackCount = Lua.GetLocalizedInt32("stackCount", CT.BaseAddress);

            return stackCount;
            //return Convert.ToInt16(stackCount);
        }


        /// <summary>
        /// A number of simple checks to determine if the currently selected target should be pulled
        /// Checks the following; GotTarget, IsTotem, IsPet & Tagged
        /// </summary>
        public static bool IsValidPullTarget
        {
            get
            {
                if (Blacklist.Contains(CT.Guid)) return false;
                if (Blacklist.Contains(CT)) return false;
                if (!Me.GotTarget || (CT.IsSwimming && !Me.IsSwimming) || CT.Distance2D > 90 || !CT.InLineOfSight || CT.IsTotem || CT.IsPet || (RaFHelper.Leader == null && CT.TaggedByOther))
                    return false;

                return true;
            }
        }


        ///// <summary>
        ///// Check how long we've been in comat, is it more than 30 seconds?
        ///// </summary>
        //public static bool CombatTimerExpired
        //{
        //    get
        //    {
        //        if (!Me.GotTarget) return false;
        //        if (Me.CurrentTarget.Dead) return false;

        //        if (_combatGuid != Me.CurrentTarget.Guid)
        //        {
        //            Utils.Log(string.Format("New combat target {0}, resetting combat timer.", Me.CurrentTarget.Name), Color.FromName("Green"));
        //            _combatGuid = Me.CurrentTarget.Guid;
        //            combatTimer.Reset();
        //            combatTimer.Start();
        //        }

        //        return combatTimer.Elapsed.Seconds >= Settings.CombatTimeout;
        //    }
        //}


        /// <summary>
        /// Have you been trying to pull the target for more than 20 seconds?
        /// </summary>
        public static bool PullTimerExpired
        {
            get
            {
                if ((Self.IsBuffOnMe("Drink") || Self.IsBuffOnMe("Eat")) && _pullGuid != 0) _pullGuid = 0;

                if (_pullGuid != Me.CurrentTarget.Guid)
                {
                    Utils.Log(string.Format("New pull target {0} ({1}), resetting pull timer.", Me.CurrentTarget.Name, Me.CurrentTarget.Level), Color.FromName("Green"));
                    _pullGuid = Me.CurrentTarget.Guid;
                    pullTimer.Reset();
                    pullTimer.Start();
                }

                return pullTimer.Elapsed.Seconds >= 20;
            }
        }

        public static bool IsBuffOnTarget(int buffID)
        {
            return Me.CurrentTarget.Auras.Any(aura => aura.Value.SpellId == buffID);
        }

        public static int StackCount(int buffID)
        {
            if (!IsBuffOnTarget(buffID)) return 0;
            return (int)Me.GetAuraById(buffID).StackCount;
        }
    }
    
    public static class Self
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        public enum AuraCheck { ActiveAuras, AllAuras }

        /// <summary>
        /// Simple buff check
        /// </summary>
        /// <param name="buffName">Name of the buff to check</param>
        /// <returns>TRUE if the buff is present on you</returns>
        public static bool IsBuffOnMe(string buffName)
        {
            return (Me.Auras.ContainsKey(buffName));
        }

        /// <summary>
        /// Returns a WoW Aura matching the buff ID. Searches Me.Auras (all auras)
        /// </summary>
        /// <param name="buffID"></param>
        /// <returns></returns>
        public static WoWAura Buff(int buffID)
        {
            foreach (KeyValuePair<string, WoWAura> aura in Me.Auras)
            {
                if (aura.Value.SpellId == buffID) return aura.Value;
            }

            return null;
        }

        public static bool IsBuffOnMe(int buffID)
        {
            return Me.ActiveAuras.Any(aura => aura.Value.SpellId == buffID);
        }

        public static bool IsBuffOnMe(int buffID, AuraCheck auras)
        {
            if (auras == AuraCheck.ActiveAuras)
            {
                return Me.ActiveAuras.Any(aura => aura.Value.SpellId == buffID);
            }

            return Me.Auras.Any(aura => aura.Value.SpellId == buffID);
        }

        public static bool IsBuffOnMe(List<int> buffID, AuraCheck auras)
        {
            if (auras == AuraCheck.ActiveAuras)
            {
                foreach (KeyValuePair<string, WoWAura> aura in Me.ActiveAuras)
                {
                    foreach (int buffid in buffID)
                    {
                        if (aura.Value.SpellId == buffid) return true;
                    }
                }

                return false;
                
            }

            foreach (KeyValuePair<string, WoWAura> aura in Me.Auras)
            {
                foreach (int buffid in buffID)
                {
                    if (aura.Value.SpellId == buffid) return true;
                }
            }

            return false;
        }

        public static bool IsBuffOnMeLUA(string buffName)
        {
            Lua.DoString(string.Format(@"buffName,_,_,stackCount,_,_,timeLeft,_,_=UnitBuff(""player"",""{0}"")", buffName));
            string buff = Lua.GetLocalizedText("buffName", Me.BaseAddress);

            return buff == buffName;
        }

        public static int StackCountLUA(string buffName)
        {
            Lua.DoString(string.Format(@"buffName,_,_,stackCount,_,_,timeLeft,_,_=UnitBuff(""player"",""{0}"")", buffName));
            int stackCount = Lua.GetLocalizedInt32("stackCount", Me.BaseAddress);

            return stackCount;
            //return Convert.ToInt16(stackCount);
        }

        public static string BuffTimeLeftLUA(string buffName)
        {
            Lua.DoString(string.Format(@"buffName,_,_,stackCount,_,_,timeLeft,_,_=UnitBuff(""player"",""{0}"")", buffName));
            string timeLeft = Lua.GetLocalizedText("timeLeft", Me.BaseAddress);

            return timeLeft;
            //return Convert.ToInt16(timeLeft);
        }

        public static string GetTimeLUA()
        {
            Lua.DoString(string.Format(@"time = GetTime()"));
            string time = Lua.GetLocalizedText("time", Me.BaseAddress);

            return time;
            //return Convert.ToInt16(timeLeft);
        }


        /// <summary>
        /// Remove a buff from the player
        /// </summary>
        /// <param name="buffName">Name of the buff to be removed</param>
        public static void RemoveBuff(string buffName)
        {
            Lua.DoString(string.Format("CancelUnitBuff('player', '{0}')", buffName));
        }

        /// <summary>
        /// Checks Spell.CanCast and Me.HasAura
        /// </summary>
        /// <param name="buffName">Name of the buff you want to cast</param>
        /// <returns></returns>
        public static bool CanBuffMe(string buffName)
        {
            return !Me.HasAura(buffName) && Spell.CanCast(buffName);
        }

        /// <summary>
        /// TRUE is the given buff is present on you
        /// </summary>
        /// <param name="buffName">Buff name you want to check for</param>
        /// <returns>TRUE if the buff is present</returns>
        public static bool BuffMe(string buffName)
        {
            Spell.Cast(buffName, Me);
            Utils.LagSleep();

            return IsBuffOnMe(buffName);
        }

        /// <summary>
        /// The number of stacks on a given buff
        /// </summary>
        /// <param name="buffName">Buff name you want to check for</param>
        /// <returns>The number (int) of stacks of the buff</returns>
        public static int StackCount(string buffName)
        {
            if (!IsBuffOnMe(buffName)) return 0;
            return (int)Me.Auras[buffName].StackCount;
        }



        /// <summary>
        /// The number of stacks of a given buff (by ID)
        /// </summary>
        /// <param name="buffID">Spell ID of the buff you want to check for</param>
        /// <returns>The number of stacks of the buff</returns>
        public static int StackCount(int buffID)
        {
            if (!IsBuffOnMe(buffID)) return 0;
            return (int) Me.GetAuraById(buffID).StackCount;
        }

        /// <summary>
        /// Scan the area (30 yards) for players of the same faction to buff
        /// </summary>
        /// <param name="spellName">Name of the buff you want to cast</param>
        /// <param name="excludeIfBuffPresent">Do not cast the buff if this spell is present on the target</param>
        /// <param name="minimumMana">Do not cast the buff if your mana is below this percent</param>
        /// <param name="buffInCombat">TRUE if you want to cast buffs on players while you are in combat</param>
        public static void BuffRandomPlayers(string spellName, string excludeIfBuffPresent, double minimumMana, bool buffInCombat)
        {
            if (Me.IsResting || IsBuffOnMe("Drink") || IsBuffOnMe("Food") || !Spell.CanCast(spellName) || Me.IsGhost || Me.Dead || Me.Mounted || (!buffInCombat && Me.Combat) || Me.ManaPercent < minimumMana)
                return;

            List<WoWPlayer> plist =
                (from o in ObjectManager.ObjectList
                 where o is WoWPlayer
                 let p = o.ToPlayer()
                 where p.Distance < 30
                       && p.Guid != Me.Guid
                       && (p.IsHorde && Me.IsHorde || p.IsAlliance && Me.IsAlliance)
                       && !p.Dead
                       && p.InLineOfSight
                       && !p.HasAura(spellName)
                       && !p.HasAura(excludeIfBuffPresent)
                 select p).ToList();


            foreach (WoWPlayer p in plist)
            {
                if (!Spell.CanCast(spellName) || !buffInCombat && p.Combat || !Me.PvpFlagged && p.PvpFlagged) return;

                Utils.Log(string.Format("Being friendly and casting {0} on a player", spellName), Utils.Colour("Green"));
                Spell.Cast(spellName, p);
                while (Spell.IsGCD) Thread.Sleep(250); 
            }

        }

        /// <summary>
        /// Scan the area (40 yards) for players of the same faction to heal
        /// </summary>
        /// <param name="spellName">Name of the sepll you want to cast</param>
        /// <param name="excludeIfBuffPresent">Do not cast the buff if this spell is present on the target</param>
        /// <param name="buffInCombat">Do not cast the buff if your mana is below this percent</param>
        /// <param name="minimumHealth">The minimum health a player must be before healing them</param>
        /// <param name="minimumMana">TRUE if you want to cast buffs on players while you are in combat</param>
        public static void HealRandomPlayers(string spellName, string excludeIfBuffPresent, bool buffInCombat, double minimumHealth, double minimumMana)
        {
            if (Me.IsResting || IsBuffOnMe("Drink") || IsBuffOnMe("Food") || !Spell.CanCast(spellName) || Me.IsGhost || Me.Dead || Me.Mounted || Me.ManaPercent < minimumMana)
                return;

            List<WoWPlayer> plist =
                (from o in ObjectManager.ObjectList
                 where o is WoWPlayer
                 let p = o.ToPlayer()
                 where p.Distance < 40
                       && p.Guid != Me.Guid
                       && (p.IsHorde && Me.IsHorde || p.IsAlliance && Me.IsAlliance)
                       && !p.Dead
                       && p.InLineOfSight
                       && p.HealthPercent > 10
                       && p.HealthPercent < minimumHealth
                       && !p.HasAura(spellName)
                       && !p.HasAura(excludeIfBuffPresent)
                 select p).ToList();



            foreach (WoWPlayer p in plist)
            {
                if (!Spell.CanCast(spellName) || (!buffInCombat && p.Combat) || p.HasAura(excludeIfBuffPresent) || p.HasAura(spellName) || (!Me.PvpFlagged && p.PvpFlagged)) return;

                while (Me.IsMoving) WoWMovement.MoveStop();

                Utils.Log("Being friendly and healing a player", Utils.Colour("Green"));
                Spell.Cast(spellName, p);
                Thread.Sleep(500);

                while (Me.IsCasting) Thread.Sleep(250);
                while (Spell.IsGCD) Thread.Sleep(250);
            }

        }

        /// <summary>
        /// Check if your health is above X value
        /// </summary>
        /// <param name="healthLevel">A value to check if your health is above</param>
        /// <returns></returns>
        public static bool IsHealthAbove(int healthLevel) { return Me.CurrentHealth > healthLevel; }

        /// <summary>
        /// Check if your health percent is above X%
        /// </summary>
        /// <param name="healthPercentLevel"></param>
        /// <returns></returns>
        public static bool IsHealthPercentAbove(int healthPercentLevel) { return Me.HealthPercent > healthPercentLevel; }

        /// <summary>
        /// Check if your power (Mana/Energy/Rage) is above X value
        /// </summary>
        /// <param name="powerLevel"></param>
        /// <returns></returns>
        public static bool IsPowerAbove(int powerLevel) { return Me.CurrentPower > powerLevel; }

        /// <summary>
        /// Check if your power (Mana/Energy/Rage) percent is above X%
        /// </summary>
        /// <param name="powerPercentLevel"></param>
        /// <returns></returns>
        public static bool IsPowerPercentAbove(int powerPercentLevel) { return Me.PowerPercent > powerPercentLevel; }

        public static bool Immobilised
            {
                get
                {
                    foreach (KeyValuePair<string, WoWAura> aura in Me.Auras)
                    {
                        if (!aura.Value.IsHarmful) continue;

                        //Utils.Log(string.Format("-Harmful spell found {0}, {1}", aura.Value.Spell.Name, aura.Value.Spell.Mechanic));
                        if (aura.Value.Spell.Mechanic == WoWSpellMechanic.Banished) return true;
                        if (aura.Value.Spell.Mechanic == WoWSpellMechanic.Asleep) return true;
                        if (aura.Value.Spell.Mechanic == WoWSpellMechanic.Charmed) return true;
                        if (aura.Value.Spell.Mechanic == WoWSpellMechanic.Fleeing) return true;
                        if (aura.Value.Spell.Mechanic == WoWSpellMechanic.Horrified) return true;
                        if (aura.Value.Spell.Mechanic == WoWSpellMechanic.Incapacitated) return true;
                        if (aura.Value.Spell.Mechanic == WoWSpellMechanic.Sapped) return true;
                        if (aura.Value.Spell.Mechanic == WoWSpellMechanic.Stunned) return true;
                        //if (aura.Value.Spell.Mechanic == WoWSpellMechanic.Snared) return true;
                    }

                    return false;
                }
            }
    }

    public static class Talents
    {
        private enum ClassType
        {
            None = 0,
            Spec1,
            Spec2,
            Spec3
        }
    
        private static void LoadCurrentSpec() { Load(ActiveGroup); }

        private static readonly string[] TabNames = new string[4];
        private static readonly int[] TabPoints = new int[4];
        private static int _indexGroup;
        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        public static int Spec
        {
            get
            {
                LoadCurrentSpec();

                int nSpec = 0;
                if (TabPoints[1] == 0 && TabPoints[2] == 0 && TabPoints[3] == 0)
                {
                    if (Me.Level > 9)
                    { Utils.Log("*** NO TALENT POINTS HAVE BEEN SPENT YET ***"); }
                    else if (Me.Level < 10)
                    { Utils.Log("*** Below level 10 no talent points available ***"); }
                    nSpec = 0;
                    return nSpec;
                }

                if (TabPoints[1] >= (TabPoints[2] + TabPoints[3])) nSpec = 1;
                else if (TabPoints[2] >= (TabPoints[1] + TabPoints[3])) nSpec = 2;
                else if (TabPoints[3] >= (TabPoints[1] + TabPoints[2])) nSpec = 3;

                return nSpec;
            }
        }

        public static void Load(int nGroup)
        {
            int nTab;
            _indexGroup = nGroup;


            for (nTab = 1; nTab <= 3; nTab++)
            {
                try
                {
                    string luaCode = String.Format("return GetTalentTabInfo({0},false,false,{1})", nTab, _indexGroup);
                    List<string> tabInfo = Lua.GetReturnValues(luaCode, "stuff.lua");

                    TabNames[nTab] = tabInfo[1];
                    TabPoints[nTab] = Convert.ToInt32(tabInfo[4]);
                }
                catch (Exception ex) { Logging.WriteException(ex); }
            }

        }

        private static int ActiveGroup { get { return Lua.GetReturnVal<int>("return GetActiveTalentGroup(false,false)", 0); } }

    }

    public static class Movement
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        public static double MinimumDistance { get; set; }
        public static double MaximumDistance { get; set; }
        public static string LazyRaider { get; set; }

        /// <summary>
        /// Stop moving. Like da!
        /// </summary>
        public static void StopMoving()
        {
            if (LazyRaider.Contains("always")) return;
            while (Me.IsMoving) { WoWMovement.MoveStop(); Thread.Sleep(50); }
        }

        /// <summary>
        /// Move to within X yards of the target
        /// </summary>
        /// <param name="distanceFromTarget">Distance to move to the target</param>
        public static void MoveTo(float distanceFromTarget)
        {
            if (LazyRaider.Contains("always")) return;
            // Let HB do the math and find a WoWPoint X yards away from the target
            WoWPoint moveToHere = WoWMathHelper.CalculatePointFrom(Me.Location, Me.CurrentTarget.Location, distanceFromTarget);

            // Use HB navigation to move to a WoWPoint. WoWPoint has been calculated in the above code
            Navigator.MoveTo(moveToHere);
        }

        public static WoWPoint PointFromTarget(float distanceFromTarget)
        {
            // Let HB do the math and find a WoWPoint X yards away from the target
            WoWPoint pointFromTarget = WoWMathHelper.CalculatePointFrom(Me.Location, Me.CurrentTarget.Location, distanceFromTarget);

            return pointFromTarget;
        }

        public static void MoveTo(WoWPoint location)
        {
            if (LazyRaider.Contains("always")) return;
            Navigator.MoveTo(location);
        }

        public static bool NeedToCheck()
        {
            float interactRange = Me.CurrentTarget.InteractRange - 2.0f;
            if (LazyRaider.Contains("always")) return false;

            if (!Target.IsFleeing && Me.CurrentTarget.Distance <= interactRange) return false;

            // If distance is less than ClassHelper.MinimumDistance and the target is NOT running away and we are moving then we should stop moving
            if (Target.IsDistanceLessThan(MinimumDistance) && !Target.IsFleeing && Me.IsMoving) WoWMovement.MoveStop();

            // TRUE if we are out of range. TRUE means we need to move closer
            bool result = Target.IsDistanceMoreThan(MaximumDistance);

            return result;
        }

        /// <summary>
        /// Move to melee distance if we need to
        /// </summary>
        public static void DistanceCheck()
        {
            // No target, nothing to do
            if (!Me.GotTarget) return;
            if (LazyRaider.Contains("always")) return;

            // If we're too close stop moving
            if (Target.IsDistanceLessThan(MinimumDistance)) StopMoving();

            // If we're more than X yards away from the current target then move to X yards from the target
            DistanceCheck(MaximumDistance, MinimumDistance);
        }

        /// <summary>
        /// TRUE if we need to perform a distance check
        /// </summary>
        public static bool NeedToCheck(double minimumDistance)
        {
            float interactRange = Me.CurrentTarget.InteractRange - 2.0f;
            if (LazyRaider.Contains("always")) return false;
            if (!Target.IsFleeing && Me.CurrentTarget.Distance <= interactRange) return false;

            // If distance is less than ClassHelper.MinimumDistance and the target is NOT running away and we are moving then we should stop moving
            if (Target.IsDistanceLessThan(minimumDistance) && !Target.IsFleeing && Me.IsMoving) WoWMovement.MoveStop();

            // TRUE if we are out of range. TRUE means we need to move closer
            bool result = Target.IsDistanceMoreThan(minimumDistance);

            return result;
        }


        /// <summary>
        /// Move to range and stop moving if we are too close
        /// </summary>
        /// <param name="maxDistance">Maximum distance away from the target you want to be. This should be max spell range or melee range</param>
        /// <param name="moveToDistance">If your distance is greater than maxDistance you will move to this distance from the target</param>
        public static void DistanceCheck(double maxDistance, double moveToDistance)
        {
            // No target, nothing to do
            if (!Me.GotTarget) return;
            if (LazyRaider.Contains("always")) return;

            // If target (NPC) is running away move as close a possible to the target
            if (Me.GotTarget && Me.CurrentTarget.Fleeing)
            {
                Utils.Log("Runner!");
                moveToDistance = -0.5;
            }

            float interactRange = Me.CurrentTarget.InteractRange - 1;
            if (!Target.IsFleeing && Target.IsDistanceLessThan(interactRange)) return;

            // We're too far from the target so move closer
            if (Me.CurrentTarget.Fleeing && Target.Distance > moveToDistance || Target.Distance > maxDistance)
            {
                Utils.Log(String.Format("Moving closer to {0}", Me.CurrentTarget.Name), Color.FromName("DarkGreen"));
                MoveTo((float)moveToDistance);
            }

            // We're too close to the target we need to stop moving
            if (Target.Distance <= moveToDistance && Me.IsMoving)
            {
                //Utils.Log("We are too close, stop moving.", Color.FromName("DarkGreen"));
                WoWMovement.MoveStop();
                return;
            }

            // We don't need to do anything so just exit
            if (Target.Distance > moveToDistance || !Me.IsMoving || Me.CurrentTarget.Fleeing)
                return;



            // When all else fails just stop moving
            WoWMovement.MoveStop();

        }

        #region Find Safe Location
        //************************************************************************************
        // Snazzy code Apoc gave me (from HB corpse rez)
        // It will find the best location furtherest away from mobs
        // I use this for kiting



        private static List<WoWPoint> AllMobsAroundUs
        {
            get
            {
                List<WoWUnit> mobs = (from o in ObjectManager.ObjectList
                                      where o is WoWUnit && o.Distance < 100
                                      let u = o.ToUnit()
                                      where u.Attackable && u.IsAlive && u.IsHostile
                                      select u).ToList();

                return mobs.Select(u => u.Location).ToList();
            }
        }

        private static WoWPoint NearestMobLoc(WoWPoint p, IEnumerable<WoWPoint> mobLocs)
        {
            var lst = (mobLocs.OrderBy(u => u.Distance(p)));
            return mobLocs.OrderBy(u => u.Distance(p)).Count() > 0 ?  lst.First() :  new WoWPoint(float.MaxValue, float.MaxValue, float.MaxValue);
        }

        public static WoWPoint FindSafeLocation(double distanceFromTarget)
        {
            return FindSafeLocation_All(distanceFromTarget,false);
        }

        /// <summary>
        /// Find a safe location with a line of sight to the target
        /// </summary>
        /// <param name="distanceFromTarget"></param>
        /// <param name="LoSFromTargetsView"></param>
        /// <returns></returns>
        public static WoWPoint FindSafeLocation(double distanceFromTarget, bool LoSFromTargetsView)
        {
            return FindSafeLocation_All(distanceFromTarget, LoSFromTargetsView);
        }


        private static WoWPoint FindSafeLocation_All(double distanceFromTarget, bool LoSFromTargetsView)
        {
            if (LazyRaider.Contains("always")) return null;

            WoWPoint originatingLocation = Me.Location;
            WoWPoint myLocation = Me.Location;
            WoWPoint destinationLocation;
            List<WoWPoint> mobLocations = new List<WoWPoint>();

            mobLocations = AllMobsAroundUs;
            double bestSafetyMargin = distanceFromTarget;

            mobLocations.Add(Me.CurrentTarget.Location);

            if (LoSFromTargetsView && Me.GotTarget) originatingLocation = Me.CurrentTarget.Location;


            // Rotate 10 degrees each itteration
            for (float degrees = 0f; degrees < 360f; degrees += 10f)
            {
                // Search 5 yards further away each itteration
                for (float distanceFromMob = 0f; distanceFromMob <= 35f; distanceFromMob += 5f)
                {
                    destinationLocation = myLocation.RayCast((float)(degrees * Math.PI / 180f), distanceFromMob);
                    double mobDistance = destinationLocation.Distance2D(NearestMobLoc(destinationLocation, mobLocations));

                    // Mob(s) too close to our current base-safe location, not a suitable location
                    if (mobDistance <= bestSafetyMargin) continue;

                    // Found a mob-free location, lets do further testing.
                    // * Check if we can generate a path
                    // * Check if we have LOS 

                    // Can we generate a path to this location?
                    if (Navigator.GeneratePath(originatingLocation, destinationLocation).Length <= 0)
                    {
                        //Utils.Log("Mob-free location failed path generation check");
                        continue;
                    }

                    // Is the destination in line of sight?
                    if (!GameWorld.IsInLineOfSight(originatingLocation, destinationLocation))
                    {
                        //Utils.Log("Mob-free location failed line of sight check");
                        continue;
                    }

                    // We pass all checks. This is a good location 
                    // Make it so 'Number 1', "Engage"
                    return destinationLocation;

                }
            }

            return Me.Location;

        }

        #endregion

    }

    public static class Inventory
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        public static class ManaPotions
        {

            private static WoWItem ManaPotion
            {
                get { return Me.CarriedItems.Where(item => item.Name.Contains("Mana Pot")).FirstOrDefault(); }
            }

            public static bool IsUseable
            {
                get
                {
                    if (ManaPotion == null) return false;

                    string luacode = String.Format("return GetItemCooldown(\"{0}\")", ManaPotion.Entry);
                    return Utils.LuaGetReturnValueString(luacode) == "0";
                }
            }

            public static void Use()
            {
                if (Me.IsCasting) Spell.StopCasting();
                Utils.LagSleep();

                WoWItem manaPotion = ManaPotion;
                Utils.Log(string.Format("We're having an 'Oh Shit' moment. Using {0}", manaPotion.Name), Utils.Colour("Red"));
                ManaPotion.Interact();
            }

        }

        public static class HealthPotions
        {
            /// <summary>
            /// WoWItem type of a suitable Healing Potion in your bags. Null if nothing is found
            /// </summary>
            private static WoWItem HealthPotion
            {
                get { return Me.CarriedItems.Where(item => item.Name.Contains("Healing Pot")).FirstOrDefault(); }
            }

            /// <summary>
            /// Checks if this item is not on cooldown and can be used. Returns TRUE is the item is ok to be used
            /// </summary>
            public static bool IsUseable
            {
                get
                {
                    if (HealthPotion == null) return false;

                    string luacode = String.Format("return GetItemCooldown(\"{0}\")", HealthPotion.Entry);
                    return Utils.LuaGetReturnValueString(luacode) == "0";

                    /*


                    if (HealthPotion == null) return false;
                    Utils.Log(string.Format("We have a health potion, {0}, lets see if its on cooldown...", HealthPotion.Name));

                    //string luacode = String.Format("return GetItemCooldown(\"{0}\")", HealthPotion.Entry);
                    //Utils.Log("***** LUA = " + luacode);
                    //Utils.Log("++++ cooldown = " + HealthPotion.Cooldown);

                    //bool result = (Utils.LuaGetReturnValueString(luacode) == "0");
                    bool result = HealthPotion.Cooldown == 0;
                    Utils.Log(string.Format("Potion is useable {0}", result));

                    return result;
                     */
                }
            }

            public static void Use()
            {
                if (Me.IsCasting) Spell.StopCasting();
                Utils.LagSleep();

                WoWItem healthPotion = HealthPotion;
                Utils.Log(string.Format("We're having an 'Oh Shit' moment. Using {0}", healthPotion.Name), Utils.Colour("Red"));
                HealthPotion.Interact();
            }

        }

        public static void Drink()
        {
            // If we're not using smart eat and drink then don't do this stuff, just sit and drink

            WoWItem drink = Styx.Logic.Inventory.Consumable.GetBestDrink(false);
            if (drink == null) return;
            Utils.Log(string.Format("Drinking {0}", drink.Name), Utils.Colour("Green"));
            LevelbotSettings.Instance.DrinkName = drink.Name;
            Styx.Logic.Common.Rest.Feed();
        }

        public static void Eat()
        {
            // If we're not using smart eat and drink then don't do this stuff, just sit and eat

            WoWItem food = Styx.Logic.Inventory.Consumable.GetBestFood(false);
            if (food == null) return;
            Utils.Log(string.Format("Eating {0}", food.Name), Utils.Colour("Green"));
            LevelbotSettings.Instance.FoodName = food.Name;
            Styx.Logic.Common.Rest.Feed();
        }

        public static bool HaveItem(string itemName)
        {
            return Me.BagItems.Any(item => item.Name.ToUpper() == itemName.ToUpper());
        }

        public static bool HaveItem(int itemNumber)
        {
            return Me.BagItems.Any(item => item.ItemInfo.Id == itemNumber);
        }

        public static bool HaveItem(uint itemNumber)
        {
            return Me.BagItems.Any(item => item.ItemInfo.Id == itemNumber);
        }
    }
    
    public static class EventHandlers
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return Me.CurrentTarget; } }

        public static void CombatLogEventHander(object sender, LuaEventArgs args)
        {
            foreach (object arg in args.Args)
            {
                if (!(arg is String)) continue;

                var s = (string)arg;

                //if (s.Contains("Crab")) Utils.Log("CRAB-EH!",Color.Red);

                if (s.Contains("EVADE") && Me.GotTarget)
                {
                    Logging.Write("My target is Evade bugged, blacking " + CT.Name, Color.Red);
                    Target.BlackList(3600);
                    Lua.DoString("StopAttack() PetStopAttack() PetFollow()");
                    StyxWoW.Me.ClearTarget();
                }
            }
        }

        public static void TalentPointEventHander(object sender, LuaEventArgs args)
        {
            //ClassHelper.ClassSpec = (Talents.ClassType)Talents.Spec;
        }


    }

    public static class CLC
    {
        //*********************************************************************************************************
        // 
        // This is what I call my "Common Language Configuration" system.
        // It takes common language terms and makes a TRUE/FALSE setting from it.
        // I'm sure there are more elegant ways of undertaking this but it suites my purposes perfectly.
        //
        // You pass a RAW string and simply check if a phrase or keyword is present
        // Its a lot more intuitive for the user and it gives me more control over the UI
        // This way I don't need hundreds of tick boxes or controls
        // 
        //*********************************************************************************************************


        /// <summary>
        /// This is the property you assign the raw 'setting' to. The raw setting is the value passed from the Settings.XXXX property
        /// Eg CLC.RawSetting = Settings.Cleanse; Checking CLC.AfterCombatEnds you will see it returns TRUE
        /// </summary>
        public static string RawSetting { get; set; }
        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        // Adds 2 - 5

        private static bool OnAdds { get { return RawSetting.Contains("only on adds"); } }
        private static bool NoAdds { get { return RawSetting.Contains("only when no adds"); } }
        private static bool OnAdds3OrMore { get { return RawSetting.Contains("only on 3+ adds"); } }
        private static bool OnAdds4OrMore { get { return RawSetting.Contains("only on 4+ adds"); } }
        private static bool OnAdds5OrMore { get { return RawSetting.Contains("only on 5+ adds"); } }
        private static bool OnAdds6OrMore { get { return RawSetting.Contains("only on 6+ adds"); } }
        private static bool OnAdds7OrMore { get { return RawSetting.Contains("only on 7+ adds"); } }
        private static bool OnAdds8OrMore { get { return RawSetting.Contains("only on 8+ adds"); } }

        // Call these properties from the CC in order to check if a condition is met. 
        private static bool Always { get { return RawSetting.Contains("always"); } }
        private static bool Never { get { return RawSetting.Contains("never"); } }
        private static bool OnRunners { get { return RawSetting.Contains("on runners") || RawSetting.Contains("and runners"); } }
        private static bool IfCasting { get { return RawSetting.Contains("on casters") || RawSetting.Contains("casters and"); } }
        private static bool IsCasting { get { return RawSetting.Contains("mob is casting"); } }
        private static bool IfCastingOrRunning { get { return RawSetting.Contains("casters and runners"); } }
        private static bool OutOfCombat { get { return RawSetting.Contains("out of combat") || RawSetting.Contains("during pull"); } }
        private static bool Immediately { get { return RawSetting.Contains("immediately"); } }
        private static bool InBGs { get { return RawSetting.Contains("only in battleground") || RawSetting.Contains("when in battleground"); } }
        private static bool NotInBGs { get { return RawSetting.Contains("not in battleground"); } }
        private static bool InInstances { get { return RawSetting.Contains("only when in instance") || RawSetting.Contains("inside an instance") || RawSetting.Contains("only in instance"); } }
        private static bool NotInInstances { get { return RawSetting.Contains("not in instance") || RawSetting.Contains("not inside an instance"); } }
        private static bool OnHumanoids { get { return RawSetting.Contains("on humanoids"); } }
        private static bool OnUndead{ get { return RawSetting.Contains("undead"); } }
        private static bool OnDemons { get { return RawSetting.Contains("demons"); } }
        private static bool OnBeasts { get { return RawSetting.Contains("beasts"); } }
        private static bool OnElementals{ get { return RawSetting.Contains("elemental"); } }
        private static bool OnFleeing { get { return RawSetting.Contains("on fleeing") || RawSetting.Contains("is fleeing") || RawSetting.Contains("or fleeing"); } }
        private static bool NotLowLevel { get { return RawSetting.Contains("not low level"); } }
        private static bool IsHighLevel { get { return RawSetting.Contains("is high level"); } }
        private static bool IsInMeleeRange { get { return RawSetting.Contains("is in melee range"); } }
        private static bool HealingSpellCast { get { return RawSetting.Contains("healing spell"); } }
        private static bool InstanceImportantSpellCast { get { return RawSetting.Contains("instance important spell"); } }

        private static bool IsPlayer { get { return RawSetting.Contains("is player"); } }
        private static bool IsNotPlayer { get { return RawSetting.Contains("is not player"); } }
        private static bool IsInParty{ get { return RawSetting.Contains("when in party"); } }
        private static bool IsNotInParty { get { return RawSetting.Contains("not in party"); } }
        private static bool IsInRaid { get { return RawSetting.Contains("when in raid"); } }
        private static bool IsNotInRaid { get { return RawSetting.Contains("when not in raid"); } }
        

        // Combo points - Rogues and Druids (Cat)
        private static bool ComboPoints12 { get { return RawSetting.Contains("1-2 combo"); } }
        private static bool ComboPoints23 { get { return RawSetting.Contains("2-3 combo"); } }
        private static bool ComboPoints34 { get { return RawSetting.Contains("3-4 combo"); } }
        private static bool ComboPoints45 { get { return RawSetting.Contains("4-5 combo"); } }
        private static bool ComboPoints1OrMore { get { return RawSetting.Contains("1+ Combo") || RawSetting.Contains("1+ combo"); } }
        private static bool ComboPoints2OrMore { get { return RawSetting.Contains("2+ Combo") || RawSetting.Contains("2+ combo"); } }
        private static bool ComboPoints3OrMore { get { return RawSetting.Contains("3+ Combo") || RawSetting.Contains("3+ combo"); } }
        private static bool ComboPoints4OrMore { get { return RawSetting.Contains("4+ Combo") || RawSetting.Contains("4+ combo"); } }
        private static bool ComboPoints5OrMore { get { return RawSetting.Contains("5+ Combo") || RawSetting.Contains("5+ combo"); } }

        // Holy Power - Paladin only
        private static bool HolyPower1OrMore { get { return RawSetting.Contains("1+ Holy Power"); } }
        private static bool HolyPower2OrMore { get { return RawSetting.Contains("2+ Holy Power"); } }
        private static bool HolyPower3OrMore { get { return RawSetting.Contains("3+ Holy Power"); } }

        // Shadow Orbs - Priest only
        private static bool MindSpike1OrMore { get { return RawSetting.Contains("1+ Mind Spike"); } }
        private static bool MindSpike2OrMore { get { return RawSetting.Contains("2+ Mind Spike"); } }
        private static bool MindSpike3OrMore { get { return RawSetting.Contains("3+ Mind Spike"); } }
        private static bool ShadowOrb1OrMore { get { return RawSetting.Contains("1+ Shadow Orb"); } }
        private static bool ShadowOrb2OrMore { get { return RawSetting.Contains("2+ Shadow Orb"); } }
        private static bool ShadowOrb3OrMore { get { return RawSetting.Contains("3+ Shadow Orb"); } }
        private static bool InShadowform { get { return RawSetting.Contains("only in Shadowform") || RawSetting.Contains("only when in Shadowform"); } }
        private static bool NotInShadowform { get { return RawSetting.Contains("not in Shadowform"); } }

        // Hunter Focus Fire Frenzy Stacks
        private static bool FocusFire1OrMore { get { return RawSetting.Contains("1+ Frenzy"); } }
        private static bool FocusFire2OrMore { get { return RawSetting.Contains("2+ Frenzy"); } }
        private static bool FocusFire3OrMore { get { return RawSetting.Contains("3+ Frenzy"); } }
        private static bool FocusFire4OrMore { get { return RawSetting.Contains("4+ Frenzy"); } }
        private static bool FocusFire5OrMore { get { return RawSetting.Contains("5+ Frenzy"); } }

        // Hunter Focus 
        private static bool Focus2OrMore { get { return RawSetting.Contains("focus 20+"); } }
        private static bool Focus25rMore { get { return RawSetting.Contains("focus 25+"); } }
        private static bool Focus3OrMore { get { return RawSetting.Contains("focus 30+"); } }
        private static bool Focus35rMore { get { return RawSetting.Contains("focus 35+"); } }
        private static bool Focus4OrMore { get { return RawSetting.Contains("focus 40+"); } }
        private static bool Focus45rMore { get { return RawSetting.Contains("focus 45+"); } }
        private static bool Focus5OrMore { get { return RawSetting.Contains("focus 50+"); } }
        private static bool Focus55rMore { get { return RawSetting.Contains("focus 55+"); } }
        private static bool Focus6OrMore { get { return RawSetting.Contains("focus 60+"); } }
        private static bool Focus65rMore { get { return RawSetting.Contains("focus 65+"); } }
        private static bool Focus7OrMore { get { return RawSetting.Contains("focus 70+"); } }
        private static bool Focus75rMore { get { return RawSetting.Contains("focus 75+"); } }
        private static bool Focus8OrMore { get { return RawSetting.Contains("focus 80+"); } }
        private static bool Focus85rMore { get { return RawSetting.Contains("focus 85+"); } }
        private static bool Focus9OrMore { get { return RawSetting.Contains("focus 90+"); } }
        private static bool Focus95rMore { get { return RawSetting.Contains("focus 95+"); } }

        // Health
        private static bool HealthLessThan90 { get { return RawSetting.Contains("health below 90"); } }
        private static bool HealthLessThan80 { get { return RawSetting.Contains("health below 80"); } }
        private static bool HealthLessThan70 { get { return RawSetting.Contains("health below 70"); } }
        private static bool HealthLessThan60 { get { return RawSetting.Contains("health below 60"); } }
        private static bool HealthLessThan50 { get { return RawSetting.Contains("health below 50"); } }
        private static bool HealthLessThan40 { get { return RawSetting.Contains("health below 40"); } }
        private static bool HealthLessThan30 { get { return RawSetting.Contains("health below 30"); } }
        private static bool HealthLessThan20 { get { return RawSetting.Contains("health below 20"); } }

        // Target Health
        private static bool TargetHealthLessThan75 { get { return RawSetting.Contains("target health % < 75"); } }
        private static bool TargetHealthLessThan50 { get { return RawSetting.Contains("target health % < 50"); } }
        private static bool TargetHealthLessThan25 { get { return RawSetting.Contains("target health % < 25"); } }

        private static bool TargetHealthMoreThan75 { get { return RawSetting.Contains("target health % > 75"); } }
        private static bool TargetHealthMoreThan50 { get { return RawSetting.Contains("target health % > 50"); } }
        private static bool TargetHealthMoreThan25 { get { return RawSetting.Contains("target health % > 25"); } }

        // Stacks
        private static bool Stacks1OrMore { get { return RawSetting.Contains("1+ stacks"); } }
        private static bool Stacks2OrMore { get { return RawSetting.Contains("2+ stacks"); } }
        private static bool Stacks3OrMore { get { return RawSetting.Contains("3+ stacks"); } }
        private static bool Stacks4OrMore { get { return RawSetting.Contains("4+ stacks"); } }
        private static bool Stacks5OrMore { get { return RawSetting.Contains("5+ stacks"); } }

        public static bool IsOkToRun
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(RawSetting)) return false;                                                 // No string passed so nothing to check
                    if (Always || Immediately) return true;                                                             // Always means always
                    if (Never) return false;                                                                            // No means no! You men are all the same
                    if (OutOfCombat && !Me.Combat) return true;                                                         // Only if we're not in combat
                    if (RawSetting.Contains("+ adds") && !Utils.Adds) return false;
                    if (OnAdds && Utils.Adds && Me.Combat) return true;                                                               // Only if we have adds
                    if (NoAdds && !Utils.Adds && Me.Combat) return true;                                                               // Only if we DON'T have adds
                    if (Me.GotTarget && IfCastingOrRunning && (Me.CurrentTarget.IsCasting || Me.CurrentTarget.Fleeing)) return true;    // Only if the target is casting or running
                    if (OnRunners && Me.GotTarget && Me.CurrentTarget.Fleeing) return true;                                              // Only if target (NPC) is running away
                    if ((IfCasting || IsCasting) && Me.GotTarget && Me.CurrentTarget.IsCasting) return true;                                            // Only if target is casting
                    if (InInstances && Me.IsInInstance) return true;                                                    // Only if you are inside an instance
                    if (InBGs && Utils.IsBattleground) return true;                                                     // Only if you are inside a battleground
                    if (NotInBGs && !Utils.IsBattleground) return true;                                                     // Only if you are NOT inside a battleground
                    if (OnHumanoids && Me.GotTarget && Me.CurrentTarget.CreatureType == WoWCreatureType.Humanoid) return true;  // Humanoids only. Mostly for runners
                    if (OnUndead && Me.GotTarget && Me.CurrentTarget.CreatureType == WoWCreatureType.Undead) return true;  
                    if (OnDemons && Me.GotTarget && Me.CurrentTarget.CreatureType == WoWCreatureType.Demon) return true;  
                    if (OnBeasts && Me.GotTarget && Me.CurrentTarget.CreatureType == WoWCreatureType.Beast) return true;
                    if (OnElementals && Me.GotTarget && Me.CurrentTarget.CreatureType == WoWCreatureType.Elemental) return true;  
                    if (OnFleeing && Me.GotTarget && Me.CurrentTarget.Fleeing) return true;  // Is target fleeing
                    if (NotInInstances && !Me.IsInInstance) return true;
                    if (NotLowLevel && Me.GotTarget && !Target.IsLowLevel) return true;
                    if (IsHighLevel && Me.GotTarget && Target.IsHighLevel) return true;
                    if (IsInMeleeRange && Me.GotTarget && Target.Distance <= Target.InteractRange * 1.2) return true;
                    if (Me.GotTarget && IsPlayer && Target.IsPlayer) return true;
                    if (Me.GotTarget && IsNotPlayer && !Target.IsPlayer) return true;
                    if (IsInParty && Me.IsInParty) return true;
                    if (IsNotInParty && !Me.IsInParty) return true;
                    if (IsInRaid && Me.IsInRaid) return true;
                    if (IsNotInRaid && !Me.IsInRaid) return true;
                    if (Me.GotTarget && HealingSpellCast && Me.CurrentTarget.IsCasting && Utils.HealingSpells.Any(spell => Me.CurrentTarget.CastingSpell.Id == spell)) return true;
                    if (Me.GotTarget && InstanceImportantSpellCast && Me.CurrentTarget.IsCasting && Utils.ImportantInterruptSpells.Any(spell => Me.CurrentTarget.CastingSpell.Id == spell)) return true;

                    // X+ Adds
                    if (RawSetting.Contains("+ adds"))
                    {
                        if (OnAdds3OrMore && Utils.AddsCount >= 3) return true;
                        if (OnAdds4OrMore && Utils.AddsCount >= 4) return true;
                        if (OnAdds5OrMore && Utils.AddsCount >= 5) return true;
                        if (OnAdds6OrMore && Utils.AddsCount >= 6) return true;
                        if (OnAdds7OrMore && Utils.AddsCount >= 7) return true;
                        if (OnAdds8OrMore && Utils.AddsCount >= 8) return true;
                    }

                    // Target health PERCENT
                    if (RawSetting.Contains("target health %"))
                    {
                        if (!Me.GotTarget) return false;
                        if (Me.CurrentTarget.Dead) return false;

                        // Less than...
                        if (TargetHealthLessThan75 && Me.CurrentTarget.HealthPercent < 75) return true;
                        if (TargetHealthLessThan50 && Me.CurrentTarget.HealthPercent < 50) return true;
                        if (TargetHealthLessThan25 && Me.CurrentTarget.HealthPercent < 25) return true;

                        // More than...
                        if (TargetHealthMoreThan75 && Me.CurrentTarget.HealthPercent > 75) return true;
                        if (TargetHealthMoreThan50 && Me.CurrentTarget.HealthPercent > 50) return true;
                        if (TargetHealthMoreThan25 && Me.CurrentTarget.HealthPercent > 25) return true;
                        
                    }

                    // Health < X
                    if (RawSetting.Contains("health below"))
                    {
                        if (HealthLessThan90 && Me.HealthPercent < 90) return true;
                        if (HealthLessThan80 && Me.HealthPercent < 80) return true;
                        if (HealthLessThan70 && Me.HealthPercent < 70) return true;
                        if (HealthLessThan60 && Me.HealthPercent < 60) return true;
                        if (HealthLessThan50 && Me.HealthPercent < 50) return true;
                        if (HealthLessThan40 && Me.HealthPercent < 40) return true;
                        if (HealthLessThan30 && Me.HealthPercent < 30) return true;
                        if (HealthLessThan20 && Me.HealthPercent < 20) return true;
                    }

                    // If you are not a Rogue or a Druid (Cat) then don't do these checks
                    if (Me.Class == WoWClass.Rogue || Me.Class == WoWClass.Druid && Me.Shapeshift == ShapeshiftForm.Cat)
                    {
                        if (Me.ComboPoints <= 0) return false;
                        if (ComboPoints45 && Me.ComboPoints >= 4) return true;
                        if (ComboPoints34 && (Me.ComboPoints >= 3 && Me.ComboPoints <= 4)) return true;
                        if (ComboPoints23 && (Me.ComboPoints >= 2 && Me.ComboPoints <= 3)) return true;
                        if (ComboPoints12 && (Me.ComboPoints >= 1 && Me.ComboPoints <= 2)) return true;

                        if (ComboPoints1OrMore && Me.ComboPoints >= 1) return true;
                        if (ComboPoints2OrMore && Me.ComboPoints >= 2) return true;
                        if (ComboPoints3OrMore && Me.ComboPoints >= 3) return true;
                        if (ComboPoints4OrMore && Me.ComboPoints >= 4) return true;
                        if (ComboPoints5OrMore && Me.ComboPoints >= 5) return true;
                    }

                    if (Me.Class == WoWClass.Paladin)
                    {
                        //if (Me.CurrentHolyPower <= 0) return false;
                        if (HolyPower1OrMore && Me.CurrentHolyPower >= 1) return true;
                        if (HolyPower2OrMore && Me.CurrentHolyPower >= 2) return true;
                        if (HolyPower3OrMore && Me.CurrentHolyPower >= 3) return true;

                        // Other Misc
                        if (RawSetting.Contains("Sacred Duty") && IsBuffPresent("Sacred Duty")) return true;
                    }

                    if (Me.Class == WoWClass.Priest)
                    {
                        // Archangel / Evangelism
                        if (Stacks1OrMore && Self.StackCountLUA("Evangelism") >= 1) return true;
                        if (Stacks2OrMore && Self.StackCountLUA("Evangelism") >= 2) return true;
                        if (Stacks3OrMore && Self.StackCountLUA("Evangelism") >= 3) return true;
                        if (Stacks4OrMore && Self.StackCountLUA("Evangelism") >= 4) return true;
                        if (Stacks5OrMore && Self.StackCountLUA("Evangelism") >= 5) return true;

                        if (InShadowform && Self.IsBuffOnMe(15473, Self.AuraCheck.AllAuras)) return true;
                        if (NotInShadowform && !Self.IsBuffOnMe(15473, Self.AuraCheck.AllAuras)) return true;

                        //if (!Self.IsBuffOnMe(87178, Self.AuraCheck.AllAuras)) return false;
                        int mindSpikeCount = Target.DebuffStackCount("Mind Spike");
                        //int mindSpikeCount = Target.StackCount(87178);
                        if (MindSpike1OrMore && mindSpikeCount >= 1) return true;
                        if (MindSpike2OrMore && mindSpikeCount >= 2) return true;
                        if (MindSpike3OrMore && mindSpikeCount >= 3) return true;

                        // If no Shadow Orbs then bail out now. 
                        if (!Self.IsBuffOnMe(77487,Self.AuraCheck.AllAuras)) return false;

                        int orbCount = Self.StackCount(77487);
                        if (ShadowOrb1OrMore && orbCount >= 1) return true;
                        if (ShadowOrb2OrMore && orbCount >= 2) return true;
                        if (ShadowOrb3OrMore && orbCount >= 3) return true;

                    }

                    if (Me.Class == WoWClass.Hunter)
                    {
                        if (Focus2OrMore && Me.FocusPercent >= 20) return true;
                        if (Focus25rMore && Me.FocusPercent >= 25) return true;
                        if (Focus3OrMore && Me.FocusPercent >= 30) return true;
                        if (Focus35rMore && Me.FocusPercent >= 35) return true;
                        if (Focus4OrMore && Me.FocusPercent >= 40) return true;
                        if (Focus45rMore && Me.FocusPercent >= 45) return true;
                        if (Focus5OrMore && Me.FocusPercent >= 50) return true;
                        if (Focus55rMore && Me.FocusPercent >= 55) return true;
                        if (Focus6OrMore && Me.FocusPercent >= 60) return true;
                        if (Focus65rMore && Me.FocusPercent >= 65) return true;
                        if (Focus7OrMore && Me.FocusPercent >= 70) return true;
                        if (Focus75rMore && Me.FocusPercent >= 75) return true;
                        if (Focus8OrMore && Me.FocusPercent >= 80) return true;
                        if (Focus85rMore && Me.FocusPercent >= 85) return true;
                        if (Focus9OrMore && Me.FocusPercent >= 90) return true;
                        if (Focus95rMore && Me.FocusPercent >= 95) return true;

                        if (FocusFire1OrMore && Me.GotAlivePet && Me.Pet.Auras.ContainsKey("Frenzy Effect") && Me.Pet.Auras["Frenzy Effect"].StackCount >= 1) return true;
                        if (FocusFire2OrMore && Me.GotAlivePet && Me.Pet.Auras.ContainsKey("Frenzy Effect") && Me.Pet.Auras["Frenzy Effect"].StackCount >= 2) return true;
                        if (FocusFire3OrMore && Me.GotAlivePet && Me.Pet.Auras.ContainsKey("Frenzy Effect") && Me.Pet.Auras["Frenzy Effect"].StackCount >= 3) return true;
                        if (FocusFire4OrMore && Me.GotAlivePet && Me.Pet.Auras.ContainsKey("Frenzy Effect") && Me.Pet.Auras["Frenzy Effect"].StackCount >= 4) return true;
                        if (FocusFire5OrMore && Me.GotAlivePet && Me.Pet.Auras.ContainsKey("Frenzy Effect") && Me.Pet.Auras["Frenzy Effect"].StackCount >= 5) return true;
                    }
                }

                catch (Exception e)
                {
                    MethodBase currMethod = MethodBase.GetCurrentMethod();
                    //Debug.ModuleName = currMethod.Name;
                    //Debug.Catch(e);
                }

                return false;   // Otherwise its not going to happen
            }

        }

        public static bool ResultOK(string clcSettingString)
        {
            bool result = false;

            if (!clcSettingString.Contains("|"))
            {
                RawSetting = clcSettingString;
                return IsOkToRun;
            }

            bool andLogic = clcSettingString.ToUpper().StartsWith("AND|");
            string workingSettingString = clcSettingString.Replace("AND|", "");
            string[] multipleSettings = workingSettingString.Split('|');

            if (andLogic) result = true;

            foreach (string setting in multipleSettings)
            {
                RawSetting = setting;
                bool answer = IsOkToRun;
                if (!andLogic && answer) { return true; }
                if (andLogic && !answer) { return false; }
            }
            return result;
        }


        private static bool IsBuffPresent(string buffToCheck)
        {
            Lua.DoString(string.Format("buffName,_,_,stackCount,_,_,_,_,_=UnitBuff(\"player\",\"{0}\")", buffToCheck));
            string buffName = Lua.GetLocalizedText("buffName", Me.BaseAddress);
            return (buffName == buffToCheck);
        }
    }

    public static class ConfigSettings
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        public static string FileName = @"CustomClasses\Default Hunter\Class Specific\Config\Settings.xml";
        private static string _userFileName = "";
        private static XmlDocument _xmlDoc = new XmlDocument();
        private static XmlNode _xvar;
        private static string _currentEnvironment;
        public static bool UIActive { get; set; }
        public static string CurrentEnvironment
        {
            get { if (String.IsNullOrEmpty(_currentEnvironment)) _currentEnvironment = "PVE"; return _currentEnvironment; }
            set { _currentEnvironment = value; }
        }

        public static string UserFileName
        {
            get { _userFileName = FileName.Replace("Settings.xml", string.Format("Settings {0} {1}.xml", Me.Name,CurrentEnvironment));  return _userFileName; }
        }

        public static bool Open()
        {
            try
            {
                if (!System.IO.File.Exists(UserFileName)) System.IO.File.Copy(FileName, UserFileName);
                _xmlDoc.Load(UserFileName);
            }
            catch (Exception e)
            {
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(String.Format("Exception in XML Load {0}", e.Message));
                Logging.WriteDebug(" .... located in ConfigSettings.Open");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(" ");
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug("**********************************************************************");
                return false;
            }
            
            return true;
        }

        public static void Save()
        {
            _xmlDoc.Save(UserFileName);
        }

        public static void SetProperty(string nodeName, string nodeValue)
        {
            try
            {
                _xmlDoc.SelectSingleNode(nodeName).InnerText = nodeValue;
                
            }
            catch (Exception e)
            {
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(String.Format("Exception in XML Save {0}", e.Message));
                Logging.WriteDebug(" ");
                Logging.WriteDebug(" ");
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug("**********************************************************************");
            }
        }

        public static string GetStringProperty(string nodeName)
        {
            _xvar = _xmlDoc.SelectSingleNode(nodeName);
            return Convert.ToString(_xvar.InnerText);
        }

        public static bool GetBoolProperty(string nodeName)
        {
            _xvar = _xmlDoc.SelectSingleNode(nodeName);
            return Convert.ToBoolean(_xvar.InnerText);
        }

        public static int GetIntProperty(string nodeName)
        {
            _xvar = _xmlDoc.SelectSingleNode(nodeName);
            return Convert.ToInt16(_xvar.InnerText);
        }

    }

    public static class RAF
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return ObjectManager.Me.CurrentTarget; } }

        public static WoWUnit PartyMemberWithoutBuff(string buffName)
        {
            // If you're not in a party then just leave
            if (!Me.IsInParty && !Me.IsInRaid) return null;
            return Me.PartyMembers.FirstOrDefault(p => p.Distance < 40 && !p.Dead && !p.IsGhost && p.InLineOfSight && !p.Auras.ContainsKey(buffName));
        }

        public static WoWUnit BuffPlayer(string buffname)
        {
            // If you're not in a party then just leave
            if (!Me.IsInParty && !Me.IsInRaid) return null;

            // MyGroup is populated with raid members or party members, whichever you are in
            List<WoWPlayer> myPartyOrRaidGroup = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
            return (from o in myPartyOrRaidGroup let p = o.ToPlayer() where p.Distance < 30 && !p.Dead && !p.IsGhost && p.InLineOfSight && !p.ActiveAuras.ContainsKey(buffname) select p).FirstOrDefault();
        }

        public static WoWUnit HealPlayer(int minimumHealth, int maximumDistance)
        {
            // If you're not in a party then just leave
            if (!Me.IsInParty && !Me.IsInRaid) return null;

            // MyGroup is populated with raid members or party members, whichever you are in
            List<WoWPlayer> myPartyOrRaidGroup = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
            return (from o in myPartyOrRaidGroup
                    let p = o.ToPlayer() where p.Distance < maximumDistance && !p.Dead && !p.IsGhost && p.InLineOfSight && p.HealthPercent < minimumHealth orderby p.HealthPercent ascending select p).FirstOrDefault();
        }

        public static WoWUnit PartyHealerRole
        {
            get
            {
                if (!Me.IsInParty) return null;

                int i = 1;
                foreach (WoWPlayer player in Me.PartyMembers)
                {
                    string memberRole = Lua.GetReturnVal<string>(string.Format(@"return UnitGroupRolesAssigned(""party{0}"")", i), 0);

                    if (memberRole == "HEALER")
                    {
                        return player;
                    }
                    i++;
                }

                return null;
            }
        }

        public static WoWUnit PartyTankRole
        {
            get
            {
                if (!Me.IsInParty) return null;

                int i = 1;
                foreach (WoWPlayer p in Me.PartyMembers)
                {
                    string partyRole = Lua.GetReturnVal<string>(string.Format(@"return UnitGroupRolesAssigned(""party{0}"")", i), 0);

                    if (partyRole == "TANK")
                    {
                        return p;
                    }
                    i++;
                }

                return null;
            }
        }

    }

    public static class HealBot
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return ObjectManager.Me.CurrentTarget; } }
        private static WoWUnit healer { get { return RAF.PartyHealerRole; } }
        private static WoWUnit tank { get { return RAF.PartyTankRole; } }

        /// <summary>
        /// Unified Healing System
        /// </summary>
        private static List<HealingSpell> _uhs = new List<HealingSpell>();

        /// <summary>
        /// Unified Healing System
        /// </summary>
        public static List<HealingSpell> UHS { get { return _uhs; } set { _uhs = value; } }

        [DefaultValue(true)]
        public static bool HealPets { get; set; }

        public class HealingSpell
        {
            /// <summary>
            /// Name of the spell to be cast
            /// </summary>
            public string SpellName { get; set; }
            /// <summary>
            /// 0 Highest priority
            /// </summary>
            [DefaultValue(0)]
            public int Priority { get; set; }
            /// <summary>
            /// Is this a buff/HoT spell. Uses spell name to identify the buff
            /// </summary>
            [DefaultValue(false)]
            public bool IsBuff { get; set; }
            /// <summary>
            /// Check for other debuffs before casting this spell. Eg PWS needs to check for Weakened Soul
            /// </summary>
            public string OtherDebuffs{ get; set; }
            /// <summary>
            /// Only evaluated if the target is the tank
            /// </summary>
            [DefaultValue(false)]
            public bool TankOnly { get; set; }
            public double HealthPercent { get; set; }
            /// <summary>
            /// Heal pets with this spell
            /// </summary>
            [DefaultValue(false)]
            public bool IncludePets { get; set; }
            /// <summary>
            /// Minimum number of mobs attacking player
            /// </summary>
            public int MinimumMobAggroCount { get; set; }
            /// <summary>
            /// Does this spell require 'click-to-cast'
            /// </summary>
            [DefaultValue(false)]
            public bool IsGroundTargeted { get; set; }
            /// <summary>
            /// Is this an AoE spell - effects multiple players
            /// </summary>
            [DefaultValue(false)]
            public bool IsAoE { get; set; }
            /// <summary>
            /// Minimum number of players below 'HealthPercent' before spell is considered
            /// </summary>
            [DefaultValue(3)]
            public int MinimumAoECount { get; set; }

            [DefaultValue(40)]
            public int MaximumDistance { get; set; }

            /// <summary>
            /// Evaluate this spell before checking the tank. 
            /// </summary>
            [DefaultValue(false)]
            public bool EvaluateBeforeTank { get; set; }
        }

        /// <summary>
        /// Sort all spells by priority.
        /// </summary>
        public static void Sort()
        {
            UHS.Sort((hs1, hs2) => hs1.Priority.CompareTo(hs2.Priority));
        }

        /// <summary>
        /// Enumerate the list of spells and party members. Heal where required
        /// </summary>
        /// <returns></returns>
        public static bool Heal()
        {
            WoWUnit Tank = tank;    // Try to cut down on the LUA calls

            // First - Check all spells to be evaluated before the tank - typically quick cast AoE spells
            foreach (HealingSpell h in UHS.Where(h => h.EvaluateBeforeTank))
            {
                if (h.IsAoE)
                {
                    int maxDistance = h.MaximumDistance > 0 ? h.MaximumDistance : 40;
                    WoWUnit AoETarget = null;

                    if (CanCastAoE(h.SpellName, maxDistance, h.MinimumAoECount, h.HealthPercent, out AoETarget))
                    {
                        Spell.Cast(h.SpellName, AoETarget ?? Me);
                        Utils.LagSleep();
                        Utils.WaitWhileCasting();
                        return true;
                    }
                }
            }

            // Now - Heal the tank
            if (Tank != null && !Tank.Dead)
            {
                string bestSpell;
                
                // But..... before healing the tank, check if we don't have any urgent heals to cast on party members
                double urgentLowHealth = Tank.HealthPercent*0.60f;
                WoWUnit urgentHealTarget = RAF.HealPlayer((int) urgentLowHealth, 40);
                if (urgentHealTarget != null && Tank.HealthPercent > 80)
                {
                    Utils.Log( string.Format("-Urgent heal required on {0}, prioritizing over the tank", urgentHealTarget.Class), Utils.Colour("Red"));
                    bestSpell = (BestSpell(UHS, urgentHealTarget));
                    if (bestSpell != null)
                    {
                        Spell.Cast(bestSpell, urgentHealTarget, true);
                        return true;
                    }
                    Utils.Log( "*** If you are seeing this message something went wrong trying to perform a pre-tank urgent heal!", Utils.Colour("Red"));
                    Utils.Log("*** 'BestSpell' was unable to find 'the best spell'!", Utils.Colour("Red"));
                }

                // Now... lets get to healing that tank
                bestSpell = (BestSpell(UHS, Tank));
                if (bestSpell != null)
                {
                    Spell.Cast(bestSpell, Tank, true);
                    return true;
                }
            }

            // Lets do some AoE healing
            foreach (HealingSpell h in UHS.Where(h => h.IsAoE))
            {
                //int maxDistance = h.MaximumDistance > 0 ? h.MaximumDistance : 40;
                WoWUnit AoETarget = null;

                if (CanCastAoE(h.SpellName, h.MaximumDistance, h.MinimumAoECount, h.HealthPercent, out AoETarget))
                {
                    Spell.Cast(h.SpellName, AoETarget ?? Me);
                    Utils.LagSleep();
                    Utils.WaitWhileCasting();
                    return true;
                }
            }

            // Time to heal ME
            if (Me != null)
            {
                string bestSpell = (BestSpell(UHS, Me));
                if (bestSpell != null)
                {
                    Spell.Cast(bestSpell, Me, true);
                    return true;
                }
            }

            // Now if we have a Healer (which means we're backup healing) heal them
            if (healer != null && healer.Guid != Me.Guid)
            {
                string bestSpell = (BestSpell(UHS, healer));
                if (bestSpell != null)
                {
                    Spell.Cast(bestSpell, healer, true);
                    return true;
                }
            }

            // Now the remaining Party / Raid members
            List<WoWPlayer> myGroup = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
            if (myGroup.Count > 0)
            {
                WoWUnit target = RAF.HealPlayer(99, 40);
                string bestSpell = (BestSpell(UHS, target));
                if (bestSpell != null)
                {
                    Spell.Cast(bestSpell, target, true);
                    return true;
                }
            }

            // Finally, pets - we are only here because no one else required healing
            if (HealPets && (Me.PartyMembers.Any(p => p.GotAlivePet && p.Pet.HealthPercent < 95) && Spell.CanCast("Flash Heal")))
            {
                foreach (WoWPlayer p in myGroup)
                {
                    if (!p.GotAlivePet) continue;
                    if (p.GotAlivePet && p.Pet.HealthPercent > 95) continue;

                    string bestSpell = (BestSpell(UHS, p.Pet));
                    if (bestSpell != null)
                    {
                        Spell.Cast(bestSpell, p.Pet, true);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Find the most suitable spell to cast. Working from the highest priority to the lowest
        /// </summary>
        /// <param name="allHealingSpells"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static string BestSpell(List<HealingSpell> allHealingSpells, WoWUnit target)
        {
            if (target == null) return null;

            foreach (HealingSpell spell in allHealingSpells)
            {
                if (target.Distance > spell.MaximumDistance) continue;
                if (spell.IsBuff && target.Auras.ContainsKey(spell.SpellName) && target.Auras[spell.SpellName].CreatorGuid == Me.Guid) continue;
                if (target.HealthPercent > spell.HealthPercent) continue;
                if (!string.IsNullOrEmpty(spell.OtherDebuffs) && target.Auras.ContainsKey(spell.OtherDebuffs)) continue;
                if (spell.TankOnly && (tank != null && target.Guid != tank.Guid)) continue;
                if (spell.IsAoE) continue;
                if (!spell.IncludePets && target.IsPet) continue;
                if (spell.MinimumMobAggroCount > 0 && Utils.CountOfMobsAttackingPlayer(target.Guid) < spell.MinimumMobAggroCount) continue;
                if (!String.IsNullOrEmpty(spell.SpellName)) continue;
                if (!Spell.CanCast(spell.SpellName)) continue;

                Utils.Log(string.Format("----- {0} HP [{1}]  Spell: {2}", target.Name, target.HealthPercent, spell.SpellName));
                if (string.IsNullOrEmpty(spell.SpellName)) return spell.SpellName;
            }

            return null;
        }

        /// <summary>
        /// Check all parameters and determine if we can cast the AoE spell. If so, pass out the lowest health member
        /// </summary>
        /// <param name="spellName"></param>
        /// <param name="maxDistance"></param>
        /// <param name="minCount"></param>
        /// <param name="minHealth"></param>
        /// <param name="AoETarget"></param>
        /// <returns></returns>
        private static bool CanCastAoE(string spellName, int maxDistance, int minCount, double minHealth, out WoWUnit AoETarget)
        {
            AoETarget = null;
            List<WoWPlayer> countOfUnits = (from o in Me.PartyMembers let p = o.ToPlayer() where p.Distance < maxDistance && !p.Dead && !p.IsGhost && p.InLineOfSight && p.HealthPercent < minHealth orderby p.HealthPercent ascending select p).ToList();
            if (countOfUnits.Count >= minHealth) AoETarget = countOfUnits[0];

            return countOfUnits.Count >= minCount  && Spell.CanCast(spellName);
        }
    }


    

    public class GlobalHotkey
    {

        public static class Constants
        {
            //modifiers
            public const int NOMOD = 0x0000;
            public const int ALT = 0x0001;
            public const int CTRL = 0x0002;
            public const int SHIFT = 0x0004;
            public const int WIN = 0x0008;

            //windows message id for hotkey
            public const int WM_HOTKEY_MSG_ID = 0x0312;
        }

        private int modifier;
        private int key;
        private IntPtr hWnd;
        private int id;

        public GlobalHotkey(int modifier, Keys key, Form form)
        {
            this.modifier = modifier;
            this.key = (int)key;
            this.hWnd = form.Handle;
            id = this.GetHashCode();
        }

        public bool Register()
        {
            return RegisterHotKey(hWnd, id, modifier, key);
        }

        public bool Unregiser()
        {
            return UnregisterHotKey(hWnd, id);
        }

        public override int GetHashCode()
        {
            return modifier ^ key ^ hWnd.ToInt32();
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }

   
 
   
}
