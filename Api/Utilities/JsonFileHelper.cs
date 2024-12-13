using System.Text.Json;

namespace Api.Utilities
{
    public class JsonFileHelper
    {
        private readonly ILogger<JsonFileHelper> logger;

        public JsonFileHelper(ILogger<JsonFileHelper> logger)
        {
            this.logger = logger;
        }

        public async Task<List<T>> GetObjectListFromJsonFile<T>(string filePath)
        {
            try 
            {
                if (!System.IO.File.Exists(filePath)) 
                {
                    await File.WriteAllTextAsync(filePath, "");
                }               
                if (System.IO.File.Exists(filePath))
                {
                    var json = await System.IO.File.ReadAllTextAsync(filePath);
                    return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                }                
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deserializing list from JSON file {filePath}; exception={ex.Message}");
            }
            return new List<T>();
        }

        public async Task<bool> SaveObjectListToJsonFile<T>(List<T> objectList, string filePath)
        {
            try
            {
                var json = JsonSerializer.Serialize(objectList, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error saving object list to JSON file {filePath}; exception={ex.Message}");
            }
            return false;
        }
    }
};
