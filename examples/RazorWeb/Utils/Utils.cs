namespace RazorWeb
{
    public class Utils
    {
        public static string GetImage(string serverPath, string nameImage)
        {
            string folder = "wwwroot\\uploads\\";
            string pathImage = LocalizeImage(serverPath, folder, nameImage);

            if (pathImage != null)
            {
                string folderPath = Path.GetDirectoryName(pathImage).Replace("\\", "/").Replace("wwwroot", "/");

                pathImage = "http://" + serverPath + $"{folderPath.Replace("//uploads", "/uploads")}/" + nameImage;
            }
            return pathImage;
        }

        public static string LocalizeImage(string serverPath, string folder, string nameImage)
        {
            try
            {
                string[] archives = Directory.GetFiles(folder);

                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).Equals(nameImage, StringComparison.OrdinalIgnoreCase))
                    {
                        return archive;
                    }
                }

                string[] subfolders = Directory.GetDirectories(folder);

                foreach (string subfolder in subfolders)
                {
                    string path = LocalizeImage(serverPath, subfolder, nameImage);
                    if (path != null)
                    {
                        return path;
                    }
                }
            }
            catch (Exception ex)
            {
                return $"   Erro ao acessar diretorio: {ex}   ";
            }
            return null;
        }
    }
}