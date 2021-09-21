using System.Collections.Generic;

namespace Singular.Dynamics
{
    public static class SessionState
    {
        private static readonly Dictionary<string, object> StateVariables = new Dictionary<string, object>();

        public static T Get<T>(string variable)
        {
            object value;
            if (StateVariables.TryGetValue(variable, out value))
                return (T)value;
            return default(T);
        }

        public static void Set(string variable, object value)
        {
            StateVariables[variable] = value;
        }

        public static void PrintState()
        {
            foreach (var stateVariable in StateVariables)
            {
                Logger.Write(stateVariable.Key + ": " + stateVariable.Value);
            }
        }
    }
}
