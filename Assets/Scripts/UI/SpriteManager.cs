using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance { get; private set; }
    
    [Header("List of Sprites")]
    public List<Sprite> spriteList;
    private void Awake() 
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    
    public Sprite GetRandomSprite()
    {
        if (spriteList.Count == 0) return null;
        return spriteList[Random.Range(0, spriteList.Count)];
    }
    
    public Sprite GetIndexSprite(int index)
    {
        if (spriteList.Count == 0) return null;
        return spriteList[index];
    }
}
