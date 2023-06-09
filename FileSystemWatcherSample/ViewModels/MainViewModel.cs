﻿using System;
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
        public MainViewModel() 
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            FileInfo assemblyFile = new(GetType().Assembly.Modules.FirstOrDefault().FullyQualifiedName);
            var parentDirectory = assemblyFile.Directory;
            Directory.SetCurrentDirectory(parentDirectory.FullName);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            ReloadTitle(Path.Combine(parentDirectory.FullName, "title.txt"));
            ReloadContent(Path.Combine(parentDirectory.FullName, "contents.txt"));
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
                    ReloadContent(e.FullPath);
                    break;
                case "title.txt":
                    ReloadTitle(e.FullPath);
                    break;
                default:
                    break;
            }
        }

        void ReloadContent(String fullPath)
        {
            try
            {
                Content = File.ReadAllText(fullPath);
            }
            catch (IOException exc)
            {
                Content = "<unreadable>";
            }
        }

        void ReloadTitle(String fullPath)
        {
            try
            {
                Title = File.ReadAllText(fullPath);
            }
            catch (IOException exc)
            {
                Title = "<unreadable>";
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
