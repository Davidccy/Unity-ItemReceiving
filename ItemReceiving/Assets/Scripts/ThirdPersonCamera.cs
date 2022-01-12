using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private Transform _posNormal = null;
    [SerializeField] private Transform _posFront = null;
    #endregion

    #region Internal Fields
    private Transform _targetPos = null;
    #endregion

    #region Mono Behaviour Hooks
    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.Space)) {
            ChangeTargetPosition(_posFront);
        }
        else {
            ChangeTargetPosition(_posNormal);
        }

        SmoothMoveCamera();
    }
    #endregion

    #region Internal Methods
    private void ChangeTargetPosition(Transform pos) {
        if (pos == _targetPos) {
            return;
        }

        _targetPos = pos;
    }

    private void SmoothMoveCamera() {
        if (_targetPos == null) {
            return;
        }

        this.transform.position = Vector3.Lerp(this.transform.position, _targetPos.position, 0.5f);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _targetPos.rotation, 1f);
    }
    #endregion
}
