using UnityEngine;

public class MusicPitchController : MonoBehaviour
{
    [SerializeField] private Timer timer; // Assign in Inspector
    private AudioSource audioSource;
    private bool pitchIncreased = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timer != null && timer._round == 9 && !pitchIncreased)
        {
            audioSource.pitch = 1.25f; // Speed up music
            pitchIncreased = true;
        }
    }
}
