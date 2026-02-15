using GTA;
using GTA.Math;
using GTA.Native;
using GTA.NaturalMotion;
using GTA.UI;
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
        Cash_a, Cash_b, Cash_c,
        Gold_a, Gold_b, Gold_c,
        LowCash_a, LowCash_b, LowCash_c,
        Diamonds_a, Diamonds_b, Diamonds_c
    }

    /// <summary>
    /// Event args for when value is added in a minigame
    /// </summary>
    public class MinigameValueAddedArgs : EventArgs
    {
        public int Amount { get; }
        public bool IsCash { get; }

        public MinigameValueAddedArgs(int amount, bool isCash)
        {
            Amount = amount;
            IsCash = isCash;
        }
    }

    /// <summary>
    /// Event handler for minigame value added
    /// </summary>
    public delegate void MinigameValueAddedEventHandler(object sender, MinigameValueAddedArgs e);
    public abstract class CashGrabMinigame
    {
        // Constants
        private const float SMOOTH_SPEED = 6f;
        private const float EASY_INCREASE = 0.10f;
        private const float MEDIUM_INCREASE = 0.09f;
        private const float HARD_INCREASE = 0.08f;
        private const float EASY_DECAY = 0.30f;
        private const float MEDIUM_DECAY = 0.375f;
        private const float HARD_DECAY = 0.405f;

        public virtual float MinRate { get; set; } = 0.65f;
        public virtual float MaxRate { get; set; } = 1.5f;
        public int State { get; set; }
        public bool IsLooted { get; set; }
        public bool CasinoTableCam { get; set; }
        public bool UseRemoteCounterSound { get; set; } = true;

        public bool IsMinigameActive
        {
            get => Function.Call<bool>(Hash.IS_MINIGAME_IN_PROGRESS);
            set => Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, value);
        }

        public bool IsScriptActive { get; set; }
        public BagManager.BagVariantTypes PrevBagType { get; set; }
        public WeaponHash PrevWeapon { get; set; }
        public AnimationRateData Data;

        public struct AnimationRateData
        {
            public int DifficultyLevel;
            public float CurrentRate;
            public float TargetRate;
            public float CurrentPhase;
        }

        public void IsActive(bool toggle)
        {
            IsMinigameActive = toggle;
            IsScriptActive = toggle;
        }

        private void UpdateAnimationRate(ref AnimationRateData data, float minRate, float maxRate)
        {
            if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE)) return;

            float deltaTime = Function.Call<float>(Hash.TIMESTEP);
            bool inputPressed = IsGrabInputPressed();

            if (inputPressed)
            {
                data.TargetRate += GetRateIncrease(data.DifficultyLevel);
                data.TargetRate = Clamp(data.TargetRate, minRate, maxRate);
            }
            else if (data.TargetRate > minRate)
            {
                data.TargetRate -= GetRateDecay(data.DifficultyLevel) * deltaTime;
                if (data.TargetRate < minRate)
                    data.TargetRate = minRate;
            }

            data.CurrentRate = SmoothRate(data.CurrentRate, data.TargetRate, deltaTime);
        }

        private static bool IsGrabInputPressed()
        {
            bool isKeyboardMouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
            int control = isKeyboardMouse ? 237 : 201;
            return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, control);
        }

        private static float GetRateIncrease(int difficulty) => difficulty switch
        {
            0 => EASY_INCREASE,
            1 => MEDIUM_INCREASE,
            2 => HARD_INCREASE,
            _ => EASY_INCREASE
        };

        private static float GetRateDecay(int difficulty) => difficulty switch
        {
            0 => EASY_DECAY,
            1 => MEDIUM_DECAY,
            2 => HARD_DECAY,
            _ => EASY_DECAY
        };

        private static float SmoothRate(float current, float target, float deltaTime)
        {
            float step = SMOOTH_SPEED * deltaTime;
            if (Math.Abs(current - target) <= step) return target;
            return current < target ? current + step : current - step;
        }

        private static float Clamp(float value, float min, float max) => value switch
        {
            var v when v < min => min,
            var v when v > max => max,
            _ => value
        };

        public void ResetValues()
        {
            Data = default;
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

            if (fPlayer.ped.IsDead && IsScriptActive)
            {
                PushResetOnDeath();
            }

            UpdateAnimationRate(ref Data, MinRate, MaxRate);
        }

        public virtual void Dispose() => ResetValues();

        public virtual void PushResetOnDeath()
        {
            State = 0;
            IsActive(false);
            fPlayer.ped.CanSwitchWeapons = true;
            Game.Player.SetControlState(true, SetPlayerControlFlags.None);
            fPlayer.ped.Task.ClearAllImmediately();
            BagManager.ApplyBag(fPlayer.ped, PrevBagType);
        }
    }

    public class CartGrab : CashGrabMinigame, IMinigame
    {
        // Constants
        private const float INTERACTION_DISTANCE = 1.75f;
        private const float BAG_APPEAR_PHASE = 0.34f;
        private const float BAG_RESTORE_PHASE = 0.7f;
        private const float GOLD_MAX_RATE = 1.2802f;
        private const string ANIM_DICT = "anim@heists@ornate_bank@grab_cash";
        private const string AUDIO_SCENE = "DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE";

        private static readonly Dictionary<CartType, string> CartModels = new()
        {
            [CartType.Standard] = "hei_prop_hei_cash_trolly_01",
            [CartType.Cocaine] = "imp_prop_impexp_coke_trolly",
            [CartType.Cash_a] = "ch_prop_ch_cash_trolly_01a",
            [CartType.Cash_b] = "ch_prop_ch_cash_trolly_01b",
            [CartType.Cash_c] = "ch_prop_ch_cash_trolly_01c",
            [CartType.Gold_a] = "ch_prop_gold_trolly_01a",
            [CartType.Gold_b] = "ch_prop_gold_trolly_01b",
            [CartType.Gold_c] = "ch_prop_gold_trolly_01c",
            [CartType.LowCash_a] = "ch_prop_cash_low_trolly_01a",
            [CartType.LowCash_b] = "ch_prop_cash_low_trolly_01b",
            [CartType.LowCash_c] = "ch_prop_cash_low_trolly_01c",
            [CartType.Diamonds_a] = "ch_prop_diamond_trolly_01a",
            [CartType.Diamonds_b] = "ch_prop_diamond_trolly_01b",
            [CartType.Diamonds_c] = "ch_prop_diamond_trolly_01c"
        };

        // Static management
        private static readonly List<CartGrab> ActiveCarts = new();

        public static void UpdateAll()
        {
            for (int i = ActiveCarts.Count - 1; i >= 0; i--)
            {
                if (ActiveCarts[i] is not null)
                {
                    ActiveCarts[i].Update();
                }
                else
                {
                    ActiveCarts.RemoveAt(i);
                }
            }
        }

        public static void DisposeAll()
        {
            foreach (var cart in ActiveCarts.ToList())
            {
                cart?.Dispose();
            }
            ActiveCarts.Clear();
        }

        // Instance properties
        public CartType Type { get; init; }
        public Model cartModel { get; private init; }
        public Vector3 position { get; private init; }
        public Vector3 rotation { get; private init; }

        private bool wantsToExit;
        private SynchronizedScene? playerScene;
        private SynchronizedScene? cartScene;
        private Prop? cart;
        private Prop? bar;
        private Prop? bag;
        public event MinigameValueAddedEventHandler ValueAdded;

        public CartGrab(CartType type, Vector3 pos, bool PutOnGround = false, Vector3 rot = default)
        {
            int model = GetModelFromType(type);
            cartModel = new(model);
            Type = type;
            rotation = rot;
            if (PutOnGround)
            {
                World.GetGroundHeight(position, out var height);
                position = new Vector3(pos.X, pos.Y, height);
            }
            else
                position = pos;
            MaxRate = IsGoldType(type) ? GOLD_MAX_RATE : 1.5f;

            // Auto-register
            ActiveCarts.Add(this);
        }

        private int GetModelFromType(CartType type) =>
            CartModels.TryGetValue(type, out var model)
                ? fMisc.joaat(model)
                : fMisc.joaat("hei_prop_hei_cash_trolly_01");

        private static bool IsGoldType(CartType type) =>
            type is CartType.Gold_a or CartType.Gold_b or CartType.Gold_c;

        private int GetBarModel() => Type switch
        {
            CartType.Cocaine => fMisc.joaat("imp_prop_impexp_coke_pile"),
            CartType.Gold_a or CartType.Gold_b or CartType.Gold_c => fMisc.joaat("ch_prop_gold_bar_01a"),
            CartType.Diamonds_a or CartType.Diamonds_b or CartType.Diamonds_c => fMisc.joaat("ch_prop_vault_dimaondbox_01a"),
            _ => fMisc.joaat("hei_prop_heist_cash_pile")
        };

        private void DisplayStartGrabText()
        {
            string helpText = Type switch
            {
                CartType.Cocaine => "MC_GRAB_6",
                CartType.Gold_a or CartType.Gold_b or CartType.Gold_c => "MC_GRAB_2",
                CartType.Diamonds_a or CartType.Diamonds_b or CartType.Diamonds_c => "MC_GRAB_7",
                _ => "MC_GRAB_1"
            };
            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, helpText, true);
        }

        private void DisplayGrabText(bool forcedGrab)
        {
            bool isKeyboardMouse = fGamePad.IsUsingKeyboardAndMouse(2);

            string helpText = (Type, forcedGrab, isKeyboardMouse) switch
            {
                (CartType.Cocaine, true, true) => "MC_GRAB_5_MK_b",
                (CartType.Cocaine, true, false) => "MC_GRAB_5_b",
                (CartType.Cocaine, false, true) => "MC_GRAB_5_MK",
                (CartType.Cocaine, false, false) => "MC_GRAB_5",

                (CartType.Diamonds_a or CartType.Diamonds_b or CartType.Diamonds_c, true, true) => "MC_GRAB_7_MK_b",
                (CartType.Diamonds_a or CartType.Diamonds_b or CartType.Diamonds_c, true, false) => "MC_GRAB_7_TP_b",
                (CartType.Diamonds_a or CartType.Diamonds_b or CartType.Diamonds_c, false, true) => "MC_GRAB_7_MK",
                (CartType.Diamonds_a or CartType.Diamonds_b or CartType.Diamonds_c, false, false) => "MC_GRAB_7_TP",

                (CartType.Gold_a or CartType.Gold_b or CartType.Gold_c, true, true) => "MC_GRAB_4b_b",
                (CartType.Gold_a or CartType.Gold_b or CartType.Gold_c, true, false) => "MC_GRAB_4_b",
                (CartType.Gold_a or CartType.Gold_b or CartType.Gold_c, false, true) => "MC_GRAB_4b",
                (CartType.Gold_a or CartType.Gold_b or CartType.Gold_c, false, false) => "MC_GRAB_4",

                (_, true, true) => "MC_GRAB_3_MK_b",
                (_, true, false) => "MC_GRAB_3_b",
                (_, false, true) => "MC_GRAB_3_MK",
                _ => "MC_GRAB_3"
            };

            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, helpText, true);
        }

        // Scene management
        private void PlayIntroScene()
        {
            playerScene = new(cart!.Position, cart.Rotation);
            playerScene.Create(2000);
            playerScene.PlayPed(fPlayer.ped, ANIM_DICT, "intro", 1.5f, -8f, (SyncedSceneFlags)13, RagdollBlockingFlags.PlayerImpact, 1.5f);
            playerScene.PlayEntity(bag!, ANIM_DICT, "bag_intro", 1000f, -1000f, SyncedSceneFlags.None, 1148846100f);
            cartScene = new(cart);
        }

        private void PlayGrabScene()
        {
            playerScene!.Create(2000);
            playerScene.PlayPed(fPlayer.ped, ANIM_DICT, "grab", 4f, -4f, (SyncedSceneFlags)13, RagdollBlockingFlags.PlayerImpact, 1000f);
            playerScene.PlayEntity(bag!, ANIM_DICT, "bag_grab", 1000f, -1000f, SyncedSceneFlags.None, 1148846100f);
            playerScene.Phase = Data.CurrentPhase;

            cartScene!.Create(2000);
            cartScene.PlayEntity(cart!, ANIM_DICT, "cart_cash_dissapear", 1000f, -4f, SyncedSceneFlags.UseKinematicPhysics, 1148846100f);
            cartScene.Phase = Data.CurrentPhase;
        }

        private void PlayIdleScene()
        {
            playerScene!.Create(2000);
            playerScene.PlayPed(fPlayer.ped, ANIM_DICT, "grab_idle", 2f, -8f, (SyncedSceneFlags)13, RagdollBlockingFlags.PlayerImpact, 1000f, AnimationIKControlFlags.LinkedFacial);
            playerScene.PlayEntity(bag!, ANIM_DICT, "bag_grab_idle", 1000f, -1000f, SyncedSceneFlags.None, 1148846100f);
            playerScene.IsLooping = true;
        }

        private void PlayExitScene()
        {
            CasinoTableCam = false;
            Screen.ClearHelpText();

            if (bar is not null && bar.Exists())
            {
                bar.Delete();
                bar = null;
            }

            playerScene = new(cart!);
            playerScene.Create(2000);
            playerScene.PlayPed(fPlayer.ped, ANIM_DICT, "exit", 1000f, -8f, SyncedSceneFlags.HideWeapon, RagdollBlockingFlags.PlayerImpact, 1000f);
            playerScene.PlayEntity(bag!, ANIM_DICT, "bag_exit", 1000f, -8f, SyncedSceneFlags.None, 1000f);

            if (cartScene is not null && cartScene.IsRunning)
            {
                cartScene.Rate = 0f;
            }

            State = 4;
        }
        public Blip GetBlip()
        {
            if (cart.AttachedBlip is not null)
            {
                return cart.AttachedBlip;
            }
            return null;
        }
        public CartGrab CreateBlip(BlipSprite sprite, BlipColor colour, string name = "", float scale = 1f, bool isShortRange = true)
        {
            if (cart == null || !cart.Exists()) return this;

            var blip = cart.AddBlip();
            blip.Sprite = sprite;
            blip.Scale = scale;
            blip.Color = colour;
            blip.IsShortRange = isShortRange;
            blip.Name = name;

            return this;  // ✅
        }

        public CartGrab ApplyBlipPreset(BlipPreset preset)
        {
            CreateBlip(preset.Sprite, preset.Color, preset.Name, preset.Scale, preset.IsShortRange);
            return this;  // ✅
        }

        public CartGrab CreateWithBlipPreset(BlipPreset preset)
        {
            Create();
            ApplyBlipPreset(preset);
            return this;  // ✅
        }

        public CartGrab CreateWithAutoBlip()
        {
            Create();
            ApplyBlipPreset(GetPresetForType(Type));
            return this;  // ✅
        }

        public CartGrab RemoveBlip()
        {
            if (cart?.AttachedBlip != null && cart.AttachedBlip.Exists())
            {
                cart.AttachedBlip.Delete();
            }
            return this;  // ✅
        }

        private static BlipPreset GetPresetForType(CartType type) => type switch
        {
            CartType.Gold_a or CartType.Gold_b or CartType.Gold_c => BlipPresets.GoldCart,
            CartType.Diamonds_a or CartType.Diamonds_b or CartType.Diamonds_c => BlipPresets.DiamondsCart,
            CartType.Cocaine => BlipPresets.CocaineCart,
            _ => BlipPresets.CashCart
        };

        public void Create()
        {
            if (cart is null)
            {
                IsLooted = false;
                cart = World.CreateProp(cartModel, position, rotation, false, false);
            }
            else
            {
                cart.IsPositionFrozen = true;
                cartScene = new(cart);
                cartScene.Create(2000);
                cartScene.PlayEntity(cart, ANIM_DICT, "cart_cash_dissapear", 1000f, -4f, SyncedSceneFlags.UseKinematicPhysics, 1148846100f);
                cartScene.Rate = 0f;
                cartScene.Phase = Data.CurrentPhase;
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
                case 0: UpdateIdleState(); break;
                case 1: UpdateApproachState(); break;
                case 2: UpdateIntroState(); break;
                case 3: UpdateGrabState(); break;
                case 4: UpdateExitState(); break;
            }
        }

        private void UpdateIdleState()
        {
            Create();
            if (IsLooted) return;

            Vector3 bottomOffset = fEntity.GetOffsetFromEntityInWorldCoords(cart!, 0f, 0f, -0.45f);
            Vector3 topOffset = fEntity.GetOffsetFromEntityInWorldCoords(cart!, 0f, 1.3f, 1.5f);
            Vector3 animStartPos = fEntity.GetAnimInitialOffsetPosition(ANIM_DICT, "intro", cart!.Position.X, cart.Position.Y, cart.Position.Z, cart.Rotation.X, cart.Rotation.Y, cart.Rotation.Z, 0f, 2);

            fStreaming.RequestAnimDict(ANIM_DICT);
            fHud.RequestAdditionalText("MC_PLAY", 0);
            fHud.RequestAdditionalText("HACK", 3);

            if (!fStreaming.HasAnimDictLoaded(ANIM_DICT) || !fHud.HasThisAdditionalTextLoaded("MC_PLAY", 0) || !fHud.HasAdditionalTextLoaded(3))
                return;

            if (fCutscene.IsCutscenePlaying() || !Game.Player.CanControlCharacter)
                return;

            bool inInteractionArea = fEntity.IsEntityInAngledArea(fPlayer.ped, bottomOffset, topOffset, INTERACTION_DISTANCE, false, true, 1)
                                     || fPlayer.ped.Position.DistanceTo(animStartPos) < 1f;

            if (!inInteractionArea || !CanPlayerInteract()) return;

            DisplayStartGrabText();

            if (Game.IsControlJustPressed(Control.Context))
            {
                StartGrabMinigame(animStartPos);
            }
        }

        private static bool CanPlayerInteract() =>
            !fPlayer.ped.IsInVehicle()
            && fPlayer.ped.IsAlive
            && !fPlayer.ped.IsPerformingMeleeAction
            && !fPlayer.ped.IsJumping
            && !fPlayer.ped.IsSprinting
            && !fPlayer.ped.IsRagdoll
            && !fPlayer.ped.IsGettingUp;

        private void StartGrabMinigame(Vector3 targetPosition)
        {
            Screen.ClearHelpText();
            IsActive(true);
            Game.Player.SetControlState(false, SetPlayerControlFlags.LeaveCameraControlOn);
            fAudio.StartAudioScene(AUDIO_SCENE);

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

            bar = World.CreateProp(GetBarModel(), fPlayer.ped.Position, false, false);
            if (bar is not null && bar.Exists())
            {
                bar.IsCollisionEnabled = false;
                bar.IsVisible = false;
                bar.IsInvincible = true;
                bar.AttachTo(fPlayer.ped.Bones[Bone.PHLeftHand]);
            }

            bag = BagManager.CreateBagPropFromPed(fPlayer.ped);
            if (bag is not null && bag.Exists())
            {
                bag.IsCollisionEnabled = false;
                bag.IsVisible = false;
                bag.IsInvincible = true;
            }

            CasinoTableCam = true;
            fPed.SetMovementModeOverride(fPlayer.ped, "DEFAULT_ACTION");
            float heading = fEntity.GetAnimInitialOffsetPosition(ANIM_DICT, "intro", cart!.Position.X, cart.Position.Y, cart.Position.Z, cart.Rotation.X, cart.Rotation.Y, cart.Rotation.Z, 0f, 2).Y;
            fPlayer.ped.Task.GoStraightTo(targetPosition, 15000, heading, 0.75f);
            fPlayer.ped.KeepTaskWhenMarkedAsNoLongerNeeded = true;

            if (cart.AttachedBlip is not null)
                cart.AttachedBlip.Alpha = 0;

            State = 1;
        }

        private void UpdateApproachState()
        {
            if (fPlayer.ped.GetScriptTaskStatus(ScriptTaskNameHash.GoStraightToCoord) == ScriptTaskStatus.Finished)
            {
                PlayIntroScene();
                State = 2;
            }
        }

        private void UpdateIntroState()
        {
            if (bar is not null && bar.Exists() && playerScene!.Phase > BAG_APPEAR_PHASE)
            {
                BagManager.RemoveBag(fPlayer.ped);
                if (fPed.HaveAllStreamingRequestsCompleted(fPlayer.ped))
                    bag!.IsVisible = true;
            }

            if (playerScene!.IsFinished)
            {
                // Initialize rate to minimum for immediate grab
                Data.CurrentRate = MinRate;
                Data.TargetRate = MinRate;
                State = 3;
            }
        }

        private void UpdateGrabState()
        {
            bool isPlayingGrab = fEntity.IsEntityPlayingAnim(fPlayer.ped, ANIM_DICT, "grab", 3);
            bool isCancelPressed = Game.IsControlJustPressed(fGamePad.IsUsingKeyboardAndMouse(2) ? Control.CursorCancel : Control.FrontendCancel);

            if (isPlayingGrab)
            {
                if (isCancelPressed)
                {
                    wantsToExit = true;
                    return;
                }

                UpdateGrabAnimation();
                HandleGrabEvents();
            }
            else
            {
                if (isCancelPressed)
                {
                    PlayExitScene();
                    return;
                }

                if (!fEntity.IsEntityPlayingAnim(fPlayer.ped, ANIM_DICT, "grab_idle", 3))
                {
                    PlayIdleScene();
                }
                else if (Data.CurrentRate > MinRate)
                {
                    PlayGrabScene();
                }
            }
        }

        private void UpdateGrabAnimation()
        {
            if (playerScene!.IsRunning)
                playerScene.Rate = Data.CurrentRate;

            if (cartScene!.IsRunning)
                cartScene.Rate = Data.CurrentRate;

            Data.CurrentPhase = playerScene.Phase;
        }

        private void HandleGrabEvents()
        {
            // Show bar when cash appears
            if (fEntity.HasAnimEventFired(fPlayer.ped, "CASH_APPEAR") && bar != null && bar.Exists() && !bar.IsVisible)
            {
                bar.IsVisible = true;
            }

            // Handle cash release
            if (fEntity.HasAnimEventFired(fPlayer.ped, "RELEASE_CASH_DESTROY"))
            {
                if (bar != null && bar.Exists() && bar.IsVisible)
                {
                    bar.IsVisible = false;
                }

                PlayCashSound();

                // ✅ ADD CASH VALUE HERE!
                int cashAmount = GetRandomCashAmount();
                OnValueAdded(cashAmount, true);

                if (wantsToExit)
                {
                    Data.CurrentRate = MinRate;
                    if (Data.CurrentPhase >= 1f)
                    {
                        Screen.ClearHelpText();
                        RemoveBlip();
                        IsLooted = true;
                    }
                    PlayExitScene();
                    wantsToExit = false;
                    return;
                }

                if (Data.CurrentRate == MinRate)
                {
                    cartScene!.Rate = 0f;
                    PlayIdleScene();
                }
            }

            // Check completion
            if (Data.CurrentPhase >= 1f)
            {
                Screen.ClearHelpText();
                RemoveBlip();
                IsLooted = true;
                PlayExitScene();
            }
        }

        /// <summary>
        /// Get random cash amount based on cart type
        /// </summary>
        private int GetRandomCashAmount()
        {
            int min, max;

            switch (Type)
            {
                case CartType.Diamonds_a:
                case CartType.Diamonds_b:
                case CartType.Diamonds_c:
                    min = 45000;
                    max = 50000;
                    break;

                case CartType.Gold_a:
                case CartType.Gold_b:
                case CartType.Gold_c:
                    min = 30000;
                    max = 40000;
                    break;

                case CartType.Cocaine:
                    min = 25000;
                    max = 30000;
                    break;

                case CartType.Cash_a:
                case CartType.Cash_b:
                case CartType.Cash_c:
                    min = 10000;
                    max = 15000;
                    break;

                case CartType.LowCash_a:
                case CartType.LowCash_b:
                case CartType.LowCash_c:
                    min = 5000;
                    max = 8000;
                    break;

                default:
                    min = 10000;
                    max = 15000;
                    break;
            }

            return Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, min, max);
        }

        /// <summary>
        /// Fire the ValueAdded event
        /// </summary>
        protected virtual void OnValueAdded(int amount, bool isCash)
        {
            ValueAdded?.Invoke(this, new MinigameValueAddedArgs(amount, isCash));
        }

        private void PlayCashSound()
        {
            if (UseRemoteCounterSound)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", false);
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", false);
            }
            else
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "LOCAL_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", false);
            }
        }

        private void UpdateExitState()
        {
            if (playerScene is null) return;

            if (playerScene.Phase >= BAG_RESTORE_PHASE)
            {
                if (bag is not null && bag.Exists())
                {
                    bag.Delete();
                    bag = null;
                }
                BagManager.ApplyBag(fPlayer.ped, PrevBagType);
            }

            if (playerScene.IsFinished)
            {
                CleanupMinigame();
            }
        }

        private void CleanupMinigame()
        {
            if (cart!.AttachedBlip is not null)
                cart.AttachedBlip.Alpha = 255;

            fPlayer.ped.CanSwitchWeapons = true;
            IsActive(false);
            fAudio.StopAudioScene(AUDIO_SCENE);
            fStreaming.RemoveAnimDict(ANIM_DICT);
            fHud.ClearAdditionalText(0);
            fHud.ClearAdditionalText(3);
            Game.Player.SetControlState(true, SetPlayerControlFlags.None);
            fPed.SetPedCanLegIK(fPlayer.ped, true);
            fPlayer.ped.SetResetFlag(PedResetFlagToggles.CannotBeTargeted, true);
            fEntity.SetEntityProofs(fPlayer.ped, false, false, false, false, false, false, false, false);

            cartScene?.Dispose();
            cartScene = null;
            playerScene!.Dispose();
            playerScene = null;
            fPlayer.ped.Task.ClearAllImmediately();

            State = 0;
        }

        public override void PushResetOnDeath()
        {
            base.PushResetOnDeath();
            CleanupProps();
            CleanupScenes();
            CleanupResources();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (cart is not null && cart.Exists())
            {
                cart.AttachedBlip?.Delete();
                cart.Delete();
                cart = null;
            }

            CleanupProps();
            CleanupScenes();
            CleanupResources();

            // Auto-unregister
            ActiveCarts.Remove(this);
        }

        private void CleanupProps()
        {
            CasinoTableCam = false;

            if (bar is not null && bar.Exists())
            {
                bar.Delete();
                bar = null;
            }

            if (bag is not null && bag.Exists())
            {
                bag.Delete();
                bag = null;
            }
        }

        private void CleanupScenes()
        {
            playerScene?.Dispose();
            playerScene = null;
            cartScene?.Dispose();
            cartScene = null;
        }

        private void CleanupResources()
        {
            if (fAudio.IsAudioSceneActive(AUDIO_SCENE))
            {
                fAudio.StopAudioScene(AUDIO_SCENE);
            }

            fStreaming.RemoveAnimDict(ANIM_DICT);

            if (fHud.HasAdditionalTextLoaded(0))
                fHud.ClearAdditionalText(0);

            if (fHud.HasAdditionalTextLoaded(3))
                fHud.ClearAdditionalText(3);
        }
    }
}
