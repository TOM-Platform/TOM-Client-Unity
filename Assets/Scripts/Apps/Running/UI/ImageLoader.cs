using Google.Protobuf.Collections;

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

using UnityEngine;
using UnityEngine.UI;

namespace TOM.Apps.Running
{

    public class ImageLoader : MonoBehaviour, IMixedRealityPointerHandler
    {
        public GameObject imageButtonPrefab;
        public GameObject confirmationDialogPrefab;
        public GridObjectCollection gridObjectCollection;
        public RunningController runningController;
        public RunningUIController runningUIController;

        private RepeatedField<RouteData> routes = new RepeatedField<RouteData>();

        private Vector3 prevPosition;
        private const float swipeThreshold = 0.1f;
        private int selectedImageIndex = -1;
        private int routesCount = 3;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        internal void UpdateRoutes(RepeatedField<RouteData> routes)
        {
            this.routes = routes;
            routesCount = routes.Count;
        }

        internal void CreateImageButtons()
        {

            for (int i = 0; i < routes.Count; i++)
            {
                GameObject imageButton = Instantiate(imageButtonPrefab, gridObjectCollection.transform);

                int routeId = routes[i].RouteId;
                byte[] routeMapImage = routes[i].RouteMapImage.ToByteArray();
                string difficulty = routes[i].Difficulty;
                string level = routes[i].Level;
                string destDist = routes[i].DestDist;
                string destDuration = routes[i].DestDuration;
                int toilets = routes[i].Toilets;
                int waterPoints = routes[i].WaterPoints;

                print("RouteData: \n " +
                      $"routeId: {routeId} \n" +
                      $"difficulty: {difficulty} \n" +
                      $"level: {level} \n" +
                      $"destDist: {destDist} \n" +
                      $"destDuration: {destDuration} \n" +
                      $"toilets: {toilets} \n" +
                      $"waterPoints: {waterPoints} \n");

                // Set the grid object name to identify it in pointer events
                imageButton.name = $"Route {routeId}";

                LoadImageContent(imageButton, routeMapImage, imageButton.name, routes[i]);
            }

            // Update the grid collection after adding all buttons
            gridObjectCollection.UpdateCollection();
        }

        private void LoadImageContent(GameObject imageButton, byte[] routeMapImage, string imageName,
            RouteData routeData)
        {
            LoadImageBytes(imageButton, routeMapImage);

            LoadTextComponents(imageButton, imageName, routeData);
        }

        private void LoadImageBytes(GameObject imageButton, byte[] imageBytes)
        {
            Texture2D texture = new Texture2D(1, 1);
            bool isLoaded = texture.LoadImage(imageBytes);

            if (isLoaded)
            {
                Image imageComponent = imageButton.GetComponentInChildren<Image>();
                imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    Vector2.one * 0.5f);
            }
            else
            {
                Debug.LogError("Failed to load image bytes: " + imageButton.name);
            }
        }

        private void LoadTextComponents(GameObject imageButton, string imageName, RouteData routeData)
        {
            Transform labels = imageButton.transform.Find("Labels");
            if (labels != null)
            {
                TextMesh headerLabel = labels.Find("HeaderLabel").GetComponent<TextMesh>();
                TextMesh distanceLabel = labels.Find("DistanceLabel").GetComponent<TextMesh>();
                TextMesh difficultyLabel = labels.Find("DifficultyLabel").GetComponent<TextMesh>();
                TextMesh levelLabel = labels.Find("LevelLabel").GetComponent<TextMesh>();
                TextMesh toiletLabel = labels.Find("ToiletLabel").GetComponent<TextMesh>();
                TextMesh waterLabel = labels.Find("WaterLabel").GetComponent<TextMesh>();

                headerLabel.text = imageName;
                distanceLabel.text = routeData.DestDist;
                difficultyLabel.text = routeData.Difficulty;
                levelLabel.text = routeData.Level;
                toiletLabel.text = routeData.Toilets.ToString();
                waterLabel.text = routeData.WaterPoints.ToString();
            }
        }

        public void OnPointerDown(MixedRealityPointerEventData eventData)
        {
            prevPosition = eventData.Pointer.Result.Details.Point;
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData)
        {
        }

        public void OnPointerUp(MixedRealityPointerEventData eventData)
        {
            Vector3 currentPosition = eventData.Pointer.Result.Details.Point;
            float distance = Vector3.Distance(prevPosition, currentPosition);
            Debug.Log("Pointer Distance: " + distance);

            prevPosition = currentPosition;

            // if user swipe less than this, it's considered a button press
            if (distance <= swipeThreshold)
            {
                GameObject imageButton = eventData.Pointer.Result.CurrentPointerTarget;
                string imageLabel = imageButton.name;
                ImageButtonClickHandler(imageLabel);
            }
        }

        public void OnPointerClicked(MixedRealityPointerEventData eventData)
        {
        }

        public void ImageButtonClickHandler(string imageLabel)
        {
            Debug.Log("Image button clicked: " + imageLabel);

            selectedImageIndex = int.Parse(imageLabel.Substring(6));

            Dialog confirmationDialog = Dialog.Open(confirmationDialogPrefab,
                DialogButtonType.Yes | DialogButtonType.No, "Confirm Route",
                $"Are you sure you want to choose {imageLabel}?", true);

            if (confirmationDialog != null)
            {
                confirmationDialog.OnClosed += OnClosedDialogEvent;
            }
        }

        public void OnClosedDialogEvent(DialogResult result)
        {
            if (result.Result == DialogButtonType.Yes)
            {
                Debug.Log($"User selected index {selectedImageIndex}");

                if (selectedImageIndex > 0 && selectedImageIndex <= routesCount)
                {
                    // send route index to server
                    runningController.SendRequestToServer(DataTypes.REQUEST_CHOSEN_ROUTE_DATA,
                        selectedImageIndex.ToString());
                    // start sending camera data
                    runningController.InvokeSendRunningCameraData();
                    // hide image carousel
                    runningUIController.UpdateImageCarousel(new RepeatedField<RouteData>());
                }
                else
                {
                    Debug.LogError($"Invalid route index: {selectedImageIndex}, routesCount: {routesCount}");
                    runningUIController.UpdateInstructions(DataTypes.DIRECTION_DATA,
                        "Unknown route selected, please try again");
                }
            }
            else
            {
                Debug.Log("User cancelled route selection");
            }
        }
    }

}
