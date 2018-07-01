using System;
using System.Collections.Generic;

namespace GradingRegistrationHelper
{
    public class Student
    {
        private string GradedSubject = null;
        private int? Grade = null;
        private List<string> Attendances = new List<string>();

        public static readonly string GradingOK = "OK";
        public static readonly string SingleMismatchingAttendanceOK = "OK, subject mismatch: attended {0}, graded {1}";
        public static string MultipleAttendedSubjectsThisOneUngradedOK
            = "OK, multiple attendances, graded {0} instead of this {1}.";
        public static string Ungraded = "Ungraded, advisor: {0}";

        public string Advisor { get; set; }

        public void SetGrade(string subject, int grade)
        {
            GradedSubject = subject;
            Grade = grade;
        }

        public void AddAttendance(string subject)
        {
            Attendances.Add(subject);
        }

        public bool IsAttending(string subject)
        {
            return Attendances.Contains(subject);
        }

        public int? GetGrade(string subject)
        {
            GetGradingSituation(subject);   // Remains for exception checking
            return Grade;
        }

        public string GetGradingSituation(string subject)
        {
            if (!Attendances.Contains(subject))
                throw new ArgumentException("Should not ask grading for unattended subject.");

            if (Grade == null)
                return string.Format(Ungraded, Advisor);

            // Now we are sure they attended the asked subject.
            if (Attendances.Count == 1)
            {
                return (subject == GradedSubject)
                    ? GradingOK
                    : string.Format(SingleMismatchingAttendanceOK, subject, GradedSubject);
            }
            else // Multiple attendances
            {
                return (GradedSubject == subject)
                    ? GradingOK
                    : string.Format(MultipleAttendedSubjectsThisOneUngradedOK,
                        GradedSubject, subject);
            }
        }
    }
}
