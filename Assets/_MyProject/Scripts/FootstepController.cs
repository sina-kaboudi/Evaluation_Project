using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public float minTimeBetweenFootsteps = 0.3f;
    public float maxTimeBetweenFootsteps = 0.6f;

    [SerializeField] private AudioClip[] footstepClips;
    private AudioSource playerAudioSource;
    private bool isWalking = false;
    private float timeSinceLastFootstep;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isWalking)
        {
            if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
            {
                AudioClip footstepSound = footstepClips[Random.Range(0, footstepClips.Length)];
                playerAudioSource.PlayOneShot(footstepSound);
                timeSinceLastFootstep = Time.time;
            }
        }
    }

    public void StartWalking()
    {
        isWalking = true;
    }

    public void StopWalking()
    {
        isWalking = false;
    }
}