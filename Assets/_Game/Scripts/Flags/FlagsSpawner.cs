using System.Collections.Generic;
using Mirror;
using UnityEngine;
namespace _Game.Scripts.Flags {
    public class FlagsSpawner : MonoBehaviour {
        [SerializeField] private GameObject flagPrefab;
        [SerializeField] private List<GameObject> spawnPoints;
        [SerializeField] private bool randomizeSpawns = true;
        [SerializeField] private Vector2 randomizeSpawnsRange = new Vector2(-9, 9);
        [SerializeField] private GameObject flagsInstancesParent;
        [SerializeField] private int spawnFlagsCount = 10;
        [SerializeField] private List<Flag> allFlags;
        private Dictionary<NetworkConnectionToClient, List<Flag>> _dictionaryPlayersFlags = new Dictionary<NetworkConnectionToClient, List<Flag>>();

        private void Start() {
            if (randomizeSpawns) {
                foreach (GameObject spawnPoint in spawnPoints) {
                    float x = Random.Range(randomizeSpawnsRange.x, randomizeSpawnsRange.y);
                    float z = Random.Range(randomizeSpawnsRange.x, randomizeSpawnsRange.y);
                    spawnPoint.transform.position = new Vector3(x, 0, z);
                }
            }
        }

        public void SpawnFlagsForPlayer(NetworkConnectionToClient networkConnectionToClient) {
            Debug.Log("networkConnectionToClient:" + networkConnectionToClient);
            List<Flag> flagsForPlayer = new List<Flag>();
            if (spawnPoints.Count >= spawnFlagsCount) {
                int indexAddLengh = spawnPoints.Count / spawnFlagsCount;
                // Debug.Log("indexAddLengh:" + indexAddLengh);
                for (int i = 0; i < spawnFlagsCount; i++) {
                    int indexSpawnPoint = i*indexAddLengh + Random.Range(1, indexAddLengh);
                    // Debug.Log("indexSpawnPoint:" + indexSpawnPoint);
                    Transform spawnPointTransform = spawnPoints[indexSpawnPoint].transform;
                    GameObject flagGameObject = Instantiate(flagPrefab, spawnPointTransform.position, Quaternion.identity, flagsInstancesParent.transform);
                    // Debug.Log("flagGameObject:" + flagGameObject);
                    Flag flag = flagGameObject.GetComponent<Flag>();
                    // Debug.Log("flag:" + flag);
                    flag.playerId = networkConnectionToClient.connectionId;
                    CheckFlagPlayerDistance checkFlagPlayerDistance = flagGameObject.GetComponent<CheckFlagPlayerDistance>();
                    checkFlagPlayerDistance.NetworkConnectionToClient = networkConnectionToClient;
                    // allFlags.Add(flag);
                    flagsForPlayer.Add(flag);
                    NetworkServer.Spawn(flagGameObject);
                }
                allFlags.AddRange(flagsForPlayer);
                _dictionaryPlayersFlags.Add(networkConnectionToClient, flagsForPlayer);
            } else {
                Debug.Log($"{spawnPoints.Count} < {spawnFlagsCount}");
            }
        }

        public void RemoveFlagsForPlayer(NetworkConnectionToClient networkConnectionToClient) {
            if (_dictionaryPlayersFlags.TryGetValue(networkConnectionToClient, out var playersFlag)) {
                foreach (Flag flag in playersFlag) {
                    NetworkServer.UnSpawn(flag.gameObject);
                }
                _dictionaryPlayersFlags.Remove(networkConnectionToClient);
            }
        }
    }
}