//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//[CustomEditor(typeof(AngleBrush))]
//public class AngleBrushEditor : GridBrushEditorBase {

//    public override void OnPaintInspectorGUI()
//    {
//        // Show button that the scene should be created
//        if (BrushEditorUtility.SceneIsPrepared())
//        {
//            GUILayout.Label("Use this custom Brush to paint Acid ponds on the map!");
//        }
//        else
//        {
//            BrushEditorUtility.UnpreparedSceneInspector();
//        }

//        base.OnPaintInspectorGUI();
//    }
//}