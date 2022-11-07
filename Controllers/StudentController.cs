using Microsoft.AspNetCore.Mvc;
using StudentManagementService.Data;
using StudentManagementService.Models;

namespace StudentManagementService.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentDbContext _studentDbContext;
        public StudentController(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }
        public IActionResult Index()
        {
            try
            {
                var studentlist = from std in _studentDbContext.tbl_Students
                                  join tch in _studentDbContext.tbl_Teachers
                                  on std.T_Id equals tch.Id
                                  into Data
                                  from tch in Data.DefaultIfEmpty()
                                  select new Student
                                  {
                                      Id = std.Id,
                                      FirstName = std.FirstName + " ",
                                      LastName = std.LastName,
                                      DateOfBirth = std.DateOfBirth,
                                      Gender = std.Gender,
                                      TeacherName = tch == null ? "" : tch.FirstName + " " + tch.LastName,
                                  };
                return View(studentlist);
            }

            catch (Exception e)
            {
                return View();
            }
        }

        public IActionResult Create()
        {
            GetTeacher();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (student.Id == 0)
                    {
                        _studentDbContext.tbl_Students.Add(student);
                        _studentDbContext.SaveChanges();
                    }
                }

                return RedirectToAction("Index");

            }

            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Edit(int? id)
        {
            GetTeacher();
            if (id == 0|| id==null)
            {
                return NotFound();
            }

           var idFromDb= _studentDbContext.tbl_Students.Find(id);
            if (idFromDb == null)
            {
                return NotFound();
            }
            return View(idFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _studentDbContext.tbl_Students.Update(student);
                    _studentDbContext.SaveChanges();
                }

                return RedirectToAction("Index");

            }

            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int? id)
        {
            GetTeacher();
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var idFromDb = _studentDbContext.tbl_Students.Find(id);
            if (idFromDb == null)
            {
                return NotFound();
            }
            return View(idFromDb);
        }

        [HttpPost]
        public IActionResult Delete(Student student)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                //    _studentDbContext.tbl_Students.Remove(student);
                //    _studentDbContext.SaveChanges();
                //}
                _studentDbContext.tbl_Students.Remove(student);
                _studentDbContext.SaveChanges();
                return RedirectToAction("Index");

            }

            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }
        private void GetTeacher()
        {
            try
            {
                List<Teacher> teachersList = new List<Teacher>();
                teachersList = _studentDbContext.tbl_Teachers.ToList();
                teachersList.Insert(0, new Teacher { Id = 0, FirstName = "select teacher name "});
                ViewBag.Teachers = teachersList;
            }
            catch (Exception e)
            {

            }
        }
    }
}
