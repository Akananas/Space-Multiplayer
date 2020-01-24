using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceMulti.Shader{
	public class ImageEffectScript : MonoBehaviour{
		[SerializeField]
		private Material mat;
		public bool IsDead{get;set;}
		private void Start() {
			IsDead = false;
		}
		void OnRenderImage(RenderTexture src, RenderTexture dest){
			if(IsDead){
      			Graphics.Blit(src, dest, mat);
			}else{
				Graphics.Blit(src, dest);
			}
   		}
	}
}
