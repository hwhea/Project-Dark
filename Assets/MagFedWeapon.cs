using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MagFedWeapon : MonoBehaviour, IMagazineFedWeapon
{
    [Header("Slide Settings")]
    [SerializeField]
    private Transform slideBackTransform;

    [Header("Magazine Settings")]
    private Transform magazineOutTransform;

    [Header("Sounds")]
    private AudioClip deadMansClick;
    private AudioClip weaponCocked;
    private AudioClip weaponFire;
    private AudioClip magazineEject;
    private AudioClip magazineInsert;
    private AudioClip handleReleased;


    //Non-inspector
    private AudioSource audioSource;

    enum WeaponReadyState
    {
        Ready,
        NotReady
    }

    enum WeaponAmmunitionState
    {
        Unloaded,
        Empty,
        Loaded
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    public void OnMagazineEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnMagazineEject()
    {
        throw new System.NotImplementedException();
    }

    public void OnMagazineEmpty()
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerPulled()
    {
        throw new System.NotImplementedException();
    }

}
