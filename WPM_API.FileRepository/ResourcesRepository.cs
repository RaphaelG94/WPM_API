using System;
using System.Collections.Generic;
using System.Text;

namespace WPM_API.FileRepository
{
    public class ResourcesRepository:FileRepository
    {
        public ResourcesRepository(string connectionString, string folder) : base(connectionString, folder)
        {
        }
    }
}
