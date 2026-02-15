using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheNorthYanktonHeist.Funcs
{
    [Obsolete("Use ScriptManager instead."), EditorBrowsable(EditorBrowsableState.Never)]
    public class fInGameScripts
    {
        public static void StartAllNeededToStartScripts()
        {
            StartScriptMissionTriggerers();
            StartScriptSideJobs();
        }

        static void StartScriptMissionTriggerers()
        {
            StartScript("mission_triggerer_a", 5050);
            StartScript("mission_triggerer_b", 5050);
            StartScript("mission_triggerer_c", 5050);
            StartScript("mission_triggerer_d", 5050);
        }
        static void StartScriptSideJobs()
        {
            StartScript("controller_taxi", 2050);
            StartScript("controller_towing", 2050);
            StartScript("controller_trafficking", 1424);
        }

        public static void TerminateScriptsExceptMissions()
        {
            TerminateLaunchers();
            TerminateAllPropertyManagement();
            TerminateAllRandomEvents();
            TerminateRaces();
            TerminateSideJobs();
            TerminateAmbientAreas();
            TerminateBaseJumps();
            TerminateMichaelEvents();
            TerminateRampages();
        }
        public static void TerminateScriptsWithMissions()
        {
            TerminateScriptsExceptMissions();
            TerminateMissionTriggerers();
        }


        static void TerminateMissionTriggerers()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "mission_triggerer_a");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "mission_triggerer_b");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "mission_triggerer_c");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "mission_triggerer_d");
        }
        static void TerminateGolf()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_golf");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "golf");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "golf_ai_foursome");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "golf_ai_foursome_putting");
        }
        static void TerminateSideJobs()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "controller_taxi");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "controller_towing");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "controller_trafficking");
        }
        static void TerminateAmbientAreas()
        {
            TerminateShops();
            TerminateStripClub();
            TerminateCinemas();
            TerminateGolf();
            TerminateBlimp();
            TerminateAmusementPark();
            TerminateCableCars();
            TerminateBootyCalls();
        }
        static void TerminateShops()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "shop_controller");
        }
        static void TerminateRaces()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_racing");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_offroadracing");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "controller_races");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "country_race_controller");
        }
        static void TerminateRampages()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_rampage");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "rampage_controller");
        }
        static void TerminateStripClub()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "stripclub");
        }
        static void TerminateCinemas()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "act_cinema");
        }
        static void TerminateBlimp()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "ambientblimp");
        }
        static void TerminateAmusementPark()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "fairgroundhub");
        }
        static void TerminateCableCars()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "cablecar");
        }
        static void TerminateBootyCalls()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "bootycallhandler");
        }
        static void TerminateBaseJumps()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "bj");
        }
        static void TerminateMichaelEvents()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "me_jimmy1");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "me_amanda1");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "me_tracey1");
        }
        static void TerminateLaunchers()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_abigail");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_barry");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_basejumpheli");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_basejumppack");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_carwash");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_darts");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_dreyfuss");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_epsilon");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_extreme");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_fanatic");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_hao");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_hunting");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_hunting_ambient");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_josh");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_maude");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_minute");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_mrsphilips");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_nigel");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_omega");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_paparazzo");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_pilotschool");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_range");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_stunts");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_tennis");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_thelastone");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_tonya");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_triathlon");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "launcher_yoga");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "bailbond_launcher");

            // Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "");
        }
        static void TerminateAllPropertyManagement()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "pm_defend");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "pm_delivery");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "pm_gang_attack");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "pm_plane_promotion");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "pm_recover_stolen");

            //Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "");
        }
        static void TerminateAllRandomEvents()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_yetarian");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_stag_do");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_snatched");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_shoprobbery");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_securityvan");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_seaplane");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_rescuehostage");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prisonvanbreak");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prisonerlift");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_paparazzi");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_muggings");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_mountdance");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_monkey");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_lured");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_lossantosintl");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_homeland_security");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_hitch_lift");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_getaway_driver");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_gangfight");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_gang_intimidation");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_duel");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_drunkdriver");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_domestic");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_dealgonewrong");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_cultshootout");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_crashrescue");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_chasethieves");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_cartheft");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_bus_tours");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_burials");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_border");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_bikethief");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_atmrobbery");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_arrests");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_armybase");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_accident");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_abandonedcar");

            //Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "");
        }

        public static void StartScript(string scriptName, int buffer)
        {
            Function.Call(Hash.REQUEST_SCRIPT, scriptName);
            while (!Function.Call<bool>(Hash.HAS_SCRIPT_LOADED, scriptName))
            {
                Function.Call(Hash.REQUEST_SCRIPT, scriptName);
                Script.Yield();
            }
            Function.Call(Hash.START_NEW_SCRIPT, scriptName, buffer);
            Function.Call(Hash.SET_SCRIPT_AS_NO_LONGER_NEEDED, scriptName);
        }

        void RequestScript(string scriptName)
        {
            Function.Call(Hash.REQUEST_SCRIPT, scriptName);
            while (!Function.Call<bool>(Hash.HAS_SCRIPT_LOADED, scriptName))
            {
                Function.Call(Hash.REQUEST_SCRIPT, scriptName);
                Script.Yield();
            }
            Function.Call(Hash.SET_SCRIPT_AS_NO_LONGER_NEEDED, scriptName);
        }
    }
}
