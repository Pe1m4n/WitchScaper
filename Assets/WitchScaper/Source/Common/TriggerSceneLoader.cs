using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using WitchScaper.Core.Character;

namespace WitchScaper.Common
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerSceneLoader : MonoBehaviour
    {
        public string sceneName;

        private void Start()
        {
            var collider = GetComponent<Collider2D>();
            collider.OnTriggerEnter2DAsObservable().Subscribe(c =>
            {
                var pcc = c.GetComponent<PlayerCharacterController>();
                if (pcc == null)
                {
                    return;
                }

                SceneManager.LoadScene(sceneName);
            });
        }
    }
}