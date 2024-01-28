using UnityEngine.Video;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameManager gameManager;
    public GameObject background;
    public AudioSource music;
    float videoStartTime;
    float minVideoSkipTime = 0.8f;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnEnable()
    {
        background.SetActive(false);
        videoStartTime = Time.time;
        music.Stop();
    }

    private void OnDisable()
    {
        background.SetActive(true);
        music.Play();
    }

    private void Update()
    {
        if (Time.time - videoStartTime < minVideoSkipTime) return;

        if (Input.anyKeyDown) {
            Skip();
        }
    }

    private void Skip()
    {
        OnVideoFinished(videoPlayer);
    }

    // This method is called when the video finishes playing
    void OnVideoFinished(VideoPlayer vp)
    {
        //gameObject.SetActive(false);
        gameManager.SetState(GameState.WaitForPromptPicking);
    }
}