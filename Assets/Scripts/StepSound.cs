using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    public AudioSource footstepAudioSource; // Reference to the audio source for footstep sounds
    public AudioClip footstepClip;          // Reference to the footstep sound clip
    public float stepInterval = 0.5f;       // Interval between steps in seconds
    public float moveThreshold = 0.1f;      // Minimum speed to consider the player moving

    private float nextStepTime = 0f;        // Time at which the next step can be played
    private Rigidbody rb;                   // Reference to the Rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
    }

    void Update()
    {
        // Check if the player is grounded using a Raycast
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // Check if the player is moving based on Rigidbody velocity
        bool isMoving = rb.velocity.magnitude > moveThreshold;

        // Only play footsteps if the player is grounded and moving
        if (isGrounded && isMoving)
        {
            if (Time.time >= nextStepTime) // Check if it's time for the next step
            {
                PlayFootstepSound();
                nextStepTime = Time.time + stepInterval; // Set the next step time
            }
        }
    }

    // Play the footstep sound
    private void PlayFootstepSound()
    {
        if (footstepAudioSource && footstepClip)
        {
            footstepAudioSource.PlayOneShot(footstepClip);
        }
    }
}
