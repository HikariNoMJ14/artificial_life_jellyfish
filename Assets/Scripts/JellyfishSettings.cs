using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class JellyfishSettings : ScriptableObject {

    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float maxSteerForce = 3;

    public float saturation = 0.71f;
    public float brightness = 1f;

    public float baseRadius = 10f;
    public float neighborhoodRadiusPerc = 1f;
    public float separationRadiusPerc = .2f;
    public float alignmentRadiusPerc = .6f;
    public float cohesionRadiusPerc = .4f;

    public float separateWeight = 1f;
    public float alignmentWeight = 0.2f;
    public float cohesionWeight = 0.4f;
    public float randomWeight = 0.1f;
    public float synchronizeGlowWeight = 0.001f;
    public float synchronizeHueWeight = 0.001f;
    public float synchronizeSaturationWeight = 0.001f;

    [Header ("Collisions")]
    public LayerMask obstacleMask;
    public float boundsRadius = .27f;
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 5;


    public float neighborhoodRadius() {
        return baseRadius * neighborhoodRadiusPerc;
    }

    public float separationRadius() {
        return baseRadius * separationRadiusPerc;
    }

    public float alignmentRadius() {
        return baseRadius * alignmentRadiusPerc;
    }

    public float cohesionRadius() {
        return baseRadius * cohesionRadiusPerc;
    }

}