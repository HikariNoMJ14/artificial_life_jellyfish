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
    [HideInInspector]
    public float hueStimulus;

    // Cached
    Material material;
    Transform cachedTransform;

    public float GetGlowOffset() {
        return material.GetFloat("GlowOffset");
    }

    public void SetGlowOffset(float offset) {
        material.SetFloat("GlowOffset", offset);
    }

    public float GetHue() {
       return material.GetColor("_Color")[0];
    }

    public void SetHue(float hue) {
        Color clr =  Color.HSVToRGB(hue, settings.saturation, settings.brightness);

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

        this.SetHue(Random.Range(.0f,1.0f));
        this.SetGlowOffset(Random.Range(-Mathf.PI, Mathf.PI));
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

            //Debug.Log("Glow offset :" + this.GetGlowOffset());
            //Debug.Log("Hue :" + this.GetHue());

            //SynchronizeGlow(glowStimulus);
            //SynchronizeHue(hueStimulus);
        } else {
            // TODO add random movement
        }

        //if (IsHeadingForCollision ()) {
        //    Vector3 collisionAvoidDir = ObstacleRays ();
        //   Vector3 collisionAvoidForce = SteerTowards (collisionAvoidDir) * settings.avoidCollisionWeight;
        //    acceleration += collisionAvoidForce;
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

    void SynchronizeGlow (float glowStimulus) {
        float glowSynchronizatonForce = ((glowStimulus / numFlockmates) - this.GetGlowOffset()) * settings.synchronizeWeight;

        if (glowSynchronizatonForce <= settings.minGlowSynchronizationForce) {
            float newGlowOffset = glowSynchronizatonForce + this.GetGlowOffset();
            this.SetGlowOffset(newGlowOffset);
        }
    }

    void SynchronizeHue (float hueStimulus) {
        float hueSynchronizatonForce = ((hueStimulus / numFlockmates) - this.GetHue()) * settings.synchronizeWeight;

        if (hueSynchronizatonForce <= settings.minHueSynchronizationForce) {
            float newHueOffset = hueSynchronizatonForce + this.GetHue();
            //this.SetHue(newHueOffset);
        }
    }

    bool IsHeadingForCollision () {
        RaycastHit hit;
        if (Physics.SphereCast (position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask)) {
            return true;
        } else { }
        return false;
    }

    Vector3 ObstacleRays () {
        Vector3[] rayDirections = JellyfishHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++) {
            Vector3 dir = cachedTransform.TransformDirection (rayDirections[i]);
            Ray ray = new Ray (position, dir);
            if (!Physics.SphereCast (ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask)) {
                return dir;
            }
        }

        return forward;
    }
}