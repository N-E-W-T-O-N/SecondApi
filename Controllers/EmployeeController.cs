using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SecondApi.Model;
 

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecondApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly MyDbContext _dbContext;
        private readonly IBank _bank;

        public EmployeeController(ILogger<EmployeeController> logger, MyDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]//get all details
        [Route("GetAllData")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            // return await _dbContext.Employees.ToListAsync();
            var emp = await _dbContext.Employees.Select(e => new Employee
            {
                Employee_id = e.Employee_id,
                First_name = e.First_name,
                Last_name = e.Last_name,
                Email = e.Email,
                Job_id = e.Job_id,
                Phone_number = e.Phone_number,
                Hire_date = e.Hire_date,
                Salary = e.Salary
            }).ToListAsync();
            if (emp == null) return NotFound();
            return Ok(emp);
        }

        [HttpGet("{id}")]//get the details based on id
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var emp = await _dbContext.Employees.Select(e => new Employee
                {
                   
                    Employee_id = e.Employee_id,
                    First_name = e.First_name,
                    Last_name = e.Last_name,
                    Email = e.Email,
                    Job_id = e.Job_id,
                    Phone_number = e.Phone_number,
                    Hire_date = e.Hire_date,
                    Salary = e.Salary

                }).Where(e => e.Employee_id == id).FirstOrDefaultAsync();//ToListAsync();

                if (emp == null)
                    return NotFound();

                return Ok(emp);
            }
            catch (Exception ex) { return BadRequest(ex); }
        }

 /*       [HttpGet("{First_Name}")]//get the details based on id 
        public async Task<ActionResult<Employee>> GetEmployee_Name(string f_name)
        {
            try
            {
                var emp = await _dbContext.Employees.Select(e => new Employee
                {
                    employee_id = e.employee_id,
                    first_name = e.first_name,
                    last_name = e.last_name,
                    email = e.email,
                    job_id = e.job_id,
                    phone_number = e.phone_number,
                    hire_date = e.hire_date,
                    salary = e.salary

                }).Where(e => e.first_name == f_name).FirstOrDefaultAsync();//ToListAsync();

                if (emp == null)
                    return NotFound();

                return Ok(emp);
            }
            catch (Exception ex) { return BadRequest(ex); }
        }*/


        [HttpPost]//ADD NEW DATA
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> AddEmployee(Employee_To_Be_Added e)
        {
            if (e.First_name == null)
            {
                ModelState.AddModelError(nameof(e.First_name), "First name is required.");
            }

            if (e.Last_name == null)
            {
                ModelState.AddModelError(nameof(e.Last_name), "Last name is required.");
            }
            if (e.Job_id == null)
            {
                ModelState.AddModelError(nameof(e.Job_id), "Job ID is required.");
            }

            if (e.Hire_date == null)
            {
                ModelState.AddModelError(nameof(e.Hire_date), "Hire date is required.");
            }

            if (e.Salary == null)
            {
                ModelState.AddModelError(nameof(e.Salary), "Salary is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                /*if (e.First_name == null) { throw new ArgumentException($"You must supply FIRST NAME"); }
                else if (e.Last_name == null) { throw new ArgumentException($"You must supply LAST NAME"); }
                else if (e.Email == null) { throw new ArgumentException($"You must supply EMAIL"); }
                else if(e.Hire_date== null) { throw new ArgumentException($"You must supply HIRE DATE"); }
                else if (e.Job_id == null) { throw new ArgumentException($"You must supply JOB ID"); }
                else if (e.Phone_number == null) { throw new ArgumentException($"You must supply Phone_number"); }
                else if (e.Salary == null) {throw new ArgumentException($"You must supply SALARY");}*/

                var emp = new Employee
                {
                    //Employee_id = e.Employee_id,
                    First_name = e.First_name,
                    Last_name = e.Last_name,
                    Email = e.Email,
                    Job_id = e.Job_id,
                    Phone_number = e.Phone_number,
                    Hire_date = e.Hire_date,
                    Salary = e.Salary
                };
                

                await _dbContext.Employees.AddAsync(emp);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEmployee), new { id = emp.Employee_id }, emp);
            }
            catch  (Exception ex) { return BadRequest(ex); }
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id , Employee emp)
        {
            try
            {
                //First check if the Employee with id exist or not
                var Old_Emp = await _dbContext.Employees.FindAsync(id);
              
                if (Old_Emp == null) return BadRequest($"No Employee with ID ={id}");//TOt exist

                if (emp.Employee_id == 0) { emp.Employee_id = Old_Emp.Employee_id; }
                //CONCURRENCY ERROR
                /*                
               else  if (emp.Employee_id != id && await _dbContext.Employees.FindAsync(emp.Employee_id) != null)
               {
               BadRequest($"Employee with {emp.Employee_id} already exist");
               }*/

                if (emp.First_name == null) { emp.First_name = Old_Emp.First_name; }
                if (emp.Last_name == null) { emp.Last_name = Old_Emp.Last_name; }
                if(emp.Email == null) { emp.Email = Old_Emp.Email; }
                if (emp.Job_id == null) { emp.Job_id = Old_Emp.Job_id; }
                if (emp.Phone_number == null) { emp.Phone_number=Old_Emp.Phone_number; }
                if (emp.Hire_date == null) { emp.Hire_date = Old_Emp.Hire_date; }
                if (emp.Salary == null) { emp.Salary = Old_Emp.Salary; }



                /*               if (id != emp.Employee_id)
                           {
                                   return BadRequest($"No Employee with ID ={id}");
                               }*/
                _dbContext.Entry(Old_Emp).State = EntityState.Detached;//remove this instance conncetion
                _dbContext.Entry(emp).State = EntityState.Modified;

            
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_dbContext.Employees.Any(e => e.Employee_id == id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the data.");
                }

                throw;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the data.");
            }

            return Ok("Employee details have been updated"); //NoContent();//We our query is succesful but dont want to change the page


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
           
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            return Ok($"Employee with {id} has been removed");//NoContent();
        }

/*        private bool EmployeeExists(int id)
        {
            return _dbContext.Employees.Any(e => e.employee_id == id);
        }*/

        [HttpGet("About")]
        public IActionResult GetAbout()
        {
            return Ok("API Created for retrieving Employee information");
        }

    }
}
