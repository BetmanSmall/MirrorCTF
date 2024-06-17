using _Game.Scripts.Flags;
using Mirror;
using UnityEngine;
namespace _Game.Scripts.MirrorNetwork {
    public class NetworkJoystickPlayerController : NetworkBehaviour {
        [SerializeField] private PlayersMaterials playersMaterials;
        [SerializeField] private MeshRenderer playerMeshRenderer;
        [SerializeField] private bool moveLimit = true;
        [SerializeField] private Vector3 moveLimitMin = new Vector3(-9, 0, -9); // todo need work with colliders
        [SerializeField] private Vector3 moveLimitMax = new Vector3(9, 0, 9); // todo need work with colliders
        [SerializeField] private float speed = 10f;
        private VariableJoystick _variableJoystick;
        [SyncVar(hook = nameof(SyncVarPlayerId))]
        private int _playerId =- 1;

        private void Start() {
            if (netIdentity?.connectionToClient != null) {
                _playerId = netIdentity.connectionToClient.connectionId;
                SyncVarPlayerId(-1, _playerId);
            }
            if (isLocalPlayer) {
                _variableJoystick = FindObjectOfType<VariableJoystick>();
            } else {
                Destroy(this);
            }
        }
        
        private void SyncVarPlayerId(int oldValue, int newValue) {
            playerMeshRenderer.material = playersMaterials.normalMaterials[newValue];
        }

        public void Update() {
            if (_variableJoystick) {
                Vector3 direction = Vector3.forward * _variableJoystick.Vertical + Vector3.right * _variableJoystick.Horizontal;
                if (moveLimit) {
                    if (transform.position.x <= moveLimitMin.x && direction.x < 0f ||
                        transform.position.x >= moveLimitMax.x && direction.x > 0f) {
                        direction.x = 0f;
                    }
                    if (transform.position.z <= moveLimitMin.z && direction.z < 0f ||
                        transform.position.z >= moveLimitMax.z && direction.z > 0f) {
                        direction.z = 0f;
                    }
                }
                gameObject.transform.Translate(direction * (speed * Time.fixedDeltaTime));
            }
        }
    }
}