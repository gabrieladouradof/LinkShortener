using Domain.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IRepositories;

namespace Domain.Handlers;
public class UploadFileHandler
{
    private readonly IFileRepository _fileRepository;

    public UploadFileHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task Handle(UploadFileCommand command)
    {
        if (command.Files == null || !command.Files.Any())
        {
            throw new InvalidOperationException("Nenhum arquivo foi enviado.");
        }

        foreach (var formFile in command.Files)
        {
            if (formFile.Length > 0)
            {
                // generate code from file
                var fileCode = Guid.NewGuid().ToString();
                var filePath = Path.Combine("Uploads", fileCode + Path.GetExtension(formFile.FileName));

                //save file to file system
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                //Create domain entity and persist in the database
                var fileEntity = new FileEntity(filePath, fileCode);
                await _fileRepository.SaveAsync(fileEntity);
            }
        }
    }
}

