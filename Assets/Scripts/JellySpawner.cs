using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySpawner : MonoBehaviour {

    public enum GizmoType { Never, SelectedOnly, Always }

    public Jellyfish prefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;
    public GizmoType showSpawnRegion;

    void Awake () {
        float species = Random.Range(0f,1f);
        for (int i = 0; i < spawnCount; i++) {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Jellyfish jellyfish = Instantiate (prefab);
            jellyfish.species = species;
            jellyfish.transform.position = pos;
            //jellyfish.transform.position = new Vector3(0,0,0);
            //jellyfish.transform.forward = Random.insideUnitSphere;

            //Debug.Log(jellyfish.transform.forward);
            //jellyfish.transform.forward = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized;
            //jellyfish.transform.forward = new Vector3(Random.Range(-.8f,.8f), Random.Range(.5f,.1f), Random.Range(-.8f,.8f)).normalized;
            jellyfish.transform.forward = new Vector3(Random.Range(-.5f,.5f), Random.Range(-.2f,1f), Random.Range(-.5f,.5f)).normalized;
            //jellyfish.transform.forward = new Vector3(0f, 1f, 0f).normalized;
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
        Gizmos.color = Color.HSVToRGB(0.5f, 0.5f, 0.5f);
        Gizmos.DrawSphere (transform.position, spawnRadius);
    }

}