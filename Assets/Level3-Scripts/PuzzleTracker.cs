using UnityEngine;

public class PuzzleTracker : MonoBehaviour
{
    public static PuzzleTracker Instance;
    public int collectedPieces = 0;

    void Awake() => Instance = this;

    public void CollectPiece()
    {
        collectedPieces++;
        Debug.Log("已收集拼图数: " + collectedPieces);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
