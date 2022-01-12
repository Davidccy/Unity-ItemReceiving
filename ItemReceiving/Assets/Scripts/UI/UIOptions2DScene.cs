using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIOptions2DScene : MonoBehaviour {
    #region Serialized Fields
    [Header("Effect")]
    [SerializeField] private Button _btnTypeA = null;
    [SerializeField] private Button _btnTypeB = null;
    [SerializeField] private FruitReceivedEffectHandler _effectTypeA = null;
    [SerializeField] private FruitReceivedEffectHandler _effectTypeB = null;

    [Header("Player Speed")]
    [SerializeField] private PlayerController2D _player = null;
    [SerializeField] private Button _btnSpeedHorPlus = null;
    [SerializeField] private Button _btnSpeedHorMinus = null;
    [SerializeField] private Button _btnSpeedVerPlus = null;
    [SerializeField] private Button _btnSpeedVerMinus = null;
    [SerializeField] private TextMeshProUGUI _textCurSpeedHor = null;
    [SerializeField] private TextMeshProUGUI _textCurSpeedVer = null;

    [Header("Fruit Spawner")]
    [SerializeField] private FruitSpawner _fruitSpawner = null;
    [SerializeField] private Button _btnSpawnerPeriodPlus = null;
    [SerializeField] private Button _btnSpawnerPeriodMinus = null;
    [SerializeField] private Button _btnSpawnerMaxAmountPlus = null;
    [SerializeField] private Button _btnSpawnerMaxAmountMinus = null;
    [SerializeField] private TextMeshProUGUI _textCurSpawnPeriod = null;
    [SerializeField] private TextMeshProUGUI _textCurSpawnMaxAmount = null;
    #endregion

    #region Internal Fields
    private int _curTypeIndex;
    #endregion

    #region Mon Behaviour Hooks
    private void Awake() {
        _curTypeIndex = 0;

        _btnTypeA.onClick.AddListener(ButtonTypeAOnClick);
        _btnTypeB.onClick.AddListener(ButtonTypeBOnClick);

        _btnSpeedHorPlus.onClick.AddListener(ButtonSpeedHorPlusOnClick);
        _btnSpeedHorMinus.onClick.AddListener(ButtonSpeedHorMinusOnClick);
        _btnSpeedVerPlus.onClick.AddListener(ButtonSpeedVerPlusOnClick);
        _btnSpeedVerMinus.onClick.AddListener(ButtonSpeedVerMinusOnClick);

        _btnSpawnerPeriodPlus.onClick.AddListener(ButtonSpawnerPeriodPlusOnClick);
        _btnSpawnerPeriodMinus.onClick.AddListener(ButtonSpawnerPeriodMinusOnClick);
        _btnSpawnerMaxAmountPlus.onClick.AddListener(ButtonSpawnerMaxAmountPlusOnClick);
        _btnSpawnerMaxAmountMinus.onClick.AddListener(ButtonSpawnerMaxAmountMinusOnClick);
    }

    private void OnEnable() {
        Refresh();
    }

    private void OnDestroy() {
        _btnTypeA.onClick.RemoveListener(ButtonTypeAOnClick);
        _btnTypeB.onClick.RemoveListener(ButtonTypeBOnClick);

        _btnSpeedHorPlus.onClick.RemoveListener(ButtonSpeedHorPlusOnClick);
        _btnSpeedHorMinus.onClick.RemoveListener(ButtonSpeedHorMinusOnClick);
        _btnSpeedVerPlus.onClick.RemoveListener(ButtonSpeedVerPlusOnClick);
        _btnSpeedVerMinus.onClick.RemoveListener(ButtonSpeedVerMinusOnClick);

        _btnSpawnerPeriodPlus.onClick.RemoveListener(ButtonSpawnerPeriodPlusOnClick);
        _btnSpawnerPeriodMinus.onClick.RemoveListener(ButtonSpawnerPeriodMinusOnClick);
        _btnSpawnerMaxAmountPlus.onClick.RemoveListener(ButtonSpawnerMaxAmountPlusOnClick);
        _btnSpawnerMaxAmountMinus.onClick.RemoveListener(ButtonSpawnerMaxAmountMinusOnClick);
    }
    #endregion

    #region UI Button Handlings
    private void ButtonTypeAOnClick() {
        _curTypeIndex = 0;

        Refresh();
    }

    private void ButtonTypeBOnClick() {
        _curTypeIndex = 1;

        Refresh();
    }

    private void ButtonSpeedHorPlusOnClick() {
        _player.SpeedHor += 0.1f;

        Refresh();
    }

    private void ButtonSpeedHorMinusOnClick() {
        _player.SpeedHor -= 0.1f;

        Refresh();
    }

    private void ButtonSpeedVerPlusOnClick() {
        _player.SpeedVer += 0.1f;

        Refresh();
    }

    private void ButtonSpeedVerMinusOnClick() {
        _player.SpeedVer -= 0.1f;

        Refresh();
    }

    private void ButtonSpawnerPeriodPlusOnClick() {
        _fruitSpawner.SpawningPeriod += 0.1f;

        Refresh();
    }

    private void ButtonSpawnerPeriodMinusOnClick() {
        _fruitSpawner.SpawningPeriod -= 0.1f;

        Refresh();
    }

    private void ButtonSpawnerMaxAmountPlusOnClick() {
        _fruitSpawner.MaxFruitCount += 1;

        Refresh();
    }

    private void ButtonSpawnerMaxAmountMinusOnClick() {
        _fruitSpawner.MaxFruitCount -= 1;

        Refresh();
    }
    #endregion

    #region Internal Methods
    private void Refresh() {
        // Effect
        _btnTypeA.interactable = _curTypeIndex != 0;
        _btnTypeB.interactable = _curTypeIndex != 1;
        _effectTypeA.gameObject.SetActive(_curTypeIndex == 0);
        _effectTypeB.gameObject.SetActive(_curTypeIndex == 1);

        // Player Speed
        _textCurSpeedHor.text = string.Format("{0:F1}", _player.SpeedHor);
        _textCurSpeedVer.text = string.Format("{0:F1}", _player.SpeedVer);

        // Fruit Spawner
        _textCurSpawnPeriod.text = string.Format("{0:F1}", _fruitSpawner.SpawningPeriod);
        _textCurSpawnMaxAmount.text = string.Format("{0}", _fruitSpawner.MaxFruitCount);
    }
    #endregion
}
