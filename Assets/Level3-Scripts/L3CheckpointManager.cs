using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class L3CheckpointManager : MonoBehaviour
{
    public string cpName;

    public static Dictionary<string, Transform> checkpointDict = new Dictionary<string, Transform>();

    void Start()
    {
        if (!checkpointDict.ContainsKey(cpName))
        {
            checkpointDict.Add(cpName, transform); // зЂВс checkpoint
        }

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_cp"))
        {
            if (PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_cp") == cpName)
            {
                PlayerController1.instance.charCon.enabled = false;
                PlayerController1.instance.transform.position = transform.position;
                PlayerController1.instance.charCon.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", cpName);
        }
    }
}