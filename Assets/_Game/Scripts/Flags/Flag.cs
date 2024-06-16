using Mirror;
using UnityEngine;
namespace _Game.Scripts.Flags {
    public class Flag : NetworkBehaviour {
        [SerializeField] private PlayersMaterials playersMaterials;
        [SerializeField] private MeshRenderer flagModelMeshRenderer;
        [SerializeField] private MeshRenderer flagRadiusMeshRenderer;
        [SyncVar]
        public int playerId = -1;

        public void Start() {
            // Debug.Log("Flag::Start(); -- playerId:" + playerId);
            SetPlayerMaterials();
        }

        public void SetPlayerMaterials() {
            // Debug.Log("Flag::SetPlayerMaterials(); -- playerId:" + playerId);
            // Debug.Log("Flag::SetPlayerMaterials(); -- flagModelMeshRenderer:" + flagModelMeshRenderer);
            // Debug.Log("Flag::SetPlayerMaterials(); -- flagRadiusMeshRenderer:" + flagRadiusMeshRenderer);
            flagModelMeshRenderer.material = playersMaterials.normalMaterials[playerId];
            flagRadiusMeshRenderer.material = playersMaterials.transparentMaterials[playerId];
        }
    }
}