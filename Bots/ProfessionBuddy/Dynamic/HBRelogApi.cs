using System;
using System.Linq;
using System.Reflection;

namespace HighVoltz.Dynamic
{
    public enum ProfileStatus
    {
        Unknown,
        Paused,
        Running,
        Stopped
    }

    public class HBRelogApi
    {
        private readonly Func<string> _currentProfileName;
        private readonly Func<string[]> _getProfileNames;
        private readonly Func<string, int> _getProfileStatus;
        private readonly Assembly _hbRelogHelperAsm;
        private readonly Action<string, TimeSpan> _idleProfile;
        private readonly Action<string> _skipCurrentTask;

        private readonly Func<bool> _isConnected;
        private readonly LogonDelegate _logon;
        private readonly Action<string> _pauseProfile;
        private readonly Action _restartHB;
        private readonly Action _restartWow;
        private readonly Action<string> _setProfileStatusText;
        private readonly Action<string> _startProfile;
        private readonly Action<string> _stopProfile;

        public HBRelogApi()
        {
            // find the HBRelogHelper plugin assembly
            _hbRelogHelperAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(
                a => a.GetName().Name.Contains("HBRelogHelper"));
            if (_hbRelogHelperAsm != null)
            {
                Type apiType = _hbRelogHelperAsm.GetType("HighVoltz.HBRelogHelper.HBRelogApi");
                // IsConnected
                PropertyInfo propertyInfo = apiType.GetProperty("IsConnected", BindingFlags.Public | BindingFlags.Static);
                _isConnected = (Func<bool>) Delegate.CreateDelegate(typeof (Func<bool>), propertyInfo.GetGetMethod());
                // CurrentProfileName
                propertyInfo = apiType.GetProperty("CurrentProfileName", BindingFlags.Public | BindingFlags.Static);
                _currentProfileName =
                    (Func<string>) Delegate.CreateDelegate(typeof (Func<string>), propertyInfo.GetGetMethod());
                // RestartWow
                MethodInfo methodInfo = apiType.GetMethod("RestartWow", BindingFlags.Public | BindingFlags.Static);
                _restartWow = (Action) Delegate.CreateDelegate(typeof (Action), methodInfo);
                // RestartHB
                methodInfo = apiType.GetMethod("RestartHB", BindingFlags.Public | BindingFlags.Static);
                _restartHB = (Action) Delegate.CreateDelegate(typeof (Action), methodInfo);
                // GetProfileNames
                methodInfo = apiType.GetMethod("GetProfileNames", BindingFlags.Public | BindingFlags.Static);
                _getProfileNames = (Func<string[]>) Delegate.CreateDelegate(typeof (Func<string[]>), methodInfo);
                // StartProfile
                methodInfo = apiType.GetMethod("StartProfile", BindingFlags.Public | BindingFlags.Static);
                _startProfile = (Action<string>) Delegate.CreateDelegate(typeof (Action<string>), methodInfo);
                // StopProfile
                methodInfo = apiType.GetMethod("StopProfile", BindingFlags.Public | BindingFlags.Static);
                _stopProfile = (Action<string>) Delegate.CreateDelegate(typeof (Action<string>), methodInfo);
                // PauseProfile
                methodInfo = apiType.GetMethod("PauseProfile", BindingFlags.Public | BindingFlags.Static);
                _pauseProfile = (Action<string>) Delegate.CreateDelegate(typeof (Action<string>), methodInfo);
                // IdleProfile
                methodInfo = apiType.GetMethod("IdleProfile", BindingFlags.Public | BindingFlags.Static);
                _idleProfile =
                    (Action<string, TimeSpan>) Delegate.CreateDelegate(typeof (Action<string, TimeSpan>), methodInfo);
                // GetProfileStatus
                methodInfo = apiType.GetMethod("GetProfileStatus", BindingFlags.Public | BindingFlags.Static);
                _getProfileStatus = (Func<string, int>) Delegate.CreateDelegate(typeof (Func<string, int>), methodInfo);
                // SetProfileStatusText
                methodInfo = apiType.GetMethod("SetProfileStatusText", BindingFlags.Public | BindingFlags.Static);
                _setProfileStatusText = (Action<string>) Delegate.CreateDelegate(typeof (Action<string>), methodInfo);
                // Logon
                methodInfo = apiType.GetMethod("Logon", BindingFlags.Public | BindingFlags.Static);
                _logon = (LogonDelegate) Delegate.CreateDelegate(typeof (LogonDelegate), methodInfo);

                methodInfo = apiType.GetMethod("SkipCurrentTask", BindingFlags.Public | BindingFlags.Static);
                _skipCurrentTask = (Action<string>) Delegate.CreateDelegate(typeof (Action<string>), methodInfo);
            }
        }

        /// <summary>
        /// Returns true of there is an open IPC connection  to HBRelog.
        /// </summary>
        public bool IsConnected
        {
            get { return _isConnected != null && _isConnected(); }
        }

        /// <summary>
        /// Name of the current HBRelog profile (the one that is managing current Honorbuddy instance)
        /// </summary>
        public string CurrentProfileName
        {
            get { return _currentProfileName != null ? _currentProfileName() : String.Empty; }
        }

        /// <summary>
        /// Restarts the Wow process as well as Honorbuddy.
        /// </summary>
        public void RestartWow()
        {
            if (_restartWow != null) _restartWow();
        }

        /// <summary>
        /// Restarts Honorbuddy.
        /// </summary>
        public void RestartHB()
        {
            if (_restartHB != null) _restartHB();
        }

        /// <summary>
        /// Returns an array of all the HBRelog profile names 
        /// </summary>
        /// <returns>Array of HBRelog profile names</returns>
        public string[] GetProfileNames()
        {
            return _getProfileNames != null ? _getProfileNames() : new string[] {};
        }

        /// <summary>
        /// Starts a HBRelog profile
        /// </summary>
        /// <param name="profileName">Name of the profile</param>
        public void StartProfile(string profileName)
        {
            if (_startProfile != null) _startProfile(profileName);
        }

        /// <summary>
        /// Stops a HBRelog profile
        /// </summary>
        /// <param name="profileName">Name of the profile</param>
        public void StopProfile(string profileName)
        {
            if (_stopProfile != null) _stopProfile(profileName);
        }

        /// <summary>
        /// Pauses a HBRelog profile
        /// </summary>
        /// <param name="profileName">Name of the profile</param>
        public void PauseProfile(string profileName)
        {
            if (_pauseProfile != null) _pauseProfile(profileName);
        }

        /// <summary>
        /// Shuts down Wow and Honorbuddy and waits the specified duration before loging back into wow and HB
        /// </summary>
        /// <param name="profileName">Name of the profile</param>
        /// <param name="time">Idle for this duration</param>
        public void IdleProfile(string profileName, TimeSpan time)
        {
            if (_idleProfile != null) _idleProfile(profileName, time);
        }

        /// <summary>
        /// Returns the status of a HBRelog profile
        /// </summary>
        /// <param name="profileName">Name of the profile</param>
        /// <returns>The status</returns>
        public ProfileStatus GetProfileStatus(string profileName)
        {
            return _getProfileStatus != null ? (ProfileStatus) _getProfileStatus(profileName) : ProfileStatus.Unknown;
        }

        /// <summary>
        /// Sets the current HBRelog profiles status text
        /// </summary>
        /// <param name="status">Status text</param>
        public void SetProfileStatusText(string status)
        {
            if (_setProfileStatusText != null) _setProfileStatusText(status);
        }

        public void SkipCurrentTask(string profileName)
        {
            if (_skipCurrentTask != null) _skipCurrentTask(profileName);
        }

        /// <summary>
        /// Re-opens wow and logs on a character
        /// </summary>
        /// <param name="character">Name of character</param>
        public void Logon(string character)
        {
            Logon(character, null, null, null, null);
        }

        /// <summary>
        /// Re-opens wow and logs on a character
        /// </summary>
        /// <param name="character">Name of character</param>
        /// <param name="server">Name of server</param>
        public void Logon(string character, string server)
        {
            Logon(character, server, null, null, null);
        }

        /// <summary>
        /// Re-opens wow and logs on a character
        /// </summary>
        /// <param name="character">Name of Character</param>
        /// <param name="server">Name of Server</param>
        /// <param name="customClass">Name of the CustomClass to use</param>
        public void Logon(string character, string server, string customClass)
        {
            Logon(character, server, customClass, null, null);
        }

        /// <summary>
        /// Re-opens wow and logs on a character
        /// </summary>
        /// <param name="character">Name of Character</param>
        /// <param name="server">Name of Server</param>
        /// <param name="customClass">Name of the CustomClass to use</param>
        /// <param name="botBase">Name of the Botbase to use</param>
        /// <param name="profilePath">Path to the Honobuddy/Professionbuddy profile to use</param>
        public void Logon(string character, string server, string customClass, string botBase, string profilePath)
        {
            if (_logon != null)
                _logon(character, server, customClass, botBase, profilePath);
        }



        #region Nested type: LogonDelegate

        private delegate void LogonDelegate(
            string character, string server, string customClass, string botBase, string profilePath);

        #endregion
    }
}