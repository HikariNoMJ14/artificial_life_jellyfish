using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour {

    JellyfishSettings settings;

    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;

    // To update:
    Vector3 acceleration;
    [HideInInspector]
    public Vector3 separationDirection;
    [HideInInspector]
    public float separationStrength;
    [HideInInspector]
    public Vector3 alignmentDirection;
    [HideInInspector]
    public Vector3 cohesionDirection;

    // Cached
    Material material;
    Transform cachedTransform;

    void Awake () {
        material = transform.GetComponentInChildren<MeshRenderer> ().material;
        cachedTransform = transform;
    }

    public void Initialize (JellyfishSettings settings) {
        this.settings = settings;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        float startSpeed = 0.1f;
        velocity = transform.forward * startSpeed;
    }

    public void SetColour (Color col) {
        if (material != null) {
            material.color = col;
        }
    }

    public void UpdateJellyfish () {
        Vector3 acceleration = Vector3.zero;

       // if (numFlockmates != 0) { // TODo restore?
        var separationForce = SteerTowards (separationDirection) * settings.separateWeight; // TODO use separation strength
        var alignmentForce = SteerTowards (alignmentDirection) * settings.alignmentWeight;
        var cohesionForce = SteerTowards (cohesionDirection) * settings.cohesionWeight;

        acceleration += alignmentForce;
        acceleration += cohesionForce;
        acceleration += separationForce;
        //} else {
            // TODO add random movement
        //}

        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp (speed, settings.minSpeed, settings.maxSpeed);
        velocity = dir * speed;

        cachedTransform.position += velocity * Time.deltaTime;
        cachedTransform.forward = dir;
        position = cachedTransform.position;
        forward = dir;
    }

    Vector3 SteerTowards (Vector3 vector) {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude (v, settings.maxSteerForce);
    }
}