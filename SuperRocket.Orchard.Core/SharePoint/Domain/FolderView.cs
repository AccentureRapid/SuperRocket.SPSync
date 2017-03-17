using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRocket.Orchard.SharePoint.Domain
{
    public class FolderView
    {
        //public FileCollection Files { get; set; }
        public int FilesCount { get; set; }
        public int ItemCount { get; set; }
        public string Name { get; set; }
        //public Folder ParentFolder { get; set; }
        public string ServerRelativeUrl { get; set; }
        //public FolderCollection Folders { get; set; }
        public List<FolderView> Children { get; set; }

        public List<File> Files { get; set; }

    }

    public class File
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string ServerRelativeUrl { get; set; }
    }
}
