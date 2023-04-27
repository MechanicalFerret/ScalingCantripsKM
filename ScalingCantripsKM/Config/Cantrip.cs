using Kingmaker.Utility;
using Newtonsoft.Json;
using ScalingCantripsKM.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using static ScalingCantripsKM.Blueprints;

namespace ScalingCantripsKM.Config
{
    public class Cantrip
    {
        [JsonProperty] public bool Enabled = true;
        [JsonProperty] public bool OverrideDefaults = false;
        [JsonProperty] public string Name;
        [JsonProperty] public bool StartImmediately { get => (OverrideDefaults) ? _startImmediately : Settings.StartImmediately; internal set => _startImmediately = value; }
        [JsonProperty] public int CasterLevelsReq { get => (OverrideDefaults) ? _casterLevelReq : Settings.CasterLevelsReq; internal set => _casterLevelReq = value; }
        [JsonProperty] public int MaxDice { get => (OverrideDefaults) ? _maxDice : Settings.MaxDice; internal set => _maxDice = value; }

        internal bool _startImmediately;
        internal int _casterLevelReq;
        internal int _maxDice;

        internal static Cantrip Load(string name)
        {
            string filePath = $"{Main.modEntry.Path}UserSettings{Path.DirectorySeparatorChar}{name}.json";
            Cantrip retVal = null;
            if (File.Exists(filePath))
            {
                SKMLogger.Log($"Loading {filePath}");
                using (StreamReader file = File.OpenText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    retVal = (Cantrip)serializer.Deserialize(file, typeof(Cantrip));
                }
            }
            if (retVal == null)
            {
                retVal = new Cantrip();
                retVal.Enabled = true;
                retVal.OverrideDefaults = false;
                retVal.Name = name;
                retVal._startImmediately = true;
                retVal._casterLevelReq = 2;
                retVal._maxDice = 6;
            }
            return retVal;
        }

        internal void Save()
        {
            string filePath = $"{Main.modEntry.Path}UserSettings{Path.DirectorySeparatorChar}{Name}.json";

            // Load the existing file to see if Writing is even necessary
            if (!this.Equals(Load(Name)))
            {
                SKMLogger.Log($"Saving {filePath}");
                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Include,
                    Formatting = Formatting.Indented,
                };
                using (StreamWriter sWriter = new StreamWriter(filePath))
                {
                    using (JsonWriter jWriter = new JsonTextWriter(sWriter))
                    {
                        serializer.Serialize(jWriter, this);
                    }
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Cantrip cantrip &&
                   Enabled == cantrip.Enabled &&
                   OverrideDefaults == cantrip.OverrideDefaults &&
                   Name == cantrip.Name &&
                   _startImmediately == cantrip._startImmediately &&
                   _casterLevelReq == cantrip._casterLevelReq &&
                   _maxDice == cantrip._maxDice;
        }

        public override int GetHashCode()
        {
            int hashCode = -166349542;
            hashCode = hashCode * -1521134295 + Enabled.GetHashCode();
            hashCode = hashCode * -1521134295 + OverrideDefaults.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + _startImmediately.GetHashCode();
            hashCode = hashCode * -1521134295 + _casterLevelReq.GetHashCode();
            hashCode = hashCode * -1521134295 + _maxDice.GetHashCode();
            return hashCode;
        }
    }
}
