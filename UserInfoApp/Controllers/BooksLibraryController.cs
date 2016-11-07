using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;

namespace UserInfoApp.Controllers
{
    public class BooksLibraryController : ApiController
    {
        // GET: BookList
        public IHttpActionResult GetBookList()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                WebConfigurationManager.AppSettings.Get("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "booksLibrary" table.
            CloudTable table = tableClient.GetTableReference("booksLibrary");

            // Construct the query operation for all books entities.
            TableQuery<BookEntity> query = new TableQuery<BookEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "LibrarySystem"));

            string getBookList = "[{";
            Boolean firstIteration = true;

            foreach (BookEntity entity in table.ExecuteQuery(query))
            {
                if (firstIteration == false)
                {
                    getBookList += ", {\"bookname\" :"  + "\""  + entity.RowKey + "\", " 
                        + "\"availability\" :" + "\"" + entity.Availability + "\""
                        + "}";
                }
                else
                {
                    getBookList += "\"bookname\" : " + "\"" + entity.RowKey + "\", "
                        + "\"availability\" :" + "\"" + entity.Availability + "\""
                        + "}";
                }
                firstIteration = false;
            }

            getBookList += "]";

            JsonResult temp = new JsonResult();
            temp.Data = getBookList;
            return Ok(temp);
        }

        public IHttpActionResult PutBookUserInfo(string userName, string bookName)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               WebConfigurationManager.AppSettings.Get("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "booksLibrary" table.
            CloudTable table = tableClient.GetTableReference("booksLibrary");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<BookEntity>("LibrarySystem", bookName);

            // Execute the operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a BookEntity object.
            BookEntity updateEntity = (BookEntity)retrievedResult.Result;
            if (updateEntity != null)
            {
                // Update book name.
                updateEntity.UserName = userName;
                updateEntity.Availability = "No";

                // Create the Replace TableOperation.
                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                // Execute the operation.
                table.Execute(updateOperation);

                return Ok("Entity updated.");
            }

            else
                return Ok("Entity could not be retrieved.");
        }
    }
 
}