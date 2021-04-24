using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class JellyfishSettings : ScriptableObject {

    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float maxSteerForce = 3;

    public static float baseRadius = 2.5f;
    public float neighborhoodRadius = baseRadius * 1;
    public float separationRadius = baseRadius * .2f;
    public float alignmentRadius = baseRadius * .6f;
    public float cohesionRadius = baseRadius * .4f;

    public float separateWeight = 0.5f;
    public float alignmentWeight = 0.2f;
    public float cohesionWeight = 1;

}