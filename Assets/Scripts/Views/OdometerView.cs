using System;
using Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.Interfaces;
using Zenject;

namespace Views
{
    public class OdometerView : MonoBehaviour, IOdometerView
    {
        [SerializeField] private Image _arrow;
        [SerializeField] private TextMeshProUGUI _currentValue, _odometerNumber;
        [SerializeField] private Toggle _serverConnectionToggle, _randomStatusToggle;
        [SerializeField] private float _odometerMinValue, _odometerMaxValue;
        [Inject] private OdometerPresenter _presenter;
        private float _currentOdometerValue;
        private float _numberTransitionDuration = 1f;


        public bool IsRandom
        {
            get => _randomStatusToggle.isOn; 
            set => _randomStatusToggle.isOn = value;
        }

        public string CurrentValue
        {
            get => _currentValue.text;
            set => _currentValue.text = value;
        }

        public string OdometerNumber
        {
            get => _odometerNumber.text;
            set => _odometerNumber.text = value;
        }

        public void UpdateArrowPosition(float odometerValue)
        {
            float angle = Mathf.Lerp(0f, 50f, Mathf.InverseLerp(_odometerMinValue, _odometerMaxValue, odometerValue));
            LeanTween.rotateZ(_arrow.gameObject, angle, 1f).setEase(LeanTweenType.easeOutQuad);
        }
        public void AnimateNumberTransition(float newOdometerValue)
        {
            if (_currentOdometerValue != newOdometerValue)
            {
                LeanTween.value(gameObject, _currentOdometerValue, newOdometerValue, _numberTransitionDuration)
                    .setOnUpdate(UpdateValueNumber)
                    .setEase(LeanTweenType.linear);
                _currentOdometerValue = newOdometerValue;
            }
        }
        private void UpdateValueNumber(float value)
        {
            _currentValue.text = value.ToString();
        }

        public void ChangeServerStatus(bool isActive)
        {
            if (isActive)
            {
                _serverConnectionToggle.targetGraphic.color = Color.green;
            }
            else
            {
                _serverConnectionToggle.targetGraphic.color = Color.red;
            }
        }
    }
}