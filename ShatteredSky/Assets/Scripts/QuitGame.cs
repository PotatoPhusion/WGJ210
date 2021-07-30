using UnityEngine;

public class QuitGame : MonoBehaviour
{
    private void Start()
    {
#if !UNITY_STANDALONE || UNITY_EDITOR
        gameObject.SetActive(false);
#endif
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
