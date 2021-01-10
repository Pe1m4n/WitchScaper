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
        
        public void SetColor(Color color)
        {
            _image.color = color;
        }
    }
}