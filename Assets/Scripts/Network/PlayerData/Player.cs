using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceMulti.Network.PlayerData{
	[Serializable]
    public struct Player{
        public string id;
        public Position position;
    }
}
