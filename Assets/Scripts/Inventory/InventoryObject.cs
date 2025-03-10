using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObject", menuName = "Scriptable Objects/InventoryObject")]
public class InventoryObject : ScriptableObject
{
    public static Dir GetNextDir(Dir dir) {
        return dir == Dir.Horizontal ? Dir.Vertical : Dir.Horizontal;
    }
    public enum Dir {
        Horizontal, Vertical, 
    }

    public string nameString;
    public Transform prefab;
    public int height;
    public int width;
    private Dir dir;

    public int GetRotationAngle(Dir dir)
    {
        return dir == Dir.Horizontal ? 0 : 90;
    }

    public void SetDir(Dir dir)
    {
        this.dir = dir; 
    }

    public Dir GetDir()
    {
        Debug.Log("direction set to " + dir);
        return dir;
    }

    // public Vector2Int GetRotationOffset(Dir dir) 
    // {
    //     switch(dir) {
    //         default:
    //         case Dir.Down: return new Vector2Int(0, 0);
    //         case Dir.Left: return new Vector2Int(0, height);
    //         case Dir.Up: return new Vector2Int(height, width);
    //         case Dir.Right: return new Vector2Int(width, 0);
    //     }
    // }

    // public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    // {
    //     List<Vector2Int> gridPosList = new List<Vector2Int>();

    //     switch(dir) {
    //         default:
    //         case Dir.Down:
    //         case Dir.Up:
    //             for(int x = 0; x < height; x++) {
    //                 for(int y = 0; y < width; y++) {
    //                     gridPosList.Add(offset + new Vector2Int(x, y));
    //                 }
    //             }
    //             break;
    //         case Dir.Left:
    //         case Dir.Right:
    //             // swap height/width for L/R rotations
    //             for(int x = 0; x < width; x++) {
    //                 for(int y = 0; y < height; y++) {
    //                     gridPosList.Add(offset + new Vector2Int(x, y));
    //                 }
    //             }
    //             break;
    //     }

    //     return gridPosList;
    // }
}
