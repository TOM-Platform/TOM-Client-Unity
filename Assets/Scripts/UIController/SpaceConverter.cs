using System;
using UnityEngine;

public class SpaceConverter
{
    public const string TAG = "SPACE_CONVERTER";

    private const float DEFAULT_Z_WORLD = -1f;

#if UNITY_EDITOR
    // offset = origin of the virtual display camera space - original of the real screen, in terms of the virtual display camera size
    public const float X_OFFSET_NORMALIZED = 0.5f;
    public const float Y_OFFSET_NORMALIZED = 0.5f;

    // proportion = real screen size / virtual display camera size
    public const float xProportion = 1f;
    public const float yProportion = 1f;
#else
    // Special Hololens modifiers. Refer to the TOM docs.
    public const float X_OFFSET_NORMALIZED = (0.43f);
    public const float Y_OFFSET_NORMALIZED = (0.35f);

    public const float xProportion = 0.84f;
    public const float yProportion = 0.79f;
#endif

    // Returns a frame rooted at its bottom-left corner
    public static Frame ApplyBoundingBox(Transform obj, Camera camera, Resolution resolution, bool enableLog)
    {
        CornerSet2D set = GetCornersOnImage(obj, camera, resolution, enableLog);

        // Prepares crop image parameters
        int startPositionX = (int)set.topLeft.x;
        int startPositionY = (int)set.topLeft.y;

        // Calculates the average width and height for higher accuracy
        int croppedImageWidth = (int)(Math.Abs(set.bottomLeft.x - set.topRight.x) + Math.Abs(set.topLeft.x - set.bottomRight.x)) / 2;
        int CroppedImageHeight = (int)(Math.Abs(set.bottomLeft.y - set.topRight.y) + Math.Abs(set.topLeft.y - set.bottomRight.y)) / 2;

        if (croppedImageWidth == 0 || CroppedImageHeight == 0)
        {
            return null;
        }

        // The crop function requires the bottom-left corner
        Frame result = new(startPositionX, startPositionY, croppedImageWidth, CroppedImageHeight, CornerPosition.TOP_LEFT);

        return result;
    }

    // Returns normalized but not constrained coordinates in the display screen space
    public static CornerSet2D GetCornersOnScreen(Transform obj, Camera camera, bool enableLog)
    {
        Vector3 centerPosition_World = obj.transform.position;
        Quaternion rotation_World = obj.transform.rotation;

        float width = obj.lossyScale.x;
        float height = obj.lossyScale.y;

        // Initializes corner coordinates at the original position and in the original orientation
        Vector3 topLeft_World = GetWorldCoordinates(centerPosition_World, rotation_World, CornerPosition.TOP_LEFT, width, height);
        Vector3 topRight_World = GetWorldCoordinates(centerPosition_World, rotation_World, CornerPosition.TOP_RIGHT, width, height);
        Vector3 bottomLeft_World = GetWorldCoordinates(centerPosition_World, rotation_World, CornerPosition.BOTTOM_LEFT, width, height);
        Vector3 bottomRight_World = GetWorldCoordinates(centerPosition_World, rotation_World, CornerPosition.BOTTOM_RIGHT, width, height);

        // Converts the corners to the display camera space
        Vector3 topLeft_Camera = WorldToCamera(topLeft_World, camera);
        Vector3 topRight_Camera = WorldToCamera(topRight_World, camera);
        Vector3 bottomLeft_Camera = WorldToCamera(bottomLeft_World, camera);
        Vector3 bottomRight_Camera = WorldToCamera(bottomRight_World, camera);

        // Converts the corners to the display screen space, and then normalizes them
        Vector2 viewportProjectionSize;
        Vector2 offset;
        GetViewportProjectionSizeAndOffset(centerPosition_World, camera, out viewportProjectionSize, out offset);

        Vector2 topLeft_Screen = CameraToScreen(topLeft_Camera, viewportProjectionSize, offset);
        Vector2 topRight_Screen = CameraToScreen(topRight_Camera, viewportProjectionSize, offset);
        Vector2 bottomLeft_Screen = CameraToScreen(bottomLeft_Camera, viewportProjectionSize, offset);
        Vector2 bottomRight_Screen = CameraToScreen(bottomRight_Camera, viewportProjectionSize, offset);

        // Get the return value
        CornerSet2D set = new();
        set.topLeft = topLeft_Screen;
        set.topRight = topRight_Screen;
        set.bottomLeft = bottomLeft_Screen;
        set.bottomRight = bottomRight_Screen;

        string s =
            $"{TAG}:\n" +
            $"BB Scale: width: {width:F4}, height: {height:F4}\n" +
            $"BB Position: {centerPosition_World:F4}\n" +
            $"BB Rotation in World: {rotation_World:F4}\n" +
            $"\n" +
            $"BB Top-Left in World: {topLeft_World:F4}\n" +
            $"BB Top-Right in World: {topRight_World:F4}\n" +
            $"BB Bottom-Left in World: {bottomLeft_World:F4}\n" +
            $"BB Bottom-Right in World: {bottomRight_World:F4}\n" +
            $"\n" +
            $"BB Top-Left in Camera: {topLeft_Camera:F4}\n" +
            $"BB Top-Right in Camera: {topRight_Camera:F4}\n" +
            $"BB Bottom-Left in Camera: {bottomLeft_Camera:F4}\n" +
            $"BB Bottom-Right in Camera: {bottomRight_Camera:F4}\n" +
            $"\n" +
            $"BB Top-Left in Screen: {topLeft_Screen:F4}\n" +
            $"BB Top-Right in Screen: {topRight_Screen:F4}\n" +
            $"BB Bottom-Left in Screen: {bottomLeft_Screen:F4}\n" +
            $"BB Bottom-Right in Screen: {bottomRight_Screen:F4}\n" +
            $"\n";
        if (enableLog) Debug.Log(s);

        s =
            $"{TAG}:\n" +
            $"\n" +
            $"Aspect Ratio: {camera.aspect:F4}\n" +
            $"\n" +
            $"Camera Position: {camera.transform.position:F4}\n" +
            $"Camera Rotation: {camera.transform.rotation:F4}\n" +
            $"\n";
        if (enableLog) Debug.Log(s);

        return set;
    }

    // Returns constrained integer coordinates on the image
    public static CornerSet2D GetCornersOnImage(Transform obj, Camera camera, Resolution resolution, bool enableLog)
    {
        CornerSet2D set = GetCornersOnScreen(obj, camera, enableLog);

        // Normalizes the coordinates in the display screen space
        set.topLeft = ScreenToImage(set.topLeft, resolution);
        set.topRight = ScreenToImage(set.topRight, resolution);
        set.bottomLeft = ScreenToImage(set.bottomLeft, resolution);
        set.bottomRight = ScreenToImage(set.bottomRight, resolution);

        String s =
            $"BB Top-Left in Image: {set.topLeft:F4}\n" +
            $"BB Top-Right in Image: {set.topRight:F4}\n" +
            $"BB Bottom-Left in Image: {set.bottomLeft:F4}\n" +
            $"BB Bottom-Right in Image: {set.bottomRight:F4}\n" +
            $"\n";
        if (enableLog) Debug.Log(s);

        return set;
    }

    // We use left-handed, Y-up, Z-forward coordinate system for the world space
    public static Vector3 GetWorldCoordinates(Vector3 centerPosition, Quaternion centerRotation, CornerPosition type, float width, float height)
    {
        float halfWidth = width / 2;
        float halfHeight = height / 2;
        Vector3 cornerPosition;

        // Initializes corner coordinates at the original position and in the original orientation
        switch (type)
        {
            case CornerPosition.TOP_LEFT:
                cornerPosition = new(-halfWidth, halfHeight, 0f);
                break;
            case CornerPosition.TOP_RIGHT:
                cornerPosition = new(halfWidth, halfHeight, 0f);
                break;
            case CornerPosition.BOTTOM_LEFT:
                cornerPosition = new(-halfWidth, -halfHeight, 0f);
                break;
            case CornerPosition.BOTTOM_RIGHT:
                cornerPosition = new(halfWidth, -halfHeight, 0f);
                break;
            default:
                cornerPosition = centerPosition;
                break;
        }

        // Applies the current position
        cornerPosition = centerRotation * cornerPosition;

        // Converts local coordinates to virtual world coordinates
        cornerPosition += centerPosition;

        return cornerPosition;
    }

    // We use the X-right, Y-up, Z-forward system with the camera center as the origin.
    public static Vector3 WorldToCamera(Vector3 position, Camera camera)
    {
        Matrix4x4 viewMatrix = camera.worldToCameraMatrix;
        return viewMatrix.MultiplyPoint3x4(position);
    }

    // We use the X-right, Y-down system with the top-left corner as the origin
    // This is different from image cropping
    public static Vector2 CameraToScreen(Vector2 position, Vector2 viewportProjectionSize, Vector2 offset)
    {
        float x_Screen = position.x + offset.x;
        float y_Screen = -position.y + offset.y;

        // Normalization
        x_Screen /= viewportProjectionSize.x;
        y_Screen /= viewportProjectionSize.y;

        return new Vector2(x_Screen, y_Screen);
    }

    public static Vector2 CameraToScreen(Vector3 position, Camera camera)
    {
        Vector2 viewportProjectionSize;
        Vector2 offset;
        GetViewportProjectionSizeAndOffset(position, camera, out viewportProjectionSize, out offset);

        return CameraToScreen(position, viewportProjectionSize, offset);
    }

    // The coordinates will be constrained within the range
    public static Vector2 ScreenToImage(Vector2 position, Resolution resolution)
    {
        float x = position.x * resolution.width;
        float y = position.y * resolution.height;

        x = Math.Max(0f, x);
        x = Math.Min(resolution.width - 1, x);

        y = Math.Max(0f, y);
        y = Math.Min(resolution.height - 1, y);

        return new Vector2(x, y);
    }

    public static Vector2 ImageToScreen(Vector2 position, Resolution resolution)
    {
        return new(position.x / resolution.width, position.y / resolution.height);
    }

    // position = discrete coordiantes on (possibly cropped) images
    // cropParameters = previous parameters when this image is taken and cropped
    public static Vector2 CroppedImageToScreen(Vector2 position, Resolution resolution, Frame cropParameters)
    {
        Debug.Assert(cropParameters.cornerPosition == CornerPosition.TOP_LEFT);
        Vector2 imageTopLeft = new(cropParameters.x, cropParameters.y);
        Vector2 position_Image = imageTopLeft + position;
        return new(position_Image.x / resolution.width, position_Image.y / resolution.height);
    }

    // position = the screen position, normalized
    public static Vector3 ScreenToCamera(Vector2 position, Vector2 viewportProjectionSize, Vector2 offset, float prevSqrDistance)
    {
        float x = position.x;
        float y = position.y;

        x *= viewportProjectionSize.x;
        y *= viewportProjectionSize.y;

        x -= offset.x;
        y -= offset.y;

        y = -y;

        float diff = prevSqrDistance - x * x - y * y;
        float z = diff >= 0 ? -(float)Math.Sqrt(diff) : DEFAULT_Z_WORLD; // TODO

        return new(x, y, z);
    }

    public static Vector3 ScreenToCamera(Vector2 position, Camera camera, Vector3 prevCameraPosition, Vector3 prevBoundingBoxPosition)
    {
        // Cannot directly call GetViewportProjectionSizeAndOffset(), because camera's position may have been updated

        // Gets the FOVs
        float aspectRatio = camera.aspect;
        float VFOV = (float)(camera.fieldOfView * Math.PI / 180);
        float HFOV = (float)(2 * Math.Atan(aspectRatio * Math.Tan(VFOV / 2d)));

        // Calculates the size of the viewport projection rectangle
        float cameraToBoundingBoxDistance = (prevBoundingBoxPosition - prevCameraPosition).magnitude;
        float viewportProjectionWidth = (float)(2 * cameraToBoundingBoxDistance * Math.Tan(HFOV / 2));
        float viewportProjectionHeight = (float)(2 * cameraToBoundingBoxDistance * Math.Tan(VFOV / 2));

        // Converts the corners to the display screen space, and then normalizes them
        Vector2 offset = new(X_OFFSET_NORMALIZED * viewportProjectionWidth, Y_OFFSET_NORMALIZED * viewportProjectionHeight);
        viewportProjectionWidth *= xProportion;
        viewportProjectionHeight *= yProportion;
        Vector2 viewportProjectionSize = new(viewportProjectionWidth, viewportProjectionHeight);

        float sqrDistance = (prevBoundingBoxPosition - prevCameraPosition).sqrMagnitude;
        return ScreenToCamera(position, viewportProjectionSize, offset, sqrDistance);
    }

    public static Vector3 CameraToWorld(Vector3 position, Matrix4x4 prevCameraTransformMatrix)
    {
        return prevCameraTransformMatrix.MultiplyPoint3x4(position);
    }

    // The same as the normal method, just with inlined code and less computation to boost the performance
    // Returns a frame rooted at its bottom-left corner
    public static Frame ApplyBoundingBoxFast(Transform obj, Camera camera, Resolution resolution, bool enableLog)
    {
        Vector3 centerPosition_World = obj.transform.position;
        Quaternion rotation_World = obj.transform.rotation;

        float width = obj.lossyScale.x;
        float height = obj.lossyScale.y;

        // Initializes corner coordinates at the original position and in the original orientation
        Vector3 topLeft_World = GetWorldCoordinates(centerPosition_World, rotation_World, CornerPosition.TOP_LEFT, width, height);
        Vector3 bottomRight_World = GetWorldCoordinates(centerPosition_World, rotation_World, CornerPosition.BOTTOM_RIGHT, width, height);

        // Converts the corners to the display camera space
        Vector3 topLeft_Camera = WorldToCamera(topLeft_World, camera);
        Vector3 bottomRight_Camera = WorldToCamera(bottomRight_World, camera);

        // Gets the FOVs
        float aspectRatio = camera.aspect;
        float VFOV = (float)(camera.fieldOfView * Math.PI / 180);
        float HFOV = (float)(2 * Math.Atan(aspectRatio * Math.Tan(VFOV / 2d)));

        // Calculates the size of the viewport projection rectangle
        float cameraToBoundingBoxDistance = (centerPosition_World - camera.transform.position).magnitude;
        float viewportProjectionWidth = (float)(2 * cameraToBoundingBoxDistance * Math.Tan(HFOV / 2));
        float viewportProjectionHeight = (float)(2 * cameraToBoundingBoxDistance * Math.Tan(VFOV / 2));

        // Converts the corners to the display screen space, and then normalizes them
        Vector2 offset = new(X_OFFSET_NORMALIZED * viewportProjectionWidth, Y_OFFSET_NORMALIZED * viewportProjectionHeight);
        viewportProjectionWidth *= xProportion;
        viewportProjectionHeight *= yProportion;
        Vector2 viewportProjectionSize = new(viewportProjectionWidth, viewportProjectionHeight);

        Vector2 topLeft_Screen = CameraToScreen(topLeft_Camera, viewportProjectionSize, offset);
        Vector2 bottomRight_Screen = CameraToScreen(bottomRight_Camera, viewportProjectionSize, offset);

        // Normalizes the coordinates in the display screen space
        Vector2 topLeft_Image = ScreenToImage(topLeft_Screen, resolution);
        Vector2 bottomRight_Image = ScreenToImage(bottomRight_Screen, resolution);

        // Prepares crop image parameters
        int startPositionX = (int)topLeft_Image.x;
        int startPositionY = (int)topLeft_Image.y;
        int croppedImageWidth = (int)Math.Abs(bottomRight_Image.x - topLeft_Image.x);
        int croppedImageHeight = (int)Math.Abs(bottomRight_Image.y - topLeft_Image.y);

        if (croppedImageWidth == 0 || croppedImageHeight == 0)
        {
            return null;
        }

        // The crop function requires the bottom-left corner
        Frame result = new(startPositionX, startPositionY, croppedImageWidth, croppedImageHeight, CornerPosition.TOP_LEFT);

        string s =
            $"{TAG}:\n" +
            $"BB Scale: width: {width:F4}, height: {height:F4}\n" +
            $"BB Position: {centerPosition_World:F4}\n" +
            $"BB Rotation in World: {rotation_World:F4}\n" +
            $"\n" +
            $"BB Top-Left in World: {topLeft_World:F4}\n" +
            $"BB Bottom-Right in World: {bottomRight_World:F4}\n" +
            $"\n" +
            $"BB Top-Left in Camera: {topLeft_Camera:F4}\n" +
            $"BB Bottom-Right in Camera: {bottomRight_Camera:F4}\n" +
            $"\n" +
            $"BB Top-Left in Screen: {topLeft_Screen:F4}\n" +
            $"BB Bottom-Right in Screen: {bottomRight_Screen:F4}\n" +
            $"\n";
        if (enableLog) Debug.Log(s);

        s =
            $"{TAG}:\n" +
            $"Camera to Bounding Box Distance: {cameraToBoundingBoxDistance:F4}\n" +
            $"\n" +
            $"Aspect Ratio: {aspectRatio:F4}\n" +
            $"HFOV: {HFOV:F4}\n" +
            $"VFOV: {VFOV:F4}\n" +
            $"RGB Resolution: {resolution.width:F4} x {resolution.height:F4}\n" +
            $"Viewport Projection Width: {viewportProjectionWidth:F4}\n" +
            $"Viewport Projection Height: {viewportProjectionHeight:F4}\n" +
            $"Offset: {offset:F4}\n" +
            $"\n" +
            $"Camera Position: {camera.transform.position:F4}\n" +
            $"Camera Rotation: {camera.transform.rotation:F4}\n" +
            $"\n";
        if (enableLog) Debug.Log(s);

        return result;
    }

    public static void GetViewportProjectionSizeAndOffset(Vector3 position, Camera camera, out Vector2 viewportProjectionSize, out Vector2 offset)
    {
        // Gets the FOVs
        float aspectRatio = camera.aspect;
        float VFOV = (float)(camera.fieldOfView * Math.PI / 180);
        float HFOV = (float)(2 * Math.Atan(aspectRatio * Math.Tan(VFOV / 2d)));

        // Calculates the size of the viewport projection rectangle
        float distance = Math.Abs(position.z);
        float viewportProjectionWidth = (float)(2 * distance * Math.Tan(HFOV / 2));
        float viewportProjectionHeight = (float)(2 * distance * Math.Tan(VFOV / 2));

        // Converts the corners to the display screen space, and then normalizes them
        offset = new(X_OFFSET_NORMALIZED * viewportProjectionWidth, Y_OFFSET_NORMALIZED * viewportProjectionHeight);
        viewportProjectionWidth *= xProportion;
        viewportProjectionHeight *= yProportion;
        viewportProjectionSize = new(viewportProjectionWidth, viewportProjectionHeight);
    }


    public class CornerSet2D
    {
        public Vector2 topLeft { get; set; }
        public Vector2 topRight { get; set; }
        public Vector2 bottomLeft { get; set; }
        public Vector2 bottomRight { get; set; }
    }


    public enum CornerPosition
    {
        TOP_LEFT = 0b00,
        TOP_RIGHT = 0b01,
        BOTTOM_LEFT = 0b10,
        BOTTOM_RIGHT = 0b11,
    }

    public class Frame
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        private CornerPosition m_cornerPosition;
        private CornerPosition m_systemOrigin = CornerPosition.TOP_LEFT;
        public CornerPosition cornerPosition
        {
            get
            {
                return m_cornerPosition;
            }
            set
            {
                if (m_systemOrigin != CornerPosition.TOP_LEFT) return;

                int horizontal = ((int)value & 1) - ((int)m_cornerPosition & 1);
                int vertical = ((int)value >> 1) - ((int)m_cornerPosition >> 1);

                switch (horizontal)
                {
                    case 1:
                        x += width;
                        break;
                    case -1:
                        x -= width;
                        break;
                    case 0:
                    default:
                        break;
                }
                switch (vertical)
                {
                    case 1:
                        y += height;
                        break;
                    case -1:
                        y -= height;
                        break;
                    case 0:
                    default:
                        break;
                }
                m_cornerPosition = value;
            }
        }

        public Frame(int x, int y, int width, int height, CornerPosition cornerPosition)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.m_cornerPosition = cornerPosition;
        }
    }
}
