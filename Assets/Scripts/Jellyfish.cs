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
    public int numFlockmates;
    [HideInInspector]
    public Vector3 flockCentre;
    [HideInInspector]
    public Vector3 flockDirection;
    [HideInInspector]
    public Vector3 separationDirection;
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
       float hue, sat, bright;
       Color rgbColor =  material.GetColor("_Color");
       Color.RGBToHSV(rgbColor, out hue, out sat, out bright);

       return Mathf.Asin (2 * hue - 1) * 2;
    }

    public float convertHue(float hue) {
        return (Mathf.Sin(2 * hue) + 1) / 2;
    }

    public void SetHue(float hue) {
        Color clr = Color.HSVToRGB(this.convertHue(hue), settings.saturation, settings.brightness);

        material.SetColor("_Color", clr);
    }

    public float GetSaturation() {
       float hue, sat, bright;
       Color rgbColor =  material.GetColor("_Color");
       Color.RGBToHSV(rgbColor, out hue, out sat, out bright);

       return sat;
    }

    public void SetSaturation(float saturation) {
        Color clr =  Color.HSVToRGB(this.GetHue(), saturation, settings.brightness);

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

        //this.SetHue(-2f);
        this.SetHue(Random.Range(-Mathf.PI, Mathf.PI));
        //this.SetGlowOffset(0f);
        this.SetGlowOffset(Random.Range(-Mathf.PI, Mathf.PI));
    }

    public void UpdateJellyfish () {
        Vector3 acceleration = Vector3.zero;

        if (numFlockmates != 0) {
            flockCentre /= numFlockmates;

            Vector3 distanceToFlockmatesCentre = (flockCentre - position);

            var separationForce = SteerTowards (separationDirection) * settings.separateWeight;
            var alignmentForce = SteerTowards (flockDirection) * settings.alignmentWeight;
            var cohesionForce = SteerTowards (distanceToFlockmatesCentre) * settings.cohesionWeight;

            acceleration += separationForce;
            acceleration += alignmentForce;
            acceleration += cohesionForce;

            SynchronizeGlow(glowStimulus);
            SynchronizeHue(hueStimulus);

            //Debug.Log(this.GetGlowOffset());
            //Debug.Log("N flockmates: " + numFlockmates);

        } else {
            var randomForce = SteerTowards (new Vector3(Random.Range(-1f,1f), Random.Range(-1f,.1f), Random.Range(-1f,1f))) * settings.randomWeight;
            acceleration += randomForce;
        }

        //UpdateSaturation();

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
        float glowSynchronizatonForce = ((glowStimulus / numFlockmates) - this.GetGlowOffset()) * settings.synchronizeGlowWeight;
        float newGlowOffset = glowSynchronizatonForce + this.GetGlowOffset();

        this.SetGlowOffset(newGlowOffset);
    }

    void UpdateSaturation() {
        float saturationForce = (Mathf.Clamp(Mathf.Log(numFlockmates * 2.0f), 0f, settings.saturation) - this.GetSaturation()) * settings.synchronizeSaturationWeight;
        float newSaturation = saturationForce + this.GetSaturation();

        this.SetSaturation(newSaturation);
    }

    void SynchronizeHue (float hueStimulus) {
        float hueSynchronizatonForce = ((hueStimulus / numFlockmates) - this.GetHue());

        if (hueSynchronizatonForce > Mathf.PI) {
            hueSynchronizatonForce -= 2 * Mathf.PI;
        } else if (hueSynchronizatonForce <= -Mathf.PI) {
            hueSynchronizatonForce = 2 * Mathf.PI + hueSynchronizatonForce;
        }

        hueSynchronizatonForce *= settings.synchronizeHueWeight;
        float newHue = hueSynchronizatonForce + this.GetHue();

        this.SetHue(newHue);
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