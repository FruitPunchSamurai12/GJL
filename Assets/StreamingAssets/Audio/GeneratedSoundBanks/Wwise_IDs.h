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
        static const AkUniqueID MX_PLAY_BASE_BABY = 2749816896U;
        static const AkUniqueID MX_PLAY_BASE_DAD = 3691357135U;
        static const AkUniqueID MX_PLAY_BASE_MOM = 3656668937U;
        static const AkUniqueID MX_SETSTATE = 1128714786U;
        static const AkUniqueID PLAY_DOOR_OPEN = 1660008929U;
        static const AkUniqueID PLAY_ITEM_PICKUP = 2652605998U;
        static const AkUniqueID PLAY_ITEM_PUTDOWN = 4250118861U;
        static const AkUniqueID PLAY_ITEM_THROW = 3065584282U;
        static const AkUniqueID PLAY_ITEM_THROW_CHARGE = 3278353889U;
        static const AkUniqueID PLAY_PICKPOCKET_KEYS = 1684266438U;
        static const AkUniqueID PLAY_UI_HOVER = 1339559671U;
        static const AkUniqueID START_AREA_AMB = 4278971852U;
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
