﻿using CpaRepository.ModelsDb;
using CpaRepository.ViewModel.AgreedModules;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Service
{
    public interface IFileService
    {
        public Task<string> SaveFileAsync(IFormFile file, string pathFolder);
        public void DeleteFile(string path);
        public void Move(string sourceFileName, string destFileName);

    }
}