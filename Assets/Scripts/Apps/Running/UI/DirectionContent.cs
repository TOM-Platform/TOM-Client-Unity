using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOM.Apps.Running
{

    public class DirectionContent : MonoBehaviour
    {
        public GameObject directionElement;

        private Image directionImageComponent;

        private static bool spritesLoaded = false;

        private static bool isStarted = false;

        [SerializeField] private static string basePath = "Images/Directions/";

        private static Sprite straight;
        private static Sprite turnSlightRight;
        private static Sprite turnRight;
        private static Sprite turnSharpRight;
        private static Sprite turnSlightLeft;
        private static Sprite turnLeft;
        private static Sprite turnSharpLeft;
        private static Sprite uTurn;


        // Start is called before the first frame update
        void Start()
        {
            loadSprites();
            directionImageComponent = directionElement.GetComponentInChildren<Image>();
            isStarted = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private static void loadSprites()
        {
            if (spritesLoaded)
            {
                return;
            }

            spritesLoaded = true;

            straight = Resources.Load<Sprite>(basePath + "straight");
            turnSlightRight = Resources.Load<Sprite>(basePath + "turn-slight-right");
            turnRight = Resources.Load<Sprite>(basePath + "turn-right");
            turnSharpRight = Resources.Load<Sprite>(basePath + "turn-sharp-right");
            turnSlightLeft = Resources.Load<Sprite>(basePath + "turn-slight-left");
            turnLeft = Resources.Load<Sprite>(basePath + "turn-left");
            turnSharpLeft = Resources.Load<Sprite>(basePath + "turn-sharp-left");
            uTurn = Resources.Load<Sprite>(basePath + "u-turn");
        }

        private static Sprite getDirectionSprite(int angle)
        {
            // 0 is North, 90 is East, 180 is South, 270 is West
            switch (angle)
            {
                case < 10:
                    return straight;
                case < 45:
                    return turnSlightRight;
                case < 135:
                    return turnRight;
                case < 170:
                    return turnSharpRight;
                case < 190:
                    return uTurn;
                case < 225:
                    return turnSharpLeft;
                case < 315:
                    return turnLeft;
                case < 350:
                    return turnSlightLeft;
                case <= 360:
                    return straight;
                default:
                    return straight;
            }

        }

        public void UpdateDirectionImage(int angle)
        {
            Debug.Log("UpdateDirectionImage: " + angle);
            if (!isStarted)
            {
                Start();
            }
            // float current_z = directionElement.transform.eulerAngles.z;
            // Angle = angle - Mathf.RoundToInt(current_z);

            directionImageComponent.sprite = getDirectionSprite(angle);
        }
    }

}
