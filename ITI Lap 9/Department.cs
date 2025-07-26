

namespace ITI_Lap_9
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
