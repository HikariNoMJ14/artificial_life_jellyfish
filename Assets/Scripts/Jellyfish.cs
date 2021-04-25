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
    public Vector3 alignmentDirection;
    [HideInInspector]
    public Vector3 cohesionDirection;
    [HideInInspector]
    public int numFlockmates;
    [HideInInspector]
    public float glowStimulus;


    // Cached
    Material material;
    Transform cachedTransform;

    public float GetGlowOffset() {
        return material.GetFloat("GlowOffset");
    }

    public void SetGlowOffset(float offset) {
        material.SetFloat("GlowOffset", offset);
    }

    public float GetColour() {
       return material.GetColor("_Color")[0];
    }

    public void SetColour(float hue, float saturation, float brightness) {
        Color clr =  Color.HSVToRGB(hue, saturation, brightness);

        material.SetColor("_Color", clr);
    }

    void Awake () {
        material = transform.GetComponentInChildren<Renderer> ().material;
        cachedTransform = transform;
    }

    public void Initialize (JellyfishSettings settings) {
        this.settings = settings;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    public void UpdateJellyfish () {
        Vector3 acceleration = Vector3.zero;

        if (numFlockmates != 0) {
            var separationForce = SteerTowards (separationDirection) * settings.separateWeight; // TODO use separation strength
            var alignmentForce = SteerTowards (alignmentDirection) * settings.alignmentWeight;
            var cohesionForce = SteerTowards (cohesionDirection) * settings.cohesionWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += separationForce;

            float synchronizatonForce = Synchronize(glowStimulus / numFlockmates) * settings.synchronizeWeight;
            float newGlowOffset = synchronizatonForce + this.GetGlowOffset();
            this.SetGlowOffset(newGlowOffset);
        } else {
            // TODO add random movement
        }

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

    float Synchronize (float stimulus) {
        return stimulus - this.GetGlowOffset();
    }
}