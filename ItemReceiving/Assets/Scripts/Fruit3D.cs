using System;
using System.Collections;
using UnityEngine;

public class Fruit3D : FruitBase {

    public Fruit3D() {
        FType = FruitType.Fruit3D;
    }

    #region Internal Fields
    private Action<Fruit3D> _onTriggeredAction = null;
    #endregion

    #region Mono Behaviour Hooks
    private void Awake() {
        StartCoroutine(StartRotating());
    }
    private void OnDestroy() {
        StopAllCoroutines();
    }
    #endregion

    #region APIs
    public void SetTriggeredAction(Action<Fruit3D> action) {
        _onTriggeredAction = action;
    }
    #endregion

    #region Internal Methods
    private IEnumerator StartRotating() {
        while (true) {
            this.transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 90f);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter(Collider collision) {
        PlayerController3DDetector detector = collision.GetComponent<PlayerController3DDetector>();
        if (detector != null && _canBeRecieved) {
            FruitReceivedEventArgs frArgs = new FruitReceivedEventArgs();
            frArgs.Amount = 1;
            frArgs.Fruit = this;
            frArgs.Position = this.transform.position;
            frArgs.Dispatch();
        }
    }
    #endregion
}
