using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySpawner : MonoBehaviour {

    public enum GizmoType { Never, SelectedOnly, Always }

    public Jellyfish prefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;
    public Color colour;
    public GizmoType showSpawnRegion;

    void Awake () {
        for (int i = 0; i < spawnCount; i++) {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Jellyfish jellyfish = Instantiate (prefab);
            jellyfish.transform.position = pos;
            //jellyfish.transform.forward = Random.insideUnitSphere;

            //Debug.Log(jellyfish.transform.forward);
            jellyfish.transform.forward = new Vector3(Random.Range(-1.0f,1.0f), 1, Random.Range(0-1.0f,1.0f)).normalized;

            jellyfish.SetColour (colour);
        }
    }

    private void OnDrawGizmos () {
        if (showSpawnRegion == GizmoType.Always) {
            DrawGizmos ();
        }
    }

    void OnDrawGizmosSelected () {
        if (showSpawnRegion == GizmoType.SelectedOnly) {
            DrawGizmos ();
        }
    }

    void DrawGizmos () {

        Gizmos.color = new Color (colour.r, colour.g, colour.b, 0.3f);
        Gizmos.DrawSphere (transform.position, spawnRadius);
    }

}