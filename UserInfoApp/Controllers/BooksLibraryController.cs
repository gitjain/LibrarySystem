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

            // Create the CloudTable object that represents the "userInfo" table.
            CloudTable table = tableClient.GetTableReference("booksLibrary");

            // Construct the query operation for all user entities.
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
    }
}