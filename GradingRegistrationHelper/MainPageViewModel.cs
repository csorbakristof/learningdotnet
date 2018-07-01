using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GradingRegistrationHelper
{
    class MainPageViewModel
    {
        List<Student> students;

        internal async void LoadFromFolder(StorageFolder dir)
        {
            string folderAuthToken = Windows.Storage.AccessCache.
                StorageApplicationPermissions.FutureAccessList.Add(dir);

            var loader = new XlsLoader();   // Builder design pattern
            var files = await dir.GetFilesAsync();

            // Temp test, won't work either...
            var stream = new StreamReader(files[0].Path);
            stream.ReadLine();
            stream.Close();

            // Original continuation...
            var gradesFile = files.Where(s => s.Name.StartsWith("tema-osztalyzatok")).Single();
            var attandanceFiles = files.Where(s => s.Name.StartsWith("jegyimport")).ToArray();
            var advisorFile = files.Where(s => s.Name.StartsWith("Terheles_")).Single();
            loader.Load(gradesFile, attandanceFiles, advisorFile);
            students = loader.GetStudentList();

        }
    }
}
