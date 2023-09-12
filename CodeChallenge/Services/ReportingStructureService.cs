using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeService> _logger;

        public ReportingStructureService(ILogger<EmployeeService> logger, IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public ReportingStructure GetById(string id)
        {
            Employee employee = _employeeService.GetDirectReportsById(id);
            ReportingStructure reportingStructure = new ReportingStructure();
            reportingStructure.Employee = employee;
            reportingStructure.NumberOfReports = GetTotalNumberOfReports(employee);
            return reportingStructure;
        }

        public int GetTotalNumberOfReports(Employee employee)
        {
            int reportCount = 0;
            //todo this doesnt work
            if (employee == null)
            {
                return reportCount;
            }
            else if (employee.DirectReports == null)
            {
                return reportCount;
            }
            else
            {
                reportCount += employee.DirectReports.Count;
                foreach (var reportingEmployee in employee.DirectReports)
                {
                    var currentEmployee = _employeeService.GetById(reportingEmployee.EmployeeId);
                    if (currentEmployee != null)
                    {
                        reportCount += GetTotalNumberOfReports(currentEmployee);
                    }
                }
                return reportCount;
            }
        }

    }
}
