using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Player Movement Reference")]
    public playerMovement playerMovementScript; // Drag your playerMovement script here

    [Header("Audio Sources")]
    public AudioSource maxSpeedAudioSource; // Plays when reaching actual movement speed of 20
    public AudioSource slideAudioSource;    // Plays when sliding
    public AudioSource deathAudioSource;    // Plays when player dies

    private bool maxSpeedAudioPlayed = false;
    private bool wasSliding = false;
    private bool wasAtMaxSpeed = false;
    private float previousActualSpeed = 0f;
    private bool gameStarted = false;
    private Vector3 lastPosition;
    private bool isDead = false;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (playerMovementScript != null && !isDead)
        {
            float actualSpeed = CalculateActualSpeed();
            CheckMaxSpeedAudio(actualSpeed);
            CheckSlideAudio();
        }
    }

    // Public method that obstacle can call directly
    public void PlayDeathSound()
    {
        if (!isDead)
        {
            TriggerDeath();
        }
    }

    float CalculateActualSpeed()
    {
        // Calculate actual movement speed based on position change
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        float actualSpeed = distanceMoved / Time.deltaTime;
        lastPosition = transform.position;
        return actualSpeed;
    }

    void CheckMaxSpeedAudio(float actualSpeed)
    {
        // Only start checking once game has actually started (speed > 0)
        if (actualSpeed > 0.1f && !gameStarted)
        {
            gameStarted = true;
        }

        // Play audio once when actual speed reaches 20 and audio hasn't played yet
        if (actualSpeed >= 20f && !maxSpeedAudioPlayed && gameStarted)
        {
            PlayMaxSpeedAudio();
            wasAtMaxSpeed = true;
        }

        // Update previous speed for death detection
        previousActualSpeed = actualSpeed;
    }

    void CheckSlideAudio()
    {
        // Use reflection to access the sliding field
        System.Type type = playerMovementScript.GetType();
        System.Reflection.FieldInfo slidingField = type.GetField("sliding", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (slidingField != null)
        {
            bool isSliding = (bool)slidingField.GetValue(playerMovementScript);

            // Play slide audio when sliding starts
            if (!wasSliding && isSliding)
            {
                PlaySlideAudio();
            }
            // Stop slide audio when sliding ends (if it's a looping sound)
            else if (wasSliding && !isSliding && slideAudioSource != null && slideAudioSource.isPlaying)
            {
                slideAudioSource.Stop();
            }

            wasSliding = isSliding;
        }
    }

    void PlayMaxSpeedAudio()
    {
        if (maxSpeedAudioSource != null)
        {
            maxSpeedAudioSource.Play();
            maxSpeedAudioPlayed = true;
            Debug.Log("Max speed audio played! Actual movement speed reached 20");
        }
    }

    void PlaySlideAudio()
    {
        if (slideAudioSource != null)
        {
            slideAudioSource.Play();
            Debug.Log("Slide audio played!");
        }
    }

    void TriggerDeath()
    {
        if (!isDead && deathAudioSource != null)
        {
            isDead = true;
            deathAudioSource.Play();
            Debug.Log("Death audio played! Player hit obstacle");

            // Stop other audio sources when death occurs
            if (maxSpeedAudioSource != null && maxSpeedAudioSource.isPlaying)
                maxSpeedAudioSource.Stop();
            if (slideAudioSource != null && slideAudioSource.isPlaying)
                slideAudioSource.Stop();
        }
    }

    // Call this method when your UI start button is clicked
    public void StartGame()
    {
        gameStarted = true;
        isDead = false;
        ResetAudio();
    }

    // Reset audio states (call this when player respawns)
    public void ResetAudio()
    {
        maxSpeedAudioPlayed = false;
        wasAtMaxSpeed = false;
        previousActualSpeed = 0f;
        isDead = false;

        if (maxSpeedAudioSource != null && maxSpeedAudioSource.isPlaying)
            maxSpeedAudioSource.Stop();
        if (slideAudioSource != null && slideAudioSource.isPlaying)
            slideAudioSource.Stop();
    }
}
