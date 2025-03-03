using UnityEngine;
using CodeMonkey.Utils;
using System.Collections.Generic;
using System;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryObject> inventoryObjects;
    private InventoryObject inventoryObject;
    private int index;

    private Grid<GridObject> grid;
    private InventoryObject.Dir dir = InventoryObject.Dir.Down;

    public int gridWidth;
    public int gridHeight;
    public float cellSize; 

    private PlacedObject objectToMove;

    void Awake()
    {
        // create the grid for the inventory
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (Grid<GridObject> g, int x, int y) => new GridObject(g, x,  y));

        inventoryObject = inventoryObjects[0];
        index = 0;

        objectToMove = null;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            Vector3 pos = grid.GetNearestGridSegment(Mouse3D.GetMouseWorldPosition());

            // need to divide by grid cell size to avoid error (note: this is bc grid starts at 0,0)
            List<Vector2Int> gridPosList = inventoryObject.GetGridPositionList(new Vector2Int((int)pos.x/10, (int)pos.y/10), dir); 

            // test build area
            bool canPlace = true;
            foreach(Vector2Int gridPos in gridPosList) {
                if(gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= gridWidth || gridPos.y >= gridHeight) {
                    canPlace = false; // out of bounds check
                    break;
                }
                if(!grid.GetGridObject(gridPos.x, gridPos.y).CanPlace()) {
                    // cannot build
                    canPlace = false;
                    break;
                }
            }

            if(canPlace && objectToMove != null) {
                // place copy of selected obj
                Vector2Int rotationOffset = inventoryObject.GetRotationOffset(dir);
                Vector3 placedPos = pos + new Vector3(rotationOffset.x, rotationOffset.y, 0) * grid.GetCellSize();

                PlacedObject placedObject = PlacedObject.Create(placedPos, new Vector2Int((int)pos.x/10, (int)pos.y/10), dir, inventoryObject);
                
                foreach(Vector2Int gridPos in gridPosList) {
                    grid.GetGridObject(gridPos.x, gridPos.y).SetPlacedObject(placedObject);
                }

                // destroy old obj
                objectToMove.DestroySelf();
                List<Vector2Int> movedPosList = objectToMove.GetGridPositionList();
                foreach(Vector2Int gridPos in movedPosList) {
                    grid.GetGridObject(gridPos.x, gridPos.y).ClearPlacedObject();
                } 
                objectToMove = null; // reset obj bc it was moved
            }
            else {
                // select an object to move
                GridObject gridObject = grid.GetGridObject(Mouse3D.GetMouseWorldPosition());
                if(gridObject == null) {
                    Debug.Log("clicked outside grid");
                    return;
                }

                objectToMove = gridObject.GetPlacedObject();
                if(objectToMove != null) {
                    inventoryObject = objectToMove.GetInventoryObjectFromPlaced();
                    Debug.Log("object selected");
                }
                else {
                    Debug.Log("empty space");
                }
                
                // UtilsClass.CreateWorldTextPopup("Cannot build here!", Mouse3D.GetMouseWorldPosition());
            }
        }

        if(Input.GetMouseButtonDown(1)) {
            // deselect objectToMove
            if(objectToMove != null) {
                objectToMove = null;
                Debug.Log("object deselected");
            }
        }

        if(Input.GetKeyDown(KeyCode.R)) {
            dir = InventoryObject.GetNextDir(dir);
            UtilsClass.CreateWorldTextPopup("" + dir, Mouse3D.GetMouseWorldPosition());
        }

        // if(Input.GetKeyDown(KeyCode.T)) {
        //     if(objectToMove == null) {
        //         Debug.Log("switched box");
        //         index++;
        //         if(index >= inventoryObjects.Count) {
        //             index = 0;
        //         }
        //         inventoryObject = inventoryObjects[index];
        //     }
        // }

        // if(Input.GetKeyDown(KeyCode.Q)) {
        //     AddObjectToInventory(inventoryObject);
        // }
    }

    public void AddObjectToInventory(InventoryObject inventoryObject) 
    {
        for(int x = 0; x < gridWidth; x++) {
            for(int y = 0; y < gridHeight; y++) {
                List<Vector2Int> gridPosList = inventoryObject.GetGridPositionList(new Vector2Int(x, y), dir);
                bool canPlace = true;
                foreach(Vector2Int gridPos in gridPosList) {
                    if(gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= gridWidth || gridPos.y >= gridHeight) {
                        canPlace = false; // out of bounds check
                        break;
                    }
                    if(!grid.GetGridObject(gridPos.x, gridPos.y).CanPlace()) {
                        canPlace = false;
                        break;
                    }
                }

                if(canPlace) {
                    Vector2Int rotationOffset = inventoryObject.GetRotationOffset(dir);
                    Vector3 placedPos = new Vector3(x*10, y*10, 0) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * grid.GetCellSize();

                    PlacedObject placedObject = PlacedObject.Create(placedPos, new Vector2Int(x, y), dir, inventoryObject);
                    
                    foreach(Vector2Int gridPos in gridPosList) {
                        grid.GetGridObject(gridPos.x, gridPos.y).SetPlacedObject(placedObject);
                    }

                    return;
                }
            }
        }
    }


    public class GridObject {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        private PlacedObject placedObject;

        public GridObject(Grid<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetPlacedObject(PlacedObject placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y); // to update grid when transform updated
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public void ClearPlacedObject()
        {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y); // ditto
        }

        public bool CanPlace()
        {
            return placedObject == null;
        }

        public override string ToString()
        {
            return x + ", " + y + "\n" + placedObject;
        }
    }
}


// code for destroying an object (saving in case it's needed later)

// GridObject gridObject = grid.GetGridObject(Mouse3D.GetMouseWorldPosition());
// PlacedObject placedObject = gridObject.GetPlacedObject();
// if(placedObject != null) {
//     // can clear
//     placedObject.DestroySelf();

//     List<Vector2Int> gridPosList = placedObject.GetGridPositionList();
    
//     foreach(Vector2Int gridPos in gridPosList) {
//         grid.GetGridObject(gridPos.x, gridPos.y).ClearPlacedObject();
//     } 
// }