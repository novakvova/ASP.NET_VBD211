using WebBimba.Interfaces;

namespace WebBimba.Services
{
    public class ImageWorker : IImageWorker
    {
        private readonly IWebHostEnvironment _environment;
        public ImageWorker(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string Save(string url)
        {
            string imageName = Guid.NewGuid().ToString() + ".jpg";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Send a GET request to the image URL
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Check if the response status code indicates success (e.g., 200 OK)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the image bytes from the response content
                        byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                        var dirName = "uploading";
                        var dirSave = Path.Combine(_environment.WebRootPath, dirName);
                        if (!Directory.Exists(dirSave))
                        {
                            Directory.CreateDirectory(dirSave);
                        }
                        var path = Path.Combine(dirSave, imageName);
                        File.WriteAllBytes(path, imageBytes);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve image. Status code: {response.StatusCode}");
                        return String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return String.Empty;
            }
            return imageName;
        }
    }
}
