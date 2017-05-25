using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Square.Droid.Activities;
using Square.Droid.Dependencies;
using Square.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoService))]
namespace Square.Droid.Dependencies
{
    public class PhotoService : IPhotoService
    {
		private readonly Context context;
		private int requestId;
		private TaskCompletionSource<byte[]> completionSource;

        public bool IsCameraAvailable { get; }

        public PhotoService()
        {
			context = Android.App.Application.Context;
			IsCameraAvailable = context.PackageManager.HasSystemFeature(PackageManager.FeatureCamera);
        }

        public async Task<byte[]> TakePhotoAsync()
        {
			if (!IsCameraAvailable)
				throw new NotSupportedException("This device does not has a camera available");
            return await TakeAsync();
        }

		private Task<byte[]> TakeAsync()
		{
			var id = GetRequestId();
			var ntcs = new TaskCompletionSource<byte[]>(id);
			if (Interlocked.CompareExchange(ref completionSource, ntcs, null) != null)
				throw new InvalidOperationException("Only one operation can be active at a time");
			context.StartActivity(CreateReaderIntent(id));
            EventHandler<PhotoEventArgs> handler = null;
			handler = (s, e) =>
			{
				var tcs = Interlocked.Exchange(ref completionSource, null);
                PhotoActivity.CardRead -= handler;
				if (e.RequestId != id)
					return;
				if (e.IsCanceled)
					tcs.SetResult(null);
				else if (e.Error != null)
					tcs.SetException(e.Error);
				else
                    tcs.SetResult(e.Photo);
			};

			PhotoActivity.CardRead += handler;

			return completionSource.Task;
		}

		private Intent CreateReaderIntent(int id)
		{
			var reader = new Intent(context, typeof(PhotoActivity));
			reader.PutExtra(PhotoActivity.ExtraId, id);
			reader.PutExtra(PhotoActivity.ExtraFront, 0);
			reader.SetFlags(ActivityFlags.NewTask);
			return reader;
		}
		private int GetRequestId()
		{
			int id = requestId;
			if (requestId == Int32.MaxValue)
				requestId = 0;
			else
				requestId++;

			return id;
		}
    }
}
