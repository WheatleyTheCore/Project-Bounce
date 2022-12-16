using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    public static Music instance;
    public void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == 4)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
}
