using UnityEngine;

namespace WitchScaper.Common
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _shotSound;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void PlayShot()
        {
            if (_shotSound == null)
            {
                return;
            }
            _audioSource.PlayOneShot(_shotSound);
        }
        
        public void PlayHit()
        {
            if (_hitSound == null)
            {
                return;
            }
            _audioSource.PlayOneShot(_hitSound);
        }
    }
}