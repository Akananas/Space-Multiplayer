using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceMulti.Utility;
namespace SpaceMulti.Network.ObjectData{
	[Serializable]
    public struct Position{
        public string x,y,z;
		public Position(Vector3	pos){
			this.x = pos.x.FloatToString();
			this.y = "0";
			this.z = pos.z.FloatToString();
		}

		public void VectorToString(Vector3 pos){
			this.x = pos.x.FloatToString();
			this.y = pos.y.FloatToString();
			this.z = pos.z.FloatToString();
		}
    }
}
