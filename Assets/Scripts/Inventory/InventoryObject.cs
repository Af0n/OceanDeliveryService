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

    public GameObject worldPrefab;
    public GameObject uiPrefab;
    public int height;
    public int width;
    public bool dontRotate;
    private Dir dir;
    public int spriteIndex;

    public int GetRotationAngle(Dir dir)
    {
        if(dontRotate) {
            return 0; // prevents 2x2 objects from rotating bc they don't need to and it screws up the visual
        }
        else {
            return dir == Dir.Horizontal ? 0 : -90;
        }
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
}
