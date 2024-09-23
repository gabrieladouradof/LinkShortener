using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IRepositories;
using TestUpload.Domain.Infra.Data;

namespace Domain.Infra.Repositories
{

    public class FileRepository : IFileRepository
    {
        private readonly DatabaseContext _context;

        public FileRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(FileEntity fileEntity)
        {
            _context.Files.Add(fileEntity);
            await _context.SaveChangesAsync();
        }
    }

}
