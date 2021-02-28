/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID MX_PLAY_FINALE = 4065079687U;
        static const AkUniqueID MX_PLAY_MAIN_MENU = 890084909U;
        static const AkUniqueID MX_SETSTATE = 1128714786U;
        static const AkUniqueID PLAY_BABY_CRYING = 753280365U;
        static const AkUniqueID PLAY_BOTTLE_SMASH = 1131775195U;
        static const AkUniqueID PLAY_DOOR_CLOSE = 2292458263U;
        static const AkUniqueID PLAY_DOOR_OPEN = 1660008929U;
        static const AkUniqueID PLAY_FLIRT = 1692061147U;
        static const AkUniqueID PLAY_FOOTSTEPS = 3854155799U;
        static const AkUniqueID PLAY_FRYING_PAN_KNOCKOUT = 3561477708U;
        static const AkUniqueID PLAY_FRYING_PAN_SWING = 4261233702U;
        static const AkUniqueID PLAY_ITEM_PICKUP = 2652605998U;
        static const AkUniqueID PLAY_ITEM_PUTDOWN = 4250118861U;
        static const AkUniqueID PLAY_ITEM_THROW = 3065584282U;
        static const AkUniqueID PLAY_ITEM_THROW_CHARGE = 3278353889U;
        static const AkUniqueID PLAY_PICKPOCKET_KEYS = 1684266438U;
        static const AkUniqueID PLAY_UI_CHARACTER_SWITCH = 3663550651U;
        static const AkUniqueID PLAY_UI_CLICK_DECISION = 2581004938U;
        static const AkUniqueID PLAY_UI_CLICK_REGULAR = 1319291932U;
        static const AkUniqueID PLAY_UI_HOVER = 1339559671U;
        static const AkUniqueID PLAY_UI_MAIN_START_BUTTON = 703069780U;
        static const AkUniqueID PLAY_VASE_SMASH = 2250361604U;
        static const AkUniqueID PLAY_WINDOW_SMASH = 3352654671U;
        static const AkUniqueID START_AREA_AMB = 4278971852U;
        static const AkUniqueID START_BBQ_AMB = 2771104862U;
        static const AkUniqueID START_FOREST_AMBIENCE = 3194767864U;
        static const AkUniqueID START_FOUNTAIN_AMB = 3248596991U;
        static const AkUniqueID START_LEAVES_AMB = 3322761575U;
        static const AkUniqueID START_WASHING_MACHINE_AMB = 3630086782U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace CURRENT_CHARACTER
        {
            static const AkUniqueID GROUP = 1176230744U;

            namespace STATE
            {
                static const AkUniqueID BABY = 1543097833U;
                static const AkUniqueID DAD = 311764516U;
                static const AkUniqueID MOM = 1082004790U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace CURRENT_CHARACTER

        namespace GAME_STATES
        {
            static const AkUniqueID GROUP = 2721494480U;

            namespace STATE
            {
                static const AkUniqueID ALERT = 721787521U;
                static const AkUniqueID ENTERED_HOUSE = 2563422499U;
                static const AkUniqueID IDLE = 1874288895U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SUSPICIOUS = 3270337040U;
            } // namespace STATE
        } // namespace GAME_STATES

    } // namespace STATES

    namespace SWITCHES
    {
        namespace CHARACTER_SELECT
        {
            static const AkUniqueID GROUP = 3311442969U;

            namespace SWITCH
            {
                static const AkUniqueID BABY = 1543097833U;
                static const AkUniqueID DAD = 311764516U;
                static const AkUniqueID MOM = 1082004790U;
            } // namespace SWITCH
        } // namespace CHARACTER_SELECT

        namespace SURFACE_TYPE
        {
            static const AkUniqueID GROUP = 4064446173U;

            namespace SWITCH
            {
                static const AkUniqueID CONCRETE = 841620460U;
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID INTERIOR = 1132214669U;
            } // namespace SWITCH
        } // namespace SURFACE_TYPE

    } // namespace SWITCHES

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SFX_MAIN = 3023356346U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MASTER_MUSIC_BUS = 48433064U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
