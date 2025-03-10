using UnityEngine;
using UnityEngine.UI;

public class GridUI
{
    private GameObject panel;
    private GameObject cellPrefab;
    private int rows, cols;
    private Vector2 cellSize; 

    private GameObject[,] gridArray;

    public GridUI(GameObject panel, GameObject cellPrefab, int rows, int cols, Vector2 cellSize)
    {
        this.panel = panel;
        this.cellPrefab = cellPrefab;
        this.rows = rows;
        this.cols = cols;
        this.cellSize = cellSize;

        gridArray = new GameObject[rows, cols];

        InitializeGrid();
    }

    private void InitializeGrid()
    {
        if (panel == null)
        {
            Debug.LogError("Grid panel is null! Please assign a valid UI panel.");
            return;
        }

        GridLayoutGroup gridLayout = panel.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = panel.AddComponent<GridLayoutGroup>();
        }

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = cols;
        gridLayout.cellSize = cellSize;
        gridLayout.spacing = new Vector2(5, 5);

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for(int y = 0; y < gridArray.GetLength(1); y++) {
                GameObject cell = Object.Instantiate(cellPrefab, panel.transform);
                cell.name = "Cell_" + x + "." + y;
                gridArray[x,y] = cell; 
            }
        }
    }

    public Vector2Int GetCellPosition(GameObject selectedCell) 
    {
        for(int x = 0; x < rows; x++) {
            for(int y = 0; y < cols; y++) {
                if(gridArray[x,y] == selectedCell) {
                    return new Vector2Int(x,y);
                }
            }
        }

        Debug.LogError("Cell not found in grid");
        return new Vector2Int(-1, -1);
    }

    public bool CanPlaceItem(InventoryObject inventoryObject, InventoryObject.Dir dir, GameObject selectedCell)
    {
        int itemHeight = inventoryObject.height;
        int itemWidth = inventoryObject.width;
        
        if(dir == InventoryObject.Dir.Vertical) {
            int temp = itemHeight;
            itemHeight = itemWidth;
            itemWidth = temp;
        }

        Vector2Int cellPos = GetCellPosition(selectedCell);
        
        if (cellPos.x + itemHeight > rows || cellPos.y + itemWidth > cols)
            return false; // Out of bounds

        // Check if all required cells are free
        for (int x = 0; x < itemHeight; x++)
        {
            for (int y = 0; y < itemWidth; y++)
            {
                int gridX = cellPos.x + x; 
                int gridY = cellPos.y + y;
                GameObject cell = gridArray[gridX, gridY];
                if(!cell.GetComponent<Cell>().GetAvailable()) {
                    // if occupied returns false
                    Debug.Log(cell.name + " occupied");
                    return false;
                }
            }
        }

        return true;
    }

    public void PlaceItem(InventoryObject inventoryObject, InventoryObject.Dir dir, GameObject selectedCell)
    {
        int itemHeight = inventoryObject.height;
        int itemWidth = inventoryObject.width;
        
        if(dir == InventoryObject.Dir.Vertical) {
            int temp = itemHeight;
            itemHeight = itemWidth;
            itemWidth = temp;
        }

        Vector2Int cellPos = GetCellPosition(selectedCell);

        if (cellPos.x + itemHeight > rows || cellPos.y + itemWidth > cols) {
            Debug.Log("cannot place out of bounds");
            return; // Out of bounds
        }

        // Mark cells as occupied by the item
        for (int x = 0; x < itemHeight; x++) {
            for (int y = 0; y < itemWidth; y++) {
                int gridX = cellPos.x + x; 
                int gridY = cellPos.y + y;
                GameObject cell = gridArray[gridX, gridY];
                
                cell.GetComponent<Cell>().SetAvailable(false);
            }
        }

        Debug.Log($"Placed item of size {itemHeight}x{itemWidth} at ({cellPos.x}, {cellPos.y})");
    }

    public void RemoveItem(InventoryObject inventoryObject, InventoryObject.Dir dir, GameObject selectedCell)
    {
        int itemHeight = inventoryObject.height;
        int itemWidth = inventoryObject.width;

        if(dir == InventoryObject.Dir.Vertical) {
            int temp = itemHeight;
            itemHeight = itemWidth;
            itemWidth = temp;
        }

        Vector2Int cellPos = GetCellPosition(selectedCell);

        // Mark cells as occupied by the item
        for (int x = 0; x < itemHeight; x++) {
            for (int y = 0; y < itemWidth; y++) {
                int gridX = cellPos.x + x; 
                int gridY = cellPos.y + y;
                GameObject cell = gridArray[gridX, gridY];
                
                cell.GetComponent<Cell>().SetAvailable(true);
            }
        }
    }

    public void SetGridObject(int x, int y)
    {
        GameObject cellObj = gridArray[x,y];
        bool available = cellObj.GetComponent<Cell>().GetAvailable();
        if(available) {
            Debug.Log(cellObj.name + " is empty");
        }
        else {
            Debug.Log(cellObj.name + " is taken");
        }
    }
}
