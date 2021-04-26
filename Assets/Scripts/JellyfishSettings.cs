using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class JellyfishSettings : ScriptableObject {

    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float maxSteerForce = 3;

    public float minGlowSynchronizationForce = 0.001f;
    public float minHueSynchronizationForce = 0.01f;

    public float saturation = 0.71f;
    public float brightness = 1f;

    public static float baseRadius = 10f;
    public float neighborhoodRadius = baseRadius * 1;
    public float separationRadius = baseRadius * .2f;
    public float alignmentRadius = baseRadius * .6f;
    public float cohesionRadius = baseRadius * .4f;

    public float separateWeight = 0.5f;
    public float alignmentWeight = 0.2f;
    public float cohesionWeight = 1;
    public float synchronizeWeight = 0.001f;

    [Header ("Collisions")]
    public LayerMask obstacleMask;
    public float boundsRadius = .27f;
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 5;

}