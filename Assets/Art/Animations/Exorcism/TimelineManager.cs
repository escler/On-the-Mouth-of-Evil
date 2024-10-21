using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance;
    private PlayableDirector _playableDirector;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _playableDirector = GetComponent <PlayableDirector>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayTimeline(TimelineAsset timeline)
    {
        if (_playableDirector != null && timeline != null)
        {
            _playableDirector.playableAsset = timeline;
            _playableDirector.Play();
        }
        else
        {
            Debug.LogError("PlayableDirector or TimelineAsset is null.");
        }
    }

    public PlayableDirector GetPlayableDirector()
    {
        return _playableDirector;
    }
}
