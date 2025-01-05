using System;

public static class DataTypes
{
    //// Format: AB, where A is the service (0-999), B is the identifier (0-99)
    //// use 0-99 (i.e., B only) for common data types, which does not have a specific service
    //// use A00-A99 for specific services

    public const int SERVICE_SWITCH_DATA = 1;

    // gesture data
    public const int FINGER_POINTING_DATA = 10;

    // speech data
    public const int SPEECH_INPUT_DATA = 20;

    // gaze data
    public const int GAZE_POINTING_DATA = 30;

    // highlight data
    public const int HIGHLIGHT_POINT_DATA = 40;

    // client data
    public const int EXERCISE_WEAR_OS_DATA = 51;

    // Template data
    public const int TEMPLATE_DATA = 101;
    public const int REQUEST_TEMPLATE_DATA = 102;


    // running data
    public const int RUNNING_LIVE_DATA = 201;
    public const int RUNNING_LIVE_UNIT = 202;
    public const int RUNNING_SUMMARY_DATA = 203;
    public const int RUNNING_SUMMARY_UNIT = 204;
    public const int DIRECTION_DATA = 205;
    public const int RUNNING_TYPE_POSITION_MAPPING_DATA = 206;
    public const int RANDOM_ROUTES_DATA = 207;
    public const int ROUTE_DATA = 208;

    public const int RUNNING_TARGET_DATA = 209;
    public const int RUNNING_CAMERA_DATA = 210;

    public const int RUNNING_LIVE_ALERT = 211;
    public const int RUNNING_PLACE_DATA = 212;

    public const int WAYPOINT_DATA = 220;
    public const int WAYPOINTS_LIST_DATA = 221;

    public const int REQUEST_RUNNING_LIVE_DATA = 251;
    public const int REQUEST_RUNNING_LIVE_UNIT = 252;
    public const int REQUEST_RUNNING_SUMMARY_DATA = 253;
    public const int REQUEST_RUNNING_SUMMARY_UNIT = 254;
    public const int REQUEST_DIRECTION_DATA = 255;
    public const int REQUEST_RUNNING_TYPE_POSITION_MAPPING = 256;
    public const int REQUEST_RANDOM_ROUTES_DATA = 257;
    public const int REQUEST_CHOSEN_ROUTE_DATA = 258;
    public const int REQUEST_RUNNING_TRAINING_MODE_DATA = 259;
    public const int REQUEST_RUNNING_TARGET_DATA = 260;


    // learning data
    public const int LEARNING_DATA = 301;
    public const int REQUEST_LEARNING_DATA = 302;
    public const int REQUEST_RESET_LEARNING = 303;


    // martial arts data
    public const int MA_UPDATE_SESSION_CONFIG_COMMAND = 401;
    public const int MA_BEGIN_SESSION_COMMAND = 402;
    public const int MA_END_SESSION_COMMAND = 403;

    public const int MA_REQUEST_SEQUENCE_DATA = 411;
    public const int MA_REQUEST_CONFIG_DATA = 412;
    public const int MA_REQUEST_LIVE_FEEDBACK_DATA = 413;
    public const int MA_REQUEST_TIMER_DATA = 414;
    public const int MA_REQUEST_POST_SESSION_FEEDBACK_DATA = 415;

    public const int MA_SEQUENCE_DATA = 421;
    public const int MA_FEEDBACK_LIVE_DATA = 422;

    public const int MA_METRICS_DATA = 423;
    public const int MA_CONFIG_DATA = 424;
    public const int MA_POST_SESSION_FEEDBACK_DATA = 425;

    // pandalens data
    public const int PANDALENS_EVENT_DATA = 501;
    public const int PANDALENS_QUESTION = 502;
    public const int PANDALENS_RESPONSE = 503;
    public const int PANDALENS_MOMENTS = 504;
    public const int PANDALENS_ERROR = 505;
    public const int PANDALENS_RESET = 506;
}
