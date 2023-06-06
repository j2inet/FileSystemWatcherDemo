using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemWatcherSample.ViewModels
{
    public  class MainViewModel: ViewModelBase
    {
        public MainViewModel() {


            var assemblyFile = new FileInfo(this.GetType().Assembly.Modules.FirstOrDefault().FullyQualifiedName);
            var parentDirectory = assemblyFile.Directory;
            Directory.SetCurrentDirectory(parentDirectory.FullName);


            FileSystemWatcher fsw = new FileSystemWatcher(parentDirectory.FullName);
            fsw.Filter = "*.txt";
            fsw.Created += FswCreatedOrChanged;
            fsw.Changed += FswCreatedOrChanged;
            fsw.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName;
            fsw.EnableRaisingEvents = true;
        }


        void FswCreatedOrChanged(object sender, FileSystemEventArgs e)
        {
            var name = e.Name.ToLower();
            switch (name)
            {
                case "contents.txt":
                    Content = File.ReadAllText(e.FullPath);
                    break;
                case "title.txt":
                    Title = File.ReadAllText(e.FullPath);
                    break;
                default:
                    break;
            }
        }

        private string _title = "<<empty>>";
        public string Title
        {
            get => _title;
            set => SetValueIfChanged(() => Title, () => _title, value);
        }

        string _content = "<<empty>>";
        public string Content
        {
            get => _content;
            set => SetValueIfChanged(()=>Content, ()=>_content, value);
        }

    }
}
