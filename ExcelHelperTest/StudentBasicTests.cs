using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExcelHelper;

namespace ExcelHelperTest
{
    /// <summary>
    /// Summary description for StudentBasicTests
    /// </summary>
    [TestClass]
    public class StudentBasicTests
    {
        private Student s = new Student();
        public StudentBasicTests()
        {
        }

        const string SubjectCode = "VIAUA000";
        const string WrongSubjectCode = "ThereIsNoSuchCode";
        const string WrongSubjectCode2 = "ThereIsNoSuchCode2";

        [TestMethod]
        public void CanStoreGrade()
        {
            s.SetGrade(SubjectCode, 5);
            s.AddAttendance(SubjectCode);
            Assert.AreEqual(5, s.GetGrade(SubjectCode));
            expectGradingException(s, WrongSubjectCode);
        }

        [TestMethod]
        public void CanStoreAttendance()
        {
            s.AddAttendance(SubjectCode);
            Assert.IsTrue(s.IsAttending(SubjectCode));
            Assert.IsFalse(s.IsAttending(WrongSubjectCode));
        }

        #region Queries for grade with various attendance and graded subject patterns
        [TestMethod]
        public void NoAttendedSubject_AskedGradedSubject_Rejects()
        {
            s.SetGrade(SubjectCode, 5);
            expectGradingException(s, SubjectCode);
        }

        [TestMethod]
        public void NoAttendedSubject_AskedDifferentSubject_Rejects()
        {
            s.SetGrade(SubjectCode, 5);
            expectGradingException(s, WrongSubjectCode);
        }

        [TestMethod]
        public void SingleDifferentAttendedSubject_AskedAttendedSubject_ReturnsGrade()
        {
            s.SetGrade(SubjectCode, 5);
            s.AddAttendance(WrongSubjectCode);
            Assert.AreEqual(string.Format(
                Student.SingleMismatchingAttendanceOK, WrongSubjectCode, SubjectCode),
                s.GetGradingSituation(WrongSubjectCode));
            Assert.AreEqual(5, s.GetGrade(WrongSubjectCode));
        }

        [TestMethod]
        public void SingleDifferentAttendedSubject_AskedUnattendedSubject_Rejects()
        {
            s.SetGrade(SubjectCode, 5);
            s.AddAttendance(WrongSubjectCode);
            expectGradingException(s, WrongSubjectCode2);
        }

        [TestMethod]
        public void SingleDifferentAttendedSubject_AskedUnattendedButGradedSubject_Rejects()
        {
            s.SetGrade(SubjectCode, 5);
            s.AddAttendance(WrongSubjectCode);
            expectGradingException(s, SubjectCode);
        }

        [TestMethod]
        public void MultipleAttendedSubject_AskedAttendedGradedSubject_ReturnsGrade()
        {
            s.SetGrade(SubjectCode, 5);
            s.AddAttendance(SubjectCode);
            s.AddAttendance(WrongSubjectCode);
            Assert.AreEqual(Student.GradingOK, s.GetGradingSituation(SubjectCode));
            Assert.AreEqual(5, s.GetGrade(SubjectCode));
        }

        [TestMethod]
        public void MultipleAttendedSubject_AskedUnattendedGradedSubject_Rejects()
        {
            s.SetGrade(SubjectCode, 5);
            s.AddAttendance(WrongSubjectCode);
            s.AddAttendance(WrongSubjectCode2);
            expectGradingException(s, SubjectCode);
        }

        [TestMethod]
        public void MultipleAttendedSubject_AskedAttendedUngradedSubject_ReturnsGrade()
        {
            s.SetGrade(SubjectCode, 5);
            s.AddAttendance(SubjectCode);
            s.AddAttendance(WrongSubjectCode);
            Assert.AreEqual(string.Format(
                Student.MultipleAttendedSubjectsThisOneUngradedOK, SubjectCode, WrongSubjectCode),
                s.GetGradingSituation(WrongSubjectCode));
            Assert.AreEqual(5, s.GetGrade(WrongSubjectCode));
        }

        private void expectGradingException(Student s, string subject)
        {
            bool eThrown = false;
            try
            {
                s.GetGrade(subject);
            }
            catch (ArgumentException)
            {
                eThrown = true;
            }
            Assert.IsTrue(eThrown);
        }
        #endregion

        #region Setting advisor, query ungraded student
        [TestMethod]
        public void AttendingButUngraded_GradingSituationReturnsAdvisorName()
        {
            s.AddAttendance(SubjectCode);
            s.Advisor = "Minta Mókus";
            Assert.AreEqual(
                string.Format(Student.Ungraded,s.Advisor),
                s.GetGradingSituation(SubjectCode));
            Assert.AreEqual(null, s.GetGrade(SubjectCode));
        }

        [TestMethod]
        public void UnattendingAndUngraded_GradingThrowsException()
        {
            s.AddAttendance(SubjectCode);
            expectGradingException(s, WrongSubjectCode);
        }
        #endregion

    }
}
