using System.Collections;
using UnityEngine;
using TMPro;

public class UIStatus : MonoBehaviour {
    #region Serialized Fields
    [Header("Icon")]
    [SerializeField] private RectTransform _rtFruit = null;
    [SerializeField] private AnimationCurve _aniCurveFruit = null;

    [Header("Amount")]
    [SerializeField] private TextMeshProUGUI _textFruitAmount = null;
    #endregion

    #region Internal Fields
    private int _fruitCount = 0;
    #endregion

    #region Mono Behaviour Hooks
    private void OnEnable() {
        Refresh();
        EventManager.Instance.Register(EventID.FRUIT_RECEIVED, OnFruitReceived);
        EventManager.Instance.Register(EventID.FRUIT_RECEIVING_EFFECT_FINISHED, OnFruitReceivingEffectFinished);
    }

    private void OnDisable() {
        EventManager.Instance.Register(EventID.FRUIT_RECEIVED, OnFruitReceived);
        EventManager.Instance.Register(EventID.FRUIT_RECEIVING_EFFECT_FINISHED, OnFruitReceivingEffectFinished);
    }
    #endregion

    #region Event Handlings
    private void OnFruitReceived(BaseEventArgs args) {
        FruitReceivedEventArgs frArgs = args as FruitReceivedEventArgs;

        int amount = frArgs.Amount;
        FruitReceived(amount);
    }

    private void OnFruitReceivingEffectFinished(BaseEventArgs args) {
        //FruitReceivingEffectFinishedEventArgs frefArgs = args as FruitReceivingEffectFinishedEventArgs;
        PlayFruitReceivedEffect();
    }
    #endregion

    #region Internal Methods
    private void Refresh() {
        _textFruitAmount.text = string.Format("{0}", _fruitCount);
    }

    private void FruitReceived(int count) {
        _fruitCount += count;

        Refresh();
    }

    private void PlayFruitReceivedEffect() {
        StopAllCoroutines();
        StartCoroutine(PlayFruitBouncing());
    }

    private IEnumerator PlayFruitBouncing() {
        float progress = 0;

        _rtFruit.localScale = Vector3.one;

        while (progress < 1) {
            float scale = _aniCurveFruit.Evaluate(progress);

            _rtFruit.localScale = Vector3.one * scale;

            yield return new WaitForEndOfFrame();

            progress += 0.05f;
        }

        _rtFruit.localScale = Vector3.one;
    }
    #endregion
}
