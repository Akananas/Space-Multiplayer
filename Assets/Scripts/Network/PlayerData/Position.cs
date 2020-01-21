using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceMulti.Network.PlayerData{
	[Serializable]
    public struct Position{
        public string x,y,z;
		public Position(string X, string Y, string Z){
			this.x = X;
			this.y = Y;
			this.z = Z;
		}
    }
}
