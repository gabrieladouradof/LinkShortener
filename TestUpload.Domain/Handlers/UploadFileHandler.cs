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
                // Gerar código do arquivo
                var fileCode = Guid.NewGuid().ToString();
                var filePath = Path.Combine("Uploads", fileCode + Path.GetExtension(formFile.FileName));

                // Salvar arquivo no sistema de arquivos
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                // Criar entidade de domínio e persistir no banco
                var fileEntity = new FileEntity(filePath, fileCode);
                await _fileRepository.SaveAsync(fileEntity);
            }
        }
    }
}

