using ITI_Lap_9;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        var options = new DbContextOptionsBuilder<CompanyDBContext>()
                    .UseSqlite("Data Source=CompanyDB.db")
                    .Options;

        using var context = new CompanyDBContext(options);
        context.Database.EnsureCreated();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=======================================");
            Console.WriteLine("        Company Management System       ");
            Console.WriteLine("=======================================");
            Console.WriteLine("1.  Add Department");
            Console.WriteLine("2.  Edit Department");
            Console.WriteLine("3.  Delete Department");
            Console.WriteLine("4.  View Departments");
            Console.WriteLine("5.  Add Employee");
            Console.WriteLine("6.  Edit Employee");
            Console.WriteLine("7.  Delete Employee");
            Console.WriteLine("8.  View Employees");
            Console.WriteLine("9.  Add Project");
            Console.WriteLine("10. Edit Project");
            Console.WriteLine("11. Delete Project");
            Console.WriteLine("12. View Projects");
            Console.WriteLine("0.  Exit");
            Console.Write("Enter your choice: ");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1": AddDepartment(context); break;
                case "2": EditDepartment(context); break;
                case "3": DeleteDepartment(context); break;
                case "4": ViewDepartments(context); break;
                case "5": AddEmployee(context); break;
                case "6": EditEmployee(context); break;
                case "7": DeleteEmployee(context); break;
                case "8": ViewEmployees(context); break;
                case "9": AddProject(context); break;
                case "10": EditProject(context); break;
                case "11": DeleteProject(context); break;
                case "12": ViewProjects(context); break;
                case "0": return;
                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    static void AddDepartment(CompanyDBContext context)
    {
        Console.Write("Enter Department Name: ");
        var name = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Name cannot be empty.");
            Pause();
            return;
        }
        context.Departments.Add(new Department { Name = name });
        context.SaveChanges();
        Console.WriteLine("Department added.");
        Pause();
    }

    static void EditDepartment(CompanyDBContext context)
    {
        var list = context.Departments.ToList();
        if (!list.Any()) { Console.WriteLine("No departments."); Pause(); return; }
        for (int i = 0; i < list.Count; i++) Console.WriteLine($"{i + 1}. {list[i].Name}");
        Console.Write("Choose department: ");
        if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > list.Count)
        {
            Console.WriteLine("Invalid selection."); Pause(); return;
        }
        var dept = list[idx - 1];
        Console.Write("Enter new name: ");
        var name = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(name)) { Console.WriteLine("Name cannot be empty."); Pause(); return; }
        dept.Name = name;
        context.SaveChanges();
        Console.WriteLine("Department updated.");
        Pause();
    }

    static void DeleteDepartment(CompanyDBContext context)
    {
        var list = context.Departments.ToList();
        if (!list.Any()) { Console.WriteLine("No departments."); Pause(); return; }
        for (int i = 0; i < list.Count; i++) Console.WriteLine($"{i + 1}. {list[i].Name}");
        Console.Write("Choose department: ");
        if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > list.Count)
        {
            Console.WriteLine("Invalid selection."); Pause(); return;
        }
        context.Departments.Remove(list[idx - 1]);
        context.SaveChanges();
        Console.WriteLine("Department deleted.");
        Pause();
    }

    static void ViewDepartments(CompanyDBContext context)
    {
        var list = context.Departments.ToList();
        if (!list.Any()) Console.WriteLine("No departments.");
        else for (int i = 0; i < list.Count; i++) Console.WriteLine($"{i + 1}. {list[i].Name}");
        Pause();
    }

    static void AddEmployee(CompanyDBContext context)
    {
        Console.Write("First Name: "); var fn = Console.ReadLine()?.Trim();
        Console.Write("Last Name: "); var ln = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(fn) || string.IsNullOrEmpty(ln))
        {
            Console.WriteLine("Names cannot be empty."); Pause(); return;
        }
        var depts = context.Departments.ToList();
        if (!depts.Any()) { Console.WriteLine("No departments."); Pause(); return; }
        for (int i = 0; i < depts.Count; i++) Console.WriteLine($"{i + 1}. {depts[i].Name}");
        Console.Write("Department: ");
        if (!int.TryParse(Console.ReadLine(), out int di) || di < 1 || di > depts.Count)
        {
            Console.WriteLine("Invalid selection."); Pause(); return;
        }
        context.Employees.Add(new Employee { FirstName = fn, LastName = ln, DepartmentID = depts[di - 1].DepartmentID });
        context.SaveChanges();
        Console.WriteLine("Employee added.");
        Pause();
    }

    static void EditEmployee(CompanyDBContext context)
    {
        var emps = context.Employees.Include(e => e.Department).ToList();
        if (!emps.Any()) { Console.WriteLine("No employees."); Pause(); return; }
        for (int i = 0; i < emps.Count; i++) Console.WriteLine($"{i + 1}. {emps[i].FirstName} {emps[i].LastName} - {emps[i].Department?.Name}");
        Console.Write("Choose employee: ");
        if (!int.TryParse(Console.ReadLine(), out int ei) || ei < 1 || ei > emps.Count)
        {
            Console.WriteLine("Invalid selection."); Pause(); return;
        }
        var emp = emps[ei - 1];
        Console.WriteLine("1.Name 2.Department");
        Console.Write("Choice: "); var c = Console.ReadLine();
        if (c == "1")
        {
            Console.Write("New First Name: "); var fn = Console.ReadLine()?.Trim();
            Console.Write("New Last Name:  "); var ln = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(fn) || string.IsNullOrEmpty(ln)) { Console.WriteLine("Names cannot be empty."); Pause(); return; }
            emp.FirstName = fn; emp.LastName = ln;
        }
        else if (c == "2")
        {
            var depts = context.Departments.ToList();
            for (int i = 0; i < depts.Count; i++) Console.WriteLine($"{i + 1}. {depts[i].Name}");
            Console.Write("Department: ");
            if (!int.TryParse(Console.ReadLine(), out int di) || di < 1 || di > depts.Count)
            {
                Console.WriteLine("Invalid selection."); Pause(); return;
            }
            emp.DepartmentID = depts[di - 1].DepartmentID;
        }
        else { Console.WriteLine("Invalid choice."); Pause(); return; }
        context.SaveChanges();
        Console.WriteLine("Employee updated.");
        Pause();
    }

    static void DeleteEmployee(CompanyDBContext context)
    {
        var emps = context.Employees.ToList();
        if (!emps.Any()) { Console.WriteLine("No employees."); Pause(); return; }
        for (int i = 0; i < emps.Count; i++) Console.WriteLine($"{i + 1}. {emps[i].FirstName} {emps[i].LastName}");
        Console.Write("Choose: ");
        if (!int.TryParse(Console.ReadLine(), out int i2) || i2 < 1 || i2 > emps.Count)
        {
            Console.WriteLine("Invalid selection."); Pause(); return;
        }
        context.Employees.Remove(emps[i2 - 1]); context.SaveChanges();
        Console.WriteLine("Employee deleted.");
        Pause();
    }

    static void ViewEmployees(CompanyDBContext context)
    {
        var emps = context.Employees.Include(e => e.Department).ToList();
        if (!emps.Any()) Console.WriteLine("No employees.");
        else for (int i = 0; i < emps.Count; i++) Console.WriteLine($"{i + 1}. {emps[i].FirstName} {emps[i].LastName} - {emps[i].Department?.Name}");
        Pause();
    }

    static void AddProject(CompanyDBContext context)
    {
        Console.Write("Project Name: "); var nm = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(nm)) { Console.WriteLine("Name cannot be empty."); Pause(); return; }
        Console.Write("Start Date (yyyy-MM-dd): "); if (!DateTime.TryParse(Console.ReadLine(), out DateTime sd)) { Console.WriteLine("Invalid date."); Pause(); return; }
        Console.Write("End Date or empty: "); var ei = Console.ReadLine(); DateTime? ed = null;
        if (!string.IsNullOrWhiteSpace(ei))
        {
            if (!DateTime.TryParse(ei, out DateTime tmp)) { Console.WriteLine("Invalid date."); Pause(); return; }
            ed = tmp;
            if (ed < sd) { Console.WriteLine("End must be after start."); Pause(); return; }
        }
        context.Projects.Add(new Project { Name = nm, StartDate = sd, EndDate = ed }); context.SaveChanges();
        Console.WriteLine("Project added.");
        Pause();
    }

    static void EditProject(CompanyDBContext context)
    {
        var prjs = context.Projects.ToList();
        if (!prjs.Any()) { Console.WriteLine("No projects."); Pause(); return; }
        for (int i = 0; i < prjs.Count; i++) Console.WriteLine($"{i + 1}. {prjs[i].Name}");
        Console.Write("Choose: "); if (!int.TryParse(Console.ReadLine(), out int pi) || pi < 1 || pi > prjs.Count) { Console.WriteLine("Invalid."); Pause(); return; }
        var p = prjs[pi - 1];
        Console.WriteLine("1.Name 2.Start 3.End"); Console.Write("Choice: "); var c = Console.ReadLine();
        if (c == "1") { Console.Write("New Name: "); var nm = Console.ReadLine()?.Trim(); if (string.IsNullOrEmpty(nm)) { Console.WriteLine("Empty."); Pause(); return; } p.Name = nm; }
        else if (c == "2") { Console.Write("Start (yyyy-MM-dd): "); if (!DateTime.TryParse(Console.ReadLine(), out DateTime ns)) { Console.WriteLine("Invalid."); Pause(); return; } p.StartDate = ns; if (p.EndDate < ns) { Console.WriteLine("End before start."); Pause(); return; } }
        else if (c == "3") { Console.Write("End or empty: "); var ei2 = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(ei2)) { if (!DateTime.TryParse(ei2, out DateTime ne)) { Console.WriteLine("Invalid."); Pause(); return; } if (ne < p.StartDate) { Console.WriteLine("End before start."); Pause(); return; } p.EndDate = ne; } else p.EndDate = null; }
        else { Console.WriteLine("Invalid."); Pause(); return; }
        context.SaveChanges(); Console.WriteLine("Project updated."); Pause();
    }

    static void DeleteProject(CompanyDBContext context)
    {
        var prjs = context.Projects.ToList();
        if (!prjs.Any()) { Console.WriteLine("No projects."); Pause(); return; }
        for (int i = 0; i < prjs.Count; i++) Console.WriteLine($"{i + 1}. {prjs[i].Name}");
        Console.Write("Choose: "); if (!int.TryParse(Console.ReadLine(), out int di) || di < 1 || di > prjs.Count) { Console.WriteLine("Invalid."); Pause(); return; }
        context.Projects.Remove(prjs[di - 1]); context.SaveChanges(); Console.WriteLine("Project deleted."); Pause();
    }

    static void ViewProjects(CompanyDBContext context)
    {
        var prjs = context.Projects.ToList();
        if (!prjs.Any()) Console.WriteLine("No projects.");
        else for (int i = 0; i < prjs.Count; i++) Console.WriteLine($"{i + 1}. {prjs[i].Name} - {prjs[i].StartDate:yyyy-MM-dd} to {prjs[i].EndDate:yyyy-MM-dd}");
        Pause();
    }

    static void Pause()
    {
        Console.WriteLine("Press any key to return to menu.");
        Console.ReadKey();
    }
}
