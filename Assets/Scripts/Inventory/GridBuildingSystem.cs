using UnityEngine;
using CodeMonkey.Utils;
using System.Collections.Generic;
using System;

public class GridBuildingSystem : MonoBehaviour
{
    public List<InventoryObject> inventoryObjects;
    private InventoryObject inventoryObject;
    private int index;

    private Grid<GridObject> grid;
    private InventoryObject.Dir dir = InventoryObject.Dir.Down;

    void Awake()
    {
        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (Grid<GridObject> g, int x, int y) => new GridObject(g, x,  y));

        inventoryObject = inventoryObjects[0];
        index = 0;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            Vector3 pos = grid.GetNearestGridSegment(Mouse3D.GetMouseWorldPosition());
            // TODO: clicking outside the grid throws an error -- fix

            // need to divide by grid cell size to avoid error (note: this is bc grid starts at 0,0)
            List<Vector2Int> gridPosList = inventoryObject.GetGridPositionList(new Vector2Int((int)pos.x/10, (int)pos.y/10), dir); 
            // Debug.Log("pos ints: " + (int)pos.x + " " + (int)pos.y);

            // test build area
            bool canBuild = true;
            foreach(Vector2Int gridPos in gridPosList) {
                if(!grid.GetGridObject(gridPos.x, gridPos.y).CanBuild()) {
                    // cannot build
                    canBuild = false;
                    break;
                }
            }

            if(canBuild) {
                Vector2Int rotationOffset = inventoryObject.GetRotationOffset(dir);
                Vector3 placedPos = pos + new Vector3(rotationOffset.x, rotationOffset.y, 0) * grid.GetCellSize();

                PlacedObject placedObject = PlacedObject.Create(placedPos, new Vector2Int((int)pos.x/10, (int)pos.y/10), dir, inventoryObject);
                // Transform builtTransform = Instantiate(inventoryObject.prefab, placedPos, Quaternion.Euler(0, 0, inventoryObject.GetRotationAngle(dir)));
                
                foreach(Vector2Int gridPos in gridPosList) {
                    grid.GetGridObject(gridPos.x, gridPos.y).SetPlacedObject(placedObject);
                }

                // gridObject.SetTransform(builtTransform);
            }
            else {
                UtilsClass.CreateWorldTextPopup("Cannot build here!", Mouse3D.GetMouseWorldPosition());
            }
        }

        if(Input.GetMouseButtonDown(1)) {
            GridObject gridObject = grid.GetGridObject(Mouse3D.GetMouseWorldPosition());
            PlacedObject placedObject = gridObject.GetPlacedObject();
            if(placedObject != null) {
                // can clear
                placedObject.DestroySelf();

                List<Vector2Int> gridPosList = placedObject.GetGridPositionList();
                
                foreach(Vector2Int gridPos in gridPosList) {
                    grid.GetGridObject(gridPos.x, gridPos.y).ClearPlacedObject();
                } 
            }
        }

        if(Input.GetKeyDown(KeyCode.R)) {
            dir = InventoryObject.GetNextDir(dir);
            UtilsClass.CreateWorldTextPopup("" + dir, Mouse3D.GetMouseWorldPosition());
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            index++;
            if(index >= inventoryObjects.Count) {
                index = 0;
            }
            inventoryObject = inventoryObjects[index];
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

        public bool CanBuild()
        {
            return placedObject == null;
        }

        public override string ToString()
        {
            return x + ", " + y + "\n" + placedObject;
        }
    }
}
