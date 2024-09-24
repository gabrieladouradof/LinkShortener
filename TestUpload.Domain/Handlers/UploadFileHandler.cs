using Domain.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;

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
                //generate code from file
                var fileCode = GenerateFileCode(6);
                var filePath = await SaveFileToDisk(formFile, fileCode);

               
                var fileEntity = new FileEntity(filePath, fileCode);
                await _fileRepository.SaveAsync(fileEntity);
            }
        }
    }
    private async Task<string> SaveFileToDisk(IFormFile formFile, string fileCode)
    {
        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        if(!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var filePath = Path.Combine(uploadFolder, $"{fileCode}{Path.GetExtension(formFile.FileName)}");

        Console.WriteLine($"Recebendo arquivo {formFile.FileName}");

        //save file to system
        //documentation of the methods: https://learn.microsoft.com/pt-br/dotnet/api/system.io.file.open?view=net-8.0
        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None)) 
        {
            await formFile.CopyToAsync(stream); 

        } 
        return filePath;
    }
    private string GenerateFileCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }
}

