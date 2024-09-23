namespace Domain.Entities
{
    public class FileEntity
    {
        public int Id { get; private set; }
        public string FilePath { get; private set; } = string.Empty;
        public string FileCode { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }  

        
        protected FileEntity() { }

        
        public FileEntity(string filePath, string fileCode)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            FileCode = fileCode ?? throw new ArgumentNullException(nameof(fileCode));
            CreatedAt = DateTime.UtcNow;
        }
    }
}

