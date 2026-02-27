using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public static class fAudio
    {
        public static void ReleaseSoundId(int soundId)
        {
            Function.Call(Hash.RELEASE_​SOUND_​ID, soundId);
        }
        public static void StopSound(int soundId)
        {
            Function.Call(Hash.STOP_​SOUND, soundId);
        }
        public static void PlaySoundFromCoord(int soundId, string audioName, Vector3 coord, string audioRef, bool isNetwork = false, int range = 0, bool p8 = false)
        {
            Function.Call(Hash.PLAY_​SOUND_​FROM_​COORD, soundId, audioName, coord.X, coord.Y, coord.Z, audioRef, isNetwork, range, p8);
        }
        public static void PlaySoundFromEntity(int soundId, string audioName, Entity entity, string audioRef, bool isNetwork = false, int p5 = 0)
        {
            Function.Call(Hash.PLAY_​SOUND_​FROM_​ENTITY, soundId, audioName, entity, audioRef, isNetwork, p5);
        }
        public static int GetSoundId() => Function.Call<int>(Hash.GET_​SOUND_​ID);
        // Shared lookup used by both ChangeMusicEventIntensity and PrepareMusicEventIntensity
        private static readonly Dictionary<MusicEventIntensity, string> MusicEventNames = new Dictionary<MusicEventIntensity, string>
    {
        { MusicEventIntensity.Idle,                            "CH_IDLE"                                },
        { MusicEventIntensity.Silent,                          "CH_SILENT"                              },
        { MusicEventIntensity.Suspense,                        "CH_SUSPENSE"                            },
        { MusicEventIntensity.MedIntensity,                    "CH_MED_INTENSITY"                       },
        { MusicEventIntensity.Gunfight,                        "CH_GUNFIGHT"                            },
        { MusicEventIntensity.VehicleAction,                   "CH_VEHICLE_ACTION"                      },
        { MusicEventIntensity.Delivering,                      "CH_DELIVERING"                          },
        { MusicEventIntensity.Fail,                            "CH_FAIL"                                },
        { MusicEventIntensity.MusicStop,                       "CH_MUSIC_STOP"                          },
    };

        // --- Music ---

        public static bool TriggerMusicEvent(string eventName)
            => Function.Call<bool>(Hash.TRIGGER_MUSIC_EVENT, eventName);

        public static bool PrepareMusicEvent(string eventName)
            => Function.Call<bool>(Hash.PREPARE_MUSIC_EVENT, eventName);

        public static bool CancelMusicEvent(string eventName)
            => Function.Call<bool>(Hash.CANCEL_MUSIC_EVENT, eventName);

        public static bool IsMusicPlaying()
            => Function.Call<bool>(Hash.AUDIO_IS_MUSIC_PLAYING);

        public static void ChangeMusicEventIntensity(MusicEventIntensity intensity)
        {
            if (MusicEventNames.TryGetValue(intensity, out string eventName))
                TriggerMusicEvent(eventName);
        }

        public static void PrepareMusicEventIntensity(MusicEventIntensity intensity)
        {
            if (MusicEventNames.TryGetValue(intensity, out string eventName))
                PrepareMusicEvent(eventName);
        }

        // --- Sounds ---

        /// <summary>
        /// Plays a frontend sound using an existing sound ID slot. Pass -1 to auto-allocate a slot.
        /// </summary>
        public static void PlaySoundFrontend(int soundId, string audioName, string audioRef, bool enableOnReplay = true)
            => Function.Call(Hash.PLAY_SOUND_FRONTEND, soundId, audioName, audioRef, enableOnReplay);

        /// <summary>
        /// Plays a frontend sound, auto-allocating a sound ID slot.
        /// </summary>
        public static void PlaySoundFrontend(string audioName, string audioRef)
            => Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, audioName, audioRef, true);

        public static void PlayStreamFrontend()
            => Function.Call(Hash.PLAY_STREAM_FRONTEND);

        // --- Alarms ---

        public static bool PrepareAlarm(string alarmName)
            => Function.Call<bool>(Hash.PREPARE_ALARM, alarmName);

        /// <param name="playUntilCancelled">If true, alarm loops until explicitly stopped.</param>
        public static void StartAlarm(string alarmName, bool playUntilCancelled)
            => Function.Call(Hash.START_ALARM, alarmName, playUntilCancelled);

        // --- Streams ---

        public static bool LoadStream(string streamName, string soundSet)
            => Function.Call<bool>(Hash.LOAD_STREAM, streamName, soundSet);

        public static bool LoadStream(string streamName, int soundSet)
            => Function.Call<bool>(Hash.LOAD_STREAM, streamName, soundSet);

        // --- Audio Scenes ---

        public static bool StartAudioScene(string scene)
            => Function.Call<bool>(Hash.START_AUDIO_SCENE, scene);

        public static void StopAudioScene(string scene)
            => Function.Call(Hash.STOP_AUDIO_SCENE, scene);

        public static void StopAllAudioScenes()
            => Function.Call(Hash.STOP_AUDIO_SCENES);

        public static bool IsAudioSceneActive(string scene)
            => Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, scene);

        // --- Slowmo ---

        public static void ActivateAudioSlowmoMode(string mode)
            => Function.Call(Hash.ACTIVATE_AUDIO_SLOWMO_MODE, mode);

        // --- Radio ---

        public static void SetUserRadioControlEnabled(bool enabled)
            => Function.Call(Hash.SET_USER_RADIO_CONTROL_ENABLED, enabled);

        public static void SetVehicleRadioStation(Vehicle vehicle, string radioStation)
            => Function.Call(Hash.SET_VEH_RADIO_STATION, vehicle, radioStation);

        public static void SetVehicleRadioEnabled(Vehicle vehicle, bool enabled)
            => Function.Call(Hash.SET_VEHICLE_RADIO_ENABLED, vehicle, enabled);

        // --- Ped Speech ---

        public static void PlayPedAmbientSpeech(Ped ped, string speechName, string speechParam, int unk = 1)
            => Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, ped, speechName, speechParam, unk);

        // --- Enum ---

        public enum MusicEventIntensity
        {
            Idle,
            Silent,
            Suspense,
            MedIntensity,
            Gunfight,
            VehicleAction,
            Delivering,
            Fail,
            MusicStop
        }
    }
}
