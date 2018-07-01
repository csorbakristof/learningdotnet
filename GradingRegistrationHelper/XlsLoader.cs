using ExcelReaderStandardLibrary;
using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Storage;

namespace GradingRegistrationHelper
{
    public class XlsLoader
    {
        private List<Student> students = new List<Student>();

        public ITableReader Reader { get; set; }

        public List<Student> GetStudentList() => this.students;

        public void Load(StorageFile gradesFile, StorageFile[] attendanceFiles, StorageFile advisorFile)
        {
            students = LoadGrades(gradesFile).ToList();
            MergeStudents(LoadAdvisors(advisorFile));
            MergeStudents(LoadAttendances(attendanceFiles));
        }

        private void MergeStudents(IEnumerable<Student> newStudents)
        {
            foreach (var s in newStudents)
            {
                Student toUpdate = students.Where(e => e.NeptunCode == s.NeptunCode).SingleOrDefault();
                if (toUpdate == null)
                    students.Add(s);
                else
                    toUpdate.Merge(s);
            }
        }

        private IEnumerable<Student> LoadGrades(StorageFile gradesFile)
        {
            var dict = Reader.Read(gradesFile.Path, 0, 4);
            return dict.Select(d => GradeEntryToStudent(d));
        }

        private IEnumerable<Student> LoadAttendances(StorageFile[] attendanceFiles)
        {
            IEnumerable<Student> result = null;
            foreach (var f in attendanceFiles)
            {
                var dict = Reader.Read(f.Path, 0, 1);
                var enumerable = dict.Select(d => GradeEntryToStudent(d));
                result = result == null ? enumerable : result.Concat(enumerable);
            }
            return result;
        }

        private IEnumerable<Student> LoadAdvisors(StorageFile advisorFile)
        {
            var dict = Reader.Read(advisorFile.Path, 1, 1);
            return dict.Select(d => AdvisorEntryToStudent(d));
        }

        static public readonly string GradingNameKey = "Hallgató neve";
        static public readonly string GradingNCodeKey = "Hallgató NEPTUN";
        static public readonly string GradingGradeKey = "Osztályzat";
        static public readonly string GradingSubjectKey = "Tárgy NEPTUN";
        private Student GradeEntryToStudent(Dictionary<string, string> entry)
        {
            var student = new Student();
            student.Name = entry[GradingNameKey];
            student.NeptunCode = entry[GradingNCodeKey];
            if (int.TryParse(entry[GradingGradeKey], out int grade))
                student.SetGrade(entry[GradingSubjectKey], grade);

            return student;
        }

        private Student AdvisorEntryToStudent(Dictionary<string, string> entry)
        {
            var student = new Student();
            student.Name = entry["Hallgató neve"];
            student.NeptunCode = entry["Hallg. nept"];
            student.Advisor = entry["Konzulens"];
            return student;
        }

        private Student AttendanceEntryToStudent(string SubjectCode, Dictionary<string, string> entry)
        {
            var student = new Student();
            student.Name = entry["Név"];
            student.NeptunCode = entry["Neptun kód"];
            student.AddAttendance(SubjectCode);
            return student;
        }
    }
}
