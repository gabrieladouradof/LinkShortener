using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commands
{
    public class UploadFileCommand
    {
        public ICollection<IFormFile> Files { get; }

        public UploadFileCommand(ICollection<IFormFile> files)
        {
            Files = files ?? throw new ArgumentNullException(nameof(files));
        }
    }
}
