namespace TheNorthYanktonHeist.Scaleforms
{
    using GTA;
    using GTA.Native;
    using TheNorthYanktonHeist.Interfaces;
    using TheNorthYanktonHeist.Funcs;
    public class MissionShard : IScaleform
    {
        public unsafe void DeleteScaleform()
        {
            int num = scaleID;
            Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
            scaleID = 0;
        }

        public void RequestScaleform()
        {
            scaleID = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MIDSIZED_MESSAGE");
            while (!fScaleforms.HasScaleformMovieLoaded(scaleID))
            {
                Script.Wait(0);
            }
        }

        public void DrawScaleform()
        {
            if (!Game.Player.Character.IsDead)
            {
                Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleID, 255, 255, 255, 255, 0);
            }
        }

        public void Shard_In(string ShardName, string ShardDescription, int color = 2, float speed = 0.5f, int colorout = 0, bool failShard = false)
        {
            DeleteScaleform();
            RequestScaleform();
            Script.Wait(500);
            fScaleforms.CallFunction(scaleID, "SHOW_SHARD_MIDSIZED_MESSAGE", ShardName, ShardDescription, color, false, true);
            if (failShard)
            {
                Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, shardFailAudioBank, true, -1);
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, shardFailSoundName, shardFailSoundSet, true);
            }
            else
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, shardInSoundName, shardInSoundSet, true);
            int num = Game.GameTime + 7000;
            while (Game.GameTime < num)
            {
                DrawScaleform();
                Script.Wait(0);
            }
            Shard_Out(colorout, speed);
            num = Game.GameTime + 2000;
            while (Game.GameTime < num)
            {
                DrawScaleform();
                Script.Wait(0);
            }
            DeleteScaleform();
            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, shardFailAudioBank);
        }

        public void Shard_Out(int color, float speed)
        {
            fScaleforms.CallFunction(scaleID, "SHARD_ANIM_OUT", color, speed);
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, shardOutSoundName, shardOutSoundSet, true);
        }

        public void Dispose()
        {
            if (scaleID != 0)
            {
                DeleteScaleform();
            }
        }

        public int scaleID = 0;

        public string shardInSoundName = "Shard_Appear";

        public string shardInSoundSet = "GTAO_FM_Events_Soundset";

        public string shardOutSoundName = "Shard_Disappear";

        public string shardOutSoundSet = "GTAO_FM_Events_Soundset";

        public string shardFailAudioBank = "DLC_MP2023_1/DLC_MP2023_1_Bicycle_Race";

        public string shardFailSoundName = "Fail";

        public string shardFailSoundSet = "Bike_Time_Trials_Soundset";

    }
}
