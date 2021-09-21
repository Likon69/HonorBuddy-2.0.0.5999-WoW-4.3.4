// Behavior originally contributed by Chinajade.
//
// LICENSE:
// This work is licensed under the 
//     Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// also known as CC-BY-NC-SA.  To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/3.0/
// or send a letter to
//      Creative Commons
//      171 Second Street, Suite 300
//      San Francisco, California, 94105, USA. 
//
// DOCUMENTATION:
//     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_UserSettings
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Styx;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Questing;


namespace BuddyWiki.CustomBehavior.UserSettings
{
    class UserSettings : CustomForcedBehavior
    {
        // To add, adjust, or remove presets, this is the only method that needs to be modified...
        // All other code in this class uses the information contained in the returned presetChangeRequests.
        // Note: If you make a spelling error while maintaining this code, by design an exception will be thrown
        // at runtime pointing you directly to the problem.
        private static Dictionary<string, ConfigurationChangeRequest> UtilBuildPresetChangeRequests(Dictionary<string, ConfigDescriptor> configurationSettings,
                                                                                                      ConfigSnapshot originalConfiguration)
        {
            Dictionary<string, ConfigurationChangeRequest> presets = new Dictionary<string, ConfigurationChangeRequest>();

            presets.Add("Grind",
                        new ConfigurationChangeRequest(configurationSettings)
                            .Add("GroundMountFarmingMode", false)
                            .Add("KillBetweenHotspots", true)
                            .Add("PullDistance", 50)
                            .Add("UseMount", false)
                        );

            presets.Add("HarvestsOff",
                        new ConfigurationChangeRequest(configurationSettings)
                            .Add("HarvestHerbs", false)
                            .Add("HarvestMinerals", false)
                            .Add("LootChests", false)
                            .Add("LootMobs", false)
                            .Add("NinjaSkin", false)
                            .Add("SkinMobs", false)
                        );

            presets.Add("HarvestsOn",
                        new ConfigurationChangeRequest(configurationSettings)
                            .Add("HarvestHerbs", (StyxWoW.Me.GetSkill(Styx.SkillLine.Herbalism).MaxValue > 0))
                            .Add("HarvestMinerals", (StyxWoW.Me.GetSkill(Styx.SkillLine.Mining).MaxValue > 0))
                            .Add("LootChests", true)
                            .Add("LootMobs", true)
                            .Add("LootRadius", 45)
                            .Add("NinjaSkin", (StyxWoW.Me.GetSkill(Styx.SkillLine.Skinning).MaxValue > 0))
                            .Add("SkinMobs", (StyxWoW.Me.GetSkill(Styx.SkillLine.Skinning).MaxValue > 0))
                        );

            presets.Add("NoDistractions",
                        new ConfigurationChangeRequest(configurationSettings)
                            .Add("GroundMountFarmingMode", true)
                            .Add("HarvestHerbs", false)
                            .Add("HarvestMinerals", false)
                            .Add("KillBetweenHotspots", false)
                            .Add("LootChests", false)
                            .Add("LootMobs", false)
                            .Add("NinjaSkin", false)
                            .Add("PullDistance", 1)
                            .Add("SkinMobs", false)
                            .Add("TrainNewSkills", false)
                        );

            presets.Add("NoTrain",
                        new ConfigurationChangeRequest(configurationSettings)
                            .Add("TrainNewSkills", false)
                            .Add("FindVendorsAutomatically", false)
                        );

            presets.Add("NormalQuesting",
                        new ConfigurationChangeRequest(configurationSettings)
                            .Add("FindMountAutomatically", true)
                            .Add("FindVendorsAutomatically", true)
                            .Add("GroundMountFarmingMode", false)
                            .Add("HarvestHerbs", (StyxWoW.Me.GetSkill(Styx.SkillLine.Herbalism).MaxValue > 0))
                            .Add("HarvestMinerals", (StyxWoW.Me.GetSkill(Styx.SkillLine.Mining).MaxValue > 0))
                            .Add("KillBetweenHotspots", false)
                            .Add("LearnFlightPaths", true)
                            .Add("LootChests", true)
                            .Add("LootMobs", true)
                            .Add("LootRadius", 45)
                            .Add("MountDistance", 75)
                            .Add("NinjaSkin", (StyxWoW.Me.GetSkill(Styx.SkillLine.Skinning).MaxValue > 0))
                            .Add("PullDistance", 30)
                            .Add("RessAtSpiritHealers", false)
                            .Add("SkinMobs", (StyxWoW.Me.GetSkill(Styx.SkillLine.Skinning).MaxValue > 0))
                            .Add("TrainNewSkills", true)
                            .Add("UseFlightPaths", true)
                            .Add("UseMount", true)
                            .Add("UseRandomMount", true)
                        );

            presets.Add("UserOriginal",
                        originalConfiguration.MakeChangeRequest());

            return (presets);
        }


        public UserSettings(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                _configurationSettings = UtilBuildConfigurationSettings();
                _persistData = new PersistedData(this.GetType().Name, _configurationSettings);
                _userChangeRequest = new ConfigurationChangeRequest(_configurationSettings);

                // If we've yet to capture the user's original settings, do so now...
                // We need to do this 'up front' because the "UserOriginal" configuration will
                // also be captured as a preset.
                _originalConfiguration = _persistData.OriginalConfiguration;
                _presetChangeRequests = UtilBuildPresetChangeRequests(_configurationSettings,
                                                                      _originalConfiguration);

                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                bool? tmpDebugShowChangesApplied;

                tmpDebugShowChangesApplied = GetAttributeAsNullable<bool>("DebugShowChangesApplied", false, null, null);
                DebugShowDetails = GetAttributeAsNullable<bool>("DebugShowDetails", false, null, null) ?? false;
                DebugShowDiff = GetAttributeAsNullable<bool>("DebugShowDiff", false, null, null) ?? false;
                PresetName = GetAttributeAs<string>("Preset", false, new ConstrainTo.SpecificValues<string>(_presetChangeRequests.Keys.ToArray()), null) ?? "";
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                IsStopBot = GetAttributeAsNullable<bool>("StopBot", false, null, null) ?? false;

                _userChangeRequest.GetChangesFromAttributes(this);

                // Transfer any 'debug' requests made by the user into our persistent copy --
                if (tmpDebugShowChangesApplied.HasValue)
                { _persistData.DebugShowChangesApplied = tmpDebugShowChangesApplied.Value; }
            }

            catch (Exception except)
            {
                // Maintenance problems occur for a number of reasons.  The primary two are...
                // * Changes were made to the behavior, and boundary conditions weren't properly tested.
                // * The Honorbuddy core was changed, and the behavior wasn't adjusted for the new changes.
                // In any case, we pinpoint the source of the problem area here, and hopefully it
                // can be quickly resolved.
                LogMessage("error", "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message
                                    + "\nFROM HERE:\n"
                                    + except.StackTrace + "\n");
                IsAttributeProblem = true;
            }
        }


        // Attributes provided by caller
        public bool DebugShowDetails { get; private set; }
        public bool DebugShowDiff { get; private set; }
        public bool IsBehaviorDone { get; private set; }
        public bool IsStopBot { get; private set; }
        public string PresetName { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }

        // Private variables for internal state
        private Dictionary<string, ConfigDescriptor> _configurationSettings;
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private ConfigSnapshot _originalConfiguration;
        private PersistedData _persistData;
        private Dictionary<string, ConfigurationChangeRequest> _presetChangeRequests;
        private ConfigurationChangeRequest _userChangeRequest;

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: UserSettings.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~UserSettings()
        {
            Dispose(false);
        }


        public void Dispose(bool isExplicitlyInitiatedDispose)
        {
            if (!_isDisposed)
            {
                // NOTE: we should call any Dispose() method for any managed or unmanaged
                // resource, if that resource provides a Dispose() method.

                // Clean up managed resources, if explicit disposal...
                if (isExplicitlyInitiatedDispose)
                {
                    // empty, for now
                }

                // Clean up unmanaged resources (if any) here...
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        private void BotEvents_OnBotStop(EventArgs args)
        {
            ConfigSnapshot tmpOriginalConfiguration = _persistData.OriginalConfiguration;

            // Restore the user's original configuration, since the bot is stopping...
            if (tmpOriginalConfiguration != null)
            {
                string tmpChanges = tmpOriginalConfiguration.Restore();

                if (_persistData.DebugShowChangesApplied)
                {
                    LogMessage("info", "Bot stopping.  Original user settings restored as follows...\n" + tmpChanges);
                }

                // Remove our OnBotStop handler
                BotEvents.OnBotStop -= BotEvents_OnBotStop;

                // Done with our persistent data, since the bot is stopping --
                // We want to  prevent acting on stale data when the bot is restarted.
                _persistData.DePersistData();
            }
        }


        #region Overrides of CustomForcedBehavior

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public override bool IsDone
        {
            get
            {
                return (_isBehaviorDone     // normal completion
                        || !UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
            }
        }


        public override void OnStart()
        {
            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {
                // The BotStop handler will put the original configuration settings back in place...
                // Note, we only want to hook it once for this behavior.
                if (!_persistData.IsBotStopHooked)
                {
                    BotEvents.OnBotStop += BotEvents_OnBotStop;
                    _persistData.IsBotStopHooked = true;
                }

                // First, process Preset request, if any...
                if (!string.IsNullOrEmpty(PresetName))
                {
                    string tmpString = _presetChangeRequests[PresetName].Apply();

                    if (_persistData.DebugShowChangesApplied)
                    { LogMessage("info", "Using preset '{0}'...\n{1}", PresetName, tmpString); }
                }

                // Second, apply any change requests...
                if (_userChangeRequest.Count > 0)
                {
                    string tmpString = _userChangeRequest.Apply();

                    if (_persistData.DebugShowChangesApplied)
                    { LogMessage("info", "Applied changes...\n{0}", tmpString); }
                }

                // Third, show state, if requested...                
                if (DebugShowDetails)
                { LogMessage("info", UtilCurrentConfigAsString(_configurationSettings)); }

                if (DebugShowDiff)
                {
                    LogMessage("info", "Changes from original user's settings--\n"
                                           + _originalConfiguration.GetChangesAsString());
                }

                // Forth, stop the bot, if requested...
                if (IsStopBot)
                {
                    LogMessage("info", "Stopping the bot per profile request.");
                    TreeRoot.Stop();
                }

                _isBehaviorDone = true;
            }
        }

        #endregion


        // Note: The RecognizedAttribute's Dictionary value field was left open for user data, by design.
        // We take advantage of that here by storing ConfigurationDescriptors to help us further process
        // the data by moving it into and out of the appropriate properties.
        // Note: If you make a spelling error while maintaining this code, by design an exception will be thrown
        // at runtime pointing you directly to the problem.
        private static Dictionary<string, ConfigDescriptor> UtilBuildConfigurationSettings()
        {
            List<string> ignoredPropertyNames = new List<string>()
                                                                              {
                                                                                  "EnabledPlugins",
                                                                                  "FormLocationX",
                                                                                  "FormLocationY",
                                                                                  "LastUsedPath",
                                                                                  "MailRecipient",
                                                                                  "MeshesFolderPath",
                                                                                  "Password",
                                                                                  "PluginKey",
                                                                                  "SelectedBotIndex",
                                                                                  "Username",
                                                                              };
            Dictionary<string, ConfigDescriptor> configurationSetting = new Dictionary<string, ConfigDescriptor>();


            // Allowed 'Configuration' attributes--
            // A default value of 'null' means the item has no default value.
            foreach (string configItemName in from propertyName in CharacterSettings.Instance.GetSettings().Keys
                                              where !ignoredPropertyNames.Contains(propertyName)
                                              select propertyName)
            {
                if (configurationSetting.ContainsKey(configItemName))
                { continue; }

                configurationSetting.Add(configItemName, new ConfigDescriptor(configItemName,
                                                                              CharacterSettings.Instance,
                                                                              null));
            }

            foreach (string configItemName in from propertyName in LevelbotSettings.Instance.GetSettings().Keys
                                              where !ignoredPropertyNames.Contains(propertyName)
                                              select propertyName)
            {
                if (configurationSetting.ContainsKey(configItemName))
                { continue; }

                configurationSetting.Add(configItemName, new ConfigDescriptor(configItemName,
                                                                              LevelbotSettings.Instance,
                                                                              null));
            }

            foreach (string configItemName in from propertyName in StyxSettings.Instance.GetSettings().Keys
                                              where !ignoredPropertyNames.Contains(propertyName)
                                              select propertyName)
            {
                if (configurationSetting.ContainsKey(configItemName))
                { continue; }

                configurationSetting.Add(configItemName, new ConfigDescriptor(configItemName,
                                                                              StyxSettings.Instance,
                                                                              null));
            }


            // Attach constraints to particular elements --
            Dictionary<string, Constraint> constraints = new Dictionary<string, Constraint>()
            {
                { "DrinkAmount",            new ConstrainInteger(0, 100) },
                { "FoodAmount",             new ConstrainInteger(0, 100) },
                { "LogoutInactivityTimer",  new ConstrainInteger(1, int.MaxValue) },
                { "LootRadius",             new ConstrainInteger(1, 100) },
                { "MountDistance",          new ConstrainInteger(1, 200) },
                { "PullDistance",           new ConstrainInteger(1, 75) },
            };

            foreach (KeyValuePair<string, Constraint> kvp in constraints)
            {
                // Maintenance error?
                if (!configurationSetting.ContainsKey(kvp.Key))
                {
                    throw (new ArgumentException(string.Format("Unable to locate configurable '{0}'"
                                                               + " in recognizedAttributes for constraint attachment",
                                                               kvp.Key)));

                }

                configurationSetting[kvp.Key].Constraint = kvp.Value;
            }

            return (configurationSetting);
        }


        private string UtilCurrentConfigAsString(Dictionary<string, ConfigDescriptor> recognizedAttributes)
        {
            string outString = "Current configuration...\n";


            // Iterate over the descriptor list, and restore the value for
            // each item identified
            var configDescriptors = (from desc in recognizedAttributes.Values
                                     where desc != null
                                     orderby desc.Name
                                     select desc);

            foreach (ConfigDescriptor configDesc in configDescriptors)
            {
                string constraint = configDesc.Constraint.AsString();

                if ((string.IsNullOrEmpty(constraint)) || (configDesc.Constraint is ConstrainBoolean))
                { outString += string.Format("    {0}: \"{1}\"\n", configDesc.Name, configDesc.Value); }
                else
                {
                    outString += string.Format("    {0}: \"{1}\" (constrained to {2})\n",
                                               configDesc.Name,
                                               configDesc.Value,
                                               configDesc.Constraint.AsString());
                }
            }

            return (outString);
        }
    }


    //==================================================
    // All classes below this point are support for getting the work done
    //

    /// <summary>
    /// Captures the details of a property that the user may alter.
    /// It provides generic Get/Set mechanics without regard of 'type'.
    /// </summary>
    class ConfigDescriptor
    {
        public ConfigDescriptor(string backingPropertyName,
                                Styx.Helpers.Settings backingInstance,
                                Constraint constraint)
        {
            // We are a bit aggressive in our error checking here--
            // The most likely source of errors will be people that maintain the code in the future,
            // and we want to weed out as many newbie mistakes as possible.
            if (string.IsNullOrEmpty(backingPropertyName))
            {
                throw (new ArgumentException(string.Format("For configurable '{0}',"
                                                            + " Null or Empty backingPropertyName not permitted",
                                                            "")));
            }

            _backingInstance = backingInstance;
            if (_backingInstance == null)
            {
                throw (new ArgumentException(string.Format("For configurable '{0}',"
                                                            + " Null backingInstance not permitted",
                                                            backingPropertyName)));
            }

            _backingPropertyInfo = backingInstance.GetType().GetProperty(backingPropertyName);
            if (_backingPropertyInfo == null)
            {
                throw (new ArgumentException(string.Format("For configurable '{0}',"
                                                            + " unable to locate Property in instance '{1}'",
                                                            backingPropertyName,
                                                            backingInstance.GetType().FullName)));
            }

            Constraint = constraint;
        }


        public Constraint Constraint
        {
            get { return (_constraint); }

            set
            {
                _constraint = value;

                // If user-provided constraint was 'null', populate with an appropriate default...
                if (_constraint == null)
                {
                    if (_backingPropertyInfo.PropertyType == typeof(bool))
                    { _constraint = new ConstrainBoolean(); }

                    else if (_backingPropertyInfo.PropertyType == typeof(int))
                    { _constraint = new ConstrainInteger(int.MinValue, int.MaxValue); }

                    else if (_backingPropertyInfo.PropertyType == typeof(string))
                    { _constraint = new ConstrainString(); }

                    else
                    {
                        throw (new ArgumentException(string.Format("For configurable '{0}',"
                                                                    + " expected constraint type of '{1}'",
                                                                    _backingPropertyInfo.Name,
                                                                    _backingPropertyInfo.PropertyType)));
                    }
                }


                // If we've a mismatch between the constraint type and our property-type, its a serious problem...
                if (_constraint.ConstraintType != _backingPropertyInfo.PropertyType)
                {
                    throw (new ArgumentException(string.Format("For configurable '{0}',"
                                                                + " mismatch between Property Type('{1}') and Contraint Type('{2}')",
                                                                _backingPropertyInfo.Name,
                                                                _backingPropertyInfo.PropertyType.Name,
                                                                _constraint.ConstraintType.Name)));
                }
            }
        }

        public string Name { get { return (_backingPropertyInfo.Name); } }

        public object Value
        {
            get { return (_backingPropertyInfo.GetValue(_backingInstance, null)); }
            set { _backingPropertyInfo.SetValue(_backingInstance, value, null); }
        }


        // Data members
        private Styx.Helpers.Settings _backingInstance;
        private PropertyInfo _backingPropertyInfo;
        private Constraint _constraint;
    }


    /// <summary>
    /// Allows coherency checks to be placed on user-provided values to the properties
    /// we will be modifying.  A Constraint class also knows how to fetch a value
    /// from the behavior's Args list.
    /// </summary>
    abstract class Constraint
    {
        // The returned object may be null.
        public abstract object AcquireUserInput(CustomForcedBehavior behavior,
                                                string configName);

        public abstract string AsString();

        public abstract Type ConstraintType { get; }
    }


    // Concrete constraint classes
    class ConstrainBoolean : Constraint
    {
        public ConstrainBoolean()
        {
            // empty
        }

        public override object AcquireUserInput(CustomForcedBehavior behavior,
                                                    string configName)
        {
            return (behavior.GetAttributeAsNullable<bool>(configName, false, null, null));
        }

        public override string AsString()
        {
            return ("[true, false]");
        }

        public override Type ConstraintType { get { return (typeof(bool)); } }
    }


    class ConstrainInteger : Constraint
    {
        public ConstrainInteger(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public override object AcquireUserInput(CustomForcedBehavior behavior,
                                                    string configName)
        {
            return (behavior.GetAttributeAsNullable<int>(configName, false, new CustomForcedBehavior.ConstrainTo.Domain<int>(MinValue, MaxValue), null));
        }

        public override string AsString()
        {
            return (string.Format("[{0}..{1}]",
                                    ((MinValue == int.MinValue) ? "int.MinValue" : MinValue.ToString()),
                                    ((MaxValue == int.MaxValue) ? "int.MaxValue" : MaxValue.ToString())));
        }

        public override Type ConstraintType { get { return (typeof(int)); } }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
    }


    class ConstrainString : Constraint
    {
        public ConstrainString()
        {
            // empty
        }

        public override object AcquireUserInput(CustomForcedBehavior behavior,
                                                    string configName)
        {
            return (behavior.GetAttributeAs<string>(configName, false, null, null));
        }

        public override string AsString()
        {
            return ("");
        }

        public override Type ConstraintType { get { return (typeof(string)); } }
    }


    class ConfigurationChangeRequest
    {
        public ConfigurationChangeRequest(Dictionary<string, ConfigDescriptor> recognizedAttributes)
        {
            _recognizedAttributes = recognizedAttributes;
        }


        public ConfigurationChangeRequest Add(string configItemName,
                                                object configItemValue)
        {
            if (!_recognizedAttributes.ContainsKey(configItemName))
            {
                throw (new ArgumentException(string.Format("Unable to locate configurable for '{0}' (spelling error?)",
                                                            configItemName)));
            }

            // Note: it is a maintenance error if we try to change the same configurable twice
            // in the same change request.  The dictionary will throw an exception if someone
            // attempts to do that.
            _changeRequests.Add(_recognizedAttributes[configItemName], configItemValue);

            return (this);
        }


        public string Apply()
        {
            var changedDescriptors = (from desc in _changeRequests.Keys
                                      orderby desc.Name
                                      select desc);
            string outString = "";

            foreach (ConfigDescriptor configDesc in changedDescriptors)
            {
                configDesc.Value = _changeRequests[configDesc];

                outString += string.Format("    Setting '{0}' to \"{1}\".\n",
                                            configDesc.Name,
                                            configDesc.Value);
            }

            return (outString);
        }

        public int Count { get { return (_changeRequests.Count); } }


        public void GetChangesFromAttributes(CustomForcedBehavior behavior)
        {
            var configDescriptors = (from attributeKvp in behavior.Args
                                     from desc in _recognizedAttributes.Values
                                     where (desc != null) && (attributeKvp.Key == desc.Name)
                                     select desc);

            foreach (ConfigDescriptor configDesc in configDescriptors)
            {
                object result = configDesc.Constraint.AcquireUserInput(behavior, configDesc.Name);

                if (result != null)
                { _changeRequests.Add(configDesc, result); }
            }
        }


        private Dictionary<ConfigDescriptor, object> _changeRequests = new Dictionary<ConfigDescriptor, object>();
        private Dictionary<string, ConfigDescriptor> _recognizedAttributes = null;
    }


    /// <summary>
    /// Takes a snapshot of the current configuration.  The snapshot can be used
    /// at a later point to restore the current configuration to the state contained
    /// within the snapshot.  This class can also show what's different between the
    /// current state and the configuration state that existed when the snapshot was taken.
    /// </summary>
    class ConfigSnapshot
    {
        public ConfigSnapshot(Dictionary<string, ConfigDescriptor> recognizedAttributes,
                                Dictionary<string, object> persistedFlatData)
        {
            _recognizedAttributes = recognizedAttributes;

            // If we're not rebuilding from persistent data, then capture the current configuration
            if (persistedFlatData == null)
            {
                var configDescriptors = (from desc in recognizedAttributes.Values
                                         where desc != null
                                         select desc);

                foreach (ConfigDescriptor configDesc in configDescriptors)
                { _configSnapshot.Add(configDesc, configDesc.Value); }
            }

            else
            {
                var configDescriptors = (from desc in recognizedAttributes.Values
                                         from configItemName in persistedFlatData.Keys
                                         where (desc != null) && (desc.Name == configItemName)
                                         select desc);

                foreach (ConfigDescriptor configDesc in configDescriptors)
                { _configSnapshot.Add(configDesc, persistedFlatData[configDesc.Name]); }
            }
        }


        // Builds a string showing how the current configuration differs from this snapshot... 
        public string GetChangesAsString()
        {
            var changedItems = (from item in _configSnapshot
                                where !item.Key.Value.Equals(item.Value)
                                orderby item.Key.Name
                                select item);
            string outString = "";

            foreach (var kvp in changedItems)
            { outString += string.Format("    {0}: \"{1}\" (was \"{2}\")\n", kvp.Key.Name, kvp.Key.Value, kvp.Value); }

            if (string.IsNullOrEmpty(outString))
            { outString = "    No changes from original settings\n"; }

            return (outString);
        }


        public Dictionary<string, object> PersistSaveData()
        {
            return (_configSnapshot.ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value));
        }


        // Sets the current configuration to the values captured by this snapshot...
        public string Restore()
        {
            var changedItems = (from item in _configSnapshot
                                where !item.Key.Value.Equals(item.Value)
                                orderby item.Key.Name
                                select item);
            string outString = "";

            foreach (KeyValuePair<ConfigDescriptor, object> kvp in changedItems)
            {
                outString += string.Format("    {0}: Restored to \"{1}\" (from change of \"{2}\")\n",
                                            kvp.Key.Name,
                                            kvp.Value,
                                            kvp.Key.Value);
                kvp.Key.Value = kvp.Value;
            }

            if (string.IsNullOrEmpty(outString))
            { outString = "    No changes from original settings\n"; }

            return (outString);
        }


        // Builds a change request from the settings captured by this snapshot...
        // We only place entries that don't agree with the snapshot's value into the change request.
        public ConfigurationChangeRequest MakeChangeRequest()
        {
            ConfigurationChangeRequest changeRequest = new ConfigurationChangeRequest(_recognizedAttributes);
            var changedDescs = (from kvp in _configSnapshot
                                where !kvp.Key.Value.Equals(kvp.Value)
                                select kvp);

            foreach (KeyValuePair<ConfigDescriptor, object> kvp in changedDescs)
            { changeRequest.Add(kvp.Key.Name, kvp.Value); }

            return (changeRequest);
        }


        private readonly Dictionary<ConfigDescriptor, object> _configSnapshot = new Dictionary<ConfigDescriptor, object>();
        private Dictionary<string, ConfigDescriptor> _recognizedAttributes = null;
    }


    /// <summary>
    // Data to be retained across consecutive calls of this custom behavior.
    // This technique is the only one we've discovered that works with the way Honorbuddy invokes
    // custom behaviors.  This technique uses reflection (ugh), thus it is a performance pig
    // compared to normal variable access.
    //
    // NOTE: You can only store and retrieve data built with C# primitives or containers this way.
    // You may not store user-defined objects, or containers with user-defined objects in them.
    // This is a limitation in the way Honorbuddy handles the invoking of CustomForcedBehavior.
    //
    // We'd prefer to handle all the persistent items as properties.  However, due to the
    // required 'flattening' to system-provided types, this is not always possible.  As such,
    // you'll see us adopt a Java-like naming convention for the troublesome items.
    //
    // NOTE: Any PersistData items should be set to 'null' OnBotStop.  Obviously, checking
    // for 'null' and populating with relevant data should be an early action for a behavior.
    // Without this paradigm, a behavior can easily pick up and act on irrelevant data.
    // 
    /// </summary>
    class PersistData
    {
        protected PersistData(string spaceName)
        {
            _spaceName = spaceName;
        }


        // Call this when you want to de-persist the data.  It will 'null out' the values for
        // all items that have been placed into the persistent storage cache.
        public void DePersistData()
        {
            foreach (string appPropertyName in _appPropertyNames.Keys)
            { _currentDomain.SetData(appPropertyName, null); }
        }


        // If the persistent information is discovered to be 'null'...
        // It will be initialized from the fnDefaultInitialValue method.  We recognized
        // that it may be semi-expensive to create the 'default value', so we only
        // invoke the function when absolutely needed.
        // If we are forced to use fnDefaultInitialValue, the persistent storage will
        // be populated with the generated value, in addition to returning the generated
        // value to the caller.
        protected object UtilAttributeGet(string attributeName,
                                                System.Type attributeType,
                                                Func<object> fnDefaultInitialValue)
        {
            object tmpData = _currentDomain.GetData(UtilBuildPersistName(attributeName));

            // If data is uninitialized, or the wrong type, we need to (re)initialize it...
            if ((tmpData == null) || (tmpData.GetType() != attributeType))
            {
                tmpData = fnDefaultInitialValue();
                _currentDomain.SetData(UtilBuildPersistName(attributeName), tmpData);
            }

            return (tmpData);
        }


        // It is acceptable for value to be 'null'--
        // Setting it to 'null' is equivalent to a 'depersist' operation.
        // If you set it to 'null', it effectively causes a reinitialization of the value
        // if UtilAttributeGet is subsequently called.
        protected void UtilAttributeSet(string attributeName,
                                                object value)
        {
            _currentDomain.SetData(UtilBuildPersistName(attributeName), value);
        }


        // For properties, use as UtilBuildPersistName(() => this.PROPERTY_NAME)
        protected string UtilPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return ((propertyExpression.Body as MemberExpression).Member.Name);
        }


        // Use as UtilBuildPersistName("DebugMumbleFoo")
        private string UtilBuildPersistName(string propertyName)
        {
            string appPropertyName = _spaceName + ":" + propertyName;

            _appPropertyNames[appPropertyName] = null;

            return (appPropertyName);
        }

        private System.AppDomain _currentDomain = System.AppDomain.CurrentDomain;
        private Dictionary<string, object> _appPropertyNames = new Dictionary<string, object>();
        private string _spaceName;
    }


    class PersistedData : PersistData
    {
        public PersistedData(string spaceName,
                                Dictionary<string, ConfigDescriptor> recognizedAttributes)
            : base(spaceName)
        {
            _recognizedAttributes = recognizedAttributes;
        }

        public bool DebugShowChangesApplied
        {
            get { return ((bool)UtilAttributeGet(UtilPropertyName(() => this.DebugShowChangesApplied), typeof(bool), () => false)); }
            set { UtilAttributeSet(UtilPropertyName(() => this.DebugShowChangesApplied), value); }
        }

        public bool IsBotStopHooked
        {
            get { return ((bool)UtilAttributeGet(UtilPropertyName(() => this.IsBotStopHooked), typeof(bool), () => false)); }
            set { UtilAttributeSet(UtilPropertyName(() => this.IsBotStopHooked), value); }
        }

        public ConfigSnapshot OriginalConfiguration
        {
            get
            {
                Dictionary<string, object> tmpDict;

                tmpDict = (Dictionary<string, object>)UtilAttributeGet(UtilPropertyName(() => this.OriginalConfiguration),
                                                                        typeof(Dictionary<string, object>),
                                                                        () => new ConfigSnapshot(_recognizedAttributes, null).PersistSaveData());
                return (new ConfigSnapshot(_recognizedAttributes, tmpDict));
            }

            set
            {
                UtilAttributeSet(UtilPropertyName(() => this.OriginalConfiguration), value.PersistSaveData());
            }
        }


        private Dictionary<string, ConfigDescriptor> _recognizedAttributes;
    }
}
