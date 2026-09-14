using UnityEngine;


public class MapManager : MonoBehaviour
{
    private static MapManager instance;
    public static MapManager Instance => instance;

    public int CurrentNodeId { get; private set; } = -1;
    public bool HasSelectedNode { get; private set; } = false;
    public EnemyData CurrentEnemyData { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("MapManager");
    }
    public void SetCurrentNode(MapNode node)
    {
        CurrentNodeId = node.NodeId;
        CurrentEnemyData = node.EnemyData;
        HasSelectedNode = true;
        Debug.Log("SetCurrentNode: " + CurrentNodeId);
    }
}