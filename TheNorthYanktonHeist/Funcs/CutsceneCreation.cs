using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class CutsceneCreation
    {
        public void AddRegisterEntityToList(int ent, string cHandle, fCutscene.CutsceneUsage usage, int modelNames, fCutscene.CutsceneEntityOptionFlag entityOptionsFlag)
        {
            RegisterEntityChunk item = new RegisterEntityChunk(ent, cHandle, usage, modelNames, entityOptionsFlag);
            theseEntities.Add(item);
        }

        public void AddRegisterEntityToList(Entity ent, string cHandle, fCutscene.CutsceneUsage usage, int modelNames, fCutscene.CutsceneEntityOptionFlag entityOptionsFlag)
        {
            RegisterEntityChunk item = new RegisterEntityChunk(ent, cHandle, usage, modelNames, entityOptionsFlag);
            theseEntities.Add(item);
        }

        public CutsceneCreation(string cutsceneName, Vector3 pos, Vector3 rot, bool setPlayerModel)
        {
            CutsceneName = cutsceneName;
            Pos = pos;
            Rot = rot;
            SetPlayerModel = setPlayerModel;
        }

        public CutsceneCreation(string cutsceneName, Vector3 pos, Vector3 rot, bool setPlayerModel, bool fadeOutAtStart = false, bool fadeInAtStart = false, bool fadeOutAtEnd = false, bool fadeInAtEnd = false)
        {
            CutsceneName = cutsceneName;
            Pos = pos;
            Rot = rot;
            SetPlayerModel = setPlayerModel;
            FadeOutAtStart = fadeOutAtStart;
            FadeInAtStart = fadeInAtStart;
            FadeOutAtEnd = fadeOutAtEnd;
            FadeInAtEnd = fadeInAtEnd;
        }

        public void StartCutscene()
        {
            for (int i = 0; i < theseEntities.Count; i++)
            {
                if (theseEntities[i].Ent != null && theseEntities[i].Ent.EntityType == EntityType.Ped)
                {
                    theseEntities[i].GetCutscenePedOutfit((Ped)theseEntities[i].Ent);
                }
            }
            fCutscene.LoadCutscene(CutsceneName);
            if (SetPlayerModel)
            {
                fPlayer.PlayerModelSet(Game.Player.Character);
            }
            for (int j = 0; j < theseEntities.Count; j++)
            {
                if (theseEntities[j].EntInt != -1)
                {
                    fCutscene.RegisterEntityForCutscene(0, theseEntities[j].CHandle, (int)theseEntities[j].Usage, theseEntities[j].ModelNames, (int)theseEntities[j].EntityOptionsFlag);
                }
                else
                {
                    fCutscene.RegisterEntityForCutscene(theseEntities[j].Ent, theseEntities[j].CHandle, (int)theseEntities[j].Usage, theseEntities[j].ModelNames, (int)theseEntities[j].EntityOptionsFlag);
                }
            }
            fCutscene.StartCutscene(fCutscene.CutscenePlaybackFlags.CUTSCENE_REQUESTED_IN_MISSION);
            if (FadeOutAtStart || FadeInAtStart || FadeOutAtEnd || FadeInAtEnd)
            {
                fCutscene.SetCutsceneFadeValues(FadeOutAtStart, FadeInAtStart, FadeOutAtEnd, FadeInAtEnd);
            }
            while (!fCutscene.IsCutscenePlaying())
            {
                Script.Wait(0);
            }
            for (int k = 0; k < theseEntities.Count; k++)
            {
                if (theseEntities[k].Ent != null && theseEntities[k].Ent.EntityType == EntityType.Ped)
                {
                    theseEntities[k].SetCutscenePedOutfit((Ped)theseEntities[k].Ent);
                }
            }
            if (SetPlayerModel)
            {
                fPlayer.PlayerModelSetBack(Game.Player.Character);
            }
        }

        public void StartCutsceneTillEnd()
        {
            fCutscene.LoadCutscene(CutsceneName);
            if (SetPlayerModel)
            {
                fPlayer.PlayerModelSet(Game.Player.Character);
            }
            for (int i = 0; i < theseEntities.Count; i++)
            {
                fCutscene.RegisterEntityForCutscene(theseEntities[i].Ent, theseEntities[i].CHandle, (int)theseEntities[i].Usage, theseEntities[i].ModelNames, (int)theseEntities[i].EntityOptionsFlag);
            }
            fCutscene.StartCutscene(fCutscene.CutscenePlaybackFlags.CUTSCENE_REQUESTED_IN_MISSION);
            Script.Wait(50);
            if (SetPlayerModel)
            {
                fPlayer.PlayerModelSetBack(Game.Player.Character);
            }
            while (!fCutscene.HasCutsceneFinished())
            {
                Script.Wait(0);
            }
        }

        public void Cleanup()
        {
            if (theseEntities.Count > 0)
            {
                theseEntities.Clear();
            }
            fCutscene.RemoveCutscene();
        }

        public string CutsceneName = "";

        public Vector3 Pos;

        public Vector3 Rot;

        public bool SetPlayerModel = true;

        public List<RegisterEntityChunk> theseEntities = new List<RegisterEntityChunk>();

        public bool FadeOutAtStart = false;

        public bool FadeInAtStart = false;

        public bool FadeOutAtEnd = false;

        public bool FadeInAtEnd = false;
    }
}
