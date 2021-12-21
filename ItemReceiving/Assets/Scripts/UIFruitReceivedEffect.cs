using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class UIFruitReceivedEffect : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private UIStatus _uiStatus = null;
    [SerializeField] private Canvas _canvas = null;
    [SerializeField] private RectTransform _rtEffectGoal = null;
    [SerializeField] private GameObject _goEffectRes = null;
    [SerializeField] private AnimationCurve _aniCurve = null;
    [SerializeField] private ParentConstraint _pc = null;
    #endregion

    #region Mono Behaviour Hooks
    private void Awake() {
        if (_pc.sourceCount > 0) {
            _pc.RemoveSource(0);
        }
        _pc.AddSource(new ConstraintSource { sourceTransform = _rtEffectGoal.transform, weight = 1 });
    }
    #endregion

    #region Internal Methods
    public void Play(Vector3 startPos) {
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

        ParticleSystem ps = newGo.GetComponent<ParticleSystem>();
        float psLiftTime = ps.main.startLifetimeMultiplier;

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
        var emission = ps.emission;
        emission.rateOverDistance = 0;

        _uiStatus.PlayFruitReceivedEffect();

        Destroy(newGo, psLiftTime);
    }
    #endregion
}
