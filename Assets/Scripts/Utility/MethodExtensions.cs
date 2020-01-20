using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace SpaceMulti.Utility {
    public static class MethodExtensions {
        public static string RemoveQuotes(this string Value) {
            return Value.Replace("\"", "");
        }
    }
}
