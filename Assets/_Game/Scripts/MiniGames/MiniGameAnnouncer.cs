using System.Collections;
using Mirror;
using UnityEngine;
namespace _Game.Scripts.MiniGames {
    public class MiniGameAnnouncer : NetworkBehaviour {
        [SerializeField] private GameObject panel;
        [SerializeField] private MiniGame miniGame;

        private void Start() {
            miniGame.failed.AddListener(() => {
                CmdPlayerFailedMiniGame(NetworkClient.localPlayer.netId);
            });
        }

        [Command(requiresAuthority = false)]
        private void CmdPlayerFailedMiniGame(uint senderNetId) {
            RpcPlayerFailedMiniGame(senderNetId);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcPlayerFailedMiniGame(uint senderNetId) {
            if (senderNetId != NetworkClient.localPlayer.netId) {
                panel.SetActive(true);
                StartCoroutine(DisablePanelAfter(2f));
            }
        }

        private IEnumerator DisablePanelAfter(float time) {
            yield return new WaitForSeconds(time);
            panel.SetActive(false);
        }
    }
}