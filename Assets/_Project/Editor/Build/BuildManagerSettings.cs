using UnityEngine;

namespace KillItMyself.Edito
{
    public class BuildManagerSettings : ScriptableObject
    {
        public int BuildCount;
        public string Branch;
        public string VersionPrefix;
        public string ExeName;
        public bool BuildAddressables = true;
        public bool IncrementBuildNumber = true;
    }
}