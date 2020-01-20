using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using SpaceMulti.Utility;

namespace SpaceMulti.Networking{
    public class NetworkClient : SocketIOComponent{

        [Header("Network Client")]
        [SerializeField]
        private Transform networkContainer;

        private Dictionary<string, GameObject> serverObjects;
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
            serverObjects = new Dictionary<string, GameObject>();
        }

        private void SetupEvents(){
            On("open", (E) => {
                Debug.Log("Connection made to the server");
            });
            On("register", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();
                Debug.LogFormat("Our client's ID ({0})", id);
            });
            On("spawn", (E) => {
                //Spawn every players
                string id = E.data["id"].ToString().RemoveQuotes();

                GameObject go = new GameObject("Server ID:" + id);
                go.transform.SetParent(networkContainer);
                serverObjects.Add(id,go);
            });
            On("disconnected", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();

                GameObject go = serverObjects[id];
                Destroy(go); //Remove gameobject from game
                serverObjects.Remove(id); //Remove id from memory
            });
        }
    }
}
