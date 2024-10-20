﻿using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using WebBimba.Interfaces;

namespace WebBimba.Services
{
    public class ImageWorker : IImageWorker
    {
        private readonly IWebHostEnvironment _environment;
        private const string dirName = "uploading";
        private int[] sizes = [50, 150, 300, 600, 1200];
        public ImageWorker(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string Save(string url)
        {
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
                        return CompresImage(imageBytes);
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
        }

        /// <summary>
        /// Стискаємо фото
        /// </summary>
        /// <param name="bytes">Набір байтів фото</param>
        /// <returns>Повертаємо назву збереженого фото</returns>
        private string CompresImage(byte[] bytes)
        {
            string imageName = Guid.NewGuid().ToString() + ".webp";
            
            var dirSave = Path.Combine(_environment.WebRootPath, dirName);
            if (!Directory.Exists(dirSave))
            {
                Directory.CreateDirectory(dirSave);
            }

            
            foreach (int size in sizes)
            {
                var path = Path.Combine(dirSave, $"{size}_{imageName}");
                using (var image = Image.Load(bytes))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(size, size),
                        Mode = ResizeMode.Max
                    }));
                    image.SaveAsWebp(path);
                    //image.Save(path, new WebpEncoder()); // Save the resized image
                }
            }

            return imageName;
        }

        public void Delete(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return; // Ігноруємо порожні назви файлів
            }

            // Складаємо шляхи до всіх зменшених версій
            var filePaths = sizes.Select(size => Path.Combine(_environment.WebRootPath, dirName, $"{size}_{fileName}"))
                               .ToList();

            // Видаляємо всі файли одночасно для покращення продуктивності
            Parallel.ForEach(filePaths, filePath =>
            {
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        // Логуємо або обробляємо помилки видалення окремих файлів
                        Console.WriteLine($"Помилка видалення файлу '{filePath}': {ex.Message}");
                    }
                }
            });
        }

        public string Save(IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    return CompresImage(imageBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return String.Empty;
            }
        }
    }
}
