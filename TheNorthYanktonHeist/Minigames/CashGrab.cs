using GTA;
using GTA.Math;
using GTA.Native;
using GTA.NaturalMotion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheNorthYanktonHeist.Funcs;
using TheNorthYanktonHeist.Interfaces;
using TheNorthYanktonHeist.Scenes;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Control = GTA.Control;
using Screen = GTA.UI.Screen;

namespace TheNorthYanktonHeist.Minigames
{
    
    public enum CartType
    {
        Standard,
        Cocaine,
        Cash_a,
        Cash_b,
        Cash_c,
        Gold_a,
        Gold_b,
        Gold_c,
        LowCash_a,
        LowCash_b,
        LowCash_c,
        Diamonds_a,
        Diamonds_b,
        Diamonds_c
    }
    public abstract class CashGrabMinigame
    {
        public virtual float MinRate { get; set; } = 0.65f;
        public virtual float MaxRate { get; set; } = 1.5f;
        public int State { get; set; }
        public bool IsMinigameActive
        {
            get => Function.Call<bool>(Hash.IS_MINIGAME_IN_PROGRESS);
            set => Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, value);
        }
        public bool IsClassBeingUsed { get; set; }
        public bool IsLooted { get; set; }
        public bool CasinoTableCam {  get; set; }
        public bool UseRemoteCounterSound { get; set; } = false;
        public void IsActive(bool toggle)
        {
            IsMinigameActive = toggle;
            IsClassBeingUsed = toggle;
        }
        private void UpdateAnimationRate(ref AnimationRateData data, float minRate, float maxRate)
        {
            if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
                return;

            float deltaTime = Function.Call<float>(Hash.TIMESTEP);

            bool inputPressed;

            if (Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2))
                inputPressed = Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 237);
            else
                inputPressed = Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201);

            if (inputPressed)
            {
                float increase = GetRateIncrease(data.DifficultyLevel);
                data.TargetRate += increase;
            }
            else
            {
                float decay = GetRateDecay(data.DifficultyLevel);
                data.TargetRate -= decay * deltaTime;
            }

            data.TargetRate = Clamp(data.TargetRate, minRate, maxRate);

            data.CurrentRate = SmoothRate(
                data.CurrentRate,
                data.TargetRate,
                deltaTime
            );
        }
        private float GetRateIncrease(int difficulty)
        {
            switch (difficulty)
            {
                case 0: return 0.10f;   // Easy
                case 1: return 0.09f;   // Medium
                case 2: return 0.08f;   // Hard
                default: return 0.10f;
            }
        }
        private float GetRateDecay(int difficulty)
        {
            switch (difficulty)
            {
                case 0: return 0.30f;
                case 1: return 0.375f;
                case 2: return 0.405f;
                default: return 0.30f;
            }
        }
        private float SmoothRate(float current, float target, float deltaTime)
        {
            float smoothingSpeed = 6f; // tweak if needed

            float step = smoothingSpeed * deltaTime;

            if (Math.Abs(current - target) <= step)
                return target;

            return current < target
                ? current + step
                : current - step;
        }
        private float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        public void ResetValues()
        {
            Data.DifficultyLevel = 0;
            Data.CurrentPhase = 0f;
            Data.CurrentRate = 0f;
            Data.TargetRate = 0f;
            MinRate = default;
            MaxRate = default;
            State = 0;
            IsActive(false);
            IsLooted = false;
            PrevBagType = BagManager.BagVariantTypes.Invalid;
            PrevWeapon = default;
        }
        public virtual void Update()
        {
            if (CasinoTableCam)
            {
                Function.Call(Hash.SET_TABLE_GAMES_CAMERA_THIS_UPDATE, fMisc.joaat("CASINO_BLACKJACK_CAMERA"));
            }
            if (fPlayer.ped.IsDead && IsClassBeingUsed)
            {
                PushResetOnDeath();
            }
            UpdateAnimationRate(ref Data, MinRate, MaxRate);
        }
        public virtual void Dispose()
        {
            ResetValues();
        }
        public virtual void PushResetOnDeath()
        {
            State = 0;
            IsActive(false);
            fPlayer.ped.CanSwitchWeapons = true;
            Game.Player.SetControlState(true, SetPlayerControlFlags.None);
            fPlayer.ped.Task.ClearAllImmediately();
            BagManager.ApplyBag(fPlayer.ped, PrevBagType);
        }
        public BagManager.BagVariantTypes PrevBagType;
        public WeaponHash PrevWeapon;
        public AnimationRateData Data;
        public struct AnimationRateData
        {
            public int DifficultyLevel;        // 0, 1, 2
            public float CurrentRate;          // What the animation is currently playing at
            public float TargetRate;           // Where we want it to move toward
            public float CurrentPhase;
        }
    }
    public class CartGrab : CashGrabMinigame, IMinigame
    {

        private static readonly Dictionary<CartType, string> CartModels = new Dictionary<CartType, string>
        {
                { CartType.Standard, "hei_prop_hei_cash_trolly_01" },
                { CartType.Cocaine, "imp_prop_impexp_coke_trolly" },
                { CartType.Cash_a, "ch_prop_ch_cash_trolly_01a" },
                { CartType.Cash_b, "ch_prop_ch_cash_trolly_01b" },
                { CartType.Cash_c, "ch_prop_ch_cash_trolly_01c" },
                { CartType.Gold_a, "ch_prop_gold_trolly_01a" },
                { CartType.Gold_b, "ch_prop_gold_trolly_01b" },
                { CartType.Gold_c, "ch_prop_gold_trolly_01c" },
                { CartType.LowCash_a, "ch_prop_cash_low_trolly_01a" },
                { CartType.LowCash_b, "ch_prop_cash_low_trolly_01b" },
                { CartType.LowCash_c, "ch_prop_cash_low_trolly_01c" },
                { CartType.Diamonds_a, "ch_prop_diamond_trolly_01a" },
                { CartType.Diamonds_b, "ch_prop_diamond_trolly_01b" },
                { CartType.Diamonds_c, "ch_prop_diamond_trolly_01c" }
        };
        public CartType Type { get; set; } = CartType.Standard;
        public Model cartModel { get; protected set; }
        public Vector3 position { get; protected set; }
        public Vector3 rotation { get; protected set; }
        public CartGrab(CartType type, Vector3 pos, Vector3 rot = default)
        {
            int model = GetModelFromType(type);
            cartModel = new Model(model);
            Type = type;
            position = pos;
            rotation = rot;
            MaxRate = IsTypeGold(type) ? MaxRate = 1.2802f : MaxRate = 1.5f;
        }
        private int GetModelFromType(CartType type)
        {
            if (CartModels.TryGetValue(type, out var model))
                return fMisc.joaat(model);
            return fMisc.joaat("hei_prop_hei_cash_trolly_01");
        }
        private bool IsTypeGold(CartType type)
        {
            return type == CartType.Gold_a || type == CartType.Gold_b || type == CartType.Gold_c;
        }
        private void DisplayStartGrabText()
        {
            switch (Type)
            {
                case CartType.Cocaine:
                    Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_6"/*"Press ~INPUT_CONTEXT~ to begin grabbing the drugs."*/, true);
                    break;
                case CartType.Gold_a:
                case CartType.Gold_b:
                case CartType.Gold_c:
                    Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_2"/*"Press ~INPUT_CONTEXT~ to begin grabbing the gold."*/, true);
                    break;
                case CartType.Diamonds_a:
                case CartType.Diamonds_b:
                case CartType.Diamonds_c:
                    Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_7"/*"Press ~INPUT_CONTEXT~ to begin grabbing the diamonds."*/, true);
                    break;
                default:
                    Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_1"/*"Press ~INPUT_CONTEXT~ to begin grabbing the cash."*/, true);
                    break;
            }
        }
        private void DisplayGrabText(bool forcedGrab)
        {
            switch (Type)
            {
                case CartType.Cocaine:
                    if (forcedGrab)
                    {
                        if (fGamePad.IsUsingKeyboardAndMouse(2 /*FRONTEND_CONTROL*/))
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_5_MK_b" /*"Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the drugs."*/, true);
                        else
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_5_b"/*"Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the drugs."*/, true);
                    }
                    else if (fGamePad.IsUsingKeyboardAndMouse(2 /*FRONTEND_CONTROL*/))
                        Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_5_MK"/*"Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the drugs. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the drugs."*/, true);
                    else
                        Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_5"/*"Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the drugs. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the drugs."*/, true);
                    break;
                case CartType.Diamonds_a:
                case CartType.Diamonds_b:
                case CartType.Diamonds_c:
                    if (forcedGrab)
                    {
                        if (fGamePad.IsUsingKeyboardAndMouse(2 /*FRONTEND_CONTROL*/))
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_7_MK_b"/*"Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the diamonds."*/, true);
                        else
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_7_TP_b"/*"Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the diamonds."*/, true);
                    }
                    else if (fGamePad.IsUsingKeyboardAndMouse(2 /*FRONTEND_CONTROL*/))
                        Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_7_MK"/*"Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the diamonds. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the diamonds."*/, true);
                    else
                        Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_7_TP"/*"Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the diamonds. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the diamonds."*/, true);
                    break;
                case CartType.Gold_a:
                case CartType.Gold_b:
                case CartType.Gold_c:
                    if (forcedGrab)
                    {
                        if (fGamePad.IsUsingKeyboardAndMouse(2 /*FRONTEND_CONTROL*/))
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_4b_b"/*"Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the gold."*/, true);
                        else
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_4_b"/*"Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the gold."*/, true);
                    }
                    else if (fGamePad.IsUsingKeyboardAndMouse(2 /*FRONTEND_CONTROL*/))
                        Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_4b"/*"Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the gold. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the gold."*/, true);
                    else
                        Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_4"/*"Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the gold. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the gold."*/, true);
                    break;
                default:
                    if (forcedGrab)
                    {
                        if (fGamePad.IsUsingKeyboardAndMouse(2 /*FRONTEND_CONTROL*/))
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_3_MK_b"/*"Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the cash."*/, true);
                        else
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_3_b"/*"Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the cash."*/, true);
                    }
                    else if (fGamePad.IsUsingKeyboardAndMouse(2 /*FRONTEND_CONTROL*/))
                        Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_3_MK"/*"Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the cash. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the cash."*/, true);
                    else
                        Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_GRAB_3"/*"Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the cash. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the cash."*/, true);
                    break;

            }
        }
        private int GetBarModel()
        {
            switch (Type)
            {
                case CartType.Standard:
                case CartType.Cash_a:
                case CartType.Cash_b:
                case CartType.Cash_c:
                case CartType.LowCash_a:
                case CartType.LowCash_b:
                case CartType.LowCash_c:
                    return fMisc.joaat("hei_prop_heist_cash_pile");
                case CartType.Cocaine:
                    return fMisc.joaat("imp_prop_impexp_coke_pile");
                case CartType.Gold_a:
                case CartType.Gold_b:
                case CartType.Gold_c:
                    return fMisc.joaat("ch_prop_gold_bar_01a");
                case CartType.Diamonds_a:
                case CartType.Diamonds_b:
                case CartType.Diamonds_c:
                    return fMisc.joaat("ch_prop_vault_dimaondbox_01a");
                default:
                    return fMisc.joaat("hei_prop_heist_cash_pile");
            }
        }
        private void PlayIntroScene()
        {
            PlayerScene = new SynchronizedScene(Cart.Position, Cart.Rotation);
            PlayerScene.Create(2000);
            PlayerScene.PlayPed(fPlayer.ped, "anim@heists@ornate_bank@grab_cash", "intro", 1.5f, -8f, (SyncedSceneFlags)13, RagdollBlockingFlags.PlayerImpact, 1.5f);
            PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_intro", 1000f, -1000f, SyncedSceneFlags.None, 1148846100f);
            CartScene = new SynchronizedScene(Cart);
        }
        private void PlayGrabScene()
        {
            PlayerScene.Create(2000);
            PlayerScene.PlayPed(fPlayer.ped, "anim@heists@ornate_bank@grab_cash", "grab", 4f, -4f, (SyncedSceneFlags)13, RagdollBlockingFlags.PlayerImpact, 1000f);
            PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_grab", 1000f, -1000f, SyncedSceneFlags.None, 1148846100f);
            PlayerScene.Phase = Data.CurrentPhase;
            CartScene.Create(2000);
            CartScene.PlayEntity(Cart, "anim@heists@ornate_bank@grab_cash", "cart_cash_dissapear", 1000f, -4f, SyncedSceneFlags.UseKinematicPhysics, 1148846100f);
            CartScene.Phase = Data.CurrentPhase;
        }
        private void PlayIdleScene()
        {
            PlayerScene.Create(2000);
            PlayerScene.PlayPed(fPlayer.ped, "anim@heists@ornate_bank@grab_cash", "grab_idle", 2f, -8f, (SyncedSceneFlags)13, RagdollBlockingFlags.PlayerImpact, 1000f, AnimationIKControlFlags.LinkedFacial);
            PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_grab_idle", 1000f, -1000f, SyncedSceneFlags.None, 1148846100f);
            PlayerScene.IsLooping = true;
        }
        private void PlayExitScene()
        {
            CasinoTableCam = false;
            Screen.ClearHelpText();
            if (Bar != null && Bar.Exists())
            {
                Bar.Delete();
                Bar = null;
            }
            PlayerScene = new SynchronizedScene(Cart);
            PlayerScene.Create(2000);
            PlayerScene.PlayPed(fPlayer.ped, "anim@heists@ornate_bank@grab_cash", "exit", 1000f, -8f, SyncedSceneFlags.HideWeapon, RagdollBlockingFlags.PlayerImpact, 1000f);
            PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_exit", 1000f, -8f, SyncedSceneFlags.None, 1000f);
            if (CartScene != null && CartScene.IsRunning)
            {
                CartScene.Rate = 0f;
            }
            State = 4;
        }
        public void CreateBlip(BlipSprite sprite, BlipColor colour, string name = "", float scale = 1f, bool isShortRange = true)
        {
            if (Cart != null && Cart.Exists())
            {
                Blip blip = Cart.AddBlip();
                blip.Sprite = sprite;
                blip.Scale = scale;
                blip.Color = colour;
                blip.IsShortRange = isShortRange;
                blip.Name = name;
            }
        }
        public void RemoveBlip()
        {
            if (Cart != null && Cart.Exists() && Cart.AttachedBlip != null && Cart.AttachedBlip.Exists())
            {
                Cart.AttachedBlip.Delete();
            }
        }
        public void Create()
        {
            if (Cart == null)
            {
                IsLooted = false;
                Cart = World.CreateProp(cartModel, position, rotation, false, false);
            }
            else
            {
                Cart.IsPositionFrozen = true;
                CartScene = new SynchronizedScene(Cart);
                CartScene.Create(2000);
                CartScene.PlayEntity(Cart, "anim@heists@ornate_bank@grab_cash", "cart_cash_dissapear", 1000f, -4f, SyncedSceneFlags.UseKinematicPhysics, 1148846100f);
                CartScene.Rate = 0f;
                CartScene.Phase = Data.CurrentPhase;
            }
        }
        public override void Update()
        {
            base.Update();
            if (CasinoTableCam)
            {
                DisplayGrabText(false);
            }
            switch (State)
            {
                case 0:
                    Create();
                    if (!IsLooted)
                    {
                        Vector3 Var0 = fEntity.GetOffsetFromEntityInWorldCoords(Cart, 0f, 0f, -0.45f);
                        Vector3 Var3 = fEntity.GetOffsetFromEntityInWorldCoords(Cart, 0f, 1.3f, 1.5f);
                        Vector3 vector = fEntity.GetAnimInitialOffsetPosition("anim@heists@ornate_bank@grab_cash", "intro", Cart.Position.X, Cart.Position.Y, Cart.Position.Z, Cart.Rotation.X, Cart.Rotation.Y, Cart.Rotation.Z, 0f, 2);
                        float y = fEntity.GetAnimInitialOffsetPosition("anim@heists@ornate_bank@grab_cash", "intro", Cart.Position.X, Cart.Position.Y, Cart.Position.Z, Cart.Rotation.X, Cart.Rotation.Y, Cart.Rotation.Z, 0f, 2).Y;
                        fStreaming.RequestAnimDict("anim@heists@ornate_bank@grab_cash");
                        fHud.RequestAdditionalText("MC_PLAY", 0);
                        fHud.RequestAdditionalText("HACK", 3);
                        if (fStreaming.HasAnimDictLoaded("anim@heists@ornate_bank@grab_cash") && fHud.HasThisAdditionalTextLoaded("MC_PLAY", 0) && fHud.HasAdditionalTextLoaded(3))
                        {
                            if (!fCutscene.IsCutscenePlaying() && Game.Player.CanControlCharacter)
                            {
                                if (fEntity.IsEntityInAngledArea(fPlayer.ped, new Vector3(Var0.X, Var0.Y, Var0.Z), new Vector3(Var3.X, Var3.Y, Var3.Z), 1.75f, false, true, 1))
                                {
                                    if (fPlayer.ped.Position.DistanceTo(vector) < 1f && !fPlayer.ped.IsInVehicle() && fPlayer.ped.IsAlive && !fPlayer.ped.IsPerformingMeleeAction && !fPlayer.ped.IsJumping && !fPlayer.ped.IsSprinting && !fPlayer.ped.IsRagdoll && !fPlayer.ped.IsGettingUp)
                                    {
                                        DisplayStartGrabText();
                                        if (Game.IsControlJustPressed(Control.Context))
                                        {
                                            Screen.ClearHelpText();
                                            IsActive(true);
                                            Game.Player.SetControlState(false, SetPlayerControlFlags.LeaveCameraControlOn);
                                            fAudio.StartAudioScene("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
                                            fPed.SetPedCanLegIK(fPlayer.ped, false);
                                            fPlayer.ped.SetResetFlag(PedResetFlagToggles.CannotBeTargeted, false);
                                            fEntity.SetEntityProofs(fPlayer.ped, false, false, false, true, false, true, false, true);
                                            fPlayer.ped.CanSwitchWeapons = false;
                                            if (fPlayer.ped.Weapons.Current != WeaponHash.Unarmed)
                                            {
                                                PrevWeapon = fPlayer.ped.Weapons.Current;
                                            }
                                            fPlayer.ped.Weapons.Select(WeaponHash.Unarmed);
                                            PrevBagType = BagManager.GetBagVariantFromPed(fPlayer.ped);
                                            if (PrevBagType == BagManager.BagVariantTypes.Invalid)
                                            {
                                                BagManager.ApplyBag(fPlayer.ped, BagManager.BagVariantTypes.OriginalHeists);
                                                PrevBagType = BagManager.BagVariantTypes.OriginalHeists;
                                            }
                                            Bar = World.CreateProp(GetBarModel(), fPlayer.ped.Position, false, false);
                                            if (Bar != null && Bar.Exists())
                                            {
                                                Bar.IsCollisionEnabled = false;
                                                Bar.IsVisible = false;
                                                Bar.IsInvincible = true;
                                                Bar.AttachTo(fPlayer.ped.Bones[Bone.PHLeftHand]);
                                            }
                                            Bag = BagManager.CreateBagPropFromPed(fPlayer.ped);
                                            if (Bag != null && Bag.Exists())
                                            {
                                                Bag.IsCollisionEnabled = false;
                                                Bag.IsVisible = false;
                                                Bag.IsInvincible = true;
                                            }
                                            CasinoTableCam = true;
                                            fPed.SetMovementModeOverride(fPlayer.ped, "DEFAULT_ACTION");
                                            fPlayer.ped.Task.GoStraightTo(vector, 20000, y, 0.75f);
                                            fPlayer.ped.KeepTaskWhenMarkedAsNoLongerNeeded = true;
                                            State += 1;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    if (fPlayer.ped.GetScriptTaskStatus(ScriptTaskNameHash.GoStraightToCoord) == ScriptTaskStatus.Finished)
                    {
                        PlayIntroScene();
                        State += 1;
                        return;
                    }
                    break;
                case 2:
                    if (Bar != null && Bar.Exists() && PlayerScene.Phase > 0.34f)
                    {
                        BagManager.RemoveBag(fPlayer.ped);
                        if (fPed.HaveAllStreamingRequestsCompleted(fPlayer.ped))
                            Bag.IsVisible = true;
                    }
                    if (PlayerScene.IsFinished)
                        State += 1;
                    break;
                case 3:
                    if (fEntity.IsEntityPlayingAnim(fPlayer.ped, "anim@heists@ornate_bank@grab_cash", "grab", 3))
                    {
                        if (PlayerScene.IsRunning)
                        {
                            PlayerScene.Rate = Data.CurrentRate;
                        }
                        if (CartScene.IsRunning)
                        {
                            CartScene.Rate = Data.CurrentRate;
                        }
                        Data.CurrentPhase = PlayerScene.Phase;
                        if (fEntity.HasAnimEventFired(fPlayer.ped, "CASH_APPEAR") && Bar != null && Bar.Exists() && !Bar.IsVisible)
                        {
                            Bar.IsVisible = true;
                        }
                        if (fEntity.HasAnimEventFired(fPlayer.ped, "RELEASE_CASH_DESTROY"))
                        {
                            if (Bar != null && Bar.Exists() && Bar.IsVisible)
                            {
                                Bar.IsVisible = false;
                            }
                            if (UseRemoteCounterSound)
                            {
                                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", false);
                                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", false);
                            }
                            else
                            {
                                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "LOCAL_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", false);
                            }
                            if (ControlTriggered)
                            {
                                Data.CurrentRate = 0.65f;
                                PlayExitScene();
                                ControlTriggered = false;
                                return;
                            }
                            /*
                            int value = 0;
                            int value2 = 0;
                            switch (Type)
                            {
                                case TrollyGrabTypes.Cash:
                                    value = 10000;
                                    value2 = 15000;
                                    break;
                                case TrollyGrabTypes.Coke:
                                    value = 25000;
                                    value2 = 30000;
                                    break;
                                case TrollyGrabTypes.Gold:
                                    value = 30000;
                                    value2 = 40000;
                                    break;
                                case TrollyGrabTypes.Diamond:
                                    value = 45000;
                                    value2 = 50000;
                                    break;
                            }
                            MinigameValueAddedEventHandler valueAdded = ValueAdded;
                            if (valueAdded != null)
                            {
                                valueAdded(this, new MinigameValueAddedArgs(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, value, value2), true));
                            }*/
                            if (Data.CurrentRate == MinRate)
                            {
                                CartScene.Rate = 0f;
                                PlayIdleScene();
                            }
                        }
                        if (Data.CurrentPhase >= 1f)
                        {
                            Screen.ClearHelpText();
                            RemoveBlip();
                            IsLooted = true;
                            PlayExitScene();
                            return;
                        }
                    }
                    else
                    {
                        if (!fEntity.IsEntityPlayingAnim(fPlayer.ped, "anim@heists@ornate_bank@grab_cash", "grab_idle", 3))
                        {
                            PlayIdleScene();
                            return;
                        }
                        if (Data.CurrentRate > 0.65f)
                        {
                            PlayGrabScene();
                        }
                    }
                    if (Game.IsControlJustPressed(fGamePad.IsUsingKeyboardAndMouse(2) ? Control.CursorCancel : Control.FrontendCancel))
                    {
                        ControlTriggered = true;
                        return;
                    }
                    break;
                case 4:
                    if (PlayerScene != null)
                    {
                        if (PlayerScene.Phase >= 0.7f)
                        {
                            if (Bag != null && Bag.Exists())
                            {
                                Bag.Delete();
                                Bag = null;
                            }
                            BagManager.ApplyBag(fPlayer.ped, PrevBagType);
                        }
                        if (PlayerScene.IsFinished)
                        {
                            fPlayer.ped.CanSwitchWeapons = true;
                            IsActive(false);
                            fAudio.StopAudioScene("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
                            fStreaming.RemoveAnimDict("anim@heists@ornate_bank@grab_cash");
                            fHud.ClearAdditionalText(0);
                            fHud.ClearAdditionalText(3);
                            Game.Player.SetControlState(true, SetPlayerControlFlags.None);
                            fPed.SetPedCanLegIK(fPlayer.ped, true);
                            fPlayer.ped.SetResetFlag(PedResetFlagToggles.CannotBeTargeted, true);
                            fEntity.SetEntityProofs(fPlayer.ped, false, false, false, false, false, false, false, false);
                            CartScene?.Dispose();
                            CartScene = null;
                            PlayerScene.Dispose();
                            PlayerScene = null;
                            fPlayer.ped.Task.ClearAllImmediately();
                            State = 0;
                        }
                    }
                    break;
            }
        }
        public override void PushResetOnDeath()
        {
            base.PushResetOnDeath();
            if (Bar != null && Bar.Exists())
            {
                Bar.Delete();
                Bar = null;
            }
            CasinoTableCam = false;
            if (Bag != null && Bag.Exists())
            {
                Bag.Delete();
                Bag = null;
            }
            BagManager.ApplyBag(fPlayer.ped, PrevBagType);
            if (PlayerScene != null)
            {
                PlayerScene.Dispose();
            }
            PlayerScene = null;
            if (CartScene != null)
            {
                CartScene.Dispose();
            }
            CartScene = null;
            if (fAudio.IsAudioSceneActive("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE"))
            {
                fAudio.StopAudioScene("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
            }
            fStreaming.RemoveAnimDict("anim@heists@ornate_bank@grab_cash");
            if (fHud.HasAdditionalTextLoaded(0))
            {
                fHud.ClearAdditionalText(0);
            }
            if (fHud.HasAdditionalTextLoaded(3))
            {
                fHud.ClearAdditionalText(3);
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            if (Cart != null && Cart.Exists())
            {
                if (Cart.AttachedBlip != null && Cart.AttachedBlip.Exists())
                {
                    Cart.AttachedBlip.Delete();
                }
                Cart.Delete();
                Cart = null;
            }
            if (Bar != null && Bar.Exists())
            {
                Bar.Delete();
                Bar = null;
            }
            CasinoTableCam = false;
            if (Bag != null && Bag.Exists())
            {
                Bag.Delete();
                Bag = null;
            }
            if (PlayerScene != null)
            {
                PlayerScene.Dispose();
            }
            PlayerScene = null;
            if (CartScene != null)
            {
                CartScene.Dispose();
            }
            CartScene = null;
            if (fAudio.IsAudioSceneActive("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE"))
            {
                fAudio.StopAudioScene("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
            }
            fStreaming.RemoveAnimDict("anim@heists@ornate_bank@grab_cash");
            if (fHud.HasAdditionalTextLoaded(0))
            {
                fHud.ClearAdditionalText(0);
            }
            if (fHud.HasAdditionalTextLoaded(3))
            {
                fHud.ClearAdditionalText(3);
            }
        }
        private bool ControlTriggered = false;
        private SynchronizedScene PlayerScene;
        private SynchronizedScene CartScene;
        private Prop Cart;
        private Prop Bar;
        private Prop Bag;
    }
}
