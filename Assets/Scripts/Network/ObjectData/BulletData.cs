using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceMulti.Network.ObjectData{
	[Serializable]
	public struct BulletData{
		public string id;
		public Position position;
		public Position direction;
	}	
}
