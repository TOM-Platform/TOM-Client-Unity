public static class DataTypes
{
    // running data
    public const int RUNNING_LIVE_DATA = 1001;
    public const int RUNNING_LIVE_UNIT = 1002;
    public const int RUNNING_SUMMARY_DATA = 1003;
    public const int RUNNING_SUMMARY_UNIT = 1004;
    public const int DIRECTION_DATA = 1005;
    public const int RUNNNING_TYPE_POSITION_MAPPING_DATA = 1006;
    public const int RANDOM_ROUTES_DATA = 1007;
    public const int ROUTE_DATA = 1008;
    public const int RUNNING_TARGET_DATA = 1011;
    public const int RUNNING_CAMERA_DATA = 1012;

    public const int REQUEST_RUNNING_LIVE_DATA = 1101;
    public const int REQUEST_RUNNING_LIVE_UNIT = 1102;
    public const int REQUEST_RUNNING_SUMMARY_DATA = 1103;
    public const int REQUEST_RUNNING_SUMMARY_UNIT = 1104;
    public const int REQUEST_DIRECTION_DATA = 1105;
    public const int REQUEST_RUNNING_TYPE_POSITION_MAPPING = 1106;
    public const int REQUEST_RANDOM_ROUTES_DATA = 1107;
    public const int REQUEST_CHOSEN_ROUTE_DATA = 1108;
    public const int REQUEST_RUNNING_TRAINING_MODE_DATA = 1109;
    public const int REQUEST_RUNNING_TARGET_DATA = 1111;

    public const int RUNNING_LIVE_ALERT = 1201;
    public const int RUNNING_PLACE_DATA = 1301;

    // learning data
    public const int LEARNING_DATA = 2001;
    public const int REQUEST_LEARNING_DATA = 2002;

    // gesture data
    public const int FINGER_POINTING_DATA = 3001;

    // speech data
    public const int SPEECH_INPUT_DATA = 4001;

    // gaze data
    public const int GAZE_POINTING_DATA = 5001;

    // highlight data
    public const int HIGHLIGHT_POINT_DATA = 6001;

    // martial arts data
    public const int MA_UPDATE_SESSION_CONFIG_COMMAND = 7001;
    public const int MA_BEGIN_SESSION_COMMAND = 7002;
    public const int MA_END_SESSION_COMMAND = 7003;

    public const int MA_REQUEST_SEQUENCE_DATA = 7011;
    public const int MA_REQUEST_CONFIG_DATA = 7014;

    public const int MA_SEQUENCE_DATA = 7021;
    public const int MA_FEEDBACK_LIVE_DATA = 7022;
    public const int MA_METRICS_DATA = 7023;
    public const int MA_CONFIG_DATA = 7025;
    public const int MA_POST_SESSION_FEEDBACK_DATA = 7026;

    // Template data
    public const int TEMPLATE_DATA = 3101;
    public const int REQUEST_TEMPLATE_DATA = 3102;
}
