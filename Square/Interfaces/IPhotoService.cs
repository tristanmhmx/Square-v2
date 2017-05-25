using System;
using System.Threading.Tasks;

namespace Square.Interfaces
{
    public interface IPhotoService
    {
        bool IsCameraAvailable { get; }
        Task<byte[]> TakePhotoAsync();
    }
}
