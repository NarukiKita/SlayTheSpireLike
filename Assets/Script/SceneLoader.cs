using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextBattle()
    {
        Debug.Log("読み直し");
        SceneManager.LoadScene("SampleScene");
    }
}
