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
		private float speed = 4;
		[SerializeField]
		private float rotation = 60;
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
			float vertical = Input.GetAxis("Vertical");
			transform.Rotate(new Vector3(0,horizontal * rotation * Time.deltaTime,0));
			transform.position += new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
		}

		private void CheckShooting(){
			shootingCooldown.CooldownUpdate();
			if(Input.GetMouseButton(0) && !shootingCooldown.IsOnCooldown()){
				shootingCooldown.StartCooldown();
				//Define bullet
				bulletData.position.VectorToString(bulletSpawnPoint.position);
				bulletData.direction.VectorToString(-bulletSpawnPoint.forward);
				Debug.Log(bulletData.direction.z); 
				//Send bullet
				networkIdentity.GetSocket().Emit("fireBullet", new JSONObject(JsonUtility.ToJson(bulletData)));
			}
		}
	}
}
