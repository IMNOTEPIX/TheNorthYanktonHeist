using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    public class fCutsceneCreation
    {
        public void AddRegisterEntityToList(int ent, string cHandle, fCutscene.CutsceneUsage usage, int modelNames, fCutscene.CutsceneEntityOptionFlag entityOptionsFlag)
        {
            RegisterEntityChunk item = new RegisterEntityChunk(ent, cHandle, usage, modelNames, entityOptionsFlag);
            this.theseEntities.Add(item);
        }

        public void AddRegisterEntityToList(Entity ent, string cHandle, fCutscene.CutsceneUsage usage, int modelNames, fCutscene.CutsceneEntityOptionFlag entityOptionsFlag)
        {
            RegisterEntityChunk item = new RegisterEntityChunk(ent, cHandle, usage, modelNames, entityOptionsFlag);
            this.theseEntities.Add(item);
        }

        public fCutsceneCreation(string cutsceneName, Vector3 pos, Vector3 rot, bool setPlayerModel)
        {
            CutsceneName = cutsceneName;
            Pos = pos;
            Rot = rot;
            SetPlayerModel = setPlayerModel;
        }

        public fCutsceneCreation(string cutsceneName, Vector3 pos, Vector3 rot, bool setPlayerModel, bool fadeOutAtStart = false, bool fadeInAtStart = false, bool fadeOutAtEnd = false, bool fadeInAtEnd = false)
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
            for (int i = 0; i < this.theseEntities.Count; i++)
            {
                bool flag = this.theseEntities[i].Ent != null && this.theseEntities[i].Ent.EntityType == EntityType.Ped;
                if (flag)
                {
                    this.theseEntities[i].SetPedOutfitCutscene((Ped)this.theseEntities[i].Ent);
                }
            }
            fCutscene.LoadCutscene(this.CutsceneName);
            bool setPlayerModel = this.SetPlayerModel;
            if (setPlayerModel)
            {
                fPlayer.PlayerModelSet(Game.Player.Character);
            }
            for (int j = 0; j < this.theseEntities.Count; j++)
            {
                bool flag2 = this.theseEntities[j].EntInt != -1;
                if (flag2)
                {
                    fCutscene.RegisterEntityForCutscene(0, this.theseEntities[j].CHandle, (int)this.theseEntities[j].Usage, this.theseEntities[j].ModelNames, (int)this.theseEntities[j].EntityOptionsFlag);
                }
                else
                {
                    fCutscene.RegisterEntityForCutscene(this.theseEntities[j].Ent, this.theseEntities[j].CHandle, (int)this.theseEntities[j].Usage, this.theseEntities[j].ModelNames, (int)this.theseEntities[j].EntityOptionsFlag);
                }
            }
            fCutscene.StartCutscene(fCutscene.CutscenePlaybackFlags.CUTSCENE_REQUESTED_IN_MISSION);
            bool flag3 = this.FadeOutAtStart || this.FadeInAtStart || this.FadeOutAtEnd || this.FadeInAtEnd;
            if (flag3)
            {
                fCutscene.SetCutsceneFadeValues(this.FadeOutAtStart, this.FadeInAtStart, this.FadeOutAtEnd, this.FadeInAtEnd);
            }
            while (!fCutscene.IsCutscenePlaying())
            {
                Script.Wait(0);
            }
            for (int k = 0; k < this.theseEntities.Count; k++)
            {
                bool flag4 = this.theseEntities[k].Ent != null && this.theseEntities[k].Ent.EntityType == EntityType.Ped;
                if (flag4)
                {
                    this.theseEntities[k].GetPedOutfitCutscene((Ped)this.theseEntities[k].Ent);
                }
            }
            bool setPlayerModel2 = this.SetPlayerModel;
            if (setPlayerModel2)
            {
                fPlayer.PlayerModelSetBack(Game.Player.Character);
            }
        }

        public void StartCutsceneTillEnd()
        {
            fCutscene.LoadCutscene(this.CutsceneName);
            bool setPlayerModel = this.SetPlayerModel;
            if (setPlayerModel)
            {
                fPlayer.PlayerModelSet(Game.Player.Character);
            }
            for (int i = 0; i < this.theseEntities.Count; i++)
            {
                fCutscene.RegisterEntityForCutscene(this.theseEntities[i].Ent, this.theseEntities[i].CHandle, (int)this.theseEntities[i].Usage, this.theseEntities[i].ModelNames, (int)this.theseEntities[i].EntityOptionsFlag);
            }
            fCutscene.StartCutscene(fCutscene.CutscenePlaybackFlags.CUTSCENE_REQUESTED_IN_MISSION);
            Script.Wait(50);
            bool setPlayerModel2 = this.SetPlayerModel;
            if (setPlayerModel2)
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
            bool flag = this.theseEntities.Count > 0;
            if (flag)
            {
                this.theseEntities.Clear();
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
