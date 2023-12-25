using Managers.SceneLoad;
using UnityEngine;
using UnityEngine.Video;

namespace Data.UI.Video
{
    [CreateAssetMenu(fileName = "VideoData", menuName = "UI/VideoData")]
    public class VideoData : ScriptableObject
    {
        [SerializeField] public VideoClip videoClip;
        [SerializeField] public SceneName playAfterScene;
        [SerializeField] public bool isLoadData;
    }
}