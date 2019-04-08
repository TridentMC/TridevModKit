using RoR2.Networking;
using TridevLoader.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TridevLoader.Core.Game.Modded {
    public class DebugBehaviour : MonoBehaviour {
        private int sceneLoadTimer = 120;

        private void Awake() {
            DebugScreen.Log("Added DebugBehaviour");
            SceneManager.activeSceneChanged += (current, next) => { this.sceneLoadTimer = 360; };
            DontDestroyOnLoad(this);
        }

        private void FixedUpdate() {
            if (this.sceneLoadTimer == 0) {
                var allObjects = FindObjectsOfType<GameObject>();
                bool foundManager = false;
                foreach (var obj in allObjects) {
                    var gameNetworkManager = obj.GetComponent<GameNetworkManager>();
                    if (gameNetworkManager) {
                        foundManager = true;
                        DebugScreen.Log("Found game network manager, dumping channel data.");
                        foreach (var channel in gameNetworkManager.channels) {
                            DebugScreen.Log(channel.ToString());
                        }
                    }
                }

                if (!foundManager) {
                    DebugScreen.Log("No network manager was found.");
                }

                this.sceneLoadTimer = -1;
            }

            this.sceneLoadTimer = this.sceneLoadTimer > 0 ? this.sceneLoadTimer - 1 : this.sceneLoadTimer;
        }
    }
}