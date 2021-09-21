using System.Collections.Generic;
using Styx;
using Styx.WoWInternals;

namespace HighVoltz
{
    public class DataStore : Dictionary<uint, int>
    {
        private readonly GlobalPBSettings _settings = Professionbuddy.Instance.GlobalSettings;
        private Professionbuddy _pb = Professionbuddy.Instance;

        public DataStore()
        {
            HasDataStoreAddon = false;
        }

        public bool HasDataStoreAddon { get; private set; }

        public new int this[uint index]
        {
            get { return ContainsKey(index) ? base[index] : 0; }
            set { base[index] = value; }
        }

        public void ImportDataStore()
        {
            Clear();
            int tableIndex = 1;
            if (_settings.DataStoreTable == null)
                _settings.DataStoreTable = Util.RandomString;
            string storeInTableLua =
                "if DataStoreDB and DataStore_ContainersDB  and DataStore_AuctionsDB and DataStore_MailsDB then " +
                "local realm = GetRealmName() " +
                "local faction = UnitFactionGroup('player') " +
                "local profiles = {} " +
                "local items = {} " +
                "local guilds = {} " +
                "local storeItem = function (id,cnt) id=tonumber(id) cnt=tonumber(cnt) if items[id]  then items[id] = items[id] + cnt else items[id] = cnt end end " +
                "for k,v in pairs(DataStoreDB.global.Characters) do " +
                @"local r = string.match(k,'[^%.]+%.([^%.]+)%.[^%.]+') " +
                "if r and r == realm and v and v.faction == faction then " +
                "table.insert (profiles,k) " +
                "if v.guildName then " +
                "guilds[string.format('%s.%s',realm,v.guildName)] = 1 " +
                "end " +
                "end " +
                "end " +
                "for k,v in ipairs(profiles) do " +
                "local char=DataStore_ContainersDB.global.Characters[v] " +
                "if char then " +
                "for i=-2,100 do " +
                "local x = char.Containers['Bag'..i] " +
                "if x then " +
                "for i=1, x.size do " +
                "if x.ids[i] then " +
                "storeItem (x.ids[i],x.counts[i] or 1) " +
                "end " +
                "end " +
                "end " +
                "end " +
                "end " +
                "char=DataStore_AuctionsDB.global.Characters[v] " +
                "if char and char.Auctions then " +
                "for k,v in ipairs(char.Auctions) do " +
                "storeItem(string.match(v,'%d+|(%d+)'),string.match(v,'%d+|%d+|(%d+)')) " +
                "end " +
                "end " +
                "char=DataStore_MailsDB.global.Characters[v] " +
                "if char then " +
                "for k,v in pairs(char.Mails) do " +
                "if v.link and v.count then " +
                "storeItem(string.match(v.link,'|Hitem:(%d+)'),v.count) " +
                "end " +
                "end " +
                "end " +
                "end " +
                "for k,v in pairs(DataStore_ContainersDB.global.Guilds) do " +
                "for g,_ in pairs(guilds) do " +
                "if string.find(k,g) and v.Tabs then " +
                "for k2,v2 in ipairs(v.Tabs) do " +
                "if v2 and v2.ids then " +
                "for k3,v3 in pairs(v2.ids) do " +
                "storeItem (v3,v2.counts[k3] or 1) " +
                "end " +
                "end " +
                "end " +
                "end " +
                "end " +
                "end " +
                _settings.DataStoreTable + " = {} " +
                "for k,v in pairs(items) do " +
                "table.insert(" + _settings.DataStoreTable + ",k) " +
                "table.insert(" + _settings.DataStoreTable + ",v) " +
                "end " +
                "return #" + _settings.DataStoreTable + " " +
                "end " +
                "return 0 ";

            using (new FrameLock())
            {
                List<string> retVals = Lua.GetReturnValues(storeInTableLua);
                if (retVals != null && retVals[0] != "0")
                {
                    HasDataStoreAddon = true;
                    int tableSize;
                    int.TryParse(retVals[0], out tableSize);
                    while (true)
                    {
                        string getTableDataLua =
                            "local retVals = {" + tableIndex + "} " +
                            "for i=retVals[1], #" + _settings.DataStoreTable + " do " +
                            "table.insert(retVals," + _settings.DataStoreTable + "[i]) " +
                            "if #retVals >= 501 then " +
                            "retVals[1] = i +1 " +
                            "return unpack(retVals) " +
                            "end " +
                            "end " +
                            "retVals[1] = #" + _settings.DataStoreTable + " " +
                            "return unpack(retVals) ";
                        retVals = Lua.GetReturnValues(getTableDataLua);
                        int.TryParse(retVals[0], out tableIndex);
                        for (int i = 2; i < retVals.Count; i += 2)
                        {
                            uint id, num;
                            uint.TryParse(retVals[i - 1], out id);
                            uint.TryParse(retVals[i], out num);
                            this[id] = (int) num;
                        }
                        if (tableIndex >= tableSize)
                            break;
                    }
                    Lua.DoString(_settings.DataStoreTable + "={}");
                    Professionbuddy.Debug("DataStore Imported");
                }
                else
                {
                    Professionbuddy.Debug("No DataStore Addon found");
                }
            }
        }
    }
}