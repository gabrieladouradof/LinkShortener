using Domain.Commands;
using Domain.Handlers;
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
            _uploadFileHandler = uploadFileHandler;
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

//List<byte[]> data = new();

//foreach (var formFile in files)
//{
//    if (formFile.Length > 0)
//    {
//        using (var stream = new MemoryStream())
//        {
//            await formFile.CopyToAsync(stream);
//            data.Add(stream.ToArray());
//        }
//    }

//}