using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Contracts;
using BookStore.API.Data;
using BookStore.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStore.API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with the books in the book store`s database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public BooksController(IBookRepository bookRepository,
            ILoggerService logger,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all books as a list
        /// </summary>
        /// <returns>List of books</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooks()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Atempted");
                var books = await _bookRepository.FindAll();
                var response = _mapper.Map<IList<BookDTO>>(books);
                _logger.LogInfo($"{location}: Success");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location} - {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Gets a book by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Book record</returns>
        [HttpGet("(id)")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBook(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Atempted {id}");
                var book = await _bookRepository.FindByID(id);
                if (book == null)
                {
                    _logger.LogWarn($"{location}: Not found {id}");
                    return NotFound();
                }
                var response = _mapper.Map<BookDTO>(book);
                _logger.LogInfo($"{location}: Success {id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location} - {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Creates new book
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns>Book object</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Atempted");
                if (bookDTO == null)
                {
                    _logger.LogWarn($"{location}: Empty request");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Invalid data");
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _bookRepository.Create(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Failed");
                }
                _logger.LogInfo($"{location}: Success");
                return Created("Create", new { book });
            }
            catch (Exception e)
            {
                return InternalError($"{location} - {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Update single book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPut("(id)")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO bookDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Atempted {id}");
                if (id < 1 || bookDTO == null || id != bookDTO.ID)
                {
                    _logger.LogWarn($"{location}: Bad data {id}");
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Invalid data {id}");
                    return BadRequest(ModelState);
                }
                var isExisting = await _bookRepository.isExisting(id);
                if (!isExisting)
                {
                    _logger.LogWarn($"{location}: Not found {id}");
                    return NotFound();
                }
                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _bookRepository.Update(book);
                if (!isSuccess)
                {
                    return InternalError($"{location} Failet {id}");
                }
                _logger.LogInfo($"{location}: Success {id}");
                return NoContent();

            }
            catch (Exception e)
            {
                return InternalError($"{location} - {e.Message} - {e.InnerException}");
            }
        }

        [HttpDelete("(id)")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Atempted {id}");
                if (id < 1)
                {
                    _logger.LogWarn($"{location}: Bad data {id}");
                    return BadRequest();
                }
                var isExisting = await _bookRepository.isExisting(id);
                if(!isExisting)
                {
                    _logger.LogWarn($"{location}: Not found {id}");
                    return NotFound();
                }
                var book = await _bookRepository.FindByID(id);
                var isSuccess = await _bookRepository.Delete(book);
                if(!isSuccess)
                {
                    return InternalError($"{location} Failet {id}");
                }
                _logger.LogInfo($"{location}: Success {id}");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location} - {e.Message} - {e.InnerException}");
            }
        }


        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return ($"{controller} - {action}");
        }
        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong. Opssie");
        }
    }
}