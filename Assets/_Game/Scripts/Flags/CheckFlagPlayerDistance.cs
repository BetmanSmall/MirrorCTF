using _Game.Scripts.MiniGames;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;
namespace _Game.Scripts.Flags {
    public class CheckFlagPlayerDistance : NetworkBehaviour {
        public NetworkConnectionToClient _networkConnectionToClient;
        public NetworkConnectionToClient NetworkConnectionToClient {
            set => _networkConnectionToClient = value;
        }
        [SerializeField] private float radiusDetectPlayer = 2.5f;
        [SerializeField] private float timeToCapture = 1.5f;
        private float _timeToCaptureElapsed = 0f;

        private void Update() {
            if (_networkConnectionToClient != null && _networkConnectionToClient.identity != null) {
                if (Vector3.Distance(transform.position, _networkConnectionToClient.identity.transform.position) < radiusDetectPlayer) {
                    _timeToCaptureElapsed += Time.deltaTime;
                    if (_timeToCaptureElapsed >= timeToCapture) {
                        _timeToCaptureElapsed = 0f;
                        if (Random.Range(0f, 1f) > 0.1f) {
                            TryOpenMiniGame();
                        }
                        NetworkServer.UnSpawn(gameObject);
                    }
                }
            }
        }

        private void TryOpenMiniGame() {
            Debug.Log("MiniGame::TryOpenMiniGame(); -- NetworkServer.localConnection:" + NetworkServer.localConnection);
            Debug.Log("MiniGame::TryOpenMiniGame(); -- _networkConnectionToClient:" + _networkConnectionToClient);
            if (NetworkServer.localConnection == _networkConnectionToClient) {
                OpenLocalMiniGame();
            } else {
                TargetOpenMiniGame(_networkConnectionToClient);
            }
        }

        [TargetRpc]
        private void TargetOpenMiniGame(NetworkConnection networkConnection) {
            OpenLocalMiniGame();
        }

        private void OpenLocalMiniGame() {
            MiniGame.Instance.gameObject.SetActive(true);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, radiusDetectPlayer);
        }
    }
}