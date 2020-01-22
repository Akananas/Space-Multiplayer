using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace SpaceMulti.Utility {
    public static class MethodExtensions {
        public static string RemoveQuotes(this string Value){
            return Value.Replace("\"", "");
        }
        public static string FloatToString(this float value) {
            return value.ToString("0.0000");
        }

        public static float JSONToFloat(this JSONObject jsonObject) {
            return float.Parse(jsonObject.ToString().Replace("\"", ""));
        }
    }
}
