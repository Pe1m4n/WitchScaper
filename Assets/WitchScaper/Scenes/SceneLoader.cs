using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
namespace WitchScaper.Common
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        private void Start()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
