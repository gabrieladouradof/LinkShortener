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
        private readonly UploadFileHandler _uploadFileHandler;

        public UploadFileController(UploadFileHandler uploadFileHandler) 
        {
            _uploadFileHandler = uploadFileHandler ?? throw new ArgumentNullException(nameof(uploadFileHandler));
           
        }

        [HttpPost]
        public async Task<ActionResult> Upload([FromForm] ICollection<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest();
            }

            var command = new UploadFileCommand(files);
            await _uploadFileHandler.Handle(command);

            return Ok("Arquivos enviados com sucesso!"); 
        }
    }

}

