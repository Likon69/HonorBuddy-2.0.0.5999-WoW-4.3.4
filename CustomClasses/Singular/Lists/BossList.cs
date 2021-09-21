#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: apoc $
// $Date: 2011-12-11 11:22:31 +0200 (Paz, 11 Ara 2011) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Lists/BossList.cs $
// $LastChangedBy: apoc $
// $LastChangedDate: 2011-12-11 11:22:31 +0200 (Paz, 11 Ara 2011) $
// $LastChangedRevision: 450 $
// $Revision: 450 $

#endregion

using System.Collections.Generic;


namespace Singular.Lists
{
    public static class BossList
    {
        #region Boss Entries

        static BossList()
        {
            foreach (var bossId in _dummies)
            {
                _bosses.Add(bossId);
            }
        }

        public static HashSet<uint> BossIds
        {
            get { return _bosses; }
        }

        public static HashSet<uint> TrainingDummies{get { return _dummies; }}

        private static HashSet<uint>  _dummies = new HashSet<uint>
            {
                31146, // Raider's
                46647, // 81-85
                32546, // Ebon Knight's (DK)
                31144, // 79-80
                32543, // Veteran's (Eastern Plaguelands)
                32667, // 70
                32542, // 65 EPL
                32666, // 60
                30527, // ?? Boss one (no idea?)
            };

        private static HashSet<uint> _bosses = new HashSet<uint>
                    {
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
                        52363, // Occu'thar

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
                        18402, //Warmaul Champion

                        // Cata Zul'gurub
                        52155, //High Priest Venoxis
                        52151, //Bloodlord Mandokir
                        52271, //Hazza'ra
                        52059, //High Priestess Kilnara
                        52053, //Zanzil
                        52148, //Jin'do the Godbreaker

                        //Firelands
                        53691, //Shannox
                        52558, //Lord Rhyolith
                        52498, //Beth'tilac
                        52530, //Alysrazor
                        53494, //Baleroc
                        52571, //Majordomo Staghelm
                        52409, //Ragnaros

                        //Dragon Soul
                        55265, // Morchok
                        57773, // Kohcrom (Heroic Morchok encounter)
                        55308, // Zon'ozz
                        55312, // Yor'sahj
                        55689, // Hagara
                        55294, // Ultraxion
                        56427, // Blackhorn

                        56846, // Arm Tentacle -- Madness of DW
                        56167, // Arm Tentacle -- Madness of DW
                        56168, // Wing Tentacle - Madness of DW
                        57962, // Deathwing ----- Madness of DW (his head)
                    };

        #endregion
    }
}