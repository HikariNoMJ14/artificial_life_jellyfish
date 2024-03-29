using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishManager : MonoBehaviour {

    const int threadGroupSize = 1024;
    int threadGroups;
    int numJellyfish;

    public JellyfishSettings settings;
    public ComputeShader compute;

    Jellyfish[] jellyfish;

    void Start () {
        jellyfish = FindObjectsOfType<Jellyfish> ();
        foreach (Jellyfish j in jellyfish) {
            j.Initialize (settings);
        }
        numJellyfish = jellyfish.Length;
        threadGroups = Mathf.CeilToInt (numJellyfish / (float) threadGroupSize);
    }

    void Update () {
        if (jellyfish != null) {
            var jellyfishData = new JellyfishData[numJellyfish];

            for (int i = 0; i < jellyfish.Length; i++) {
                jellyfishData[i].position = jellyfish[i].position;
                jellyfishData[i].direction = jellyfish[i].forward;

                jellyfishData[i].glowOffset = jellyfish[i].GetGlowOffset();
                jellyfishData[i].hue = jellyfish[i].GetHue();
            }

            var jellyfishBuffer = new ComputeBuffer (numJellyfish, JellyfishData.Size);
            jellyfishBuffer.SetData (jellyfishData);

            compute.SetBuffer (0, "jellys", jellyfishBuffer);
            compute.SetInt ("numJelly", numJellyfish);
            compute.SetFloat ("neighborhoodRadius", settings.neighborhoodRadius());
            compute.SetFloat ("separationRadius", settings.separationRadius());
            compute.SetFloat ("alignmentRadius", settings.alignmentRadius());
            compute.SetFloat ("cohesionRadius", settings.cohesionRadius());

            compute.Dispatch (0, threadGroups, 1, 1);

            jellyfishBuffer.GetData (jellyfishData);

            for (int i = 0; i < jellyfish.Length; i++) {
                jellyfish[i].numFlockmates = jellyfishData[i].numFlockmates;
                jellyfish[i].separationDirection = jellyfishData[i].separationDirection;
                jellyfish[i].flockDirection = jellyfishData[i].flockDirection;
                jellyfish[i].flockCentre = jellyfishData[i].flockCentre;
                jellyfish[i].glowStimulus = jellyfishData[i].glowStimulus;
                jellyfish[i].hueStimulus = jellyfishData[i].hueStimulus;

                jellyfish[i].UpdateJellyfish ();
            }

            //Debug.Log("post-compute shader");

            jellyfishBuffer.Release ();
        }
    }

    public struct JellyfishData {
        public Vector3 position;
        public Vector3 direction;
        public float glowOffset;
        public float hue;

        public int numFlockmates;
        public Vector3 flockCentre;
        public Vector3 flockDirection;
        public Vector3 separationDirection;
        public float glowStimulus;
        public float hueStimulus;

        public static int Size {
            get {
                return sizeof (float) * 3 * 5 + sizeof (float) * 4 + sizeof (int);
            }
        }
    }
}