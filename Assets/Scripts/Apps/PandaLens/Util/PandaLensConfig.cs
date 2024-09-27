namespace TOM.Apps.PandaLens
{
    public static class PandaLensConfig
    {
        public const int GESTURE_SEND_GAP_SECONDS = 1;
        public const int HAND_GESTURE_AVG_BUFFER_SIZE = 10;
        public const float GAZE_CURSOR_DISTANCE = 2f;
        public const float GAZE_DURATION_THRESHOLD = 10.0f;
        public const float GAZE_POSITION_DISPLACEMENT_THRESHOLD = 0.65f;
        public const float GAZE_ANGLE_DISPLACEMENT_THRESHOLD = 75.0f;
        public const int GAZE_AVG_BUFFER_SIZE = 30;
        public const float PANDALENS_FEEDBACK_GAP_SECONDS = 4f;
        
        public const float PANDALENS_IDLE_THRESHOLD_SECONDS = 300;

        //Dialogue UI Indexes 
        public const int PANDALENS_QUESTION_UI_INDEX = 0;
        public const int PANDALENS_RESPONSE_UI_INDEX = 1;
        public const int PANDALENS_LISTEN_UI_INDEX = 2;

        //User Action Keys
        public const string PANDALENS_CAMERA_ACTION = "photo";
        public const string PANDALENS_IDLE_ACTION = "idle";
        public const string PANDALENS_SUMMARY_ACTION = "summary";

        //PandaLens LLM Keywords
        public const string PANDALENS_LLM_NO_QUESTIONS = "None";
    }
}
