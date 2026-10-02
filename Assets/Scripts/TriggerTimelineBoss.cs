using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Core
{
    public class TriggerTimelineBoss : MonoBehaviour
    {
        PlayableDirector _director;
        bool _isPlayed = false;

        void Awake()
        {
            _director = GetComponent<PlayableDirector>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (_isPlayed)
                {
                    return;
                }

                PlayTimeline();
                _isPlayed = true;
            }
        }

        void Start()
        {
            GameManager.Instance.OnPlayAgain += PlayAgain;
        }

        void OnDestroy()
        {
            GameManager.Instance.OnPlayAgain -= PlayAgain;
        }

        void PlayAgain()
        {
            _isPlayed = false;
        }

        public void PlayTimeline()
        {
            StartCoroutine(PlayTimelineRoutine());
        }

        IEnumerator PlayTimelineRoutine()
        {
            GameManager.Instance.OnPausePlayerController.Invoke();
            _director.Play();

            yield return new WaitUntil(() => _director.state != PlayState.Playing);

            GameManager.Instance.OnResumePlayerController.Invoke();
        }
    }
}

