using Microsoft.IdentityModel.Tokens;
using System.Text;
using AcademyGroupLibrary;
using AcademyGroupContextLibrary;

namespace CodeFirst.LazyLoading
{
    class MainClass
    {
        // Для роботи з БД MS SQL Server необхідно додати пакет:
        // Microsoft.EntityFrameworkCore.SqlServer (представляє функціональність Entity Framework для роботи з MS SQL Server)

        // Lazy loading або ліниве завантаження передбачає неявне автоматичне завантаження пов'язаних даних при зверненні до навігаційної властивості.
        // Microsoft.EntityFrameworkCore.Proxies


        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("1. Показати всі групи");
                    Console.WriteLine("2. Додати групу");
                    Console.WriteLine("3. Редагувати групу");
                    Console.WriteLine("4. Видалити групу");
                    Console.WriteLine("5. Показати всіх студентів");
                    Console.WriteLine("6. Показати студентів конкретної групи");
                    Console.WriteLine("7. Додати студента");
                    Console.WriteLine("8. Редагувати студента");
                    Console.WriteLine("9. Видалити студента");
                    Console.WriteLine("0. Вихід");
                    int result = int.Parse(Console.ReadLine()!);
                    switch (result)
                    {
                        case 1:
                            ShowAllGroups();
                            break;
                        case 2:
                            AddNewGroup();
                            break;
                        case 3:
                            EditGroup();
                            break;
                        case 4:
                            RemoveGroup();
                            break;
                        case 5:
                            ShowAllStudents();
                            break;
                        case 6:
                            ShowStudentsByGroup();
                            break;
                        case 7:
                            AddNewStudent();
                            break;
                        case 8:
                            EditStudent();
                            break;
                        case 9:
                            RemoveStudent();
                            break;
                        case 0:
                            return;
                    }
                    ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ShowAllGroups()
        {
            Console.Clear();
            using (var db = new AcademyGroupContext())
            {
                var query = from gr in db.AcademyGroups
                            select gr;
                int iter = 0;
                foreach (var group in query)
                    Console.WriteLine($"Група #{++iter} {group.Name}");
            }
            Console.ReadKey();
        }

        static void AddNewGroup()
        {
            Console.Clear();
            string groupname;
            do
            {
                Console.WriteLine("Введіть назву нової групи: ");
                groupname = Console.ReadLine()!;
            }
            while (groupname.Trim().IsNullOrEmpty());
            using (var db = new AcademyGroupContext())
            {
                var academygroup = new AcademyGroup { Name = groupname };
                db.AcademyGroups.Add(academygroup);
                db.SaveChanges();
                Console.WriteLine("Групу успішно додано!");

            }
            Console.ReadKey();
        }

        static void EditGroup()
        {
            Console.Clear();
            using (var db = new AcademyGroupContext())
            {
                Console.WriteLine("Введіть порядковий номер групи: ");
                int number = int.Parse(Console.ReadLine()!);
                var query = (from gr in db.AcademyGroups
                             select gr).ToList()[number - 1];
                string groupname;
                do
                {
                    Console.WriteLine("Введіть нову назву групи: ");
                    groupname = Console.ReadLine()!;
                }
                while (groupname.Trim().IsNullOrEmpty());
                query.Name = groupname;
                db.SaveChanges();
                Console.WriteLine("Групу успішно змінено!");
            }
            Console.ReadKey();
        }

        static void RemoveGroup()
        {
            Console.Clear();
            using (var db = new AcademyGroupContext())
            {
                Console.WriteLine("Введіть порядковий номер групи: ");
                int number = int.Parse(Console.ReadLine()!);
                var query = (from gr in db.AcademyGroups
                             select gr).ToList()[number - 1];
                db.AcademyGroups.RemoveRange(query);
                db.SaveChanges();
                Console.WriteLine("Групу успішно видалено!");
            }
            Console.ReadKey();
        }

        static void ShowAllStudents()
        {
            Console.Clear();
            using (var db = new AcademyGroupContext())
            {
                var query = (from st in db.Students
                             select st).ToList();
                int iter = 0;
                foreach (var st in query)
                {
                    Console.Write($"Студент #{++iter}{st.FirstName,15}");
                    Console.Write($"{st.LastName,15}");
                    Console.Write($"{st.Age,10}");
                    Console.Write($"{st.GPA,10}");
                    Console.WriteLine($"{st.AcademyGroup?.Name,10}");
                }
            }
            Console.ReadKey();
        }

        static void ShowStudentsByGroup()
        {
            Console.Clear();
            using (var db = new AcademyGroupContext())
            {
                Console.WriteLine("Введіть порядковий номер групи: ");
                int number = int.Parse(Console.ReadLine()!);
                var query = (from gr in db.AcademyGroups
                             select gr).ToList()[number - 1];
                int iter = 0;
                foreach (var st in query.Students!)
                {
                    Console.Write($"Студент #{++iter}{st.FirstName,15}");
                    Console.Write($"{st.LastName,15}");
                    Console.Write($"{st.Age,10}");
                    Console.Write($"{st.GPA,10}");
                    Console.WriteLine($"{st.AcademyGroup?.Name,10}");
                }
            }
            Console.ReadKey();
        }

        static void AddNewStudent()
        {
            Console.Clear();
            string firstname, lastname;

            using (var db = new AcademyGroupContext())
            {
                do
                {
                    Console.WriteLine("Введіть ім'я студента: ");
                    firstname = Console.ReadLine()!;
                }
                while (firstname.Trim().IsNullOrEmpty());
                do
                {
                    Console.WriteLine("Введіть прізвище студента: ");
                    lastname = Console.ReadLine()!;
                }
                while (lastname.Trim().IsNullOrEmpty());
                Console.WriteLine("Введіть вік студента: ");
                int age = int.Parse(Console.ReadLine()!);
                Console.WriteLine("Введіть середній бал студента: ");
                double gpa = double.Parse(Console.ReadLine()!);
                Console.WriteLine("Введіть порядковий номер групи: ");
                int number = int.Parse(Console.ReadLine()!);
                var query = (from gr in db.AcademyGroups
                             select gr).ToList()[number - 1];
                var st = new Student { FirstName = firstname, LastName = lastname, Age = age, GPA = gpa, AcademyGroup = query };
                db.Students?.Add(st);
                db.SaveChanges();
                Console.WriteLine("Студента успішно додано!");

            }
            Console.ReadKey();
        }

        static void EditStudent()
        {
            Console.Clear();
            string firstname, lastname;

            using (var db = new AcademyGroupContext())
            {
                Console.WriteLine("Введіть порядковий номер студента: ");
                int number = int.Parse(Console.ReadLine()!);
                var student = (from st in db.Students
                               select st).ToList()[number - 1];
                do
                {
                    Console.WriteLine("Введіть ім'я студента: ");
                    firstname = Console.ReadLine()!;
                }
                while (firstname.Trim().IsNullOrEmpty());
                do
                {
                    Console.WriteLine("Введіть прізвище студента: ");
                    lastname = Console.ReadLine()!;
                }
                while (lastname.Trim().IsNullOrEmpty());
                Console.WriteLine("Введіть вік студента: ");
                int age = int.Parse(Console.ReadLine()!);
                Console.WriteLine("Введіть середній бал студента: ");
                double gpa = double.Parse(Console.ReadLine()!);
                Console.WriteLine("Введіть порядковий номер групи: ");
                number = int.Parse(Console.ReadLine()!);
                var group = (from gr in db.AcademyGroups
                             select gr).ToList()[number - 1];
                student.FirstName = firstname;
                student.LastName = lastname;
                student.Age = age;
                student.GPA = gpa;
                student.AcademyGroup = group;
                db.SaveChanges();
                Console.WriteLine("Дані студента успішно змінено!");

            }
            Console.ReadKey();
        }

        static void RemoveStudent()
        {
            Console.Clear();
            using (var db = new AcademyGroupContext())
            {
                Console.WriteLine("Введіть порядковий номер студента: ");
                int number = int.Parse(Console.ReadLine()!);
                var student = (from st in db.Students
                               select st).ToList()[number - 1];
                db.Students.RemoveRange(student);
                db.SaveChanges();
                Console.WriteLine("Студента успішно видалено!");

            }
            Console.ReadKey();
        }
    }
}