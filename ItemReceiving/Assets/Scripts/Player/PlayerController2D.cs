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

    #region Properties
    public float SpeedHor {
        get {
            return Mathf.Max(0.1f, _speedHor);
        }
        set {
            _speedHor = Mathf.Max(0.1f, value);
        }
    }

    public float SpeedVer {
        get {
            return Mathf.Max(0.1f, _speedVer);
        }
        set {
            _speedVer = Mathf.Max(0.1f, value);
        }
    }
    #endregion

    #region Mono Behaviour Hooks
    private void Update() {
        _inputHor = Input.GetAxisRaw("Horizontal");
        _inputVer = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate() {
        this.transform.Translate(
            SpeedHor * _inputHor * Time.fixedDeltaTime,
            SpeedVer * _inputVer * Time.fixedDeltaTime,
            0);
    }
    #endregion
}
