using System;
using System.Collections;
using UnityEngine;

public class Fruit : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private AnimationCurve _aniCurve = null; // For bouncing
    #endregion

    #region Internal Fields
    private Action<Fruit> _onTriggeredAction = null;
    #endregion

    #region Mono Behaviour Hooks
    private void Awake() {
        StartCoroutine(StartBouncing());
    }
    private void OnDestroy() {
        StopAllCoroutines();
    }
    #endregion

    #region APIs
    public void SetTriggeredAction(Action<Fruit> action) {
        _onTriggeredAction = action;
    }
    #endregion

    #region Internal Methods
    private IEnumerator StartBouncing() {
        float progress = 0;
        bool isReverse = false;

        while (true) {
            float scale = _aniCurve.Evaluate(progress);
            this.transform.localScale = Vector3.one * scale * 0.1f;

            yield return new WaitForEndOfFrame();

            progress += isReverse ? -0.01f : 0.01f;
            progress = Mathf.Clamp(progress, 0.0f, 1.0f);

            if (!isReverse && progress >= 1) {
                isReverse = true;
            }
            else if (isReverse && progress <= 0) {
                isReverse = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController2D player = collision.GetComponent<PlayerController2D>();
        if (player != null) {
            if (_onTriggeredAction != null) {
                _onTriggeredAction(this);
            }
        }
    }
    #endregion
}
