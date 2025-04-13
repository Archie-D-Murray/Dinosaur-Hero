using UnityEngine;
using UnityEditor;

using Entities.Dinos;
using System;
using System.Linq;

[CustomEditor(typeof(DinoSpawner))]
public class DinosSpawnerEditor : Editor {

    static DinoType[] _types = Enum.GetValues(typeof(DinoType)).Cast<DinoType>().ToArray();

    public override void OnInspectorGUI() {
        DinoSpawner spawner = target as DinoSpawner;
        foreach (DinoType type in _types) {
            if (GUILayout.Button($"Spawn {type}")) {
                if (Application.isPlaying) {
                    spawner.QueueSpawn(type);
                }
            }
        }
        DrawDefaultInspector();
    }
}