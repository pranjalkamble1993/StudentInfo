using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StudentInfo.Models;
using StudentInfo.Models.Context;
using StudentInfo.Models.Entities;

namespace StudentInfo.Controllers
{
	/// <summary>
	/// Student info
	/// </summary>
	public class StudentController : Controller
	{
		#region Member Variable
		/// <summary>
		/// Db context
		/// </summary>
		private StudentContext _studentContext;
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="studentContext">DB context injection</param>
		public StudentController(StudentContext studentContext)
		{
			_studentContext = studentContext;
		}
		#endregion

		#region Public function
		/// <summary>
		/// Get student data
		/// </summary>
		/// <returns>List of student data</returns>
		public IActionResult Index()
		{
			List<StudentData> lstStudentData = GetStudentData(); 
			return View(lstStudentData);
		}

		/// <summary>
		/// Edit student data
		/// </summary>
		/// <param name="studentData">Student data</param>
		/// <returns>Student data</returns>
		public IActionResult Edit(StudentData studentData)
		{
			GetSchoolList();
			return View(studentData);
		}

		/// <summary>
		/// Save student data
		/// </summary>
		/// <param name="studentData">Student data</param>
		/// <returns>Student data</returns>
		[HttpPost]
		public IActionResult EditStudentData(StudentData studentData)
		{
			try
			{
				if (ModelState.IsValid)
				{
					SaveStudentData(studentData);

					return RedirectToAction("Index");
				}
				return View();
			}
			catch (Exception)
			{
				return RedirectToAction("Index");
			}
		}
		#endregion

		#region Private function
		/// <summary>
		/// Get student data from database
		/// </summary>
		/// <returns>List of student data</returns>
		private List<StudentData> GetStudentData()
		{
			List<StudentData> studentDatas = new List<StudentData>();

			var parentStudentId = new List<ParentStudent>();
			parentStudentId = _studentContext.ParentStudent.OrderBy(a => a.Id).ToList();
			foreach (var item in parentStudentId)
			{
				StudentData studentData = new StudentData();
				var parentInfo = _studentContext.Users.FirstOrDefault(p => p.Id == item.ParentId && p.UserType == UserType.Parent);
				if (parentInfo != null)
				{
					studentData.ParentName = string.Join(' ', parentInfo.FirstName, parentInfo.LastName);
					studentData.ParentEmail = parentInfo.Email;
					studentData.ParentMobile = parentInfo.Phone;
				}
				
				var studentInfo = _studentContext.Users.FirstOrDefault(s => s.Id == item.StudentId && s.UserType == UserType.Student);
				if (studentInfo != null)
				{
					studentData.Id = studentInfo.Id;
					studentData.StudentName = string.Join(' ', studentInfo.FirstName, studentInfo.LastName);
					studentData.Active = studentInfo.Active;

					var studentClassInfo = _studentContext.StudentClass.FirstOrDefault(s => s.StudentId == studentInfo.Id);
					if (studentClassInfo != null)
					{
						var studentSchoolInfo = _studentContext.SchoolClass.FirstOrDefault(s => s.Id == studentClassInfo.ClassId);
						studentData.StudentClass = studentSchoolInfo.Name;
					}
				}

				studentDatas.Add(studentData);
			}
			return studentDatas;
		}

		/// <summary>
		/// Save student data to database
		/// </summary>
		/// <param name="studentData">Student data</param>
		private void SaveStudentData(StudentData studentData)
		{
			var parentStudentId = new List<ParentStudent>();
			parentStudentId = _studentContext.ParentStudent.Where(a => a.StudentId == studentData.Id).ToList();
			foreach (var item in parentStudentId)
			{
				var parentInfo = _studentContext.Users.FirstOrDefault(p => p.Id == item.ParentId && p.UserType == UserType.Parent);
				if (parentInfo != null)
				{					
					parentInfo.Email = studentData.ParentEmail;
					parentInfo.Phone = studentData.ParentMobile;
				}

				var studentInfo = _studentContext.Users.FirstOrDefault(s => s.Id == item.StudentId && s.UserType == UserType.Student);
				if (studentInfo != null)
				{
					string[] name = studentData.StudentName.Split(' ');
                    if (name.Length > 0)
                    {
                        if (name.Length == 1)
                        {
							studentInfo.FirstName = name[0];
						}
						else
						{
							studentInfo.FirstName = name[0];
							studentInfo.LastName = name[1];
						}
					}
					
					studentInfo.Active = studentData.Active;

					var studentClassInfo = _studentContext.StudentClass.FirstOrDefault(s => s.StudentId == studentInfo.Id);					
					if (studentClassInfo != null)
					{
						studentClassInfo.ClassId = Convert.ToInt32(studentData.StudentClass);
					}
				}

				_studentContext.SaveChanges();
			}
		}

		/// <summary>
		/// Get school list
		/// </summary>
		private void GetSchoolList()
		{
			try
			{
				List<SchoolClass> schoolClasses = new List<SchoolClass>();
				schoolClasses = _studentContext.SchoolClass.OrderBy(s => s.Id).ToList();
				schoolClasses.Insert(0, new SchoolClass { Name = "Please select School" });
				ViewBag.SchoolList = schoolClasses;
			}
			catch (Exception)
			{
				throw;
			}
		}
		#endregion
	}
}
