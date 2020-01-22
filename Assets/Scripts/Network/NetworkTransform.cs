using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceMulti.Utility;
using SpaceMulti.Utility.Attributes;
using SpaceMulti.Network.ObjectData;
namespace SpaceMulti.Network{
	[RequireComponent(typeof(NetworkIdentity))]
	public class NetworkTransform : MonoBehaviour{
		[SerializeField]
		[GreyOut]
		private Vector3 oldPosition;
		private Quaternion oldRotation;
		private NetworkIdentity networkIdentity;
		private Player player;
		private float stillCounter = 0;

		private void Start(){
			networkIdentity = GetComponent<NetworkIdentity>();
			oldPosition = transform.position;
			oldRotation = transform.rotation;
			player = new Player();
			player.id = networkIdentity.GetID();
			player.position = new Position(transform.position);
			if(!networkIdentity.IsControlling()){
				enabled = false;
			}	
		}

		private void Update() {
			if(networkIdentity.IsControlling()){
				if(oldPosition != transform.position || oldRotation != transform.rotation){
					oldPosition = transform.position;
					oldRotation = transform.rotation;
					stillCounter = 0;
					SendData();
				}else{
					stillCounter += Time.deltaTime;
					if(stillCounter >= 1){
						stillCounter = 0;
						SendData();
					}
				}
			}
		}

		private void SendData(){
			//Update player information
			player.position.VectorToString(transform.position);
			player.rotation = transform.rotation.eulerAngles.y.FloatToString();
			networkIdentity.GetSocket().Emit("updatePosition", new JSONObject(JsonUtility.ToJson(player)));
		}
	}
}
