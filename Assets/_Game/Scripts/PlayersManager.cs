using System.Collections.Generic;
using _Game.Scripts.Flags;
using Mirror;
using UnityEngine;
namespace _Game.Scripts {
    public class PlayersManager : MonoBehaviour {
        [SerializeField] private ActionsNetworkManager actionsNetworkManager;
        [SerializeField] private FlagsSpawner flagsSpawner;
        [SerializeField] private List<NetworkConnectionToClient> _clients = new List<NetworkConnectionToClient>();

        private void Start() {
            actionsNetworkManager.OnServerAddPlayerAction += OnServerAddPlayerAction;
            actionsNetworkManager.OnServerDisconnectAction += OnServerDisconnectAction;
        }

        private void OnServerAddPlayerAction(NetworkConnectionToClient networkConnectionToClient) {
            Debug.Log("PlayersManager::OnServerAddPlayerAction(); -- networkConnectionToClient:" + networkConnectionToClient);
            Debug.Log("PlayersManager::OnServerAddPlayerAction(); -- networkConnectionToClient.identity:" + networkConnectionToClient.identity);
            flagsSpawner.SpawnFlagsForPlayer(networkConnectionToClient);
            _clients.Add(networkConnectionToClient);
        }

        private void OnServerDisconnectAction(NetworkConnectionToClient networkConnectionToClient) {
            Debug.Log("PlayersManager::OnServerDisconnectAction(); -- " + networkConnectionToClient);
            Debug.Log("PlayersManager::OnServerDisconnectAction(); -- networkConnectionToClient.identity:" + networkConnectionToClient.identity);
            flagsSpawner.RemoveFlagsForPlayer(networkConnectionToClient);
        }
    }
}