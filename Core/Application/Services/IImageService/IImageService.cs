using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Services
{
    public interface IImageService
    {
        byte[] ResizeImage(byte[] image);
        string GetImageSrc(byte[] image);
    }
}
