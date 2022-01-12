using UnityEngine;

public class PlayerController3D : PlayerControllerBase {
    #region Serialized Fields
    [SerializeField] private float _forwardMaxSpeed = 0;
    [SerializeField] private float _backwardMaxSpeed = 0;
    [SerializeField] private float _rotationMaxSpeed = 0;
    #endregion

    #region Properties
    private float ForwardMaxSpeed {
        get {
            return Mathf.Max(0.1f, _forwardMaxSpeed);
        }
    }

    private float BackwardMaxSpeed {
        get {
            return Mathf.Max(0.1f, _backwardMaxSpeed);
        }
    }

    private float RotationMaxSpeed {
        get {
            return Mathf.Max(0.1f, _rotationMaxSpeed);
        }
    }
    #endregion

    #region Mono Behaviour Hooks
    private void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Movement
        float speed = v >= 0 ? ForwardMaxSpeed * v : BackwardMaxSpeed * v;
        Vector3 velocity = new Vector3(0, 0, speed);
        velocity = transform.TransformDirection(velocity);

        transform.localPosition += velocity * Time.fixedDeltaTime;

        // Rotation
        transform.Rotate(0, h * RotationMaxSpeed, 0);
    }
    #endregion
}
