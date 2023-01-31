using Styx.WoWInternals;

namespace CLU.Helpers
{
    using System.Drawing;

    using Styx.Helpers;

    using global::CLU.GUI;

    public class Keybinds
    {
        /* putting all the Keybind logic here */

        private static readonly Keybinds KeybindsInstance = new Keybinds();

        /// <summary>
        /// An instance of the Item Class
        /// </summary>
        public static Keybinds Instance { get { return KeybindsInstance; } }

        /// <summary>
        /// An Enum of keyboard keys to assign
        /// </summary>
        public enum Keyboardfunctions
        {
            Nothing, // - default
            IsAltKeyDown, // - Returns whether an Alt key on the keyboard is held down.
            IsControlKeyDown, // - Returns whether a Control key on the keyboard is held down
            IsLeftAltKeyDown, // - Returns whether the left Alt key is currently held down
            IsLeftControlKeyDown, // - Returns whether the left Control key is held down
            IsLeftShiftKeyDown, // - Returns whether the left Shift key on the keyboard is held down
            IsModifierKeyDown, // - Returns whether a modifier key is held down
            IsRightAltKeyDown, // - Returns whether the right Alt key is currently held down
            IsRightControlKeyDown, // - Returns whether the right Control key on the keyboard is held down
            IsRightShiftKeyDown, // - Returns whether the right shift key on the keyboard is held down
            IsShiftKeyDown, // - Returns whether a Shift key on the keyboard is held down
        };

        internal static void Pulse()
        {
            if (SettingsFile.Instance.KBindCooldownManagement != null && IsKeyDown(SettingsFile.Instance.KBindCooldownManagement))
            {
                SettingsFile.Instance.HandleCooldowns = !SettingsFile.Instance.HandleCooldowns;
                Logging.Write(Color.Red, " HandleCooldowns= {0}", SettingsFile.Instance.HandleCooldowns);
            }

            if (SettingsFile.Instance.KBindDsExtraButtonClick != null && IsKeyDown(SettingsFile.Instance.KBindDsExtraButtonClick))
            {
                SettingsFile.Instance.Handleextraactionbutton = !SettingsFile.Instance.Handleextraactionbutton;
                Logging.Write(Color.Red, " Handleextraactionbutton= {0}", SettingsFile.Instance.Handleextraactionbutton);
            }
            if (SettingsFile.Instance.KBindAoEManagement != null && IsKeyDown(SettingsFile.Instance.KBindAoEManagement))
            {
                SettingsFile.Instance.HandleAoE = !SettingsFile.Instance.HandleAoE;
                Logging.Write(Color.Red, " HandleAoE= {0}", SettingsFile.Instance.HandleAoE);
            }
            if (SettingsFile.Instance.KBindRaidPartyBuffManagement != null && IsKeyDown(SettingsFile.Instance.KBindRaidPartyBuffManagement))
            {
                SettingsFile.Instance.HandleRaidPartyBuff = !SettingsFile.Instance.HandleRaidPartyBuff;
                Logging.Write(Color.Red, " HandleRaidPartyBuff= {0}", SettingsFile.Instance.HandleRaidPartyBuff);
            }
            if (SettingsFile.Instance.KBindInteruptManagement != null && IsKeyDown(SettingsFile.Instance.KBindInteruptManagement))
            {
                SettingsFile.Instance.HandleInterrupts = !SettingsFile.Instance.HandleInterrupts;
                Logging.Write(Color.Red, " HandleInterrupts= {0}", SettingsFile.Instance.HandleInterrupts);
            }
            if (SettingsFile.Instance.KBindHealDefensiveManagement != null && IsKeyDown(SettingsFile.Instance.KBindHealDefensiveManagement))
            {
                SettingsFile.Instance.HandleHealing = !SettingsFile.Instance.HandleHealing;
                Logging.Write(Color.Red, " HandleHealing= {0}", SettingsFile.Instance.HandleHealing);
            }
            if (SettingsFile.Instance.KBindMultiDottingManagement != null && IsKeyDown(SettingsFile.Instance.KBindMultiDottingManagement))
            {
                SettingsFile.Instance.HandleMultiDotting = !SettingsFile.Instance.HandleMultiDotting;
                Logging.Write(Color.Red, " HandleMultiDotting= {0}", SettingsFile.Instance.HandleMultiDotting);
            }
            if (SettingsFile.Instance.KBindPauseRotation != null && IsKeyDown(SettingsFile.Instance.KBindPauseRotation))
            {
                SettingsFile.Instance.PauseRotation = !SettingsFile.Instance.PauseRotation;
                Logging.Write(Color.Red, " PauseRotation= {0}", SettingsFile.Instance.PauseRotation);
            }


            // I only wanted this to play once but the bitch continues..Will figure it out soon...
            // if (!SettingsFile.Instance.AutoManagePauseRotation)
            // {
            //    Sound.loadSoundFilePath(@"\CustomClasses\CLU\rotationenabled.wav");
            //    Sound.SoundPlay();
            // }
            // else
            // {
            //    Sound.loadSoundFilePath(@"\CustomClasses\CLU\rotationpaused.wav");
            //    Sound.SoundPlay();
            // }
        }

        /// <summary>
        /// checks to see if the specified key has been pressed within wow.
        /// </summary>
        /// <param name="key">The key to check for (see Keyboardfunctions)</param>
        /// <returns>true if the player has pressed the key</returns>
        private static bool IsKeyDown(string key)
        {
            try
            {
                    if (key == null || key == "Nothing") return false;
                    var raw = Lua.GetReturnValues("if " + key + "() then return 1 else return 0 end");
                    return raw[0] == "1";
            }
            catch
            {
                CLU.DebugLog("Lua failed in IsKeyDown" + key);
                return false;
            }
        }
    }
}
