using StudentWebAPI.Dtos;
using StudentWebAPI.Models;

namespace StudentWebAPI.Mappers
{
    public static class StudentMapper
    {
        public static Student ToStudent(AddStudentDto dto)
        {
            return new Student
            {
                StudentNumber = dto.StudentNumber,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                Gender = dto.Gender,
                Address = dto.Address,
                Birthday = dto.Birthday,
                Birthplace = dto.Birthplace
            };
        }

        public static void UpdateStudent(Student student, EditStudentDto dto)
        {
            student.StudentNumber = dto.StudentNumber;
            student.LastName = dto.LastName;
            student.FirstName = dto.FirstName;
            student.Gender = dto.Gender;
            student.Address = dto.Address;
            student.Birthday = dto.Birthday;
            student.Birthplace = dto.Birthplace;
        }
    }
}