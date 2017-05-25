using System;
using System.Threading.Tasks;
using Square.Interfaces;
using Square.iOS.Dependencies;
using Xamarin.Forms;
using UIKit;
using System.Threading;
using Square.iOS.ViewControllers;

[assembly: Dependency(typeof(PhotoService))]
namespace Square.iOS.Dependencies
{
    public class PhotoService : IPhotoService
    {
        UINavigationController navigationController;
        int requestId;
        TaskCompletionSource<byte[]> completionSource;
        CameraContoller cameraController;
        public PhotoService()
        {
            navigationController = FindNavigationController();
            IsCameraAvailable = UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera);
        }

        public bool IsCameraAvailable { get; }

        public async Task<byte[]> TakePhotoAsync()
        {
            if (!IsCameraAvailable)
                throw new NotSupportedException("Este dispositivo no tiene camara");
            return await TakeAsync();
        }

        Task<byte[]> TakeAsync()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            if(window == null)
            {
                throw new InvalidOperationException("No hay una ventana activa");
            }
            if(navigationController == null)
            {
                throw new InvalidOperationException("No hay un controlador de navegacion en la aplicación");
            }
            var id = GetRequestId();
            var ntcs = new TaskCompletionSource<byte[]>(id);
            if (Interlocked.CompareExchange(ref completionSource, ntcs, null) != null)
                throw new InvalidOperationException("Sólo se puede hacer una operación a la vez");
            cameraController = new CameraContoller(id);
            navigationController.PresentModalViewController(cameraController, true);
            EventHandler<PhotoEventArgs> handler = null;
            handler = (s, e) =>
            {
                var tcs = Interlocked.Exchange(ref completionSource, null);
                cameraController.PhotoRead -= handler;
                if(e.RequestId != id)
                {
                    navigationController.DismissModalViewController(true);
                }
                if(e.IsCancelled)
                {
                    navigationController.DismissModalViewController(true);
                    tcs.SetResult(null);
                }
                else if(e.Error != null)
                {
                    navigationController.DismissModalViewController(true);
                    tcs.SetException(e.Error);
                }
                else
                {
                    navigationController.DismissModalViewController(true);
                    tcs.SetResult(e.Photo);
                }
            };

            cameraController.PhotoRead += handler;

            return completionSource.Task;
        }
        int GetRequestId()
        {
            var id = requestId;
            if (requestId == int.MaxValue)
                requestId = 0;
            else
                requestId++;
            return id;
        }
        UINavigationController FindNavigationController()
        {
            foreach(var window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController.NavigationController != null)
                    return window.RootViewController.NavigationController;
                var val = CheckSubs(window.RootViewController.ChildViewControllers);
                if (val != null)
                    return val;
            }
            return null;
        }

        UINavigationController CheckSubs(UIViewController[] controllers)
        {
            foreach(var controller in controllers)
            {
                if (controller.NavigationController != null)
                    return controller.NavigationController;
                var val = CheckSubs(controller.ChildViewControllers);
                if (val != null)
                    return val;
            }
            return null;
        }
    }
}
