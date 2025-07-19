using UnityEngine;
using UnityEngine.SceneManagement;

public class L2CheckpointManager : MonoBehaviour
{
    public string cpName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_cp"))
        {
            if (PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_cp") == cpName)
            {
                PlayerController1.instance.GetComponent<CharacterController>().enabled = false;
                PlayerController1.instance.transform.position = transform.position;
                PlayerController1.instance.GetComponent<CharacterController>().enabled = true;
            }
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", cpName);
            //Debug.Log("Touching" + cpName);
        }
    }
}
