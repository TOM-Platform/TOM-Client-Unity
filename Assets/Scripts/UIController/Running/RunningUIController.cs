using Google.Protobuf.Collections;
using UnityEngine;
using static TextContentUnit;
using static TextContentUnit.TextContentUnitType;
using static RunningTypePositonMapping;
using static ValidInput;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class RunningUIController : BaseUIController
{
    public GameObject runningUI;

    public GameObject runningTop;
    public GameObject runningTopLeft;
    public GameObject runningTopCenter;
    public GameObject runningTopRight;
    public GameObject runningBottomLeftTop;
    public GameObject runningBottomLeftBottom;
    public GameObject runningBottomRight;

    public GameObject runningDirection;
    public GameObject imageCarousel;
    public GameObject placeInfoPrefab;
    private Dictionary<int, GameObject> placeInfoDictionary = new Dictionary<int, GameObject>();

    public TextMesh panelDirectionInstruction;
    public TextMesh panelRunningInstruction;
    public TextMesh panelTurnDistance;
    public TextMesh panelTurnDuration;

    private const string speedColor = "#b51d1d";
    private const string distanceColor = "#FFFF00";
    private Sprite toiletIcon;
    private Sprite waterIcon;

    // Start is called before the first frame update
    void Start()
    {
        toiletIcon = Resources.Load<Sprite>("Images/Places/toilet");
        if (toiletIcon == null)
        {
            Debug.LogError("Failed to load toilet icon.");
        }
        waterIcon = Resources.Load<Sprite>("Images/Places/water");
        if (waterIcon == null)
        {
            Debug.LogError("Failed to load water icon.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ResetUI()
    {
        UpdateInstructions(DataTypes.RUNNING_LIVE_DATA, "");
        UpdateInstructions(DataTypes.DIRECTION_DATA, "");
        UpdateTurnDistance("");
        UpdateTurnDuration("");

        UpdateText(UIPosition.TopLeft, "", Content);
        UpdateText(UIPosition.TopCenter, "", Content);
        UpdateText(UIPosition.TopRight, "", Content);
        UpdateText(UIPosition.BottomLeftTop, "", Content);
        UpdateText(UIPosition.BottomLeftBottom, "", Content);

        UpdateDirectionAngle(null);

        UpdateText(UIPosition.TopLeft, "", Unit);
        UpdateText(UIPosition.TopCenter, "", Unit);
        UpdateText(UIPosition.TopRight, "", Unit);

        UpdateText(UIPosition.BottomLeftTop, "", Footer);
        UpdateText(UIPosition.BottomLeftBottom, "", Footer);
    }

    protected override GameObject getRootObject()
    {
        return runningUI;
    }

    protected override GameObject getPanel(UIPosition position)
    {
        switch (position)
        {
            case UIPosition.Top:
                return runningTop;
            case UIPosition.TopLeft:
                return runningTopLeft;
            case UIPosition.TopCenter:
                return runningTopCenter;
            case UIPosition.TopRight:
                return runningTopRight;
            case UIPosition.BottomLeftTop:
                return runningBottomLeftTop;
            case UIPosition.BottomLeftBottom:
                return runningBottomLeftBottom;
            case UIPosition.BottomRight:
                return runningBottomRight;
            default:
                return null;
        }
    }

    public override void UpdateText(UIPosition position, string value, TextContentUnitType contentUnitType)
    {
        base.UpdateText(position, value, contentUnitType);

        if (runningTopLeft.activeSelf || runningTopCenter.activeSelf || runningTopRight.activeSelf)
        {
            runningTop.SetActive(true);
        }
        else
        {
            runningTop.SetActive(false);
        }
    }


    internal void UpdateImageCarousel(RepeatedField<RouteData> routes)
    {
        bool isVisible = routes?.Count != 0;
        imageCarousel.SetActive(isVisible);

        if (isVisible)
        {
            Transform container = imageCarousel.transform.Find("Container");

            if (container != null)
            {
                Transform gridObjectCollection = container.transform.Find("GridObjectCollection");

                if (gridObjectCollection != null)
                {
                    ImageLoader imageLoader = gridObjectCollection.GetComponent<ImageLoader>();
                    imageLoader.UpdateRoutes(routes);
                    imageLoader.CreateImageButtons();
                }
            }
        }
    }

    internal void UpdateTargetFooter(string targetFooter)
    {
        UpdateText(getRunningTypePosition(UIDataType.RunningTarget), targetFooter, Footer);
    }

    internal void UpdateRunningUI(RunningLiveData runningLiveData, string targetContent, bool isUnit, bool isAlert)
    {
        Debug.Log("RunningLiveData:\n" +
                                "isUnit: " + isUnit + "\n" +
                                "isAlert: " + isAlert + "\n" +
                                "Distance: " + runningLiveData.Distance + "\n" +
                                "HeartRate: " + runningLiveData.HeartRate + "\n" +
                                "Calories: " + runningLiveData.Calories + "\n" +
                                "Speed: " + runningLiveData.Speed + "\n" +
                                "Duration: " + runningLiveData.Duration + "\n" +
                                "Time: " + runningLiveData.Time + "\n" +
                                "Instruction: " + runningLiveData.Instruction + "\n" +
                                "Audio: " + runningLiveData.AudioInstr);

        if (isAlert)
        {
            //if (isValidBool(runningLiveData.Distance))
            //{
            //    UpdateTextColor(getRunningTypePosition(UIDataType.RunningDistance), bool.Parse(runningLiveData.Distance), distanceColor);
            //}
            if (isValidBool(runningLiveData.Speed))
            {
                UpdateBackgroundColor(getRunningTypePosition(UIDataType.RunningSpeed), bool.Parse(runningLiveData.Speed), speedColor);
            }

            UpdateInstructions(DataTypes.RUNNING_LIVE_DATA, runningLiveData.Instruction);
            return;
        }

        TextContentUnitType contentUnitType = isUnit ? Unit : Content;

        UpdateText(getRunningTypePosition(UIDataType.RunningHeartRate), runningLiveData.HeartRate, contentUnitType);
        UpdateText(getRunningTypePosition(UIDataType.RunningSpeed), runningLiveData.Speed, contentUnitType);
        UpdateText(getRunningTypePosition(UIDataType.RunningCalories), runningLiveData.Calories, contentUnitType);
        UpdateText(getRunningTypePosition(UIDataType.RunningTime), runningLiveData.Time, contentUnitType);
        //runningUIController.UpdateText(getTypePosition(UIDataType.RunningDuration), runningLiveData.Duration, contentUnitType);
        //runningUIController.UpdateText(getTypePosition(UIDataType.RunningDistance), runningLiveData.Distance, contentUnitType);
        if (!isUnit)
        {
            UpdateText(getRunningTypePosition(UIDataType.RunningTarget), targetContent, contentUnitType);
        }
    }

    internal void UpdateDirectionUI(DirectionData directionData, Boolean isGeneratingRoutes)
    {
        Debug.Log("DirectionData:\n" +
                                 "destDist: " + directionData.DestDist + "\n" +
                                 "destDuration: " + directionData.DestDuration + "\n" +
                                 "currDist: " + directionData.CurrDist + "\n" +
                                 "currDuration: " + directionData.CurrDuration + "\n" +
                                 "currInstr: " + directionData.CurrInstr + "\n" +
                                 "currDirection: " + directionData.CurrDirection + "\n" +
                                 "Audio: " + directionData.AudioInstr);
        UpdateDirectionAngle(directionData.CurrDirection);
        UpdateTurnDistance(directionData.CurrDist);
        UpdateTurnDuration(directionData.CurrDuration);
        if (!isGeneratingRoutes)
        {
            UpdateInstructions(DataTypes.DIRECTION_DATA, directionData.CurrInstr);
        }
    }

    internal void UpdateDirectionAngle(string angleString)
    {
        if (string.IsNullOrEmpty(angleString))
        {
            runningDirection.SetActive(false);
            return;
        }
        int angle = -1;
        if (int.TryParse(angleString, out angle))
        {
            if (angle < 0 || angle > 360)
            {
                Debug.LogError("Invalid angle: " + angle);
                runningDirection.SetActive(false);
                return;
            }
            runningDirection.SetActive(true);
            runningDirection.GetComponent<DirectionContent>().UpdateDirectionImage(angle);
        }
        else
        {
            Debug.LogError("Angle cannot be parsed as int: " + angleString);
            runningDirection.SetActive(false);
        }
    }

    internal void UpdateInstructions(int dataType, string instruction)
    {
        switch (dataType)
        {
            case DataTypes.RUNNING_LIVE_DATA:
                panelRunningInstruction.text = instruction;
                break;
            case DataTypes.DIRECTION_DATA:
                panelDirectionInstruction.text = instruction;
                break;
            default:
                Debug.LogError("Unknown dataType: " + dataType);
                break;
        }
    }

    private void UpdateTurnDistance(string distance)
    {
        panelTurnDistance.text = distance;
    }

    private void UpdateTurnDuration(string duration)
    {
        panelTurnDuration.text = duration;
    }

    internal void SetPlaceInfo(RunningPlaceData runningPlaceData)
    {
        Debug.Log("RunningPlaceData:\n" +
                        "Place id: " + runningPlaceData.PlaceId + "\n" +            
                        "Place Type: " + runningPlaceData.Facility + "\n" +
                        "Name: " + runningPlaceData.Location + "\n" +
                        "Level: " + runningPlaceData.Level + "\n" +
                        "Distance: " + runningPlaceData.Distance + "\n" +
                        "Position: " + runningPlaceData.Position + "\n");

        if (!placeInfoDictionary.ContainsKey(runningPlaceData.PlaceId))
        {
            CreatePlaceInfo(runningPlaceData);
            return;
        }

        if (runningPlaceData.Location == "")
        {
            Debug.Log("Removing placeInfo prefab " + runningPlaceData.PlaceId);
            Destroy(placeInfoDictionary[runningPlaceData.PlaceId]);
            placeInfoDictionary.Remove(runningPlaceData.PlaceId);
        }
        else
        {
            Debug.Log("Updating placeInfo prefab " + runningPlaceData.PlaceId);
            GameObject placeInfo = placeInfoDictionary[runningPlaceData.PlaceId];
            UpdatePlaceInfoText(placeInfo, runningPlaceData);
        }
    }

    private void CreatePlaceInfo(RunningPlaceData runningPlaceData)
    {
        Vector3 position;
        // TODO: determine position from actual coordinates
        switch (runningPlaceData.Position)
        {
            case "left":
                position = new Vector3(
                        -0.12f,
                        -0.05f,
                        -0.2f
                    );
                break;
            case "center":
                position = new Vector3(
                        0f,
                        -0.12f,
                        -0.2f
                    );
                break;
            case "right":
                position = new Vector3(
                        0.12f,
                        -0.05f,
                        -0.2f
                    );
                break;
            default:
                Debug.LogError("Unknown position: " + runningPlaceData.Position);
                return;
        }


        GameObject placeInfo = Instantiate(placeInfoPrefab, position, Quaternion.identity);
        placeInfo.transform.SetParent(runningUI.transform, false);
        UpdatePlaceInfoIcon(placeInfo, runningPlaceData.Facility);
        UpdatePlaceInfoText(placeInfo, runningPlaceData);
        placeInfoDictionary.Add(runningPlaceData.PlaceId, placeInfo);
    }

    private void UpdatePlaceInfoIcon(GameObject placeInfo, string facility)
    {
        try
        {
            Image icon = placeInfo.GetComponentInChildren<Image>();
            switch (facility)
            {
                case "Restroom":
                    icon.sprite = toiletIcon;
                    break;
                case "Waterpoint":
                    icon.sprite = waterIcon;
                    break;
                default:
                    Debug.LogError("Unknown facility: " + facility);
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("UpdatePlaceInfoIcon: " + e.Message);
        }

    }

    private void UpdatePlaceInfoText(GameObject placeInfo, RunningPlaceData runningPlaceData)
    {
        try
        {
            TextMesh facilityText = placeInfo.transform.Find("TMPTextFacility").GetComponent<TextMesh>();
            TextMesh distanceText = placeInfo.transform.Find("TMPTextDistance").GetComponent<TextMesh>();
            TextMesh locationText = placeInfo.transform.Find("TMPTextLocation").GetComponent<TextMesh>();
            TextMesh levelText = placeInfo.transform.Find("TMPTextLevel").GetComponent<TextMesh>();

            facilityText.text = runningPlaceData.Facility;
            distanceText.text = runningPlaceData.Distance;
            locationText.text = runningPlaceData.Location;
            levelText.text = runningPlaceData.Level;
        }
        catch (Exception e)
        {
            Debug.LogError("UpdatePlaceData: " + e.Message);
        }
    }
}
