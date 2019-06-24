//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//[CreateAssetMenu(fileName = "New Angle Tile", menuName = "Angle Tile")] // Alt Asset Creation.
//public class AngleTile : TileBase {

//#if UNITY_EDITOR
    //[MenuItem("Assets/Create/CustomAssets/AngleTile", false, 1)]
    //private static void CreateAngleTile()
    //{
    //    Sprite[] myTextures = InitiateSlots();

    //    if (myTextures != null)
    //    {
    //        Debug.Log("Loaded mySprites.");
    //        Debug.Log(myTextures.GetType() + "Length: " + myTextures.Length);
    //        Debug.Log(myTextures[0].name);
    //    }
    //    else
    //    {
    //        Debug.Log("Texture not loaded.");
    //    }

    //    string fName2 = "AcidTile";
    //    AngleTile myAT = new AngleTile();

    //    myAT.name = fName2 + ".asset";
    //    // Has to be placed before the Creation of Asset data or else it will be lost after saving
    //    myAT.InitiateSlots(myTextures); // Parameter here? (myTextures)
    //    AssetDatabase.CreateAsset(myAT, "Assets/CustomAssets/Tiles/" + myAT.name + "");
    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //}

    //public static Sprite[] InitiateSlots()
    //{
    //    Sprite[] myTextures = Resources.LoadAll<Sprite>("Blizzard_Peaks_Act_1");
    //    return myTextures;
    //}
//#endif
   
    //public float slopeAngle;
    //public SpriteSlot[] spriteSlots;

    //public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    //{
    //    Transform root = tilemap.GetComponent<Tilemap>().transform.parent;
    //    Tilemap angleTileMap = null;
    //    Tilemap angleTileMap = null;

    //    if (root != null)
    //    {
    //        // These are the layers for Floortiles, which are created with the Brusheditor Utility
    //        Transform acidGo = root.Find(AngleBrush.k_AcidLayerName);
    //        if (acidGo != null)
    //        {
    //            angleTileMap = acidGo.GetComponent<Tilemap>();
    //        }
    //    }

    //    Debug.Log("Slope Angle: " + slopeAngle);
    //    base.GetTileData(position, tilemap, ref tileData);
    //}

    //public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    //{
    //    base.RefreshTile(position, tilemap);
    //}

    //public static Sprite[] LoadTexture()
    //{
    //    Sprite[] myTextures = Resources.LoadAll<Sprite>("Blizzard_Peaks_Act_1");       
    //    return myTextures;
    //}

    //[System.Serializable]
    //public class SpriteSlot
    //{
    //    [SerializeField]
    //    public List<SpriteSlotItem> sprites;

    //    public SpriteSlot(Sprite spSprite)
    //    {
    //        sprites = new List<SpriteSlotItem>();
    //        sprites.Add(new SpriteSlotItem());
    //        sprites[0].sprite = spSprite;
    //    }
    //}

    //[System.Serializable]
    //public class SpriteSlotItem
    //{
    //    [SerializeField]
    //    public Sprite sprite;
    //    [SerializeField]
    //    public int probability = 1;
    //}
//}