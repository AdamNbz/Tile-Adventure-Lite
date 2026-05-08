using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform tileContainer;
    [SerializeField] private Sprite tileBaseSprite;
    [SerializeField] private Sprite[] iconSprites;

    private List<Tile> activeTiles = new List<Tile>();

    public void GenerateBoard(LevelData levelData, System.Action<Tile> onTileTapped)
    {
        ClearBoard();

        int totalTiles = levelData.tileLayouts.Count;
        List<int> iconsPool = new List<int>();
        
        int iconCount = levelData.iconIndices.Count;
        int triplesNeeded = totalTiles / 3;
        
        for (int i = 0; i < triplesNeeded; i++)
        {
            int iconID = levelData.iconIndices[i % iconCount];
            iconsPool.Add(iconID);
            iconsPool.Add(iconID);
            iconsPool.Add(iconID);
        }
        
        while (iconsPool.Count < totalTiles)
        {
             iconsPool.Add(levelData.iconIndices[0]);
        }
        
        iconsPool = iconsPool.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < levelData.tileLayouts.Count; i++)
{
            var layout = levelData.tileLayouts[i];
            GameObject tileObj = Instantiate(tilePrefab, tileContainer);
            tileObj.transform.localPosition = layout.position;
            
            Tile tile = tileObj.GetComponent<Tile>();
            int iconID = iconsPool[i];
            tile.Initialize(iconID, layout.layer, tileBaseSprite, iconSprites[iconID - 1]);
            tile.OnTileTapped = onTileTapped;
            
            activeTiles.Add(tile);
            
            AnimateTileEntrance(tile, i * 0.02f);
        }

        UpdateExposedTiles();
        }

        private async void AnimateTileEntrance(Tile tile, float delay)
        {
        tile.gameObject.SetActive(false);
        await System.Threading.Tasks.Task.Delay((int)(delay * 1000));
        if (tile == null) return;
        tile.gameObject.SetActive(true);
        
        Vector3 finalScale = Vector3.one;
        tile.transform.localScale = Vector3.zero;
        
        float duration = 0.3f;
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            tile.transform.localScale = Vector3.Lerp(Vector3.zero, finalScale, elapsed / duration);
            await System.Threading.Tasks.Task.Yield();
        }
        tile.transform.localScale = finalScale;
        }

    public void UpdateExposedTiles()
    {
        Canvas.ForceUpdateCanvases();
        
        var sortedTiles = activeTiles.OrderBy(t => t.Layer).ToList();
        for (int i = 0; i < sortedTiles.Count; i++)
        {
            sortedTiles[i].transform.SetSiblingIndex(i);
        }

        foreach (var tile in activeTiles)
        {
            bool isBlocked = false;
            foreach (var other in activeTiles)
            {
                if (other == tile) continue;
                if (other.Layer > tile.Layer)
                {
                    if (IsOverlapping(tile, other))
                    {
                        isBlocked = true;
                        break;
                    }
                }
            }
            tile.SetExposed(!isBlocked);
        }
    }

    private bool IsOverlapping(Tile t1, Tile t2)
    {
        Rect r1 = GetWorldRect(t1.RectTransform);
        Rect r2 = GetWorldRect(t2.RectTransform);
        
        float epsilon = 0.05f;
        Rect r1Shrunk = new Rect(r1.x + r1.width * epsilon, r1.y + r1.height * epsilon, r1.width * (1 - 2*epsilon), r1.height * (1 - 2*epsilon));
        
        return r1Shrunk.Overlaps(r2);
    }

    private Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }

    public bool IsBoardEmpty() => activeTiles.Count == 0;

    public void RemoveTile(Tile tile)
    {
        activeTiles.Remove(tile);
        UpdateExposedTiles();
    }

    private void ClearBoard()
    {
        foreach (var tile in activeTiles)
        {
            if (tile != null) Destroy(tile.gameObject);
        }
        activeTiles.Clear();
    }
    }
