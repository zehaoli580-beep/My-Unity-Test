using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] [Range(0f, 3f)] private float volume = 1f;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        audioSource.playOnAwake = false;

        if (GameManager.Instance != null && GameManager.Instance.IsFruitCollected(gameObject.name))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsFruitCollected(gameObject.name))
            {
                GameManager.Instance.AddScore();
                GameManager.Instance.MarkFruitCollected(gameObject.name);

                if (collectSound != null && audioSource != null)
                {
                    audioSource.clip = collectSound;
                    audioSource.volume = volume;
                    audioSource.Play();
                }
                Destroy(gameObject, audioSource != null && audioSource.clip != null ? audioSource.clip.length : 0f);
            }
        }
    }
}
