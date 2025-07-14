using UnityEngine;

public class PuzzleTracker : MonoBehaviour
{
    public static PuzzleTracker Instance;
    public int collectedPieces = 0;

    void Awake() => Instance = this;

    public void CollectPiece()
    {
        collectedPieces++;
        Debug.Log("���ռ�ƴͼ��: " + collectedPieces);
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
