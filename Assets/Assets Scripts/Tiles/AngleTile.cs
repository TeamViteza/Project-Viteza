using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New AngleTile", menuName = "AngleTile")]
public class AngleTile : Tile {

    public float slopeAngle;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        Debug.Log("Slope Angle: " + slopeAngle);
        base.GetTileData(position, tilemap, ref tileData);
    }
}