using System;
using AVFoundation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Square.iOS.ViewControllers
{
    public partial class CameraContoller : UIViewController
    {
        readonly int requestCode;
        UIView liveCameraStream;
        UIButton takePhotoButton;
        AVCaptureSession captureSession;
        AVCaptureDeviceInput captureDeviceInput;
        AVCaptureStillImageOutput stillImageOutput;
        internal event EventHandler<PhotoEventArgs> PhotoRead;

        public CameraContoller(int request) 
        {
            requestCode = request;    
        }

        ~CameraContoller()
        {
            takePhotoButton.TouchUpInside -= TakePhotoButton_TouchUpInside;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            SetupUserInterface();
            SetupEventHandlers();
            AuthorizeCameraUse();
            SetupLiveCameraStream();
        }

        void SetupUserInterface()
        {
            var centerButtonX = View.Bounds.GetMidX() - 35f;
            var bottomButtonY = View.Bounds.Bottom - 150;
            var buttonWidth = 70;
            var buttonHeight = 70;
            liveCameraStream = new UIView()
            {
                Frame = new CGRect(0f, 0f, View.Bounds.Width, View.Bounds.Height)
            };
            takePhotoButton = new UIButton
            {
                Frame = new CGRect(centerButtonX, bottomButtonY, buttonWidth, buttonHeight)
            };
            takePhotoButton.SetBackgroundImage(UIImage.FromFile("TakePhotoButton"), UIControlState.Normal);
            Add(liveCameraStream);
            Add(takePhotoButton);
        }

        void SetupEventHandlers()
        {
            takePhotoButton.TouchUpInside += TakePhotoButton_TouchUpInside;
        }

        async void AuthorizeCameraUse()
        {
            var autorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if(autorizationStatus != AVAuthorizationStatus.Authorized)
            {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }
        void SetupLiveCameraStream()
        {
            captureSession = new AVCaptureSession();
            var videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
            {
                Frame = liveCameraStream.Bounds
            };
            liveCameraStream.Layer.AddSublayer(videoPreviewLayer);
            var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
            ConfigureCameraForDevice(captureDevice);
            captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);
            stillImageOutput = new AVCaptureStillImageOutput
            {
                OutputSettings = new NSDictionary()
            };
            captureSession.AddOutput(stillImageOutput);
            captureSession.AddInput(captureDeviceInput);
            captureSession.StartRunning();
        }

        void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            NSError error;
            if(device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
            {
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
			}
            else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
			{
				device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
				device.UnlockForConfiguration();
			}
            else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
			{
				device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
				device.UnlockForConfiguration();
			}
        }

        private async void TakePhotoButton_TouchUpInside(object sender, EventArgs e)
        {
            var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
            var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            OnPhotoRead(new PhotoEventArgs(requestCode, false, jpegImageAsNsData.ToArray()));

        }
        void OnPhotoRead(PhotoEventArgs e)
        {
            var picked = PhotoRead;
            picked?.Invoke(null, e);
        }
    }
    internal class PhotoEventArgs : EventArgs
    {
        public int RequestId
        {
            get;
            private set;
        }
        public bool IsCancelled
        {
            get;
        }
        public Exception Error
        {
            get;
        }
        public byte[] Photo
        {
            get;
        }
        public PhotoEventArgs(int id, Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            RequestId = id;
            Error = error;
        }
        public PhotoEventArgs(int id, bool isCancelled, byte[] photo = null)
        {
            RequestId = id;
            IsCancelled = isCancelled;
            if (!IsCancelled && photo == null)
                throw new ArgumentNullException(nameof(photo));
            Photo = photo;
        }
    }
}

