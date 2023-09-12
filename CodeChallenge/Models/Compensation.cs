using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public string EmployeeId { get; set; }
        public decimal Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Employee Employee { get; internal set; }
    }
}