using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("General Setup Settings")]
    [Tooltip("How fast ship moves up and down based upon player input")]
    [SerializeField]float controlSpeed = 10f;
    [Tooltip("How fast player moves horizontally")]
    [SerializeField]float xRange = 10f;
    [Tooltip("How fast player moves vertically")]
    [SerializeField]float yRange = 7f;

    [Header("Laser gun array")]
    [Tooltip("Add all player lasers here")]
    [SerializeField]GameObject[] lasers;

    [Header("Screen position based tuning")]
    [SerializeField]float positionPitchFactor = -2f;
    [SerializeField]float positionYawFactor = 2f;

    [Header("Player input based tuning")]    
    [SerializeField]float controlPitchFactor = -10f;
    [SerializeField]float controlRollFactor = -20f;

    float xThrow, yThrow, zThrow;
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControl = yThrow * controlPitchFactor;

        float pitch = pitchDueToPosition + pitchDueToControl;
        float yaw = transform.localPosition.x * positionYawFactor + zThrow * controlPitchFactor;
        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3
        (clampedXPos, clampedYPos, transform.localPosition.z);
    }

    private void ProcessFiring()
    {
        if(Input.GetButton("Fire1"))
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    private void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
