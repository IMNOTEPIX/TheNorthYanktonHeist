using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist
{
	/*
	
					if (OBJECT::DOES_RAYFIRE_MAP_OBJECT_EXIST(iLocal_1232))
					{
						if (OBJECT::GET_STATE_OF_RAYFIRE_MAP_OBJECT(iLocal_1232) != 5)
						{
							OBJECT::SET_STATE_OF_RAYFIRE_MAP_OBJECT(iLocal_1232, 4);
						}
					}
					if (func_689(128))
					{
						func_487(PLAYER::PLAYER_ID(), 0, 0);
						func_812(3);
						HUD::CLEAR_PRINTS();
						func_729(1);
						if (AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_THREATEN_HOSTAGES"))
						{
							AUDIO::STOP_AUDIO_SCENE("PROLOGUE_THREATEN_HOSTAGES");
						}
						if (!AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_DETONATE_CHARGES"))
						{
							AUDIO::START_AUDIO_SCENE("PROLOGUE_DETONATE_CHARGES");
						}
						if (!AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_MUTE_SPRINKLERS"))
						{
							AUDIO::START_AUDIO_SCENE("PROLOGUE_MUTE_SPRINKLERS");
						}
						if (!func_761(297))
						{
							AUDIO::ACTIVATE_AUDIO_SLOWMO_MODE("SLOWMO_PROLOGUE_VAULT");
							func_760(297, 1);
						}
						RECORDING::REPLAY_RECORD_BACK_FOR_TIME(1.5f, 1f, 3);
						func_473();
					}
				}
				if (!PED::IS_SYNCHRONIZED_SCENE_RUNNING(iLocal_601))
				{
					if (!ENTITY::IS_ENTITY_AT_COORD(iLocal_657, 5313.7812f, -5205.144f, 83.5237f, 0.5f, 0.5f, 2f, false, true, 0))
					{
						if (TASK::GET_SCRIPT_TASK_STATUS(iLocal_657, joaat("SCRIPT_TASK_FOLLOW_NAV_MESH_TO_COORD")) != 1)
						{
							TASK::TASK_FOLLOW_NAV_MESH_TO_COORD(iLocal_657, 5313.7812f, -5205.144f, 83.5237f, 1f, 20000, 0.25f, 0, 114.0175f);
						}
					}
				}
				break;
			
			case 1:
				AUDIO::LOAD_STREAM("PROLOGUE_BLOW_THE_VAULT_MASTER", 0);
				AUDIO::PREPARE_ALARM("PROLOGUE_VAULT_ALARMS");
				if (SYSTEM::TIMERA() > 2500)
				{
					func_9(&(iLocal_659[2]));
					func_9(&(iLocal_659[0]));
					func_482(&uLocal_1747, 7);
					func_482(&uLocal_1747, 8);
					func_9(&(iLocal_659[1]));
					func_9(&(iLocal_659[3]));
					func_628(1);
					func_6();
					iLocal_87 = 0;
					HUD::DISPLAY_RADAR(false);
					HUD::DISPLAY_HUD(false);
					func_451(0, 0);
					MISC::CLEAR_AREA(5296.97f, -5188.88f, 82.74f, 10f, true, false, false, false);
					CAM::SET_CAM_PARAMS(iLocal_1156, 5297.292f, -5187.3296f, 83.824295f, 6.358143f, -8.767557f, -122.514175f, 28.3404f, 0, 3, 3, 2);
					CAM::SET_CAM_PARAMS(iLocal_1156, 5297.325f, -5187.351f, 83.82872f, 6.358143f, -8.767557f, -122.514175f, 28.3404f, 1800, 3, 3, 2);
					CAM::SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE(iLocal_1156, 1.1f);
					CAM::SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL(iLocal_1156, 1f);
					CAM::SET_CAM_DOF_FNUMBER_OF_LENS(iLocal_1156, 2.8f);
					CAM::RENDER_SCRIPT_CAMS(true, false, 3000, true, false, 0);
					func_452(1, 1, 1, 0, 0, 0, 0);
					if (!iLocal_96)
					{
						AUDIO::SET_AUDIO_FLAG("DisableReplayScriptStreamRecording", true);
						iLocal_96 = 1;
					}
					AUDIO::PLAY_STREAM_FRONTEND();
					GRAPHICS::SET_TIMECYCLE_MODIFIER("cashdepot");
					func_451(0, 0);
					INTERIOR::SET_ROOM_FOR_GAME_VIEWPORT_BY_NAME("V_CashD_vault");
					func_483(iLocal_653, 5308.671f, -5206.5474f, 82.5186f, 269.1302f, 1);
					if (!WEAPON::HAS_PED_GOT_WEAPON(iLocal_653, iLocal_1226, false))
					{
						WEAPON::GIVE_WEAPON_TO_PED(iLocal_653, iLocal_1226, 500, true, true);
					}
					WEAPON::SET_CURRENT_PED_WEAPON(iLocal_653, iLocal_1226, true);
					if (!iLocal_84)
					{
						RECORDING::REPLAY_START_EVENT(3);
						iLocal_84 = 1;
					}
					if (!ENTITY::DOES_ENTITY_EXIST(iLocal_1195[2]))
					{
						iLocal_1195[2] = OBJECT::CREATE_OBJECT(joaat("prop_vault_door_scene"), 5297.717f, -5188.909f, 81.575f, true, true, false);
						INTERIOR::FORCE_ROOM_FOR_ENTITY(iLocal_1195[2], iLocal_1161, joaat("V_CashD_side"));
					}
					func_473();
				}
				break;
			
			case 2:
				AUDIO::PREPARE_ALARM("PROLOGUE_VAULT_ALARMS");
				if (ENTITY::IS_ENTITY_VISIBLE(iLocal_1174) && SYSTEM::TIMERA() > 500)
				{
					ENTITY::SET_ENTITY_VISIBLE(iLocal_1174, false, false);
					ENTITY::SET_ENTITY_VISIBLE(iLocal_1175, true, false);
				}
				if (ENTITY::IS_ENTITY_VISIBLE(iLocal_1175) && SYSTEM::TIMERA() > 1500)
				{
					ENTITY::SET_ENTITY_VISIBLE(iLocal_1175, false, false);
				}
				if (SYSTEM::TIMERA() > 1000)
				{
					ENTITY::SET_ENTITY_VISIBLE(iLocal_1171, false, false);
					ENTITY::SET_ENTITY_VISIBLE(iLocal_1172, true, false);
				}
				if (SYSTEM::TIMERA() > 1800)
				{
					ENTITY::SET_ENTITY_VISIBLE(iLocal_1171, false, false);
					ENTITY::SET_ENTITY_VISIBLE(iLocal_1172, true, false);
					if (OBJECT::DOES_RAYFIRE_MAP_OBJECT_EXIST(iLocal_1232))
					{
						if (OBJECT::GET_STATE_OF_RAYFIRE_MAP_OBJECT(iLocal_1232) == 5)
						{
							OBJECT::SET_STATE_OF_RAYFIRE_MAP_OBJECT(iLocal_1232, 6);
						}
					}
					if (!AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_VAULT_RAYFIRE"))
					{
						AUDIO::START_AUDIO_SCENE("PROLOGUE_VAULT_RAYFIRE");
					}
					if (AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_DETONATE_CHARGES"))
					{
						AUDIO::STOP_AUDIO_SCENE("PROLOGUE_DETONATE_CHARGES");
					}
					if (CAM::DOES_CAM_EXIST(iLocal_1156))
					{
						CAM::SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE(iLocal_1156, 0f);
						CAM::SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL(iLocal_1156, 0f);
						CAM::SET_CAM_DOF_FNUMBER_OF_LENS(iLocal_1156, 2.8f);
						CAM::DESTROY_CAM(iLocal_1156, false);
					}
					if (!CAM::DOES_CAM_EXIST(iLocal_1156))
					{
						iLocal_1156 = CAM::CREATE_CAM("DEFAULT_SCRIPTED_CAMERA", true);
					}
					CAM::SET_CAM_PARAMS(iLocal_1156, 5292.704f, -5185.751f, 82.84772f, 9.034329f, -2.898424f, -131.66974f, 45f, 0, 0, 0, 2);
					func_449(&(iLocal_1195[2]));
					func_473();
				}
				break;
			
			case 3:
				AUDIO::PREPARE_ALARM("PROLOGUE_VAULT_ALARMS");
				if (!func_761(282))
				{
					if (SYSTEM::TIMERA() > 300)
					{
						GRAPHICS::START_PARTICLE_FX_NON_LOOPED_AT_COORD("ent_ray_pro1_vault_exp_lit", 5298.2007f, -5189.052f, 83.86238f, Local_97, 1f, false, false, false);
						CAM::SHAKE_CAM(iLocal_1156, "GRENADE_EXPLOSION_SHAKE", 3f);
						PAD::SET_CONTROL_SHAKE(0 /*PLAYER_CONTROL*//*, 500, 256);
						if (STREAMING::HAS_PTFX_ASSET_LOADED())
						{
							if (!GRAPHICS::DOES_PARTICLE_FX_LOOPED_EXIST(iLocal_1200))
							{
								iLocal_1200 = GRAPHICS::START_PARTICLE_FX_LOOPED_AT_COORD("scr_prologue_vault_haze", 5299f, -5189f, 82.6f, Local_97, 1f, false, false, false, false);
							}
if (!GRAPHICS::DOES_PARTICLE_FX_LOOPED_EXIST(iLocal_1201))
{
    iLocal_1201 = GRAPHICS::START_PARTICLE_FX_LOOPED_AT_COORD("scr_prologue_vault_fog", 5299f, -5189f, 82.6f, Local_97, 1f, false, false, false, false);
}
						}
						if (AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_MUTE_SPRINKLERS"))
{
    AUDIO::STOP_AUDIO_SCENE("PROLOGUE_MUTE_SPRINKLERS");
}
MISC::SET_TIME_SCALE(0.5f);
func_760(282, 1);
					}
				}
				if (SYSTEM::TIMERA() > 400)
{
    func_812(4);
    AUDIO::START_ALARM("PROLOGUE_VAULT_ALARMS", false);
    func_449(&iLocal_1171);
    func_449(&iLocal_1172);
    func_449(&iLocal_1173);
    func_449(&iLocal_1174);
    func_449(&iLocal_1175);
    func_473();
}
break;


            case 4:
    if (SYSTEM::TIMERA() > 250)
    {
        func_473();
    }
    break;

case 5:
    if (SYSTEM::TIMERA() > 450)
    {
        MISC::SET_TIME_SCALE(1f);
        if (AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_VAULT_RAYFIRE"))
        {
            AUDIO::STOP_AUDIO_SCENE("PROLOGUE_VAULT_RAYFIRE");
        }
        CAM::STOP_CAM_SHAKING(iLocal_1156, false);
        GRAPHICS::CLEAR_TIMECYCLE_MODIFIER();
        func_423(216, "PRO_Safe", "PRO_Safe_1", 8, 1, 0, 0);
        if (!CAM::DOES_CAM_EXIST(iLocal_1158))
        {
            iLocal_1158 = CAM::CREATE_CAMERA(joaat("DEFAULT_ANIMATED_CAMERA"), true);
        }
        iLocal_608 = PED::CREATE_SYNCHRONIZED_SCENE(Local_609, Local_612, 2);
        iLocal_615 = PED::CREATE_SYNCHRONIZED_SCENE(Local_616, Local_619, 2);
        func_483(iLocal_653, Vector(82.5187f, -5207.1147f, 5307.475f) + Vector(0f, 0.32f, -0.08f), 272.3664f, 1);
        TASK::TASK_PLAY_ANIM(iLocal_653, sLocal_568, "react_to_explosion_player_zero", 1000f, -8f, -1, 0, (0.075f + 0.05f), false, false, false);
        PED::FORCE_PED_AI_AND_ANIMATION_UPDATE(iLocal_653, false, false);
        TASK::CLEAR_PED_TASKS_IMMEDIATELY(uLocal_1669[2]);
        TASK::TASK_SYNCHRONIZED_SCENE(uLocal_1669[2], iLocal_608, sLocal_568, "react_to_explosion_player_two", 1000f, -1000f, 4, 0, 1000f, 0);
        TASK::TASK_SYNCHRONIZED_SCENE(iLocal_657, iLocal_608, sLocal_568, "react_to_explosion_brad", 1000f, -1000f, 4, 0, 1000f, 0);
        CAM::PLAY_SYNCHRONIZED_CAM_ANIM(iLocal_1158, iLocal_615, "react_to_explosion_cam", sLocal_569);
        PED::SET_SYNCHRONIZED_SCENE_PHASE(iLocal_608, (0.075f + 0.05f));
        PED::SET_SYNCHRONIZED_SCENE_PHASE(iLocal_615, 0.421f);
        CAM::SET_CAM_ACTIVE(iLocal_1158, true);
        func_451(0, 0);
        INTERIOR::SET_ROOM_FOR_GAME_VIEWPORT_BY_NAME("V_CashD_reception");
        func_473();
    }
    break;

case 6:
    if (STREAMING::HAS_PTFX_ASSET_LOADED())
    {
        GRAPHICS::START_PARTICLE_FX_NON_LOOPED_AT_COORD("scr_prologue_ceiling_debris", 5310.245f, -5205.663f, 85.2259f, 0f, 0f, 0f, 1f, false, false, false);
    }
    func_473();
    break;

case 7:
    if (PED::GET_SYNCHRONIZED_SCENE_PHASE(iLocal_608) >= (0.085f + 0.05f))
    {
        if (STREAMING::HAS_PTFX_ASSET_LOADED())
        {
            GRAPHICS::START_PARTICLE_FX_NON_LOOPED_AT_COORD("scr_prologue_ceiling_debris", 5309.8f, -5207.6f, 85.40824f, 0f, 0f, 90f, 1f, false, false, false);
        }
        func_473();
    }
    break;

case 8:
    if (PED::GET_SYNCHRONIZED_SCENE_PHASE(iLocal_608) >= (0.087f + 0.05f))
    {
        if (STREAMING::HAS_PTFX_ASSET_LOADED())
        {
            GRAPHICS::START_PARTICLE_FX_NON_LOOPED_AT_COORD("scr_prologue_ceiling_debris", 5313.9927f, -5207.3f, 85.34588f, 0f, 0f, 180f, 1f, false, false, false);
        }
        func_473();
    }
    break;

case 9:
    if (PED::GET_SYNCHRONIZED_SCENE_PHASE(iLocal_608) >= (0.35f + 0.05f))
    {
        CAM::STOP_CAM_SHAKING(iLocal_1156, true);
        if (!CAM::DOES_CAM_EXIST(iLocal_1157))
        {
            iLocal_1157 = CAM::CREATE_CAMERA(joaat("DEFAULT_SCRIPTED_CAMERA"), true);
        }
        CAM::SET_CAM_PARAMS(iLocal_1157, 5295.859f, -5188.994f, 82.99249f, 3.961173f, -0.003078f, -90.428894f, 35.788742f, 0, 0, 0, 2);
        CAM::SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE(iLocal_1157, 8f);
        CAM::SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL(iLocal_1157, 1f);
        CAM::SET_CAM_DOF_FNUMBER_OF_LENS(iLocal_1157, 1f);
        CAM::SET_CAM_DOF_MAX_NEAR_IN_FOCUS_DISTANCE_BLEND_LEVEL(iLocal_1157, 0f);
        CAM::SHAKE_CAM(iLocal_1157, "HAND_SHAKE", 0.5f);
        CAM::SET_CAM_PARAMS(iLocal_1156, 5296.3735f, -5188.994f, 83.0277f, 3.408814f, -0.003078f, -91.27811f, 35.788742f, 0, 0, 0, 2);
        if (CAM::DOES_CAM_EXIST(iLocal_1158))
        {
            CAM::DESTROY_CAM(iLocal_1158, false);
        }
        CAM::SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE(iLocal_1156, 8f);
        CAM::SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL(iLocal_1156, 1f);
        CAM::SET_CAM_DOF_FNUMBER_OF_LENS(iLocal_1156, 1.3f);
        CAM::SET_CAM_DOF_MAX_NEAR_IN_FOCUS_DISTANCE_BLEND_LEVEL(iLocal_1156, 0f);
        CAM::SHAKE_CAM(iLocal_1156, "HAND_SHAKE", 0.5f);
        CAM::SET_CAM_ACTIVE_WITH_INTERP(iLocal_1156, iLocal_1157, 3000, 0, 0);
        TASK::CLEAR_PED_TASKS(iLocal_653);
        TASK::CLEAR_PED_TASKS(uLocal_1669[2]);
        TASK::CLEAR_PED_TASKS(iLocal_657);
        func_451(0, 0);
        INTERIOR::SET_ROOM_FOR_GAME_VIEWPORT_BY_NAME("V_CashD_vault");
        iLocal_1183 = OBJECT::CREATE_OBJECT_NO_OFFSET(joaat("v_ilev_cd_dust"), 5312.14f, -5209.04f, 83.02f, true, true, false, 0);
        ENTITY::FREEZE_ENTITY_POSITION(iLocal_1183, true);
        STREAMING::SET_MODEL_AS_NO_LONGER_NEEDED(joaat("v_ilev_cd_dust"));
        if (STREAMING::HAS_PTFX_ASSET_LOADED())
        {
            GRAPHICS::START_PARTICLE_FX_NON_LOOPED_AT_COORD("scr_prologue_ceiling_debris", 5298.206f, -5189.0635f, 85.281166f, 0f, 0f, 180f, 1f, false, false, false);
        }
        func_473();
    }
    break;

case 10:
    if (SYSTEM::TIMERA() > 3000)
    {
        TASK::CLEAR_PED_TASKS_IMMEDIATELY(uLocal_1669[2]);
        func_483(uLocal_1669[2], 5310.6885f, -5204.9897f, 82.5199f, (355.824f + 90f), 1);
        TASK::TASK_PUT_PED_DIRECTLY_INTO_COVER(uLocal_1669[2], 5310.6885f, -5204.9897f, 82.5199f, -1, false, 0f, true, true, iLocal_1206[0], false);
        func_483(iLocal_653, 5308.856f, -5206.294f, (85.7187f - 3.2f), 355.824f, 1);
        PED::FORCE_PED_MOTION_STATE(iLocal_653, joaat("MotionState_ActionMode_Idle"), false, 0, false);
        iLocal_87 = 1;
        HUD::DISPLAY_RADAR(true);
        HUD::DISPLAY_HUD(true);
        func_451(0, 0);
        if (iLocal_84)
        {
            RECORDING::REPLAY_STOP_EVENT();
            iLocal_84 = 0;
        }
        if (CAM::DOES_CAM_EXIST(iLocal_1156))
        {
            CAM::SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE(iLocal_1156, 0f);
            CAM::SET_CAM_DOF_OVERRIDDEN_FOCUS_DISTANCE_BLEND_LEVEL(iLocal_1156, 0f);
            CAM::SET_CAM_DOF_FNUMBER_OF_LENS(iLocal_1156, 2.8f);
            CAM::DESTROY_CAM(iLocal_1156, false);
        }
        if (!CAM::DOES_CAM_EXIST(iLocal_1156))
        {
            iLocal_1156 = CAM::CREATE_CAM("DEFAULT_SCRIPTED_CAMERA", true);
        }
        CAM::RENDER_SCRIPT_CAMS(false, false, 3000, true, false, 0);
        func_452(0, 1, 1, 0, 0, 0, 0);
        CAM::SET_GAMEPLAY_CAM_RELATIVE_HEADING(0f);
        if (CAM::GET_CAM_VIEW_MODE_FOR_CONTEXT(0) == 4)
        {
            CAM::SET_GAMEPLAY_CAM_RELATIVE_PITCH(-5f, 1f);
        }
        else
        {
            CAM::SET_GAMEPLAY_CAM_RELATIVE_PITCH(-30f, 1f);
        }
        func_686(128);
        func_487(PLAYER::PLAYER_ID(), 1, 0);
        if (CAM::IS_SCREEN_FADED_OUT())
        {
            CAM::DO_SCREEN_FADE_IN(800);
        }
        func_441();
    }
    break;
}
if (iLocal_27 > 1 && iLocal_27 < 10)
{
}
if (ENTITY::IS_ENTITY_IN_ANGLED_AREA(iLocal_653, 5311.4976f, -5204.6772f, (85.71863f - 3.2f), 5311.415f, -5220.3394f, (88.71863f - 3.2f), 12f, false, true, 0) && (ENTITY::IS_ENTITY_IN_ANGLED_AREA(uLocal_1669[2], 5311.4976f, -5204.6772f, (85.71863f - 3.2f), 5311.415f, -5220.3394f, (88.71863f - 3.2f), 12f, false, true, 0) && !ENTITY::IS_ENTITY_AT_COORD(uLocal_1669[2], 5308.8022f, -5204.5786f, 84.01863f, 1f, 1f, 1.5f, false, true, 0)))
{
    func_760(37, 1);
}
else
{
    func_758(1, joaat("v_ilev_cd_door"));
    func_758(2, joaat("v_ilev_cd_door"));
}
	}
	if (func_422())
{
    CAM::SET_CAM_ACTIVE(iLocal_1156, true);
    if (CAM::DOES_CAM_EXIST(iLocal_1158))
    {
        CAM::DESTROY_CAM(iLocal_1158, false);
    }
    if (CAM::DOES_CAM_EXIST(iLocal_1157))
    {
        CAM::DESTROY_CAM(iLocal_1157, false);
    }
    GRAPHICS::CLEAR_TIMECYCLE_MODIFIER();
    func_449(&(iLocal_1195[2]));
    func_9(&(iLocal_659[2]));
    func_9(&(iLocal_659[0]));
    func_482(&uLocal_1747, 8);
    func_9(&(iLocal_659[1]));
    func_9(&(iLocal_659[3]));
    func_449(&iLocal_1171);
    func_449(&iLocal_1172);
    func_449(&iLocal_1173);
    func_449(&iLocal_1174);
    func_449(&iLocal_1175);
    if (!ENTITY::DOES_ENTITY_EXIST(iLocal_1183))
    {
        iLocal_1183 = OBJECT::CREATE_OBJECT_NO_OFFSET(joaat("v_ilev_cd_dust"), 5312.14f, -5209.04f, 83.02f, true, true, false, 0);
        ENTITY::FREEZE_ENTITY_POSITION(iLocal_1183, true);
        STREAMING::SET_MODEL_AS_NO_LONGER_NEEDED(joaat("v_ilev_cd_dust"));
    }
    func_812(4);
    if (iLocal_1235 != -1)
    {
        AUDIO::STOP_SOUND(iLocal_1235);
        AUDIO::RELEASE_SOUND_ID(iLocal_1235);
        iLocal_1235 = -1;
    }
    if (func_761(297))
    {
        AUDIO::DEACTIVATE_AUDIO_SLOWMO_MODE("SLOWMO_PROLOGUE_VAULT");
    }
    if (AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_THREATEN_HOSTAGES"))
    {
        AUDIO::STOP_AUDIO_SCENE("PROLOGUE_THREATEN_HOSTAGES");
    }
    if (AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_VAULT_RAYFIRE"))
    {
        AUDIO::STOP_AUDIO_SCENE("PROLOGUE_VAULT_RAYFIRE");
    }
    if (AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_MUTE_SPRINKLERS"))
    {
        AUDIO::STOP_AUDIO_SCENE("PROLOGUE_MUTE_SPRINKLERS");
    }
    if (AUDIO::IS_AUDIO_SCENE_ACTIVE("PROLOGUE_DETONATE_CHARGES"))
    {
        AUDIO::STOP_AUDIO_SCENE("PROLOGUE_DETONATE_CHARGES");
    }
    func_486(&iLocal_1097);
    func_568(&iLocal_1092, &(uLocal_1669[2]), 0);
    func_568(&iLocal_1093, &iLocal_657, 0);
    func_729(1);
    func_686(128);
    MISC::SET_TIME_SCALE(1f);
    func_628(1);
    if (CAM::DOES_CAM_EXIST(iLocal_1156))
    {
        if (CAM::IS_CAM_RENDERING(iLocal_1156))
        {
            CAM::RENDER_SCRIPT_CAMS(false, false, 3000, true, false, 0);
            func_452(0, 1, 1, 0, 0, 0, 0);
            CAM::SET_GAMEPLAY_CAM_RELATIVE_HEADING(0f);
            CAM::SET_GAMEPLAY_CAM_RELATIVE_PITCH(-10f, 1f);
        }
    }
    func_487(PLAYER::PLAYER_ID(), 1, 0);
    func_627(1);
    TASK::CLEAR_PED_TASKS(iLocal_653);
    WEAPON::SET_CURRENT_PED_WEAPON(iLocal_653, iLocal_1226, true);
    if (OBJECT::DOES_RAYFIRE_MAP_OBJECT_EXIST(iLocal_1232))
    {
        if (OBJECT::GET_STATE_OF_RAYFIRE_MAP_OBJECT(iLocal_1232) != 10)
        {
            OBJECT::SET_STATE_OF_RAYFIRE_MAP_OBJECT(iLocal_1232, 9);
        }
    }
    func_758(1, joaat("v_ilev_cd_door"));
    func_758(2, joaat("v_ilev_cd_door"));
    iLocal_1251++;
}
}
	 */
    /*
    
    Todo list:


















    			Local_301.f_2 = 9;
			Local_301.f_3 = { Local_76 };
			Local_301.f_6 = joaat("freight");
			Local_301.f_7 = joaat("freightcar");
			Local_301.f_8 = joaat("freightgrain");
			Local_301.f_9 = joaat("S_M_M_Trucker_01");





    			Local_301.f_2 = 23;
			Local_301.f_3 = { 3154.38f, -4698.16f, 111.63f };
			Local_301.f_6 = joaat("freight");
			Local_301.f_7 = joaat("freightcar");
			Local_301.f_8 = joaat("freightcont1");
			Local_301.f_9 = joaat("S_M_M_Trucker_01");



							AUDIO::SET_VEH_RADIO_STATION(Local_167.f_0, "OFF");
						AUDIO::SET_VEHICLE_RADIO_ENABLED(Local_167.f_0, false);





    */
}
