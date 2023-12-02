using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        public enum SoundType
        {
            Bgm,
            Sfx,
            UISfx,
            Ambient,
            End
        }
        //풀 생성 후, 플레이하고자 하는 bgm을 딕셔너리에서 찾아서 오디오 소스에 붙여서 해당 위치로 이동시킨 후 플레이
        private Pool audioControllerPool;

        private AudioController audioControllerPrefab;
        //오디오 클립들이 담긴 딕셔너리
        private Dictionary<string, AudioClip> AudioClips = new();
        
        private Coroutine nowPlayingBgmCoroutine;
        private AudioController nowPlayingBgmController;
        private bool isBgmPlaying = false;
        private bool isBgmPaused = false;

        private List<AudioController> nowPlayingSfxAudioControllerList = new();

        private Dictionary<string, AudioController> ambientDict = new();
        private Dictionary<string, Coroutine> ambientCoroutines = new();
        private Coroutine nowPlayingUISfxCoroutine;

        //volmues
        private float BGMVolume = 1.0f;
        private float SFXVolume = 1.0f;
        private float UISfxVolume = 1.0f;


        public bool IsBgmPlaying() => isBgmPlaying;

        public override void Awake()
        {
            base.Awake();
            InitAudioDict();
            InitAudioSourcePool();
        }
        private void InitAudioDict()
        {
            audioControllerPrefab = Resources.Load<AudioController>("Prefabs/SoundController/AudioController");
            AudioClip[] clips = Resources.LoadAll<AudioClip>("Extern/Sounds");
            foreach (AudioClip clip in clips)
            {
                AudioClips.Add(clip.name, clip);
            }
        }
        private void InitAudioSourcePool()
        {
            audioControllerPool = PoolManager.Instance.CreatePool(audioControllerPrefab.gameObject, 10);
        }

        public void PlaySound(SoundType type, string name, Vector3 playingPos = default(Vector3), bool loop = false, float pitch = 1.0f)
        {
            switch (type)
            {
                case SoundType.Bgm:
                    if (AudioClips.TryGetValue(name, out AudioClip bgmClip))
                    {
                        AudioController audioController = audioControllerPool.Pop() as AudioController;

                        //플레이 중인 Bgm이 이미 존재한다면
                        if (isBgmPlaying && nowPlayingBgmCoroutine != null)
                        {
                            StopCoroutine(nowPlayingBgmCoroutine);
                            nowPlayingBgmController.Stop();
                            audioControllerPool.Push(nowPlayingBgmController);
                        }
                        isBgmPlaying = true;
                        nowPlayingBgmController = audioController;
                        nowPlayingBgmCoroutine = StartCoroutine(PlaySoundCoroutine(type, name, audioController, bgmClip, pitch, loop));
                    }
                    else
                    {
                        Debug.Log($"{name}사운드가 존재하지 않습니다.");
                        return;
                    }
                    break;
                case SoundType.Sfx:
                    if (AudioClips.TryGetValue(name, out AudioClip sfxClip))
                    {
                        AudioController audioController = audioControllerPool.Pop() as AudioController;
                        //audioController의 위치를 해당 위치로
                        audioController.Activate3DEffect();
                        audioController.transform.position = playingPos;

                        nowPlayingSfxAudioControllerList.Add(audioController);
                        StartCoroutine(PlaySoundCoroutine(type, name, audioController, sfxClip, pitch, loop));
                    }
                    else
                    {
                        Debug.Log($"{name}사운드가 존재하지 않습니다.");
                        return;
                    }
                    break;
                case SoundType.UISfx:
                    //해당 사운드가 현재 재생중인지 먼저 체크
                    if (nowPlayingUISfxCoroutine != null)
                    {
                        //이미 재생중이라면 재생하지 않음
                        Debug.Log("NowPlayingUISfxCoroutine이 null이 아님");
                        return;
                    }

                    if (AudioClips.TryGetValue(name, out AudioClip uiSfxClip))
                    {
                        AudioController audioController = audioControllerPool.Pop() as AudioController;

                        nowPlayingUISfxCoroutine = StartCoroutine(PlaySoundCoroutine(type, name, audioController, uiSfxClip, pitch, loop));
                    }
                    else
                    {
                        Debug.Log($"{name}사운드가 존재하지 않습니다.");
                        return;
                    }
                    break;
                case SoundType.Ambient:
                    if (AudioClips.TryGetValue(name, out AudioClip ambientClip))
                    {
                        AudioController audioController = audioControllerPool.Pop() as AudioController;

                        ambientDict.Add(name, audioController);
                        ambientCoroutines[name] = StartCoroutine(PlaySoundCoroutine(type, name, audioController, ambientClip, pitch, loop));
                    }
                    else
                    {
                        Debug.Log($"{name}사운드가 존재하지 않습니다.");
                        return;
                    }
                    break;
            }
        }

        public void SetVolume(SoundType type, float volume)
        {
            switch (type)
            {
                case SoundType.Bgm:
                    BGMVolume = volume;
                    break;
                case SoundType.Sfx:
                    SFXVolume = volume;
                    break;
                case SoundType.UISfx:
                    UISfxVolume = volume;
                    break;
                default:
                    break;
            }
        }

        private void StopSound(string name, AudioController nowPlayingAudioController)
        {
            
            nowPlayingSfxAudioControllerList.Remove(nowPlayingAudioController);
            nowPlayingAudioController.Deactivate3DEffect();
            audioControllerPool.Push(nowPlayingAudioController);
        }

        public void StopAmbient(string name)
        {
            if (ambientCoroutines.TryGetValue(name, out Coroutine ambientCoroutine))
            {
                StopCoroutine(ambientCoroutine);
                ambientCoroutines.Remove(name);
            }
            if (ambientDict.TryGetValue(name, out AudioController controller))
            {
                controller.Stop();
                audioControllerPool.Push(controller);
                ambientDict.Remove(name);
            }
        }
        public void StopBgm()
        {
            if (!isBgmPlaying) return;
            isBgmPlaying = false;
            isBgmPaused = false;
            //코루틴 꺼주기
            nowPlayingBgmController.Stop();
            StopCoroutine(nowPlayingBgmCoroutine);

            audioControllerPool.Push(nowPlayingBgmController);
        }

        public void PauseBgm()
        {
            if (!isBgmPlaying) return;
            isBgmPaused = true;
        }

        public void ResumeBgm()
        {
            isBgmPaused = false;
        }

        public void StopSfx(string clipName)
        {
            foreach(var controller in nowPlayingSfxAudioControllerList)
            {
                if (controller.clip.name == clipName)
                {
                    controller.Stop();
                }
            }
        }
        private IEnumerator PlaySoundCoroutine(SoundType type, string name, AudioController audioController, AudioClip clip, float pitch, bool loop)
        {
            audioController.SetClip(clip);
            audioController.Play(pitch, type);
            if(type == SoundType.Bgm || type == SoundType.Ambient)
            {
                while (true)
                {
                    if (isBgmPaused && !audioController.isPaused)
                    {
                        audioController.Pause();
                    }

                    if (!isBgmPaused && audioController.isPaused)
                    {
                        audioController.Resume();
                    }

                    //bgm 또는 Ambient 실행중에는 무한루프
                    yield return null;
                }
            }
            else 
            {
                yield return new WaitForSeconds(clip.length);
                if (type == SoundType.Sfx && loop)
                {
                    while(true)
                    {
                        if (!audioController.isPlaying) break;
                        audioController.Play(pitch, type);
                        yield return new WaitForSeconds(clip.length);
                    }
                }
                
                if(type == SoundType.UISfx)
                {
                    nowPlayingUISfxCoroutine = null;
                }
                StopSound(name, audioController);
            }
        }
    }
}
