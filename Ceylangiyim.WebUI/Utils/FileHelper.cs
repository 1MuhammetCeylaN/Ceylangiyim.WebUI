namespace Ceylangiyim.WebUI.Utils
{
    public class FileHelper
    {
        public static async Task<string> FileLoaderAsync(IFormFile formFile, string filePath = "/Img/")
        {
            string fileName = "";
            if (formFile != null && formFile.Length > 0)
            {
                fileName = formFile.FileName.ToLower();
                string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName; // Uygulamanın çalıştığı dizini bulur.

                using var stream = new FileStream(directory, FileMode.Create);

                await formFile.CopyToAsync(stream);

            }

            return fileName;
        }

        public static bool FileRemover(string fileName, string filePath = "/Img/")
        {
            string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName;

            if (File.Exists(directory)) // Bir dosya varmı bunu kontrol eder.
            {
                File.Delete(directory); // Varsa siler ve geriye true döner yoksa false döner.
                return true;
            }
            return false;
        }

        public static bool FileRemoverWithColors(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return false;

            // Eğer tam yol "/Img/ProductsColors/dosya.jpg" gibi bir şeyse, başındaki '/' işaretini kaldır
            if (imageUrl.StartsWith("/"))
                imageUrl = imageUrl.Substring(1);

            // Tam dosya yolunu oluştur
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

    }
}
