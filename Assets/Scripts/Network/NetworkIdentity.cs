using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using SpaceMulti.Utility.Attributes;
namespace SpaceMulti.Network{
	public class NetworkIdentity : MonoBehaviour{
		[Header("Helpful Values")]
		[SerializeField]
		[GreyOut]
		private string id;
		[SerializeField]
		private bool isControlling;
		private SocketIOComponent socket;

		private void Awake() {
			isControlling = false;
		}

		public void SetControllerID(string ID){
			id = ID;
			isControlling = NetworkClient.ClientID == ID; // Set IsControlling to what the conditional evaluates to
		}

		public void SetSocketReference(SocketIOComponent Socket){
			socket = Socket;
		}

		public string GetID(){
			return id;
		}

		public bool IsControlling(){
			return isControlling;
		}

		public SocketIOComponent GetSocket(){
			return socket;
		}
	}
}
