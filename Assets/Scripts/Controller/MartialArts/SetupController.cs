using UnityEngine;

namespace MartialArts
{
    public class SetupController : MonoBehaviour
    {
        public SetupUIController setupUIController;
        public MartialArtsController maController;

        private const int SOCKET_COMMUNICATION_START_DELAY_SECONDS = 4;

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }

        public void SendUpdatedConfigToServer()
        {
            SessionConfigData sessionConfig = setupUIController.GetConfig();
            maController.SendDataToServer<SessionConfigData>(
                DataTypes.MA_UPDATE_SESSION_CONFIG_COMMAND,
                sessionConfig
            );
        }

        public void HideSetup()
        {
            setupUIController.SetUIVisible(false);
        }

        public SessionConfigData GetCurrentConfig()
        {
            return setupUIController.GetConfig();
        }

        public void ShowSetup()
        {
            setupUIController.ResetUI();
            setupUIController.SetUIVisible(true);

            // request any saved/recommended config for the training session
            Invoke(nameof(RequestConfigData), SOCKET_COMMUNICATION_START_DELAY_SECONDS);
        }

        public void ShowConfigPanel()
        {
            setupUIController.ShowConfigPanel();
        }

        public void LoadConfig(SessionConfigData config)
        {
            setupUIController.SetConfig(config);
        }

        private void RequestConfigData()
        {
            maController.SendRequestToServer(DataTypes.MA_REQUEST_CONFIG_DATA);
        }
    }
}
