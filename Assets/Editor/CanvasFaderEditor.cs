using UnityEngine;
using UnityEditor;

using UI;

[CustomEditor(typeof(CanvasFader))]
public class CanvasFaderEditor : Editor {
    public override void OnInspectorGUI() {
        CanvasFader fader = target as CanvasFader;
        DrawDefaultInspector();
        if (GUILayout.Button("Show") && Application.isPlaying) {
            fader.Show();
        }
        if (GUILayout.Button("Hide") && Application.isPlaying) {
            fader.Hide();
        }
    }
}