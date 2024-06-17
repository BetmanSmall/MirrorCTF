using System.Collections;
using _Game.Scripts.MiniGames;
using Mirror;
using UnityEngine;
namespace _Game.Scripts.Flags {
    public class FlagsCollectingLimiter : NetworkBehaviour {
        [SerializeField] private MiniGame miniGame;
        [SerializeField] private FlagsSpawner flagsSpawner;
        [SerializeField] private float timeLimit = 5f;

        private void Start() {
            miniGame.failed.AddListener(() => {
                CmdLimitFlagsCollectForPlayer();
            });
            if (flagsSpawner == null) flagsSpawner.GetComponent<FlagsSpawner>();
        }

        [Command(requiresAuthority = false)]
        private void CmdLimitFlagsCollectForPlayer(NetworkConnectionToClient sender = null) {
            FindAndForeachFlags(sender, false);
            StartCoroutine(ReturnCanChechFlagsForPlayer(sender));
        }

        private IEnumerator ReturnCanChechFlagsForPlayer(NetworkConnectionToClient sender) {
            yield return new WaitForSeconds(timeLimit);
            FindAndForeachFlags(sender, true);
        }

        private void FindAndForeachFlags(NetworkConnectionToClient sender, bool canCheck) {
            if (flagsSpawner.DictionaryPlayersFlags.TryGetValue(sender, out var flags)) {
                foreach (Flag flag in flags) {
                    flag.checkFlagPlayerDistance.canCheck = canCheck;
                }
            }
        }
    }
}