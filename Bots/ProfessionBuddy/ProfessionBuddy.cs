//!CompilerOption:Optimize:On
// Professionbuddy botbase by HighVoltz

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Linq;
using HighVoltz.Composites;
using HighVoltz.Dynamic;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Profiles;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Tripper.Tools.Math;
using Action = System.Action;
using Application = System.Windows.Application;
using Color = System.Drawing.Color;
using RichTextBox = System.Windows.Controls.RichTextBox;
using Timer = System.Threading.Timer;

namespace HighVoltz
{
    public class Professionbuddy : BotBase
    {
        #region Declarations 

        private const string _name = "ProfessionBuddy";
        private const string PbSvnUrl = "http://professionbuddy.googlecode.com/svn/trunk/Professionbuddy/";
        public static readonly string BotPath = Logging.ApplicationPath + @"\Bots\" + _name;
        private static readonly LocalPlayer Me = ObjectManager.Me;
        public static readonly Svn Svn = new Svn();

        public readonly string ProfilePath = Environment.UserName == "highvoltz"
                                                 ? @"C:\Users\highvoltz\Desktop\Buddy\Projects\Professionbuddy\Profiles"
                                                 : Path.Combine(BotPath, "Profiles");

        private readonly bool _ctorRunOnce;
        private readonly PbProfileSettings _profileSettings = new PbProfileSettings();
        public bool IsRunning;

        public Professionbuddy()
        {
            Instance = this;
            new Thread((ThreadStart) delegate
                                         {
                                             try
                                             {
                                                 ProcessModule mod = Process.GetCurrentProcess().MainModule;
                                                 using (HashAlgorithm hashAlg = new SHA1Managed())
                                                 {
                                                     using (
                                                         Stream file = new FileStream(mod.FileName, FileMode.Open,
                                                                                      FileAccess.Read))
                                                     {
                                                         byte[] hash = hashAlg.ComputeHash(file);
                                                         Logging.WriteDebug("H: {0}", BitConverter.ToString(hash));
                                                     }
                                                 }
                                                 FileVersionInfo vInfo = mod.FileVersionInfo;
                                                 Logging.WriteDebug("V: {0}", vInfo.FileVersion);
                                             }
                                             catch (Exception ex)
                                             {
                                                 Err(ex.ToString());
                                             }
                                         }).Start();
            // Initialize is called when bot is started.. we need to hook these events before that.
            if (!_ctorRunOnce)
            {
                BotEvents.Profile.OnNewOuterProfileLoaded += Profile_OnNewOuterProfileLoaded;
                Profile.OnUnknownProfileElement += Profile_OnUnknownProfileElement;
                _ctorRunOnce = true;
            }
        }

        public ProfessionBuddySettings MySettings { get; private set; }
        public GlobalPBSettings GlobalSettings { get; private set; }
        public List<TradeSkill> TradeSkillList { get; private set; }
        public Dictionary<string, string> Strings { get; private set; }

        // <itemId,count>
        public DataStore DataStore { get; private set; }
        // dictionary that keeps track of material list using item ID for key and number required as value
        public Dictionary<uint, int> MaterialList { get; private set; }
        public List<uint> ProtectedItems { get; private set; }
        public bool IsTradeSkillsLoaded { get; private set; }

        // ReSharper disable InconsistentNaming
        // DataStore is an addon for WOW thats stores bag/ah/mail item info and more.
        public bool HasDataStoreAddon
        {
            get { return DataStore != null && DataStore.HasDataStoreAddon; }
        }

        // profile Settings.

        public PbProfileSettings ProfileSettings
        {
            get { return _profileSettings; }
        }

        // static instance
        public static Professionbuddy Instance { get; private set; }

        private Version Version
        {
            get { return new Version(1, Svn.Revision); }
        }

        public event EventHandler OnTradeSkillsLoaded;

        // test some culture specific stuff.

        #endregion

        #region Overrides

        private bool _firstStartDone;
        private InvisiForm _ivisibleForm;

        public override string Name
        {
            get { return _name; }
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All; }
        }

        public override Form ConfigurationForm
        {
            get
            {
                if (!_init)
                    Init();
                return _ivisibleForm ?? (_ivisibleForm = new InvisiForm());
            }
        }

        public override void Start()
        {
            Debug("Start Called");
            IsRunning = true;
            // reattach lua events on bot start in case it they get destroyed from loging out of game
            Lua.Events.DetachEvent("BAG_UPDATE", OnBagUpdate);
            Lua.Events.DetachEvent("SKILL_LINES_CHANGED", OnSkillUpdate);
            Lua.Events.DetachEvent("SPELLS_CHANGED", OnSpellsChanged);
            Lua.Events.DetachEvent("BANKFRAME_OPENED", Util.OnBankFrameOpened);
            Lua.Events.DetachEvent("BANKFRAME_CLOSED", Util.OnBankFrameClosed);

            Lua.Events.AttachEvent("BAG_UPDATE", OnBagUpdate);
            Lua.Events.AttachEvent("SKILL_LINES_CHANGED", OnSkillUpdate);
            Lua.Events.AttachEvent("SPELLS_CHANGED", OnSpellsChanged);
            Lua.Events.AttachEvent("BANKFRAME_OPENED", Util.OnBankFrameOpened);
            Lua.Events.AttachEvent("BANKFRAME_CLOSED", Util.OnBankFrameClosed);

            if (!_isChangingBot)
            {
                // reset all actions 
                PbBehavior.Reset();
                if (DynamicCodeCompiler.CodeWasModified)
                {
                    DynamicCodeCompiler.GenorateDynamicCode();
                }
            }

            if (MainForm.IsValid)
                MainForm.Instance.UpdateControls();

            if (!_firstStartDone)
            {
                try
                {
                    if (!string.IsNullOrEmpty(_profileToLoad))
                    {
                        LoadPBProfile(_profileToLoad);
                        LastProfileIsHBProfile = false;
                    }
                    else if (!string.IsNullOrEmpty(MySettings.LastProfile))
                    {
                        LoadPBProfile(MySettings.LastProfile);
                    }
                }
                catch (Exception ex)
                {
                    Err(ex.ToString());
                }
                _firstStartDone = true;
            }
            try
            {
                if (SecondaryBot != null)
                    SecondaryBot.Start();
            }
            catch (Exception ex)
            {
                Logging.WriteDebug(ex.ToString());
            }
        }

        public override void Stop()
        {
            IsRunning = false;
            Debug("Stop Called");
            if (MainForm.IsValid)
                MainForm.Instance.UpdateControls();
            if (SecondaryBot != null)
                SecondaryBot.Stop();
        }

        // used as a hack to get a modeless form for pb's GUI window. 

        public override void Pulse()
        {
            // Due to some non-thread safe HB api I need to make sure the callbacks are executed from main thread
            // Throttling the events so the callback doesn't get called repeatedly in a short time frame.
            if (_onBagUpdateSpamSW.ElapsedMilliseconds >= 1000)
            {
                _onBagUpdateSpamSW.Stop();
                _onBagUpdateSpamSW.Reset();
                OnBagUpdateTimerCB();
            }
            if (_onSkillUpdateSpamSW.ElapsedMilliseconds >= 1000)
            {
                _onSkillUpdateSpamSW.Stop();
                _onSkillUpdateSpamSW.Reset();
                OnSkillUpdateTimerCB();
            }
            if (_onSpellsChangedSpamSw.ElapsedMilliseconds >= 1000)
            {
                _onSpellsChangedSpamSw.Stop();
                _onSpellsChangedSpamSw.Reset();
                OnSpellsChangedCB();
            }
            if (SecondaryBot != null)
                SecondaryBot.Pulse();
        }

        public override void Initialize()
        {
            Init();
        }

        private class InvisiForm : Form
        {
            public InvisiForm()
            {
                Size = new Size(0, 0);
            }

            protected override void OnLoad(EventArgs e)
            {
                if (!MainForm.IsValid)
                {
                    new MainForm {TopLevel = true}.Show();
                    MainForm.Instance.TopLevel = true;
                }
                DialogResult = DialogResult.OK;
                MainForm.Instance.Activate();
            }
        }

        #endregion

        #region Callbacks

        #region OnBagUpdate

        private readonly Stopwatch _onBagUpdateSpamSW = new Stopwatch();

        private void OnBagUpdate(object obj, LuaEventArgs args)
        {
            if (!_onBagUpdateSpamSW.IsRunning)
            {
                Lua.Events.DetachEvent("BAG_UPDATE", OnBagUpdate);
                _onBagUpdateSpamSW.Start();
            }
        }

        private void OnBagUpdateTimerCB()
        {
            try
            {
                Lua.Events.AttachEvent("BAG_UPDATE", OnBagUpdate);
                foreach (TradeSkill ts in TradeSkillList)
                {
                    ts.PulseBags();
                }
                UpdateMaterials();
                if (MainForm.IsValid)
                {
                    MainForm.Instance.RefreshTradeSkillTabs();
                    MainForm.Instance.RefreshActionTree(typeof (CastSpellAction));
                }
            }
            catch (Exception ex)
            {
                Err(ex.ToString());
            }
        }

        #endregion

        #region OnSkillUpdate

        private readonly Stopwatch _onSkillUpdateSpamSW = new Stopwatch();

        private void OnSkillUpdate(object obj, LuaEventArgs args)
        {
            foreach (object o in args.Args)
                Debug("spell changed {0}", o);
            if (!_onSkillUpdateSpamSW.IsRunning)
            {
                Lua.Events.DetachEvent("SKILL_LINES_CHANGED", OnSkillUpdate);
                _onSkillUpdateSpamSW.Start();
            }
        }

        private void OnSkillUpdateTimerCB()
        {
            Lua.Events.AttachEvent("SKILL_LINES_CHANGED", OnSkillUpdate);
            try
            {
                UpdateMaterials();
                // check if there was any tradeskills added or removed.
                WoWSkill[] skills = SupportedTradeSkills;
                bool changed = skills.
                                   Count(s => TradeSkillList.Count(l => l.SkillLine == (SkillLine) s.Id) == 1) !=
                               TradeSkillList.Count ||
                               skills.Length != TradeSkillList.Count;
                if (changed)
                {
                    Debug("A profession was added or removed. Reloading Tradeskills (OnSkillUpdateTimerCB)");
                    OnTradeSkillsLoaded += Professionbuddy_OnTradeSkillsLoaded;
                    LoadTradeSkills();
                }
                else
                {
                    Debug("Updated tradeskills from OnSkillUpdateTimerCB");
                    foreach (TradeSkill ts in TradeSkillList)
                    {
                        ts.PulseSkill();
                    }
                    if (MainForm.IsValid)
                    {
                        MainForm.Instance.RefreshTradeSkillTabs();
                        MainForm.Instance.RefreshActionTree(typeof (CastSpellAction));
                    }
                }
            }
            catch (Exception ex)
            {
                Err(ex.ToString());
            }
        }

        public void Professionbuddy_OnTradeSkillsLoaded(object sender, EventArgs e)
        {
            if (MainForm.IsValid)
                MainForm.Instance.InitTradeSkillTab();
            OnTradeSkillsLoaded -= Professionbuddy_OnTradeSkillsLoaded;
        }

        #endregion

        #region OnSpellsChanged

        private readonly Stopwatch _onSpellsChangedSpamSw = new Stopwatch();

        private void OnSpellsChanged(object obj, LuaEventArgs args)
        {
            if (!_onSpellsChangedSpamSw.IsRunning)
            {
                Lua.Events.DetachEvent("SPELLS_CHANGED", OnSpellsChanged);
                _onSpellsChangedSpamSw.Start();
            }
        }

        private void OnSpellsChangedCB()
        {
            try
            {
                Lua.Events.AttachEvent("SPELLS_CHANGED", OnSpellsChanged);
                Debug("Pulsing Tradeskills from OnSpellsChanged");
                foreach (TradeSkill ts in TradeSkillList)
                {
                    ts.PulseSkill();
                }
                if (MainForm.IsValid)
                {
                    MainForm.Instance.InitTradeSkillTab();
                    MainForm.Instance.RefreshActionTree(typeof (CastSpellAction));
                }
            }
            catch (Exception ex)
            {
                Err(ex.ToString());
            }
        }

        #endregion

        // Used as a fix when profile is loaded before Inititialize is called
        private static string _profileToLoad = "";
        private static string _lastProfilePath = "";
        internal static bool LastProfileIsHBProfile;

        private void Profile_OnUnknownProfileElement(object sender, UnknownProfileElementEventArgs e)
        {
            if (e.Element.Ancestors("Professionbuddy").Any())
            {
                e.Handled = true;
            }
        }

        //private static bool _botIsStartingUp;

        private static void Profile_OnNewOuterProfileLoaded(BotEvents.Profile.NewProfileLoadedEventArgs args)
        {
            if (args.NewProfile.XmlElement.Name == "Professionbuddy")
            {
                // prevents HB from reloading current profile when bot is started.
                if (!Instance.IsRunning && ProfileManager.XmlLocation == Instance.CurrentProfile.XmlPath)
                    return;
                if (_init)
                {
                    if (_isChangingBot)
                        return;
                    if (Instance.IsRunning)
                    {
                        try
                        {
                            Application.Current.Dispatcher.Invoke(
                                new Action(() =>
                                               {
                                                   TreeRoot.Stop();
                                                   LoadPBProfile(ProfileManager.XmlLocation);
                                                   if (MainForm.IsValid)
                                                   {
                                                       if (Instance.ProfileSettings.SettingsDictionary.Count > 0)
                                                           MainForm.Instance.AddProfileSettingsTab();
                                                       else
                                                           MainForm.Instance.RemoveProfileSettingsTab();
                                                   }
                                                   TreeRoot.Start();
                                               }
                                    ));
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        LoadPBProfile(ProfileManager.XmlLocation);
                        if (MainForm.IsValid)
                        {
                            if (Instance.ProfileSettings.SettingsDictionary.Count > 0)
                                MainForm.Instance.AddProfileSettingsTab();
                            else
                                MainForm.Instance.RemoveProfileSettingsTab();
                        }
                    }
                    LastProfileIsHBProfile = false;
                }
                else
                {
                    _profileToLoad = ProfileManager.XmlLocation;
                }
            }
            else if (args.NewProfile.XmlElement.Name == "HBProfile")
            {
                LastProfileIsHBProfile = true;
                _lastProfilePath = ProfileManager.XmlLocation;
            }
        }

        #endregion

        #region Behavior Tree

        private readonly PbProfile _currentProfile = new PbProfile();

        private readonly PbRootComposite _root = new PbRootComposite(new PbDecorator(), null);

        public PbProfile CurrentProfile
        {
            get { return _currentProfile; }
        }

        public override Composite Root
        {
            get { return _root; }
        }

        public PbDecorator PbBehavior
        {
            get { return _root.PbBotBase; }
            private set { _root.PbBotBase = value; }
        }


        public BotBase SecondaryBot
        {
            get { return _root.SecondaryBot; }
            set { _root.SecondaryBot = value; }
        }

        #endregion

        #region Misc

        private static bool _init;
        private static bool _isChangingBot;

        private WoWSkill[] SupportedTradeSkills
        {
            get
            {
                IEnumerable<WoWSkill> skillList = from skill in TradeSkill.SupportedSkills
                                                  where
                                                      skill != SkillLine.Archaeology && Me.GetSkill(skill).MaxValue > 0
                                                  select Me.GetSkill(skill);
                return skillList.ToArray();
            }
        }

        private void Init()
        {
            try
            {
                if (!_init)
                {
                    Debug("Initializing ...");
                    if (!Directory.Exists(BotPath))
                        Directory.CreateDirectory(BotPath);
                    DynamicCodeCompiler.WipeTempFolder();
                    // force Tripper.Tools.dll to load........
                    new Vector3(0, 0, 0);

                    MySettings = new ProfessionBuddySettings(
                        Path.Combine(Logging.ApplicationPath, string.Format(@"Settings\{0}\{0}[{1}-{2}].xml",
                                                                            Name, Me.Name,
                                                                            Lua.GetReturnVal<string>(
                                                                                "return GetRealmName()", 0)))
                        );
                    GlobalSettings = new GlobalPBSettings(
                        Path.Combine(Logging.ApplicationPath, string.Format(@"Settings\{0}\{0}.xml", Name)));

                    IsTradeSkillsLoaded = false;
                    MaterialList = new Dictionary<uint, int>();
                    TradeSkillList = new List<TradeSkill>();
                    LoadProtectedItems();
                    LoadTradeSkills();
                    DataStore = new DataStore();
                    DataStore.ImportDataStore();
                    // load localized strings
                    LoadStrings();
                    BotBase bot =
                        BotManager.Instance.Bots.Values.FirstOrDefault(
                            b =>
                            b.Name.IndexOf(MySettings.LastBotBase, StringComparison.InvariantCultureIgnoreCase) >= 0);
                    if (bot != null)
                        _root.SecondaryBot = bot;
                    // check for Professionbuddy updates
                    new Thread(Updater.CheckForUpdate) {IsBackground = true}.Start();
                    _init = true;
                }
            }
            catch (Exception ex)
            {
                Logging.Write(Color.Red, ex.ToString());
            }
        }

        private void LoadStrings()
        {
            Strings = new Dictionary<string, string>();
            string directory = Path.Combine(BotPath, "Localization");
            string defaultStringsPath = Path.Combine(directory, "Strings.xml");
            LoadStringsFromXml(defaultStringsPath);
            // file that includes language and country/region
            string langAndCountryFile = Path.Combine(directory,
                                                     "Strings." + Thread.CurrentThread.CurrentUICulture.Name + ".xml");
            // file that includes language only;
            string langOnlyFile = Path.Combine(directory,
                                               "Strings." +
                                               Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName + ".xml");
            if (File.Exists(langAndCountryFile))
            {
                Log("Loading strings for language {0}", Thread.CurrentThread.CurrentUICulture.Name);
                LoadStringsFromXml(langAndCountryFile);
            }
            else if (File.Exists(langOnlyFile))
            {
                Log("Loading strings for language {0}", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
                LoadStringsFromXml(langOnlyFile);
            }
        }

        private void LoadStringsFromXml(string path)
        {
            XElement root = XElement.Load(path);
            foreach (XElement element in root.Elements())
            {
                Strings[element.Name.ToString()] = element.Value;
            }
        }

        public void LoadTradeSkills()
        {
            new Timer(state =>
                          {
                              try
                              {
                                  lock (TradeSkillList)
                                  {
                                      TradeSkillList.Clear();
                                      //IEnumerable<WoWSkill> skillList = from skill in TradeSkill.SupportedSkills
                                      //                                  select Me.GetSkill(skill);

                                      //foreach (WoWSkill skill in skillList)
                                      //{
                                      //    Log("Adding TradeSkill {0}", skill.Name);
                                      //    TradeSkill ts = TradeSkill.GetTradeSkill((SkillLine)skill.Id);
                                      //    if (ts != null)
                                      //    {
                                      //        TradeSkillList.Add(ts);
                                      //    }
                                      //    else
                                      //    {
                                      //        IsTradeSkillsLoaded = false;
                                      //        Log("Unable to load tradeskill {0}", (SkillLine)skill.Id);
                                      //        return;
                                      //    }
                                      //}
                                      foreach (WoWSkill skill in SupportedTradeSkills)
                                      {
                                          Log("Adding TradeSkill {0}", skill.Name);
                                          TradeSkill ts = TradeSkill.GetTradeSkill((SkillLine) skill.Id);
                                          if (ts != null)
                                          {
                                              TradeSkillList.Add(ts);
                                          }
                                          else
                                          {
                                              IsTradeSkillsLoaded = false;
                                              Log("Unable to load tradeskill {0}", (SkillLine) skill.Id);
                                              return;
                                          }
                                      }
                                  }
                                  Log("Done Loading Tradeskills.");
                                  IsTradeSkillsLoaded = true;
                                  if (OnTradeSkillsLoaded != null)
                                  {
                                      OnTradeSkillsLoaded(this, null);
                                  }
                              }
                              catch (Exception ex)
                              {
                                  Logging.Write(Color.Red, ex.ToString());
                              }
                          }, null, 0, Timeout.Infinite);
        }

        public void UpdateMaterials()
        {
            if (!_init)
                return;
            try
            {
                lock (MaterialList)
                {
                    MaterialList.Clear();
                    List<CastSpellAction> castSpellList =
                        CastSpellAction.GetCastSpellActionList(PbBehavior);
                    if (castSpellList != null)
                    {
                        foreach (CastSpellAction ca in castSpellList)
                        {
                            if (ca.IsRecipe && ca.RepeatType != CastSpellAction.RepeatCalculationType.Craftable)
                            {
                                foreach (Ingredient ingred in ca.Recipe.Ingredients)
                                {
                                    MaterialList[ingred.ID] = MaterialList.ContainsKey(ingred.ID)
                                                                  ? MaterialList[ingred.ID] +
                                                                    (ca.CalculatedRepeat > 0
                                                                         ? (int) ingred.Required*
                                                                           (ca.CalculatedRepeat - ca.Casted)
                                                                         : 0)
                                                                  : (ca.CalculatedRepeat > 0
                                                                         ? (int) ingred.Required*
                                                                           (ca.CalculatedRepeat - ca.Casted)
                                                                         : 0);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Err(ex.ToString());
            }
        }

        public static void LoadPBProfile(string path)
        {
            bool preloadedHBProfile = false;
            if (File.Exists(path))
            {
                Log("Loading profile {0}", Path.GetFileName(path));
                PbDecorator idComp = Instance.CurrentProfile.LoadFromFile(path);
                if (idComp != null)
                {
                    Instance.PbBehavior = idComp;
                    Instance.MySettings.LastProfile = path;
                    Instance.ProfileSettings.Load();
                    DynamicCodeCompiler.GenorateDynamicCode();
                    Instance.UpdateMaterials();
                    preloadedHBProfile = PreLoadHbProfile();
                    if (MainForm.IsValid)
                    {
                        MainForm.Instance.InitActionTree();
                        MainForm.Instance.RefreshTradeSkillTabs();
                    }
                }
            }
            else
            {
                Err("Profile: {0} does not exist", path);
                Instance.MySettings.LastProfile = path;
                return;
            }
            if (MainForm.IsValid)
                MainForm.Instance.UpdateControls();
            if (!preloadedHBProfile && LastProfileIsHBProfile && !string.IsNullOrEmpty(_lastProfilePath))
                ProfileManager.LoadNew(_lastProfilePath, true);
            Instance.MySettings.Save();
        }

        public static void ChangeSecondaryBot(string botName)
        {
            BotBase bot =
                BotManager.Instance.Bots.Values.FirstOrDefault(
                    b => b.Name.IndexOf(botName, StringComparison.InvariantCultureIgnoreCase) >= 0);

            if (bot != null)
            {
                if (Instance.SecondaryBot != null && Instance.SecondaryBot.Name != bot.Name ||
                    Instance.SecondaryBot == null)
                {
                    Instance.IsRunning = false;
                    // execute from GUI thread since this thread will get aborted when switching bot
                    Application.Current.Dispatcher.Invoke(
                        new Action(() =>
                                       {
                                           _isChangingBot = true;
                                           bool isRunning = TreeRoot.IsRunning;
                                           if (isRunning)
                                               TreeRoot.Stop();
                                           Instance.SecondaryBot = bot;
                                           if (!bot.Initialized)
                                               bot.Initialize();
                                           if (ProfessionBuddySettings.Instance.LastBotBase != bot.Name)
                                           {
                                               ProfessionBuddySettings.Instance.LastBotBase = bot.Name;
                                               ProfessionBuddySettings.Instance.Save();
                                           }
                                           if (MainForm.IsValid)
                                               MainForm.Instance.UpdateBotCombo();
                                           if (isRunning)
                                               TreeRoot.Start();
                                           _isChangingBot = false;
                                       }
                            ));
                    Log("Changing SecondaryBot to {0}", botName);
                }
            }
            else
                Err("Bot with name: {0} was not found", botName);
        }

        // returns true if a profile was preloaded
        private static bool PreLoadHbProfile()
        {
            if (!string.IsNullOrEmpty(Instance.CurrentProfile.ProfilePath) && Instance.PbBehavior != null)
            {
                var dict = new Dictionary<string, Uri>();
                PbProfile.GetHbprofiles(Instance.CurrentProfile.ProfilePath, Instance.PbBehavior, dict);
                if (dict.Count > 0)
                {
                    foreach (var kv in dict)
                    {
                        if (!string.IsNullOrEmpty(kv.Key) && File.Exists(kv.Key))
                        {
                            Log("Preloading profile {0}", kv.Key);
                            // unhook event to prevent recursive loop
                            BotEvents.Profile.OnNewOuterProfileLoaded -= Profile_OnNewOuterProfileLoaded;
                            ProfileManager.LoadNew(kv.Key, true);
                            BotEvents.Profile.OnNewOuterProfileLoaded += Profile_OnNewOuterProfileLoaded;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal static List<T> GetListOfActionsByType<T>(Composite comp, List<T> list) where T : Composite
        {
            if (list == null)
                list = new List<T>();
            if (comp.GetType() == typeof (T))
            {
                list.Add((T) comp);
            }
            var groupComposite = comp as GroupComposite;
            if (groupComposite != null)
            {
                foreach (Composite c in groupComposite.Children)
                {
                    GetListOfActionsByType(c, list);
                }
            }
            return list;
        }

        private void LoadProtectedItems()
        {
            List<uint> tempList = null;
            string path = Path.Combine(BotPath, "Protected Items.xml");
            if (File.Exists(path))
            {
                XElement xml = XElement.Load(path);
                tempList = xml.Elements("Item").Select(x =>
                                                           {
                                                               XAttribute xAttribute = x.Attribute("Entry");
                                                               return xAttribute != null ? xAttribute.Value.ToUint() : 0;
                                                           }).Distinct().ToList();
            }
            ProtectedItems = tempList ?? new List<uint>();
        }

        #endregion

        #region Utilies

        private static string _logHeader;
        private static RichTextBox _rtbLog;

        private static string Header
        {
            get { return _logHeader ?? (_logHeader = string.Format("PB {0}: ", Instance.Version)); }
        }

        public static void Err(string format, params object[] args)
        {
            Logging.Write(Color.Red, "Err: " + format, args);
        }

        public static void Log(string format, params object[] args)
        {
            //Logging.Write(System.Drawing.Color.DodgerBlue, string.Format("PB {0}:", Instance.Version) + format, args);
            LogInvoker(Color.DodgerBlue, Header, Color.LightSteelBlue, format, args);
        }

        public static void Log(Color headerColor, string header, Color msgColor, string format, params object[] args)
        {
            LogInvoker(headerColor, header, msgColor, format, args);
        }

        public static void Debug(string format, params object[] args)
        {
            Logging.WriteDebug(Color.DodgerBlue, string.Format("PB {0}:", Instance.Version) + format, args);
        }

        private static void LogInvoker(Color headerColor, string header, Color msgColor, string format,
                                       params object[] args)
        {
            if (Application.Current.Dispatcher.Thread == Thread.CurrentThread)
                LogInternal(headerColor, header, msgColor, format, args);
            else
                Application.Current.Dispatcher.BeginInvoke(new LogDelegate(LogInternal), headerColor, header, msgColor,
                                                           format, args);
        }

        private static void LogInternal(Color headerColor, string header, Color msgColor, string format,
                                        params object[] args)
        {
            try
            {
                if (_rtbLog == null)
                    _rtbLog = (RichTextBox) Application.Current.MainWindow.FindName("rtbLog");
                System.Windows.Media.Color headerColorMedia = System.Windows.Media.Color.FromArgb(headerColor.A,
                                                                                                  headerColor.R,
                                                                                                  headerColor.G,
                                                                                                  headerColor.B);
                System.Windows.Media.Color msgColorMedia = System.Windows.Media.Color.FromArgb(msgColor.A, msgColor.R,
                                                                                               msgColor.G, msgColor.B);

                var headerTR = new TextRange(_rtbLog.Document.ContentEnd, _rtbLog.Document.ContentEnd) {Text = header};
                headerTR.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(headerColorMedia));

                var messageTR = new TextRange(_rtbLog.Document.ContentEnd, _rtbLog.Document.ContentEnd);
                string msg = String.Format(format, args);
                messageTR.Text = msg + Environment.NewLine;
                messageTR.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(msgColorMedia));
                Logging.WriteDebug(header + msg);
            }
            catch
            {
                Logging.Write("PB: " + format, args);
            }
        }

        private delegate void LogDelegate(
            Color headerColor, string header, Color msgColor, string format, params object[] args);

        #endregion
    }
}