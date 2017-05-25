using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Hardware;
using Android.OS;
using Android.Views;
using Android.Widget;
#pragma warning disable 618
using Camera = Android.Hardware.Camera;
#pragma warning restore 618
namespace Square.Droid.Activities
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class PhotoActivity : Activity, TextureView.ISurfaceTextureListener
    {
#pragma warning disable 618
		private Camera camera;
#pragma warning restore 618
		private Button takePhotoButton;
		private CameraFacing cameraType;
		private TextureView textureView;
		internal static event EventHandler<PhotoEventArgs> CardRead;

		// ReSharper disable InconsistentNaming
		internal const string ExtraId = "id";
		internal const string ExtraFront = "android.intent.extras.CAMERA_FACING";
		// ReSharper enable InconsistentNaming

		private int id;
		private int front;

		private byte[] imageBytes;

		protected override void OnSaveInstanceState(Bundle outState)
		{
			outState.PutInt(ExtraId, id);
			outState.PutInt(ExtraFront, front);
			base.OnSaveInstanceState(outState);
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var b = savedInstanceState ?? Intent.Extras;

			id = b.GetInt(ExtraId);
			front = b.GetInt(ExtraFront);

			SetContentView(Resource.Layout.CameraLayout);

			cameraType = (CameraFacing)front;

			takePhotoButton = FindViewById<Button>(Resource.Id.takePhotoButton);
			takePhotoButton.Click += TakePhotoButtonTapped;

			textureView = FindViewById<TextureView>(Resource.Id.textureView);
			textureView.SurfaceTextureListener = this;

		}

		/// <summary>
		/// Events when surfacetexture is available, sets camera parameters
		/// </summary>
		/// <param name="surface">Surface</param>
		/// <param name="width">Width</param>
		/// <param name="height">Height</param>
		public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
		{
#pragma warning disable 618
			camera = Camera.Open((int)cameraType);
			var parameters = camera.GetParameters();
			if (parameters.SupportedFocusModes.Contains(Camera.Parameters.FocusModeContinuousPicture))
			{
				parameters.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
			}
			camera.SetParameters(parameters);
#pragma warning restore 618
			textureView.LayoutParameters = new FrameLayout.LayoutParams(width, height);
			camera.SetPreviewTexture(surface);
			PrepareAndStartCamera();
		}

		/// <summary>
		/// Does nothing
		/// </summary>
		/// <param name="surface">Surface</param>
		/// <returns></returns>
		public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
		{
			return true;
		}

		/// <summary>
		/// Resets camera
		/// </summary>
		/// <param name="surface">Surface</param>
		/// <param name="width">Width</param>
		/// <param name="height">Height</param>
		public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
		{
			PrepareAndStartCamera();
		}

		/// <summary>
		/// Does nothing
		/// </summary>
		/// <param name="surface"></param>
		public void OnSurfaceTextureUpdated(SurfaceTexture surface)
		{

		}

		private void PrepareAndStartCamera()
		{
			camera.StopPreview();

			var display = WindowManager.DefaultDisplay;
			if (display.Rotation == SurfaceOrientation.Rotation0)
			{
				camera.SetDisplayOrientation(90);
			}

			if (display.Rotation == SurfaceOrientation.Rotation270)
			{
				camera.SetDisplayOrientation(180);
			}

			camera.StartPreview();
		}

		private async void TakePhotoButtonTapped(object sender, EventArgs e)
		{
			try
			{
				camera.StopPreview();
				camera.Release();

				var image = textureView.Bitmap;
				using (var imageStream = new MemoryStream())
				{
					await image.CompressAsync(Bitmap.CompressFormat.Jpeg, 50, imageStream);
					image.Recycle();
					imageBytes = imageStream.ToArray();
				}
                OnCardRead(new PhotoEventArgs(id, false, imageBytes));
				Finish();

			}
			catch (Exception ex)
			{
				OnCardRead(new PhotoEventArgs(id, ex));
				Finish();
			}
		}


		private static void OnCardRead(PhotoEventArgs e)
		{
			var picked = CardRead;
			picked?.Invoke(null, e);
		}
    }
	internal class PhotoEventArgs : EventArgs
	{
		public PhotoEventArgs(int id, Exception error)
		{
			if (error == null)
				throw new ArgumentNullException(nameof(error));

			RequestId = id;
			Error = error;
		}

		public PhotoEventArgs(int id, bool isCanceled, byte[] card = null)
		{
			RequestId = id;
			IsCanceled = isCanceled;
			if (!IsCanceled && card == null)
				throw new ArgumentNullException(nameof(card));

			Photo = card;
		}

		public int RequestId
		{
			get;
			private set;
		}

		public bool IsCanceled
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

	}
}
