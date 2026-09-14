using UnityEngine;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public enum NodeType
    {
        Battle
    }

    [SerializeField] private NodeType nodeType;
    [SerializeField] private int nodeIndex;
    [SerializeField] private int nodeId;
    [SerializeField] private MapNode[] nextNodes;
    [SerializeField] private EnemyData enemyData;
    
    private Button button;

    public NodeType Type => nodeType;
    public int NodeId => nodeId;
    public EnemyData EnemyData => enemyData;
    private void Start()
    {
        button = GetComponent<Button>();

        UpdateNodeState();
    }

    private void UpdateNodeState()
    {
        if (!MapManager.Instance.HasSelectedNode)
        {
            // まだ一度もNodeを選択していない
            // 最初のNodeだけ選択可能
            button.interactable = nodeIndex == 0;
        }
        else
        {
            // 前回選択したNodeを現在のMapSceneから探す
            MapNode[] allNodes = FindObjectsByType<MapNode>(
                FindObjectsSortMode.None
            );

            MapNode currentNode = null;

            foreach (MapNode node in allNodes)
            {
                if (node.NodeId == MapManager.Instance.CurrentNodeId)
                {
                    currentNode = node;
                    break;
                }
            }

            // 前回選択したNodeから繋がっているNodeだけ選択可能
            if (currentNode != null)
            {
                button.interactable =
                    System.Array.Exists(
                        currentNode.GetNextNodes(),
                        node => node == this
                    );
            }
            else
            {
                button.interactable = false;
                Debug.Log("現在のNodeが見つかりません");
            }
        }
    }

    public MapNode[] GetNextNodes()
    {
        return nextNodes;
    }

    public void SelectNode()
    {
        MapManager.Instance.SetCurrentNode(this);
        if (nodeType == NodeType.Battle)
        {
            SceneLoader sceneLoader = FindFirstObjectByType<SceneLoader>();
            sceneLoader.LoadNextBattle();
        }
    }
}