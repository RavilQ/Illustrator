namespace Illustration.Helper
{
    public class FileHelper
    {
        public static string Save(IFormFile file, string rootPath, string folder)
        {
            string oldFileImage = file.FileName;
            string newFileName = Guid.NewGuid().ToString() + (oldFileImage.Length > 80 ? oldFileImage.Substring(oldFileImage.Length - 80) : oldFileImage);
            string path = Path.Combine(rootPath, folder, newFileName);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            return newFileName;
        }

        public static bool Delete(string rootPath, string folder, string fileName)
        {
            string path = Path.Combine(rootPath, folder, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }
    }
}
