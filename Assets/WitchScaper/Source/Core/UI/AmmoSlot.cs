using System;
using UnityEngine;
using UnityEngine.UI;
using WitchScaper.Core.Character;
using WitchScaper.Core.State;

namespace WitchScaper.Core.UI
{
    public class AmmoSlot : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _notActiveShield;

        public void SetEnabled(bool state)
        {
            _notActiveShield.SetActive(!state);
        }
        
        public void SetColor(ColorType colorType)
        {
            switch (colorType)
            {
                case ColorType.Green:
                    _image.color = Color.green;
                    break;
                case ColorType.Red:
                    _image.color = Color.red;
                    break;
                case ColorType.Blue:
                    _image.color = Color.blue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null);
            }   
        }
    }
}