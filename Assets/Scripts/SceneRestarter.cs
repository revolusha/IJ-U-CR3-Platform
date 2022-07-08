using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonUp("Reset"))
            SceneManager.LoadScene("SampleScene");
    }
}
