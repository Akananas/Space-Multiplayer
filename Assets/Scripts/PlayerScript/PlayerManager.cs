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
		private ParticleSystem particle;
		private Cooldown shootingCooldown;
		private BulletData bulletData;
		private Rigidbody rigidbody;
		private AudioSource audioSource;
		private bool canRotate;
		private void Start() {
			shootingCooldown = new Cooldown(0.75f);
			canRotate = true;
			bulletData = new BulletData();
			bulletData.position = new Position();
			bulletData.direction = new Position();
			particle = bulletSpawnPoint.gameObject.GetComponent<ParticleSystem>();
			rigidbody = GetComponent<Rigidbody>();
			audioSource = GetComponent<AudioSource>();
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
			transform.position += transform.forward * vertical * speed * Time.deltaTime;
			if(canRotate){
				transform.Rotate(new Vector3(0,horizontal * rotation * Time.deltaTime,0));
			}
		}

		private void CheckShooting(){
			shootingCooldown.CooldownUpdate();
			if(Input.GetButton("Fire1") && !shootingCooldown.IsOnCooldown()){
				StartCoroutine(Aiming());
				shootingCooldown.StartCooldown();
				//Define bullet
				bulletData.activator = NetworkClient.ClientID;
				bulletData.position.VectorToString(bulletSpawnPoint.position);
				bulletData.direction.VectorToString(-bulletSpawnPoint.forward);
				//Send bullet
				networkIdentity.GetSocket().Emit("fireBullet", new JSONObject(JsonUtility.ToJson(bulletData)));
			}
		}

		public void ShootingEffects(){
			Debug.Log("particles");
			particle.Play();
			audioSource.Play();
		}

		private void OnCollisionExit(Collision other) {
			rigidbody.velocity = Vector3.zero;
		}

		IEnumerator Aiming(){
			canRotate = false;
			yield return new WaitForSeconds(0.25f);
			canRotate = true;
		}
	}
}
