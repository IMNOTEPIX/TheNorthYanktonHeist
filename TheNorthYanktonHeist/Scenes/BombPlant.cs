using GTA;
using GTA.Math;
using GTA.Native;
using fAudio = IMNOTEPIX.Framework.fAudio;
using fCore = IMNOTEPIX.Framework.fCore;
using fUI = IMNOTEPIX.Framework.fUI;
using IMNOTEPIX.Framework.fWorld.Anims;
using IMNOTEPIX.Framework.fWorld.Effects;
using fWorld = IMNOTEPIX.Framework.fWorld;
using fPlayer = IMNOTEPIX.Framework.fPlayer;
using fInterior = IMNOTEPIX.Framework.fWorld.Interior;
using fBlip = IMNOTEPIX.Framework.fBlip;
using IMNOTEPIX.Framework.fInterfaces;

namespace TheNorthYanktonHeist.Scenes
{
    public class BombPlantScene : IScene
    {
        private int _sequence = 0;

        public static Prop _permBomb;
        private Prop _bomb;
        private Prop _bag;
        private Camera _cam;

        private SynchronizedScene _playerScene;
        private SynchronizedScene _bombScene;

        private WeaponHash _prevWeapon;
        private fPlayer.BagManager.BagVariantTypes _bagVariant;

        public bool IsFinished { get; private set; }

        public BombPlantScene()
        {
            Vector3 pos = new Vector3(5298.2f, -5187.9f, 84.21848f);

            _playerScene = new SynchronizedScene(pos, -85f);
            _bombScene = new SynchronizedScene(pos, -85f);
        }

        public void Start()
        {
            _sequence = 0;
            IsFinished = false;
        }

        public void Update()
        {
            switch (_sequence)
            {
                case 0:
                    Setup();
                    break;

                case 1:
                    RunScene();
                    break;

                case 2:
                    Cleanup();
                    break;
            }
        }

        private void Setup()
        {
            Ped ped = Game.Player.Character;
            int l_1196 = fWorld.Interior.Interior.GetInteriorAtCoordsWithType(new Vector3(5311.236f, -5212.563f, (85.7187f - 3.2f)), "V_CashDepot");

            _prevWeapon = ped.Weapons.Current.Hash;
            _bagVariant = fPlayer.BagManager.GetBagVariantFromPed(ped);

            if (_bagVariant == fPlayer.BagManager.BagVariantTypes.Invalid)
            {
                fPlayer.BagManager.ApplyBag(ped, fPlayer.BagManager.BagVariantTypes.OriginalHeists);
                _bagVariant = fPlayer.BagManager.BagVariantTypes.OriginalHeists;
            }

            if (_cam == null)
            {
                _cam = fWorld.Camera.CreateScriptedCam();
                fWorld.Camera.SetupMovingCam(_cam, new Vector3(5297.32f, -5186.582f, 84f), new Vector3(0f, 0f, 200f), 48f, CameraShake.Hand, 1f);
            }

            if (_bomb == null)
            {
                _bomb = World.CreateProp("h4_prop_h4_ld_bomb_02a",
                    ped.Position, true, false);
                Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, _bomb, l_1196, fWorld.Interior.Interior.GetRoomKeyFromEntity(fPlayer.Player.Character));
            }

            if (_bag == null)
                _bag = fPlayer.BagManager.CreateBagPropFromPed(ped);

            if (_bomb != null && _bag != null)
                _sequence = 1;
        }

        private void RunScene()
        {
            Ped ped = Game.Player.Character;

            if (!_playerScene.IsRunning)
            {
                Game.Player.SetControlState(false);

                fPlayer.BagManager.RemoveBag(ped);
                ped.Weapons.Select(WeaponHash.Unarmed);

                string animDict = fCore.Streaming.RequestAnimDict("anim@scripted@player@mission@tun_bomb_plant@male@");

                _playerScene.Create();
                _bombScene.HoldLastFrame = true;
                _bombScene.Create();

                _playerScene.PlayPed(ped, animDict, "enter");
                _playerScene.PlayEntity(_bag, animDict, "enter_bag");
                _bombScene.PlayEntity(_bomb, animDict, "enter_bomb");
                fWorld.Camera.RenderScriptCams(true, true, 800);
            }
            else if (_playerScene.Phase >= 0.8f)
            {
                fWorld.Camera.RenderScriptCams(false, true, 1000);
            }
            if (_playerScene.IsFinished)
            {
                _sequence = 2;
            }
        }

        private void Cleanup()
        {
            if (_permBomb == null)
            {
                _permBomb = World.CreatePropNoOffset("h4_prop_h4_ld_bomb_02a", new Vector3(5298.2f, -5187.901f, 84.21844f), new Vector3(0f, 0f, -84.99383f), false);
            }
            Ped ped = Game.Player.Character;

            fWorld.Camera.RenderScriptCams(false, true, 1000);
            ped.Weapons.Select(_prevWeapon);
            fPlayer.BagManager.ApplyBag(ped, _bagVariant);
            fPlayer.Player.Character.Task.ClearAllImmediately();
            Game.Player.SetControlState(true);

            Stop();
            IsFinished = true;
        }

        public void Stop()
        {
            _playerScene?.Dispose();
            _bombScene?.Dispose();

            if (_cam?.Exists() == true)
            {
                _cam.Delete();
                _cam = null;
            }

            if (_bomb?.Exists() == true)
            {
                _bomb.Delete();
                _bomb = null;
            }

            if (_bag?.Exists() == true)
            {
                _bag.Delete();
                _bag = null;
            }
        }
    }
}
