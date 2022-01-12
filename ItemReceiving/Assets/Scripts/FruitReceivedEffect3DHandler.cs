using System.Collections;
using UnityEngine;

public class FruitReceivedEffect3DHandler : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private Camera _mainCamera = null;
    [SerializeField] private UIStatus _uiStatus = null;
    [SerializeField] private Fruit3D _goFruitRes = null;
    [SerializeField] private AnimationCurve _aniCurve = null;
    #endregion

    #region Mono Behaviour Hooks
    private void OnEnable() {
        EventManager.Instance.Register(EventID.FRUIT_RECEIVED, OnFruitReceived);
    }

    private void OnDisable() {
        EventManager.Instance.Register(EventID.FRUIT_RECEIVED, OnFruitReceived);
    }
    #endregion

    #region Event Handlings
    private void OnFruitReceived(BaseEventArgs args) {
        FruitReceivedEventArgs frArgs = args as FruitReceivedEventArgs;

        FruitBase.FruitType ft = frArgs.Fruit.FType;
        if (ft != FruitBase.FruitType.Fruit3D) {
            return;
        }

        Vector3 receivedPos = frArgs.Position;
        StartCoroutine(Play(frArgs.Fruit as Fruit3D));
    }
    #endregion

    #region Internal Methods
    private IEnumerator Play(Fruit3D fruit) {
        Debug.LogErrorFormat("play");
        Fruit3D newFruit = Instantiate(_goFruitRes, this.transform);
        newFruit.SetCanBeRecieved(false);
        newFruit.transform.position = fruit.transform.position;
        newFruit.transform.rotation = fruit.transform.rotation;
        newFruit.transform.localScale = fruit.transform.localScale;

        float progress = 0;
        while (progress < 1.0f) {
            float interpolates = _aniCurve.Evaluate(progress);
            Vector3 startPos = newFruit.transform.position;
            Vector3 goalPos = _mainCamera.ViewportToWorldPoint(_uiStatus.FruitViewport);
            newFruit.transform.position = Vector3.Lerp(startPos, goalPos, interpolates);

            yield return new WaitForEndOfFrame();

            progress += 0.05f;
        }

        Destroy(newFruit.gameObject);
    }
    #endregion
}
