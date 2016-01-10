using System.Linq;
using System.Drawing;
using System.Reflection;
using System.IO;
using IDenticard.AccessUI;

namespace WPFSecurityControlSystem
{
    public class ResourcesHelper
    {
        public const string ImagesFolderName = "Images";

        static string _imagesFolderLocation;
        public static string ImagesFolderLocation
        {
            get
            {
                if (string.IsNullOrEmpty(_imagesFolderLocation))
                {
                    string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    DirectoryInfo imagesParentFolder = new DirectoryInfo(path);

                    while (imagesParentFolder != null && !imagesParentFolder.GetDirectories(ImagesFolderName).Any())
                        imagesParentFolder = imagesParentFolder.Parent;

                    if (new DirectoryInfo(path) != null)
                        _imagesFolderLocation = Path.Combine(imagesParentFolder.FullName, "Images");
                }
                return _imagesFolderLocation;
            }
        }

        //public static string GetImagePath(int imageIndex)
        //{
        //    Resources.ResourceManager.GetObject();
        //}

        public static string GetImagePath(string imageName)
        {
            return Path.Combine(ImagesFolderLocation, imageName);
        }

        public static string GetImagePathForHWItem(object hwNode)
        {
            string fileName = string.Empty;

            if (hwNode is string)
            {
                string nodeTypeName = (string)hwNode;
                if (nodeTypeName.Contains("Collection"))
                    fileName = ResourcesHelper.GetImagePath("Folder.png");
                else if (nodeTypeName.Contains("Site"))
                    fileName = ResourcesHelper.GetImagePath("Site.png");
                else if (nodeTypeName.Contains("SCP"))
                    fileName = ResourcesHelper.GetImagePath("Controller.png");
                else if (nodeTypeName.Contains("SIO"))
                    fileName = ResourcesHelper.GetImagePath("IOBoard.png");
                else
                {

                }
            }
            else
            {
                if (hwNode is bool && (bool)hwNode)
                    fileName = ResourcesHelper.GetImagePath("Folder.png");
                else if (hwNode is Site)
                    fileName = ResourcesHelper.GetImagePath("Site.png");
                else if (hwNode is SCP)
                    fileName = ResourcesHelper.GetImagePath("Controller.png");
                else if (hwNode is SIO)
                    fileName = ResourcesHelper.GetImagePath("IOBoard.png");
            }
            return Path.Combine(ImagesFolderLocation, fileName);
        }

        public static Image GetImage(string fileName)
        {
            try
            {
                Image image = Image.FromFile(Path.Combine(ImagesFolderLocation, fileName));
                return image;
            }
            catch
            {
                //Image.GetPixelFormatSize();
            }

            return null;
        }
    }
}
