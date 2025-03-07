﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{

    [SerializeField] Entity playerCharacter;

    [Header("Shake")]
    [SerializeField] float shakeTime = .25f;
    [SerializeField] float shakeStrenght = .5f;

    [Header("Post Processing")]
    [SerializeField] PostProcessVolume postProcessVolume;
    DepthOfField depthOfField;
    ChromaticAberration chromaticAberration;
    Grain grain;
    float startingChromaticValue;

    Camera cam;

    bool shaking = false;
    private void Awake()
    {

        AddPostProccesingReferences();
        playerCharacter.OnEntityTakeDamage += CameraShake;
    }

    void CameraShake(DamageInfo info) 
    {
        if(!shaking) 
        {
            StartCoroutine(CameraShakeCoroutine());
        }
    }

    IEnumerator CameraShakeCoroutine() 
    {
        float t = 0;
        Vector3 startingPos = transform.localPosition;
        shaking = true;
        ApplyShakePostProccesing();
        do
        {
            t += Time.deltaTime;
            Vector3 rand = UnityEngine.Random.insideUnitSphere * shakeStrenght;
            transform.localPosition = new Vector3(startingPos.x + rand.x, startingPos.y + rand.y, startingPos.z);
            yield return null;
        } while (t < shakeTime);
        transform.localPosition = startingPos;
        shaking = false;
        UnapplyShakePostProccesing();
    }

    void AddPostProccesingReferences() 
    {
        postProcessVolume.profile.TryGetSettings(out depthOfField);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
        postProcessVolume.profile.TryGetSettings(out grain);
        startingChromaticValue = chromaticAberration.intensity;
        depthOfField.enabled.value = false;
        grain.enabled.value = false;
    }

    void ApplyShakePostProccesing() 
    {
        depthOfField.enabled.value = true;
        grain.enabled.value = true;
        chromaticAberration.intensity.value = 1;
    }

    void UnapplyShakePostProccesing()
    {
        depthOfField.enabled.value = false;
        grain.enabled.value = false;
        chromaticAberration.intensity.value = startingChromaticValue;
    }

}
