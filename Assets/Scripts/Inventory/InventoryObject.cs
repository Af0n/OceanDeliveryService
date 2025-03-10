using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObject", menuName = "Scriptable Objects/InventoryObject")]
public class InventoryObject : ScriptableObject
{
    public static Dir GetNextDir(Dir dir) {
        switch(dir) {
            default:
            case Dir.Down: return Dir.Left;
            case Dir.Left: return Dir.Up;
            case Dir.Up: return Dir.Right;
            case Dir.Right: return Dir.Down;
        }
    }
    public enum Dir {
        Down, Left, Up, Right,
    }

    public string nameString;
    public Transform prefab;
    public int width;
    public int height;

    public int GetRotationAngle(Dir dir)
    {
        switch(dir) {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 270;
            case Dir.Up: return 180;
            case Dir.Right: return 90;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir) 
    {
        switch(dir) {
            default:
            case Dir.Down: return new Vector2Int(0, 0);
            case Dir.Left: return new Vector2Int(0, width);
            case Dir.Up: return new Vector2Int(width, height);
            case Dir.Right: return new Vector2Int(height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> gridPosList = new List<Vector2Int>();

        switch(dir) {
            default:
            case Dir.Down:
            case Dir.Up:
                for(int x = 0; x < width; x++) {
                    for(int y = 0; y < height; y++) {
                        gridPosList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                // swap height/width for L/R rotations
                for(int x = 0; x < height; x++) {
                    for(int y = 0; y < width; y++) {
                        gridPosList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }

        return gridPosList;
    }
}
