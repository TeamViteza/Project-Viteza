using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AngleBrush : GridBrushBase {
#if UNITY_EDITOR
    [MenuItem("Assets/Create/CustomAssets/AngleBrush", false, 0)]
    // This function is called when you click the menu entry.
    private static void CreateAcidBrush()
    {
        string fileName = "AngleBrush";
        AngleBrush angleBrush = new AngleBrush();
        angleBrush.name = fileName + ".asset";
        AssetDatabase.CreateAsset(angleBrush, "Assets/CustomAssets/Brushes/" + angleBrush.name + "");
    }
#endif 

    public const string ANGLE_LAYER_NAME = "Acid";
    public TileBase angleTileBase;

    public override void Paint(GridLayout grid, GameObject layer, Vector3Int position)
    {
        GridInformation info = BrushUtility.GetRootGridInformation(true);
        Tilemap angleTileMap = GetAngleTileMap();

        if (angleTileMap != null)
        {
            PaintInternal(position, angleTileMap);
        }
    }

    private void PaintInternal(Vector3Int position, Tilemap acid)
    {
        acid.SetTile(position, angleTileBase);
    }

    public static Tilemap GetAngleTileMap()
    {
        GameObject go = GameObject.Find(ANGLE_LAYER_NAME);
        return go != null ? go.GetComponent<Tilemap>() : null;
    }
}