using UnityEngine;

namespace CuteAliens.Core
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource audioSource;

        [Header("Card Sound")]
        [SerializeField] private AudioClip cardPlayClip;

        [Header("Volume")]
        [Range(0f, 1f)]
        [SerializeField] private float volume = 0.7f;

        public void PlayCardPlay()
        {
            if (cardPlayClip == null)
            {
                Debug.LogWarning("Card play sound is not assigned.");
                return;
            }

            if (audioSource == null)
            {
                Debug.LogWarning("AudioSource is not assigned.");
                return;
            }

            audioSource.PlayOneShot(cardPlayClip, volume);
        }
    }
}