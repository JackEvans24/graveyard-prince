using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GrabbyGrave : MonoBehaviour
{
    [SerializeField] private float cooldown;

    private Animator anim;
    private SoundFromArray growl;
    private float currentCooldown;

    private void Awake()
    {
        this.anim = GetComponent<Animator>();
        this.growl = GetComponentInChildren<SoundFromArray>();

        this.currentCooldown = this.cooldown + 1;
    }

    private void Update()
    {
        if (this.currentCooldown <= this.cooldown)
            this.currentCooldown += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this.currentCooldown < this.cooldown)
            return;

        if (collision.CompareTag("Player") || collision.CompareTag("Princess"))
        {
            this.currentCooldown = 0;
            this.anim.SetTrigger("Grab");
            StartCoroutine(this.growl.Play());
        }
    }
}
