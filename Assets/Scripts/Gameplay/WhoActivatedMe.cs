using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceMulti.Utility.Attributes;
namespace SpaceMulti.Gameplay{
	public class WhoActivatedMe : MonoBehaviour{
		[SerializeField]
		[GreyOut]
		private string whoActivateMe;
		public string WhoActivateMe{
			get{return whoActivateMe;}
			set{whoActivateMe = value;}
		}
	}
}
