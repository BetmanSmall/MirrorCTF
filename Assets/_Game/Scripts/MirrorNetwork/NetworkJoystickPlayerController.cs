using Mirror;
using UnityEngine;
namespace _Game.Scripts.MirrorNetwork {
    public class NetworkJoystickPlayerController : NetworkBehaviour {
        [SerializeField] private bool moveLimit = true;
        [SerializeField] private Vector3 moveLimitMin = new Vector3(-9, 0, -9); // todo need work with colliders
        [SerializeField] private Vector3 moveLimitMax = new Vector3(9, 0, 9); // todo need work with colliders
        [SerializeField] private float speed = 10f;
        private VariableJoystick _variableJoystick;

        private void Start() {
            Debug.Log("isClient:" + isClient);
            Debug.Log("isOwned:" + isOwned);
            Debug.Log("isServer:" + isServer);
            Debug.Log("isClientOnly:" + isClientOnly);
            Debug.Log("isServerOnly:" + isServerOnly);
            Debug.Log("isLocalPlayer:" + isLocalPlayer);
            Debug.Log("isActiveAndEnabled:" + isActiveAndEnabled);
            if (isLocalPlayer) {
                Debug.Log("1_variableJoystick:" + _variableJoystick);
                _variableJoystick = FindObjectOfType<VariableJoystick>();
                Debug.Log("2_variableJoystick:" + _variableJoystick);
            } else {
                Destroy(this);
            }
        }

        public void Update() {
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