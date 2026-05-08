using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class RackManager : MonoBehaviour
{
    [SerializeField] private Transform[] slots;
    [SerializeField] private float moveDuration = 0.3f;
    
    private List<Tile> rackTiles = new List<Tile>();
    private bool isProcessingMatch = false;

    public int MaxSlots => slots.Length;
    public int TileCount => rackTiles.Count;

    public async Task<bool> AddTile(Tile tile)
    {
        if (rackTiles.Count >= MaxSlots) return false;

        int insertIndex = rackTiles.Count;
        for (int i = 0; i < rackTiles.Count; i++)
        {
            if (rackTiles[i].IconID == tile.IconID)
            {
                insertIndex = i + 1;
            }
        }

        rackTiles.Insert(insertIndex, tile);
        tile.transform.SetParent(transform);

        await UpdateRackPositions();

        await CheckForMatches();

        return true;
    }

    private async Task UpdateRackPositions()
    {
        List<Task> moveTasks = new List<Task>();
        for (int i = 0; i < rackTiles.Count; i++)
        {
            moveTasks.Add(rackTiles[i].MoveToRack(slots[i].position, moveDuration));
        }
        await Task.WhenAll(moveTasks);
    }

    [SerializeField] private GameManager gameManager;
    private async Task CheckForMatches()
    {
        if (isProcessingMatch) return;
        isProcessingMatch = true;

        var groups = rackTiles.GroupBy(t => t.IconID).Where(g => g.Count() >= 3).ToList();

        if (groups.Count > 0)
        {
            List<Tile> tilesToRemove = new List<Tile>();
            foreach (var group in groups)
            {
                var match = group.Take(3).ToList();
                tilesToRemove.AddRange(match);
                
                foreach (var t in match)
                {
                    rackTiles.Remove(t);
                }
                AudioManager.Instance.PlayMatch();
                gameManager.OnTripleMatched();
}

            List<Task> removalTasks = new List<Task>();
            foreach (var t in tilesToRemove)
            {
                removalTasks.Add(AnimateMatchClear(t));
            }
            await Task.WhenAll(removalTasks);

            foreach (var t in tilesToRemove)
            {
                Destroy(t.gameObject);
            }

            await UpdateRackPositions();
            
            isProcessingMatch = false;
            await CheckForMatches();
        }
        else if (IsFull())
        {
            gameManager.CheckLoseCondition();
        }

        isProcessingMatch = false;
    }

    private async Task AnimateMatchClear(Tile tile)
    {
        float elapsed = 0;
        float duration = 0.3f;
        Vector3 startScale = tile.transform.localScale;
        Image bg = tile.GetComponent<Image>();
        Color startColor = bg.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            float scaleT = Mathf.Sin(t * Mathf.PI);
            tile.transform.localScale = Vector3.Lerp(startScale, startScale * 1.5f, scaleT);
            
            bg.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0), t);
            
            await Task.Yield();
        }
    }

    public bool IsFull() => rackTiles.Count >= MaxSlots;

    public void ClearRack()
    {
        foreach (var tile in rackTiles)
        {
            if (tile != null) Destroy(tile.gameObject);
        }
        rackTiles.Clear();
    }
}
