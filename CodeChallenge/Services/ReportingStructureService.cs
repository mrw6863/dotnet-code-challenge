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
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public ReportingStructureService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure GetById(string id)
        {

            Employee employee = _employeeRepository.GetById(id);

            if (employee == null) {
                throw new Exception("Invalid employeeId: " + id);
            }

            ReportingStructure reportingStructure = new ReportingStructure(employee);

            int numberOfReports = 0;
            List<Employee> directReports = employee.DirectReports;
            numberOfReports = directReports.Count();
            for (Employee e : directReports) {
                List<Employee> dp = emp.getDirectReports();
                if (dp != null) {
                    numberOfReports += emp.getDirectReports().size();
                }
            }
            reportingStructure.setNumberOfReports(numberOfReports);

            return reportingStructure;
        }
    }
}
