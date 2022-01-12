using UnityEngine;

public class PlayerController2D : PlayerControllerBase {
    #region Serialized Fields
    [SerializeField] private float _speedHor = 0;
    [SerializeField] private float _speedVer = 0;
    #endregion

    #region Internal Fields
    private float _inputHor = 0;
    private float _inputVer = 0;
    #endregion

    #region Mono Behaviour Hooks
    private void Update() {
        _inputHor = Input.GetAxisRaw("Horizontal");
        _inputVer = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate() {
        this.transform.Translate(
            _speedHor * _inputHor * Time.fixedDeltaTime,
            _speedVer * _inputVer * Time.fixedDeltaTime,
            0);
    }
    #endregion
}
