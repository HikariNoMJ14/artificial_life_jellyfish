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
            }

            //Debug.Log("pre-compute shader");

            var jellyfishBuffer = new ComputeBuffer (numJellyfish, JellyfishData.Size);
            jellyfishBuffer.SetData (jellyfishData);

            compute.SetBuffer (0, "jellys", jellyfishBuffer);
            compute.SetInt ("numJelly", numJellyfish);
            compute.SetFloat ("separationRadius", settings.separationRadius);
            compute.SetFloat ("alignmentRadius", settings.alignmentRadius);
            compute.SetFloat ("cohesionRadius", settings.cohesionRadius);

            compute.Dispatch (0, threadGroups, 1, 1);

            jellyfishBuffer.GetData (jellyfishData);

            for (int i = 0; i < jellyfish.Length; i++) {
                jellyfish[i].separationDirection = jellyfishData[i].separationDirection;
                jellyfish[i].alignmentDirection = jellyfishData[i].alignmentDirection;
                jellyfish[i].cohesionDirection = jellyfishData[i].cohesionDirection;

                jellyfish[i].UpdateJellyfish ();
            }

            //Debug.Log("post-compute shader");

            jellyfishBuffer.Release ();
        }
    }

    public struct JellyfishData {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 separationDirection;
        public Vector3 alignmentDirection;
        public Vector3 cohesionDirection;

        public static int Size {
            get {
                return sizeof (float) * 3 * 5;
            }
        }
    }
}