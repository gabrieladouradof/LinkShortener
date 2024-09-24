using Domain.Commands;
using Domain.Entities;
using Domain.Handlers;
using Domain.Infra.Repositories;
using Domain.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace TestUploadFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadFileController : ControllerBase
    {
        private readonly IFileRepository _fileRepository; // Repository injection
        private readonly UploadFileHandler _uploadFileHandler;
        private readonly string _uploadFolder;

        public UploadFileController(UploadFileHandler uploadFileHandler, IFileRepository fileRepository) 
        {
            _uploadFileHandler = uploadFileHandler;
            _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
        }

        [HttpPost]
        public async Task<ActionResult> Upload([FromForm] ICollection<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest();
            }


            //saving the file to disk
            if(!Directory.Exists(_uploadFolder)) 
            {
                Directory.CreateDirectory(_uploadFolder);   
            }

            foreach (var formFile in files)
            { 
                if (formFile.Length > 0) 
                {
                    //generate the code the six-character code 
                    string fileCode = GenerateFileCode(6);

                    var filePath = Path.Combine(_uploadFolder, formFile.FileName);

                    //save the path in the disk
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    //create one new entity of the file
                    var fileEntity = new FileEntity(filePath, fileCode)
                    {
                        FilePath = filePath,
                        FileCode = fileCode,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _fileRepository.SaveAsync(fileEntity);

                }
            }

            //These lines are unnecessary for now, they are saved publicly and encrypted in the database

            //var command = new UploadFileCommand(files);
            //await _uploadFileHandler.Handle(command);

            return Ok("Arquivos enviados com sucesso!"); 
        }

        //in this method need to correctly treat the "chars"
        private string GenerateFileCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Range(1, length)
                .Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }
    }

}

