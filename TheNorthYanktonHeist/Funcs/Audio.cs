using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fAudio
    {
        public static bool TriggerMusicEvent(string eventName)
        {
            return Function.Call<bool>(Hash.TRIGGER_MUSIC_EVENT, eventName);
        }
        public static bool PrepareMusicEvent(string eventName)
        {
            return Function.Call<bool>(Hash.PREPARE_​MUSIC_​EVENT, eventName);
        }
        public static bool AudioIsMusicPlaying()
        {
            return Function.Call<bool>(Hash.AUDIO_​IS_​MUSIC_​PLAYING);
        }
        public static void ChangeMusicEventIntensity(MusicEventIntensity intensity)
        {
            if (intensity == MusicEventIntensity.StaffProblemsCroupierChaseStart)
                TriggerMusicEvent("CH_STAFF_PROBLEMS_CROUPIER_CHASE_START");
            if (intensity == MusicEventIntensity.IdleStart)
                TriggerMusicEvent("CH_IDLE_START");
            if (intensity == MusicEventIntensity.MedIntensityStart)
                TriggerMusicEvent("CH_MED_INTENSITY_START");
            if (intensity == MusicEventIntensity.GunfightStart)
                TriggerMusicEvent("CH_GUNFIGHT_START");
            if (intensity == MusicEventIntensity.DeliveringStart)
                TriggerMusicEvent("HEI4_DELIVERING_START");
            if (intensity == MusicEventIntensity.SuspenseStart)
                TriggerMusicEvent("HEI4_SUSPENSE_START");
            if (intensity == MusicEventIntensity.Suspense)
                TriggerMusicEvent("CH_SUSPENSE");
            if (intensity == MusicEventIntensity.MedIntensity)
                TriggerMusicEvent("CH_MED_INTENSITY");
            if (intensity == MusicEventIntensity.Delivering)
                TriggerMusicEvent("CH_DELIVERING");
            if (intensity == MusicEventIntensity.Gunfight)
                TriggerMusicEvent("CH_GUNFIGHT");
            if (intensity == MusicEventIntensity.VehicleAction)
                TriggerMusicEvent("CH_VEHICLE_ACTION");
            if (intensity == MusicEventIntensity.Idle)
                TriggerMusicEvent("CH_IDLE");
            if (intensity == MusicEventIntensity.Silent)
                TriggerMusicEvent("CH_SILENT");
            if (intensity == MusicEventIntensity.Fail)
                TriggerMusicEvent("CH_FAIL");
            if (intensity == MusicEventIntensity.MusicStop)
                TriggerMusicEvent("CH_MUSIC_STOP");
        }
        public static void PrepareMusicEventIntensity(MusicEventIntensity intensity)
        {
            if (intensity == MusicEventIntensity.StaffProblemsCroupierChaseStart)
                PrepareMusicEvent("CH_STAFF_PROBLEMS_CROUPIER_CHASE_START");
            if (intensity == MusicEventIntensity.IdleStart)
                PrepareMusicEvent("CH_IDLE_START");
            if (intensity == MusicEventIntensity.MedIntensityStart)
                PrepareMusicEvent("CH_MED_INTENSITY_START");
            if (intensity == MusicEventIntensity.GunfightStart)
                PrepareMusicEvent("CH_GUNFIGHT_START");
            if (intensity == MusicEventIntensity.DeliveringStart)
                PrepareMusicEvent("HEI4_DELIVERING_START");
            if (intensity == MusicEventIntensity.SuspenseStart)
                PrepareMusicEvent("HEI4_SUSPENSE_START");
            if (intensity == MusicEventIntensity.Suspense)
                PrepareMusicEvent("CH_SUSPENSE");
            if (intensity == MusicEventIntensity.MedIntensity)
                PrepareMusicEvent("CH_MED_INTENSITY");
            if (intensity == MusicEventIntensity.Delivering)
                PrepareMusicEvent("CH_DELIVERING");
            if (intensity == MusicEventIntensity.Gunfight)
                PrepareMusicEvent("CH_GUNFIGHT");
            if (intensity == MusicEventIntensity.VehicleAction)
                PrepareMusicEvent("CH_VEHICLE_ACTION");
            if (intensity == MusicEventIntensity.Idle)
                PrepareMusicEvent("CH_IDLE");
            if (intensity == MusicEventIntensity.Silent)
                PrepareMusicEvent("CH_SILENT");
            if (intensity == MusicEventIntensity.Fail)
                PrepareMusicEvent("CH_FAIL");
            if (intensity == MusicEventIntensity.MusicStop)
                PrepareMusicEvent("CH_MUSIC_STOP");
        }
        public static void PlayPedAmbientSpeechNative(Ped ped, string speechName, string speechParam, int p3 = 1)
        {
            Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, ped, speechName, speechParam, p3);
        }
        public static void SetUserRadioControlEnabled(bool toggle)
        {
            Function.Call(Hash.SET_USER_RADIO_CONTROL_ENABLED, toggle);
        }
        public static void SetVehicleRadioStation(Vehicle vehicle, string radioStation)
        {
            Function.Call(Hash.SET_​VEH_​RADIO_​STATION, vehicle, radioStation);
        }
        public static void SetVehicleRadioEnabled(Vehicle vehicle, bool toggle)
        {
            Function.Call(Hash.SET_​VEHICLE_​RADIO_​ENABLED, vehicle, toggle);
        }
        public static bool StartAudioScene(string scene)
        {
            return Function.Call<bool>(Hash.START_​AUDIO_​SCENE, scene);
        }
        public static void StopAudioScene(string scene)
        {
            Function.Call(Hash.STOP_​AUDIO_​SCENE, scene);
        }
        public static void StopAudioScenes()
        {
            Function.Call(Hash.STOP_​AUDIO_​SCENES);
        }
        public static bool IsAudioSceneActive(string scene)
        {
            return Function.Call<bool>(Hash.IS_​AUDIO_​SCENE_​ACTIVE, scene);
        }
        public enum MusicEventIntensity
        {
            StaffProblemsCroupierChaseStart,
            IdleStart,
            MedIntensityStart,
            GunfightStart,
            DeliveringStart,
            SuspenseStart,
            Suspense,
            MedIntensity,
            Delivering,
            Gunfight,
            VehicleAction,
            Idle,
            Silent,
            Fail,
            MusicStop
        }
    }
}
