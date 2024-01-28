using UnityEngine.Video;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameManager gameManager;
    public GameObject background;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        
    }

    private void OnEnable()
    {
        background.SetActive(false);
    }

    private void OnDisable()
    {
        background.SetActive(true);
    }

    // This method is called when the video finishes playing
    void OnVideoFinished(VideoPlayer vp)
    {
        //gameObject.SetActive(false);
        gameManager.SetState(GameState.WaitForPromptPicking);
    }
}