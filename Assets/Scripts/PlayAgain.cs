using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayAgain : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadTargetScene(SelectEnterEventArgs args)
    {
        SceneManager.LoadScene("LabRoom");
    }
}
