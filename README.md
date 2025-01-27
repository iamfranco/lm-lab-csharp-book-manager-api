# Minimalist Book Manager API

This is a C# ASP.NET Core MVC Web API solution for a Book Manager API.

The Base URL is `https://localhost:7230/`

The API has the following endpoints:

| Action     | Endpoint           | What it does                                   | API Documentation                                     |
| ---------- | ------------------ | ---------------------------------------------- | ----------------------------------------------------- |
| **GET**    | `api/v1/book`      | **Get All** books in collection                | [Click Here](#get-all-books-in-collection)            |
| **GET**    | `api/v1/book/{id}` | **Get** book with id `{id}` from collection    | [Click Here](#get-book-with-id-id-from-collection)    |
| **POST**   | `api/v1/book`      | **Add** book to collection                     | [Click Here](#add-book-to-collection)                 |
| **PUT**    | `api/v1/book/{id}` | **Update** book with id `{id}` from collection | [Click Here](#update-book-with-id-id-from-collection) |
| **DELETE** | `api/v1/book/{id}` | **Delete** book with id `{id}` from collection | [Click Here](#delete-book-with-id-id-from-collection) |

Here we have 3 folders:

1. The `BookManagerApi` folder contains the C# solution to the challenge
2. The `BookManagerApi.Tests` folder contains the unit tests for the solution
3. The `diagrams` folder contains diagrams related to the solution

# Instructions

**Prerequisite**: The machine running the application should have [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (or above), and [PostgreSQL](https://www.postgresql.org/) installed.

Clone the repository to your computer.

Then navigate to the `BookManagerApi` folder (with `cd` command or otherwise).

Then modify the content of `BookManagerApi/appsettings.Development.json` so that it contains the appropriate `ConnectionStrings` with appropriate `username` and `password`.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "MySqlBookManagerApi": "server=localhost; database=bookshop; user=bookmanagerapi; password=apiuser123",
    "PostgreSqlBookManagerApi": "server=localhost; database=bookshop; username=bookmanagerapi; password=apiuser123"
  }
}
```

Then have PostgreSQL running in the background, using Task Manager or otherwise:

![PostgreSQL Task Manager Service](diagrams/postgresTaskManager.png)

Then execute the **migration commands** to create a new database `bookshop` with a table `Books` in your PostgreSQL server:

```
dotnet ef migrations add InitialCommit
```

```
dotnet ef database update
```

Then run the application:

```
dotnet run
```

So now you can go to [https://localhost:7230/swagger/index.html](https://localhost:7230/swagger/index.html) to see the available endpoints:

![Swagger example](diagrams/swagger.png)

The **Base URL** is [https://localhost:7230/](https://localhost:7230/) so for example in [Postman](https://www.postman.com/) we can send GET request to `https://localhost:7230/api/v1/book` to get all books in database (initally empty).

![Postman example](diagrams/postman.png)

# API Documentation

## Get All books in collection

[[Back To Top]](#minimalist-book-manager-api)

### Request

**GET** `api/v1/book`

### Response samples

Status Code: `200 OK`

Content type: `application/json`

```json
[
  {
    "id": 1,
    "title": "Clean code",
    "description": "Even bad code can function. But if code isn't clean, it can bring a development organization to its knees. Every year, countless hours and significant resources are lost because of poorly written code. But it doesn't have to be that way.",
    "author": "Robert Cecil Martin",
    "genre": "Education"
  },
  {
    "id": 2,
    "title": "Design Patterns: Elements of Reusable Object-Oriented Software",
    "description": "Design Patterns: Elements of Reusable Object-Oriented Software is a software engineering book describing software design patterns. The book was written by Erich Gamma, Richard Helm, Ralph Johnson, and John Vlissides, with a foreword by Grady Booch.",
    "author": "Erich Gamma, John Vlissides, Ralph Johnson, Richard Helm",
    "genre": "Education"
  }
]
```

## Get book with id `{id}` from collection

[[Back To Top]](#minimalist-book-manager-api)

### Request

**GET** `api/v1/book/{id}`

For example, **GET** `api/v1/book/1`

### Response samples

Status Code: `200 OK`

Content type: `application/json`

```json
{
  "id": 1,
  "title": "Clean code",
  "description": "Even bad code can function. But if code isn't clean, it can bring a development organization to its knees. Every year, countless hours and significant resources are lost because of poorly written code. But it doesn't have to be that way.",
  "author": "Robert Cecil Martin",
  "genre": "Education"
}
```

If `{id}` does not match any book id in the collection, then the response status code would be `404 Not Found`.

## Add book to collection

[[Back To Top]](#minimalist-book-manager-api)

### Request

**POST** `api/v1/book`

with request body

```json
{
  "id": 0,
  "title": "string",
  "description": "string",
  "author": "string",
  "genre": "Thriller"
}
```

where the `genre` could be one of the following: `Thriller`,
`Romance`, `Fantasy`, `Fiction`, `Education`.

For example, the request body could be:

```json
{
  "id": 3,
  "title": "Some Book Title",
  "description": "Some Book Description",
  "author": "Some Author",
  "genre": "Education"
}
```

### Response samples

Status Code: `201 Created`

Content type: `application/json`

```json
{
  "id": 3,
  "title": "Some Book Title",
  "description": "Some Book Description",
  "author": "Some Author",
  "genre": "Education"
}
```

If Status Code is `201 Created`, then the new book in the request body will be added into the book collection in the data store.

If the request body has an `{id}` matching with an already existing book's id in the collection, then the response code would be `409 Conflict`, and the new book in the request body will **not** be added to the book collection in the data store.

## Update book with id `{id}` from collection

[[Back To Top]](#minimalist-book-manager-api)

### Request

**PUT** `api/v1/book/{id}`

with request body

```json
{
  "id": 0,
  "title": "string",
  "description": "string",
  "author": "string",
  "genre": "Thriller"
}
```

where the `genre` could be one of the following: `Thriller`,
`Romance`, `Fantasy`, `Fiction`, `Education`.

For example, the request body could be:

```json
{
  "id": 3,
  "title": "Some Book Title",
  "description": "Some Book Description",
  "author": "Some Author",
  "genre": "Education"
}
```

### Response samples

Status Code: `204 No Content`

If status code is `204 No Content` then the book in the collection with id matching `{id}` would be updated to match the content of the request body.

If the `{id}` does not match any book id in the collection, then the response status code would be `404 Not Found`, and no modification would be applied to any book in the collection.

## Delete book with id `{id}` from collection

[[Back To Top]](#minimalist-book-manager-api)

### Request

**DELETE** `api/v1/book/{id}`

For example, **DELETE** `api/v1/book/1`

### Response samples

Status Code: `204 No Content`

If status code is `204 No Content` then the book in the collection with id matching `{id}` would be delete from the collection in the data store.

If `{id}` does not match any book id in the collection, then the response status code would be `404 Not Found` and none of the books in the collection would get deleted.
