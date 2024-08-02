using UnityEngine;
using static RunningTypePositonMapping;

public class TrainingModeSelectionUIController : BaseUIController
{
    public GameObject selectionUI;
    public RunningController runningController;

    [SerializeField] private static string basePath = "Images/Selection/";

    public GameObject selectionLeft;
    public GameObject selectionRight;

    private static Sprite distanceSprite;
    private static Sprite speedSprite;

    // Start is called before the first frame update
    void Start()
    {
        LoadSprites();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private static void LoadSprites()
    {
        distanceSprite = Resources.Load<Sprite>(basePath + "distance");
        speedSprite = Resources.Load<Sprite>(basePath + "speed");
    }

    public override void ResetUI()
    {
        return;
    }

    protected override GameObject getRootObject()
    {
        return selectionUI;
    }

    protected override GameObject getPanel(UIPosition position)
    {
        switch (position)
        {
            case UIPosition.SelectionLeft:
                return selectionLeft;
            case UIPosition.SelectionRight:
                return selectionRight;
            default:
                return null;
        }
    }

    public void UpdateSelectionUI()
    {
        UpdateButtonTextAndIcon(getRunningTypePosition(UIDataType.SelectionDistance), "Distance", distanceSprite, runningController.SelectDistanceTraining);
        UpdateButtonTextAndIcon(getRunningTypePosition(UIDataType.SelectionSpeed), "Speed", speedSprite, runningController.SelectSpeedTraining);
    }
}
