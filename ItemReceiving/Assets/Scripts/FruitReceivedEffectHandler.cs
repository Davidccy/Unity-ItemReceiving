﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FruitReceivedEffectHandler : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private Canvas _canvas = null;
    [SerializeField] private RectTransform _rtEffectGoal = null;
    [SerializeField] private GameObject _goEffectRes = null;
    [SerializeField] private AnimationCurve _aniCurve = null;
    [SerializeField] private ParentConstraint _pc = null;
    #endregion

    #region Internal Fields
    private List<GameObject> _performingGoList = new List<GameObject>();
    #endregion

    #region Mono Behaviour Hooks
    private void Awake() {
        if (_pc.sourceCount > 0) {
            _pc.RemoveSource(0);
        }
        _pc.AddSource(new ConstraintSource { sourceTransform = _rtEffectGoal.transform, weight = 1 });
    }

    private void OnEnable() {
        EventManager.Instance.Register(EventID.FRUIT_RECEIVED, OnFruitReceived);
    }

    private void OnDisable() {
        EventManager.Instance.Unregister(EventID.FRUIT_RECEIVED, OnFruitReceived);
        StopAllCoroutines();

        // Destroy all performaing gameobjects
        for (int i = 0; i < _performingGoList.Count; i++) {
            Destroy(_performingGoList[i]);
        }
    }
    #endregion

    #region Event Handlings
    private void OnFruitReceived(BaseEventArgs args) {
        FruitReceivedEventArgs frArgs = args as FruitReceivedEventArgs;

        FruitBase.FruitType ft = frArgs.Fruit.FType;
        Vector3 receivedPos = frArgs.Position;
        Play(receivedPos);
    }
    #endregion

    #region Internal Methods
    private void Play(Vector3 startPos) {
        // NOTE:
        // "startpos" is world position

        RectTransform rtCanvas = _canvas.transform as RectTransform;

        float canvasWidth = rtCanvas.rect.width;
        float canvasHeight = rtCanvas.rect.height;

        // World position => viewport point => canvas (UI) position
        Camera c = Camera.main;
        Vector3 viewportPoint = c.WorldToViewportPoint(startPos);
        Vector3 canvasPos = new Vector3(
            viewportPoint.x * canvasWidth - canvasWidth / 2,
            viewportPoint.y * canvasHeight - canvasHeight / 2,
            0);

        StartCoroutine(ShowReceivedEffect(canvasPos, _pc.transform.localPosition));
    }

    private IEnumerator ShowReceivedEffect(Vector3 startPos, Vector3 goalPos) {
        GameObject newGo = Instantiate(_goEffectRes, this.transform);
        RectTransform newGoRect = newGo.transform as RectTransform;
        newGo.SetActive(true);
        _performingGoList.Add(newGo);

        float progress = 0;
        while (progress < 1.0f) {
            float interpolates = _aniCurve.Evaluate(progress);
            Vector3 pos = Vector3.Lerp(startPos, goalPos, interpolates);
            newGoRect.anchoredPosition = pos;

            yield return new WaitForEndOfFrame();

            progress += 0.02f;
        }

        newGoRect.anchoredPosition = goalPos;

        // Goal arrived
        FruitReceivingEffectFinishedEventArgs args = new FruitReceivingEffectFinishedEventArgs();
        args.Dispatch();

        ParticleSystem ps = newGo.GetComponent<ParticleSystem>();
        float psLiftTime = 0;
        if (ps != null) {
             psLiftTime = ps.main.startLifetimeMultiplier;

            var emission = ps.emission;
            emission.rateOverDistance = 0;
        }

        Destroy(newGo, psLiftTime);
        _performingGoList.Remove(newGo);
    }
    #endregion
}
