using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Grid<GridObject>
{
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPos;
    private GridObject[,] gridArray; // define multidimensional array
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPos, Func<Grid<GridObject>, int, int, GridObject> CreateGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;

        gridArray = new GridObject[width, height];

        // set up object type for grid
        for(int x = 0; x < gridArray.GetLength(0); x++) {
            for(int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = CreateGridObject(this, x, y); // use a func in case custom type
            }
        }

        bool showDebug = true;
        if(showDebug) {
            debugTextArray = new TextMesh[width, height];

            // iterate thru x and y dimensions of array
            for(int x = 0; x < gridArray.GetLength(0); x++) {
                for(int y = 0; y < gridArray.GetLength(1); y++) {
                    // borrowed from external source
                    debugTextArray[x,y] = UtilsClass.CreateWorldText(gridArray[x,y]?.ToString(), null, GetWoldPos(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter);
                    // use ? to check for null -- this way ToString will still work even if data type is null

                    Debug.DrawLine(GetWoldPos(x,y), GetWoldPos(x,y+1), Color.white, 100f);
                    Debug.DrawLine(GetWoldPos(x,y), GetWoldPos(x+1,y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWoldPos(0,height), GetWoldPos(width,height), Color.white, 100f);
            Debug.DrawLine(GetWoldPos(width,0), GetWoldPos(width,height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
                // ? here again for same reason as prev
            };
        }
        
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    private Vector3 GetWoldPos(int x, int y)
    {
        return new Vector3(x,y) * cellSize + originPos;
    }

    private void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos.x - originPos.x) / cellSize);
        y = Mathf.FloorToInt((worldPos.y - originPos.y) / cellSize);
    }

    public void SetGridObject(int x, int y, GridObject value)
    {
        // check for invalid pos
        if(x >= 0 && y >= 0 && x < width && y < height) {
            gridArray[x,y] = value;
            if(OnGridValueChanged != null) {
                OnGridValueChanged(this, new OnGridValueChangedEventArgs{x = x, y = y});
            }
        }
    }

    // func to call when grid is updated
    public void TriggerGridObjectChanged(int x, int y)
    {
        if(OnGridValueChanged != null) {
            OnGridValueChanged(this, new OnGridValueChangedEventArgs{x = x, y = y});
        }
    }

    public void SetGridObject(Vector3 worldPos, GridObject value)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        SetGridObject(x, y, value);
    }

    public GridObject GetGridObject(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height) {
            return gridArray[x,y];
        }
        else {
            return default; // invalid value case
        }
    }

    public GridObject GetGridObject(Vector3 worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetGridObject(x, y);
    }

    public Vector3 GetNearestGridSegment(Vector3 mouseWorldPos)
    {
        int x, y;
        GetXY(mouseWorldPos, out x, out y);

        // Calculate the center position of the nearest grid segment
        Vector3 nearestGridPos = GetWoldPos(x, y);
        return nearestGridPos;
    }
}
