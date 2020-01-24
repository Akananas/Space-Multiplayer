using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using SpaceMulti.Utility;
using SpaceMulti.Scriptable;
using SpaceMulti.Gameplay;
using SpaceMulti.PlayerScript;
using SpaceMulti.Shader;
namespace SpaceMulti.Network{
    public class NetworkClient : SocketIOComponent{
        public const float SERVER_UPDATE_TIME = 10;
        [Header("Network Client")]
        [SerializeField]
        private Transform networkContainer;
        [SerializeField]
        private GameObject playerPrefab; 
        [SerializeField]
        private ServerObjects serverSpawnables;
        [SerializeField]
        private ImageEffectScript imageEffectScript;
        [SerializeField]
        List<Transform> spawnPoint = new List<Transform>();
        public static string ClientID{get; private set;}
        private Dictionary<string, NetworkIdentity> serverObjects;
        // Start is called before the first frame update
        public override void Start(){
            base.Start();
            Initialize();
            SetupEvents();
        }

        // Update is called once per frame
        public override void Update(){
            base.Update();
        }
        private void Initialize(){
            serverObjects = new Dictionary<string, NetworkIdentity>();
        }

        private void SetupEvents(){
            On("open", (E) => {
                Debug.Log("Connection made to the server");
            });
            On("register", (E) => {
                ClientID = E.data["id"].ToString().RemoveQuotes();
                Debug.LogFormat("Our client's ID {0}", ClientID);
            });
            On("spawn", (E) => {
                //Spawn every players
                string id = E.data["id"].ToString().RemoveQuotes();
                GameObject go = Instantiate(playerPrefab, networkContainer);
                go.transform.position = spawnPoint[Random.Range(0,4)].position;
                go.name = string.Format("Player {0}", id);
                NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
                ni.SetControllerID(id);
                ni.SetSocketReference(this);
                serverObjects.Add(id,ni);
            });
            On("disconnected", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();

                GameObject go = serverObjects[id].gameObject;
                Destroy(go); //Remove gameobject from game
                serverObjects.Remove(id); //Remove id from memory
            });
            On("updatePosition", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();
                float x = E.data["position"]["x"].JSONToFloat();
                float y = E.data["position"]["y"].JSONToFloat();
                float z = E.data["position"]["z"].JSONToFloat();
                NetworkIdentity ni = serverObjects[id];
                ni.transform.rotation = Quaternion.Euler(0,float.Parse(E.data["rotation"].str),0);
                ni.transform.position = new Vector3(x,y,z);
            });
            //Need to replace with a pool
            On("serverSpawn",(E) => {
                string name = E.data["name"].str;
                string id = E.data["id"].ToString().RemoveQuotes();
                float x = E.data["position"]["x"].JSONToFloat();
                float y = E.data["position"]["y"].JSONToFloat();
                float z = E.data["position"]["z"].JSONToFloat();
                Debug.LogFormat("Server wants us to spawn a '{0}' with id '{1}'", name, id);

                if(!serverObjects.ContainsKey(id)){
                    ServerObjectData sod = serverSpawnables.GetObjectByName(name);
                    var spawnedObject = Instantiate(sod.Prefab, networkContainer);
                    spawnedObject.transform.position = new Vector3(x,y,z);
                    var ni = spawnedObject.GetComponent<NetworkIdentity>();
                    ni.SetControllerID(id);
                    ni.SetSocketReference(this);
                    
                    if(name == "Bullet"){
                        float directionX = E.data["direction"]["x"].JSONToFloat();
                        float directionY = E.data["direction"]["y"].JSONToFloat();
                        float directionZ = E.data["direction"]["z"].JSONToFloat();
                        string activator = E.data["activator"].ToString().RemoveQuotes();
                        float speed = E.data["speed"].JSONToFloat();
                        GameObject activatorGO = serverObjects[activator].gameObject;
                        spawnedObject.transform.rotation = serverObjects[activator].gameObject.transform.rotation;
                        WhoActivatedMe whoActivatedMe = spawnedObject.GetComponent<WhoActivatedMe>();
                        whoActivatedMe.WhoActivateMe = activator;
                        activatorGO.GetComponent<PlayerManager>().ShootingEffects();
                        Projectile projectile = spawnedObject.GetComponent<Projectile>();
                        projectile.Direction = new Vector3(directionX,directionY,directionZ);
                        projectile.Speed = speed;
                    }
                    
                    serverObjects.Add(id,ni);
                }
            });
            On("serverUnspawn", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();
                NetworkIdentity ni = serverObjects[id];
                serverObjects.Remove(id);
                DestroyImmediate(ni.gameObject);
            });
            On("playerDied", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();
                NetworkIdentity ni = serverObjects[id];
                if(id == ClientID){
                    imageEffectScript.IsDead = true;
                }
                ni.gameObject.SetActive(false);
            });
            On("playerRespawn", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();
                if(id == ClientID){
                    imageEffectScript.IsDead = false;
                }
                NetworkIdentity ni = serverObjects[id];
                ni.transform.position = spawnPoint[Random.Range(0,4)].position;
                ni.gameObject.SetActive(true);
            });
        }
    }
}
