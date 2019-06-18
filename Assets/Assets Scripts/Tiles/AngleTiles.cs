using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New AngleTile", menuName = "AngleTile")]
public class AngleTiles : Tile {

    public float slopeAngle;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        Debug.Log("Slope Angle: " + slopeAngle);
        base.GetTileData(position, tilemap, ref tileData);
    }
}