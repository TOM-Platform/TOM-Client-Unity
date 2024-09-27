using UnityEngine;
using UnityEngine.Windows.WebCam;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace TOM.Common.Utils
{
    public static class PhotoCamera
    {
        private static PhotoCapture photoCaptureObject = null;
        private static TaskCompletionSource<byte[]> photoDataCompletionSource;
        private static bool isCaptureDone = true;

        public static async void CaptureAndHandlePhoto(System.Action<byte[]> callback)
        {
            if(isCaptureDone) 
            {
                byte[] imageData = await CapturePhotoAsync();
                callback(imageData);
                isCaptureDone = true;
            }
        }
        private static Task<byte[]> CapturePhotoAsync()
        {
            isCaptureDone = false;
            photoDataCompletionSource = new TaskCompletionSource<byte[]>();
            PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
            return photoDataCompletionSource.Task;
        }
        // FROM MRTK DOCUMENTATION: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/locatable-camera-in-unity
        private static void OnPhotoCaptureCreated(PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            CameraParameters c = new CameraParameters();
            c.hologramOpacity = 0.0f;
            c.cameraResolutionWidth = cameraResolution.width;
            c.cameraResolutionHeight = cameraResolution.height;
            c.pixelFormat = CapturePixelFormat.BGRA32;
            captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
        }
        // FROM MRTK DOCUMENTATION: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/locatable-camera-in-unity
        private static void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
        {
            if (result.success)
            {
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            }
            else
            {
                Debug.LogError("Unable to start photo mode!");
                photoDataCompletionSource.SetException(new Exception("Unable to start photo mode"));
            }
        }
        
        // FROM MRTK DOCUMENTATION: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/locatable-camera-in-unity
        private static void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        {
            if (result.success)
            {
                // Get the raw image data
                byte[] imageData;
                photoCaptureFrame.TryGetCameraToWorldMatrix(out Matrix4x4 cameraToWorldMatrix);
                photoCaptureFrame.TryGetProjectionMatrix(out Matrix4x4 projectionMatrix);
                // Create a texture to store the image
                Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
                Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
                photoCaptureFrame.UploadImageDataToTexture(targetTexture);
                // Convert the texture to a byte array
                imageData = targetTexture.EncodeToJPG(75); // Adjust quality as needed (0-100)
                // Clean up
                UnityEngine.Object.Destroy(targetTexture);

                photoDataCompletionSource.SetResult(imageData);
                photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
                // VisualLog.Log("Saved Image");
            }
            else
            {
                Debug.Log("Failed to capture photo to memory");
                photoDataCompletionSource.SetException(new Exception("Failed to capture photo to memory"));
            }
        }
        // FROM MRTK DOCUMENTATION: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/locatable-camera-in-unity
        private static void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
        {
            photoCaptureObject.Dispose();
            photoCaptureObject = null;
        }
    }
}
