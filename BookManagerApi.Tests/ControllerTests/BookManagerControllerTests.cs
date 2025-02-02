﻿using System.Collections.Generic;
using System.Linq;
using BookManagerApi.Controllers;
using BookManagerApi.Models;
using BookManagerApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace BookManagerApi.Tests;

public class BookManagerControllerTests
{
    private BookManagerController _controller;
    private Mock<IBookManagementService> _mockBookManagementService;

    [SetUp]
    public void Setup()
    {
        //Arrange
        _mockBookManagementService = new Mock<IBookManagementService>();
        _controller = new BookManagerController(_mockBookManagementService.Object);
    }

    [Test]
    public void GetBooks_Returns_AllBooks()
    {
        //Arange
        _mockBookManagementService.Setup(b => b.GetAllBooks()).Returns(GetTestBooks());

        //Act
        var result = _controller.GetBooks();

        //Assert
        result.Should().BeOfType(typeof(ActionResult<IEnumerable<Book>>));
        result.Value.Should().BeEquivalentTo(GetTestBooks());
        result.Value.Count().Should().Be(3);
    }

    [Test]
    public void GetBookById_Returns_CorrectBook()
    {
        //Arrange
        var testBookFound = GetTestBooks().FirstOrDefault();
        _mockBookManagementService.Setup(b => b.FindBookById(1)).Returns(testBookFound);
        _mockBookManagementService.Setup(b => b.BookExists(1)).Returns(true);

        //Act
        var result = _controller.GetBookById(1);

        //Assert
        result.Should().BeOfType(typeof(ActionResult<Book>));
        result.Value.Should().Be(testBookFound);
    }

    [Test]
    public void GetBookById_With_NonExistent_Id_Should_Return_NotFound()
    {
        //Arrange
        _mockBookManagementService.Setup(b => b.BookExists(10)).Returns(false);

        //Act
        var result = _controller.GetBookById(10);

        //Assert
        result.Result.Should().BeOfType(typeof(NotFoundResult));
    }

    [Test]
    public void UpdateBookById_Updates_Correct_Book()
    {
        //Arrange
        long existingBookId = 3;
        _mockBookManagementService.Setup(b => b.BookExists(3)).Returns(true);
        Book existingBookFound = GetTestBooks()
            .FirstOrDefault(b => b.Id.Equals(existingBookId));

        var bookUpdates = new Book() { Id = 3, Title = "Book Three", Description = "I am updating this for Book Three", Author = "Person Three", Genre = Genre.Education };

        _mockBookManagementService.Setup(b => b.FindBookById(existingBookId)).Returns(existingBookFound);

        //Act
        var result = _controller.UpdateBookById(existingBookId, bookUpdates);

        //Assert
        result.Should().BeOfType(typeof(NoContentResult));
    }

    [Test]
    public void UpdateBookById_With_NonExistent_Id_Should_Return_NotFound()
    {
        //Arrange
        long existingBookId = 10;
        _mockBookManagementService.Setup(b => b.BookExists(10)).Returns(false);

        var bookUpdates = new Book() { Id = 10, Title = "Book Three", Description = "I am updating this for Book Three", Author = "Person Three", Genre = Genre.Education };

        //Act
        var result = _controller.UpdateBookById(existingBookId, bookUpdates);

        //Assert
        result.Should().BeOfType(typeof(NotFoundResult));
    }

    [Test]
    public void AddBook_Creates_A_Book()
    {
        //Arrange
        _mockBookManagementService.Setup(b => b.BookExists(4)).Returns(false);
        var newBook = new Book() { Id = 4, Title = "Book Four", Description = "This is the description for Book Four", Author = "Person Four", Genre = Genre.Education };

        _mockBookManagementService.Setup(b => b.Create(newBook)).Returns(newBook);

        //Act
        var result = _controller.AddBook(newBook);

        //Assert
        result.Should().BeOfType(typeof(ActionResult<Book>));
    }

    [Test]
    public void AddBook_With_Book_On_Already_Existing_Id_Should_Return_Conflict()
    {
        //Arrange
        _mockBookManagementService.Setup(b => b.BookExists(4)).Returns(true);
        var newBook = new Book() { Id = 4, Title = "Book Four", Description = "This is the description for Book Four", Author = "Person Four", Genre = Genre.Education };

        _mockBookManagementService.Setup(b => b.Create(newBook)).Returns(newBook);

        //Act
        var result = _controller.AddBook(newBook);

        //Assert
        result.Result.Should().BeOfType(typeof(ConflictResult));
    }

    [Test]
    public void DeleteBook_Deletes_A_Book()
    {
        // Arrange
        long existingBookId = 3;
        _mockBookManagementService.Setup(b => b.BookExists(3)).Returns(true);

        //Act
        var result = _controller.DeleteBookById(existingBookId);

        //Assert
        result.Should().BeOfType(typeof(NoContentResult));
    }

    [Test]
    public void DeleteBook_With_NonExistent_Id_Should_Return_NotFound()
    {
        // Arrange
        long existingBookId = 3;
        _mockBookManagementService.Setup(b => b.BookExists(3)).Returns(false);

        //Act
        var result = _controller.DeleteBookById(existingBookId);

        //Assert
        result.Should().BeOfType(typeof(NotFoundResult));
    }

    private static List<Book> GetTestBooks()
    {
        return new List<Book>
        {
            new Book() { Id = 1, Title = "Book One", Description = "This is the description for Book One", Author = "Person One", Genre = Genre.Education },
            new Book() { Id = 2, Title = "Book Two", Description = "This is the description for Book Two", Author = "Person Two", Genre = Genre.Fantasy },
            new Book() { Id = 3, Title = "Book Three", Description = "This is the description for Book Three", Author = "Person Three", Genre = Genre.Thriller },
        };
    }
}
