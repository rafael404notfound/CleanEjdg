
namespace CleanEjdg.Core.Application.Services
{
    public class ImageService : IImageService
    {
        public string GetImageSrc(byte[] image)
        {
            if (image != null) return "data:image/jpeg;base64," + Convert.ToBase64String(image);
            else return "";
        }

        public byte[] ResizeImage(byte[] buffer)
        {
            using var image = Image.Load(buffer);
            int smallestDimension = Math.Min(image.Height, image.Width);
            image.Mutate(x => x.Crop(new SixLabors.ImageSharp
                .Rectangle((image.Width - smallestDimension) / 2, (image.Height - smallestDimension) / 2, smallestDimension, smallestDimension)));
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.Metadata.DecodedImageFormat);
                return ms.ToArray();
            }
            //using (MemoryStream stream = new MemoryStream(imageBytes))

            //{
            /*Image originalImage = Image.FromStream(image.OpenReadStream());
            int largestDimension = Math.Max(originalImage.Height, originalImage.Width);
            Size squareSize = new Size(largestDimension, largestDimension);
            Bitmap squareImage = new Bitmap(squareSize.Width, squareSize.Height);
            using (Graphics graphics = Graphics.FromImage(squareImage))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, squareSize.Width, squareSize.Height);
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                graphics.DrawImage(originalImage, (squareSize.Width / 2) - (originalImage.Width / 2), (squareSize.Height / 2) - (originalImage.Height / 2), originalImage.Width, originalImage.Height);
            }*/

            /*Bitmap orig = new Bitmap(stream);
            int dim = Math.Max(orig.Width, orig.Height);
            Bitmap dest;
            using (Graphics origG = Graphics.FromImage(orig))
            {
                dest = new Bitmap(dim, dim, origG);
            }
            using (Graphics g = Graphics.FromImage(dest))
            {
                Pen white = new Pen(Color.White, 22);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, dim, dim);
                g.DrawImage(orig, new Point((dim - orig.Width) / 2, (dim - orig.Height) / 2));
            }*/

            //ImageConverter converter = new ImageConverter();
            //return (byte[])converter.ConvertTo(squareImage, typeof(byte[]));
            //}
        }
    }
}
