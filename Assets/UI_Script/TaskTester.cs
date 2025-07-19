using UnityEngine;

public class TaskTester : MonoBehaviour
{
    private GameTaskManager taskManager;

    private int collectedHPProps = 0;
    private int killedEnemies = 0;

    [System.Obsolete]
    void Start()
    {
        taskManager = FindObjectOfType<GameTaskManager>();

        if (taskManager != null)
        {
            taskManager.AddGameTask("Collect 10 hpprops");
            taskManager.AddGameTask("Kill 5 enemies");
            taskManager.AddGameTask("Talk to the NPC");

            InvokeRepeating(nameof(SimulateCollectHPProp), 1f, 0.5f);
            InvokeRepeating(nameof(SimulateKillEnemy), 1f, 0.75f);
            Invoke(nameof(CompleteTalkToNPC), 6f);
        }
        else
        {
            Debug.LogError("❌ GameTaskManager not found in scene!");
        }
    }

    void SimulateCollectHPProp()
    {
        collectedHPProps++;
        Debug.Log("Collected HP prop: " + collectedHPProps);

        if (collectedHPProps >= 10)
        {
            taskManager.CompleteTask("Collect 10 hpprops");
            CancelInvoke(nameof(SimulateCollectHPProp));
        }
    }

    void SimulateKillEnemy()
    {
        killedEnemies++;
        Debug.Log("Killed Enemy: " + killedEnemies);

        if (killedEnemies >= 5)
        {
            taskManager.CompleteTask("Kill 5 enemies");
            CancelInvoke(nameof(SimulateKillEnemy));
        }
    }

    void CompleteTalkToNPC()
    {
        taskManager.CompleteTask("Talk to the NPC");
    }
}
