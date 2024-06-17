using Mirror;
using UnityEngine;
namespace _Game.Scripts.Flags {
    public class Flag : NetworkBehaviour {
        [SerializeField] private PlayersMaterials playersMaterials;
        [SerializeField] private MeshRenderer flagModelMeshRenderer;
        [SerializeField] private MeshRenderer flagRadiusMeshRenderer;
        [SyncVar]
        public int playerId = -1;
        public CheckFlagPlayerDistance checkFlagPlayerDistance;

        public void Start() {
            SetPlayerMaterials();
        }

        public void SetPlayerMaterials() {
            flagModelMeshRenderer.material = playersMaterials.normalMaterials[playerId];
            flagRadiusMeshRenderer.material = playersMaterials.transparentMaterials[playerId];
        }
    }
}