using SRML.Config.Attributes;

namespace Configs
{
    [ConfigFile("SETTINGS")]
    class Values
    {
        public static readonly VACPACK_ENUMS VACPACK = VACPACK_ENUMS.DEFAULT;
    }

    enum VACPACK_ENUMS
    {
        DEFAULT,
        NIMBLE_VALLEY,
        AUTOMATIC
    }
}