using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using SpaceMulti.Utility;

namespace SpaceMulti.Network{
    public class NetworkClient : SocketIOComponent{

        [Header("Network Client")]
        [SerializeField]
        private Transform networkContainer;
        [SerializeField]
        private GameObject playerPrefab; 
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
                Debug.LogFormat("Our client's ID ({0})", ClientID);
            });
            On("spawn", (E) => {
                //Spawn every players
                string id = E.data["id"].ToString().RemoveQuotes();

                GameObject go = Instantiate(playerPrefab, networkContainer);
                go.name = string.Format("Player ({0})", id);
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
                Debug.Log(id);
                float x = float.Parse(E.data["position"]["x"].str);
                float y = float.Parse(E.data["position"]["y"].str);
                float z = float.Parse(E.data["position"]["z"].str);

                NetworkIdentity ni = serverObjects[id];
                ni.transform.position = new Vector3(x,y,z);
            });
        }
    }
}
