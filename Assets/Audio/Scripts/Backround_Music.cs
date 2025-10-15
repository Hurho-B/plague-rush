using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Player Movement Reference")]
    public playerMovement playerMovementScript; // Drag your playerMovement script here

    [Header("Audio Sources")]
    public AudioSource maxSpeedAudioSource; // Plays when reaching forward speed
    public AudioSource slideAudioSource;    // Plays when sliding
    public AudioSource deathAudioSource;    // Plays when player dies

    private bool maxSpeedAudioPlayed = false;
    private bool wasSliding = false;
    private bool isAlive = true;

    void Update()
    {
        if (playerMovementScript != null && isAlive)
        {
            CheckMaxSpeedAudio();
            CheckSlideAudio();
        }
    }

    void CheckMaxSpeedAudio()
    {
        // Use reflection to access the forwardSpeed field
        System.Type type = playerMovementScript.GetType();
        System.Reflection.FieldInfo forwardSpeedField = type.GetField("forwardSpeed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        if (forwardSpeedField != null)
        {
            float forwardSpeed = (float)forwardSpeedField.GetValue(playerMovementScript);

            // Play audio once when forward speed is reached and audio hasn't played yet
            if (forwardSpeed >= 20f && !maxSpeedAudioPlayed)
            {
                PlayMaxSpeedAudio();
            }
        }
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
            Debug.Log("Max speed audio played!");
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

    // Call this method when the player dies (from your game manager or death detection script)
    public void PlayDeathAudio()
    {
        if (deathAudioSource != null && isAlive)
        {
            deathAudioSource.Play();
            isAlive = false;
            Debug.Log("Death audio played!");

            // Stop other audio sources when player dies
            if (maxSpeedAudioSource != null && maxSpeedAudioSource.isPlaying)
                maxSpeedAudioSource.Stop();
            if (slideAudioSource != null && slideAudioSource.isPlaying)
                slideAudioSource.Stop();
        }
    }

    // Reset audio states when player respawns
    public void ResetAudio()
    {
        maxSpeedAudioPlayed = false;
        isAlive = true;

        if (maxSpeedAudioSource != null && maxSpeedAudioSource.isPlaying)
            maxSpeedAudioSource.Stop();
        if (slideAudioSource != null && slideAudioSource.isPlaying)
            slideAudioSource.Stop();
    }
}
