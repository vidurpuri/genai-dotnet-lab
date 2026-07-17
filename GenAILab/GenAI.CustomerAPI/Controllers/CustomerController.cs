using GenAI.CustomerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenAI.CustomerAPI.Controllers
{
    /// <summary>
    /// Manages customer information and operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(CustomerRepository customer, ILogger<CustomerController> logger)
        {
            _customerRepository = customer;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all customers
        /// </summary>
        /// <returns>A list of all customers</returns>
        /// <response code="200">Returns the list of customers</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Models.Customer>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_customerRepository.GetAllCustomers());
        }

        /// <summary>
        /// Retrieves a specific customer by ID
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve</param>
        /// <returns>The requested customer</returns>
        /// <response code="200">Returns the requested customer</response>
        /// <response code="404">If the customer is not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Models.Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customer">The customer information to create</param>
        /// <returns>The created customer</returns>
        /// <response code="201">Returns the newly created customer</response>
        /// <response code="400">If the customer data is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(Models.Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(Models.Customer customer)
        {
            _customerRepository.AddCustomer(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="id">The ID of the customer to update</param>
        /// <param name="customer">The updated customer information</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the customer was successfully updated</response>
        /// <response code="400">If the ID doesn't match the customer's ID</response>
        /// <response code="404">If the customer is not found</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, Models.Customer customer)
        {
            if (id != customer.Id) return BadRequest();

            var success = _customerRepository.UpdateCustomer(customer);
            if (!success) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific customer
        /// </summary>
        /// <param name="id">The ID of the customer to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the customer was successfully deleted</response>
        /// <response code="404">If the customer is not found</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var success = _customerRepository.DeleteCustomer(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
