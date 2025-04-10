using System.Collections;
using System.Collections.Generic;

using Entities;

using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] public Transform[] Locations;
    public int counter = 0;
    [SerializeField] float speed = 5;
    [SerializeField] float rotationSpeed = 360;
    private bool canMove = true;
    [SerializeField] private float speedModifier = 1f;
    [SerializeField] private float modifierDuration = 0f;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start() {
        if (TryGetComponent<Health>(out Health health)) {
            health.OnDeath += () => canMove = false;
        }
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    // Update is called once per frame
    void FixedUpdate() {
        if (Vector2.Distance((Vector2)transform.position, Locations[counter].position) < 0.1) {
            if (counter == Locations.Length - 1) {
                canMove = false;
            } else {
                counter = Mathf.Min(Locations.Length - 1, counter + 1);
            }
        } else if (canMove) {
            transform.position = Vector3.MoveTowards(transform.position, Locations[counter].position, speed * speedModifier * Time.fixedDeltaTime);
            spriteRenderer.transform.rotation = Quaternion.RotateTowards(
                spriteRenderer.transform.rotation,
                Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, Locations[counter].position - transform.position), Vector3.forward),
                rotationSpeed * Time.fixedDeltaTime
            );
        }

        if (speedModifier != 1f) {
            modifierDuration = Mathf.Max(0f, modifierDuration - Time.fixedDeltaTime);
            if (modifierDuration == 0f) {
                speedModifier = 1f;
            }
        }
    }

    public void ApplySlow(float speedModifier, float duration) {
        this.speedModifier = speedModifier;
        modifierDuration = duration;
    }

    public void AllowMovement() {
        canMove = true;
    }

    public void StopMovement() {
        canMove = false;
    }
}