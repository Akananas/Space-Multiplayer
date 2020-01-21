using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceMulti.Utility.Attributes;
using SpaceMulti.Network.PlayerData;
namespace SpaceMulti.Network{
	[RequireComponent(typeof(NetworkIdentity))]
	public class NetworkTransform : MonoBehaviour{
		[SerializeField]
		[GreyOut]
		private Vector3 oldPosition;
		private NetworkIdentity networkIdentity;
		private Player player;
		private float stillCounter = 0;

		private void Start(){
			networkIdentity = GetComponent<NetworkIdentity>();
			oldPosition = transform.position;
			player = new Player();
			player.id = networkIdentity.GetID();
			player.position = new Position("0","0","0");
			if(!networkIdentity.IsControlling()){
				enabled = false;
			}	
		}

		private void Update() {
			if(networkIdentity.IsControlling()){
				if(oldPosition != transform.position){
					oldPosition = transform.position;
					stillCounter = 0;
					sendData();
				}else{
					stillCounter += Time.deltaTime;
					if(stillCounter >= 1){
						stillCounter = 0;
						sendData();
					}
				}
			}
		}

		private void sendData(){
			//Update player information
			player.position.x = Mathf.Round((transform.position.x * 1000.0f)/1000.0f).ToString();
			player.position.z = Mathf.Round((transform.position.z * 1000.0f)/1000.0f).ToString();
			networkIdentity.GetSocket().Emit("updatePosition", new JSONObject(JsonUtility.ToJson(player)));
		}
	}
}
