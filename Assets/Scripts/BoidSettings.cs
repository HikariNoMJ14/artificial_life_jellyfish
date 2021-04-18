using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject {
    // Settings
    public float minSpeed = 2;
    public float maxSpeed = 5;
    public static float baseRadius = 2.5f;
    public float perceptionRadius = baseRadius * 1;
    public float neighborhoodRadius = baseRadius * 1;
    public float alignmentRadius = baseRadius * .6f;
    public float cohesionRadius = baseRadius * .4f;
    public float visionRadius = baseRadius * .4f;
    public float avoidRadius = baseRadius * .2f;
    public float avoidanceRadius = 1;
    public float maxSteerForce = 3;

    public float alignWeight = 1;
    public float cohesionWeight = 1;
    public float seperateWeight = .5f;

    public float targetWeight = 1;

    [Header ("Collisions")]
    public LayerMask obstacleMask;
    public float boundsRadius = .27f;
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 5;


}