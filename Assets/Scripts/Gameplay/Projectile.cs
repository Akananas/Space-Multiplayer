using System.Collections;
using System.Collections.Generic;
using SpaceMulti.Network;
using UnityEngine;
namespace SpaceMulti.Gameplay{
	public class Projectile : MonoBehaviour{
		private Vector3 direction;
		public Vector3 Direction{
			set{
				direction = value;
			}
		}
		private float speed;
		public float Speed{
			set{
				speed = value;
			}
		}
		private void Update() {
			Vector3 pos = direction * speed * NetworkClient.SERVER_UPDATE_TIME * Time.deltaTime;
			transform.position += pos;
		}
	}
}
