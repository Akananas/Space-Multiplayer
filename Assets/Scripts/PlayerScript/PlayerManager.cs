using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceMulti.Utility;
using SpaceMulti.Network;
using SpaceMulti.Network.ObjectData;
namespace SpaceMulti.PlayerScript{
	public class PlayerManager : MonoBehaviour{
		[Header("Data")]
		[SerializeField]
		private float speed = 10;
		[SerializeField]
		private float rotation = 90;
		[Header("Class References")]
		[SerializeField]
		private NetworkIdentity networkIdentity;
		[SerializeField]
		private Transform bulletSpawnPoint;
		private Cooldown shootingCooldown;
		private BulletData bulletData;
		private void Start() {
			shootingCooldown = new Cooldown(1);
			bulletData = new BulletData();
			bulletData.position = new Position();
			bulletData.direction = new Position();
		}
		private void Update() {
			if(networkIdentity.IsControlling()){
				CheckMovement();
				CheckShooting();
			}
		} 
		private void CheckMovement(){
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxisRaw("Vertical");
			transform.Rotate(new Vector3(0,horizontal * rotation * Time.deltaTime,0));
			transform.position += transform.forward * vertical * speed * Time.deltaTime;
		}

		private void CheckShooting(){
			shootingCooldown.CooldownUpdate();
			if(Input.GetButton("Fire1") && !shootingCooldown.IsOnCooldown()){
				shootingCooldown.StartCooldown();
				//Define bullet
				bulletData.activator = NetworkClient.ClientID;
				bulletData.position.VectorToString(bulletSpawnPoint.position);
				bulletData.direction.VectorToString(-bulletSpawnPoint.forward);
				//Send bullet
				networkIdentity.GetSocket().Emit("fireBullet", new JSONObject(JsonUtility.ToJson(bulletData)));
			}
		}
	}
}
