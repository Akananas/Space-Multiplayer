using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceMulti.Network;
using SpaceMulti.Network.ObjectData;
namespace SpaceMulti.Gameplay{
	public class CollisionDestroy : MonoBehaviour{
		[SerializeField]
		private NetworkIdentity networkIdentity;
		[SerializeField]
		private WhoActivatedMe whoActivatedMe;

		private void OnCollisionEnter(Collision other) {
			NetworkIdentity ni = other.gameObject.GetComponent<NetworkIdentity>();
			if(ni == null || ni.GetID() != whoActivatedMe.WhoActivateMe){
				networkIdentity.GetSocket().Emit("collisionDestroy", new JSONObject(JsonUtility.ToJson(new IDData(){
					id = networkIdentity.GetID()
				})));
			}
		}
	}
}
